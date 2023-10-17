using System;
using System.Runtime.InteropServices;
using ComputeSharp.D2D1.Shaders.Interop.Extensions;
using ComputeSharp.Win32;

namespace ComputeSharp.D2D1.Shaders.Interop.Effects.ResourceManagers;

/// <inheritdoc/>
partial struct D2D1ResourceTextureManagerImpl
{
    /// <summary>
    /// The implementation for <see cref="ID2D1ResourceTextureManagerInternal"/>.
    /// </summary>
    private static unsafe class ID2D1ResourceTextureManagerInternalMethods
    {
#if !NET6_0_OR_GREATER
        /// <inheritdoc cref="Initialize"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int InitializeDelegate(D2D1ResourceTextureManagerImpl* @this, ID2D1EffectContext* effectContext, uint* dimensions);

        /// <inheritdoc cref="GetResourceTexture"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int GetResourceTextureDelegate(D2D1ResourceTextureManagerImpl* @this, ID2D1ResourceTexture** resourceTexture);

        /// <summary>
        /// A cached <see cref="QueryInterfaceDelegate"/> instance wrapping <see cref="QueryInterface"/>.
        /// </summary>
        public static readonly QueryInterfaceDelegate QueryInterfaceWrapper = QueryInterface;

        /// <summary>
        /// A cached <see cref="AddRefDelegate"/> instance wrapping <see cref="AddRef"/>.
        /// </summary>
        public static readonly AddRefDelegate AddRefWrapper = AddRef;

        /// <summary>
        /// A cached <see cref="ReleaseDelegate"/> instance wrapping <see cref="Release"/>.
        /// </summary>
        public static readonly ReleaseDelegate ReleaseWrapper = Release;

        /// <summary>
        /// A cached <see cref="InitializeDelegate"/> instance wrapping <see cref="Initialize"/>.
        /// </summary>
        public static readonly InitializeDelegate InitializeWrapper = Initialize;

        /// <summary>
        /// A cached <see cref="GetResourceTextureDelegate"/> instance wrapping <see cref="GetResourceTexture"/>.
        /// </summary>
        public static readonly GetResourceTextureDelegate GetResourceTextureWrapper = GetResourceTexture;
#endif

        /// <inheritdoc cref="D2D1ResourceTextureManagerImpl.QueryInterface"/>
        [UnmanagedCallersOnly]
        public static int QueryInterface(D2D1ResourceTextureManagerImpl* @this, Guid* riid, void** ppvObject)
        {
            @this = (D2D1ResourceTextureManagerImpl*)&((void**)@this)[-1];

            return @this->QueryInterface(riid, ppvObject);
        }

        /// <inheritdoc cref="D2D1ResourceTextureManagerImpl.AddRef"/>
        [UnmanagedCallersOnly]
        public static uint AddRef(D2D1ResourceTextureManagerImpl* @this)
        {
            @this = (D2D1ResourceTextureManagerImpl*)&((void**)@this)[-1];

            return @this->AddRef();
        }

        /// <inheritdoc cref="D2D1ResourceTextureManagerImpl.Release"/>
        [UnmanagedCallersOnly]
        public static uint Release(D2D1ResourceTextureManagerImpl* @this)
        {
            @this = (D2D1ResourceTextureManagerImpl*)&((void**)@this)[-1];

            return @this->Release();
        }

        /// <inheritdoc cref="ID2D1ResourceTextureManagerInternal.Initialize"/>
        [UnmanagedCallersOnly]
        public static int Initialize(D2D1ResourceTextureManagerImpl* @this, ID2D1EffectContext* effectContext, uint* dimensions)
        {
            @this = (D2D1ResourceTextureManagerImpl*)&((void**)@this)[-1];

            if (effectContext is null)
            {
                return E.E_POINTER;
            }

            lock (@this->lockHandle.Target!)
            {
                // If the dimensions are available, validate them
                if (dimensions is not null)
                {
                    uint expectedDimensions = *dimensions;

                    if (@this->resourceTextureProperties.extents is not null &&
                        @this->resourceTextureProperties.dimensions != expectedDimensions)
                    {
                        return E.E_INVALIDARG;
                    }

                    @this->expectedDimensions = expectedDimensions;
                }

                // If this is the first time the method is called, just store the context.
                // Before doing so, get an ID2D1Multithread instance, to ensure thread safety.
                // For additional info, see docs on ID2D1EffectFactoryExtensions.GetD2D1Multithread.
                if (@this->d2D1EffectContext is null)
                {
                    using ComPtr<ID2D1Multithread> d2D1Multithread = default;

                    HRESULT hresult = effectContext->GetD2D1Multithread(d2D1Multithread.GetAddressOf());

                    // If an ID2D1Multithread object is available, we can safely store the context. That
                    // is, under the condition that the required multithread support is also available.
                    if (Windows.SUCCEEDED(hresult))
                    {
                        if (@this->requiresMultithread > d2D1Multithread.Get()->GetMultithreadProtected())
                        {
                            return E.E_INVALIDARG;
                        }

                        // Now, the effect context can actually be stored safely while holding the lock.
                        // This is guaranteed to be the case here, as this method is only called (as per
                        // contact of the COM interface) from ID2D1EffectImpl, which holds the D2D lock.
                        ComPtr.WriteTo(effectContext, out @this->d2D1EffectContext);

                        @this->d2D1Multithread = d2D1Multithread.Detach();
                    }

                    return hresult;
                }

                // Otherwise, just do nothing and return S_FALSE. This allows an existing resource texture
                // manager to be shared across effects. If the resource cannot be shared, it will just
                // return an error when effect is actually set, with ID2D1DrawInfo::SetResourceTexture.
                return S.S_FALSE;
            }
        }

        /// <inheritdoc cref="ID2D1ResourceTextureManagerInternal.GetResourceTexture"/>
        [UnmanagedCallersOnly]
        public static int GetResourceTexture(D2D1ResourceTextureManagerImpl* @this, ID2D1ResourceTexture** resourceTexture)
        {
            @this = (D2D1ResourceTextureManagerImpl*)&((void**)@this)[-1];

            if (*resourceTexture is not null)
            {
                return E.E_POINTER;
            }

            lock (@this->lockHandle.Target!)
            {
                // If the effect context is null, it means Initialize has not been called yet
                if (@this->d2D1EffectContext is null)
                {
                    return E.E_NOT_VALID_STATE;
                }

                // If the texture has already been created, just return it
                if (@this->d2D1ResourceTexture is not null)
                {
                    ComPtr.WriteTo(@this->d2D1ResourceTexture, out *resourceTexture);

                    return S.S_OK;
                }

                // If the data is null at this point, it means CreateResourceTexture has not been called yet
                if (@this->data is null)
                {
                    return E.E_NOT_VALID_STATE;
                }

                using ComPtr<ID2D1ResourceTexture> d2D1ResourceTexture = default;

                // Create the resource now, as it hasn't been created yet
                int result = @this->d2D1EffectContext->CreateResourceTexture(
                    resourceId: @this->resourceId,
                    resourceTextureProperties: &@this->resourceTextureProperties,
                    data: @this->data,
                    strides: @this->strides,
                    dataSize: @this->dataSize,
                    resourceTexture: d2D1ResourceTexture.GetAddressOf());

                // If creation was successful, release the buffered data. Going forwards,
                // the resource texture will be used directly for all updates requested.
                if (result == S.S_OK)
                {
                    // Store the resource texture for later
                    _ = d2D1ResourceTexture.CopyTo(&@this->d2D1ResourceTexture);

                    // Also return it to callers
                    _ = d2D1ResourceTexture.CopyTo(resourceTexture);

                    // Free the staging buffers
                    NativeMemory.Free(@this->resourceTextureProperties.extents);
                    NativeMemory.Free(@this->resourceTextureProperties.extendModes);
                    NativeMemory.Free(@this->data);
                    NativeMemory.Free(@this->strides);

                    // Reset the stored pointers to avoid double frees from Release()
                    @this->resourceTextureProperties.extents = null;
                    @this->resourceTextureProperties.extendModes = null;
                    @this->data = null;
                    @this->strides = null;
                }

                return result;
            }
        }
    }
}