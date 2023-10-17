using System;
using System.Runtime.InteropServices;
using ComputeSharp.D2D1.Extensions;
using ComputeSharp.D2D1.Shaders.Interop.Effects.ResourceManagers;
using ComputeSharp.D2D1.Shaders.Interop.Extensions;
using ComputeSharp.Win32;

namespace ComputeSharp.D2D1.Interop.Effects;

/// <summary>
/// A simple <see cref="ID2D1EffectImpl"/> and <see cref="ID2D1DrawTransform"/> implementation for a given pixel shader.
/// </summary>
internal unsafe partial struct PixelShaderEffect
{
    /// <summary>
    /// The implementation for <see cref="ID2D1EffectImpl"/>.
    /// </summary>
    private static class ID2D1EffectImplMethods
    {
#if !NET6_0_OR_GREATER
        /// <inheritdoc cref="Initialize"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int InitializeDelegate(PixelShaderEffect* @this, ID2D1EffectContext* effectContext, ID2D1TransformGraph* transformGraph);

        /// <inheritdoc cref="PrepareForRender"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int PrepareForRenderDelegate(PixelShaderEffect* @this, D2D1_CHANGE_TYPE changeType);

        /// <inheritdoc cref="SetGraph"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int SetGraphDelegate(PixelShaderEffect* @this, ID2D1TransformGraph* transformGraph);

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
        /// A cached <see cref="PrepareForRenderDelegate"/> instance wrapping <see cref="PrepareForRender"/>.
        /// </summary>
        public static readonly PrepareForRenderDelegate PrepareForRenderWrapper = PrepareForRender;

        /// <summary>
        /// A cached <see cref="SetGraphDelegate"/> instance wrapping <see cref="SetGraph"/>.
        /// </summary>
        public static readonly SetGraphDelegate SetGraphWrapper = SetGraph;
#endif

        /// <inheritdoc cref="ID2D1EffectImpl.QueryInterface"/>
        [UnmanagedCallersOnly]
        public static int QueryInterface(PixelShaderEffect* @this, Guid* riid, void** ppvObject)
        {
            return @this->QueryInterface(riid, ppvObject);
        }

        /// <inheritdoc cref="ID2D1EffectImpl.AddRef"/>
        [UnmanagedCallersOnly]
        public static uint AddRef(PixelShaderEffect* @this)
        {
            return @this->AddRef();
        }

        /// <inheritdoc cref="ID2D1EffectImpl.Release"/>
        [UnmanagedCallersOnly]
        public static uint Release(PixelShaderEffect* @this)
        {
            return @this->Release();
        }

        /// <inheritdoc cref="ID2D1EffectImpl.Initialize"/>
        [UnmanagedCallersOnly]
        public static int Initialize(PixelShaderEffect* @this, ID2D1EffectContext* effectContext, ID2D1TransformGraph* transformGraph)
        {
            try
            {
                ReadOnlySpan<byte> bytecode = @this->GetGlobals().HlslBytecode.Span;

                fixed (Guid* pShaderId = &@this->GetGlobals().EffectId)
                fixed (byte* pBytecode = bytecode)
                {
                    HRESULT hresult = effectContext->LoadPixelShader(
                        shaderId: pShaderId,
                        shaderBuffer: pBytecode,
                        shaderBufferCount: (uint)bytecode.Length);

                    // If E_INVALIDARG was returned, try to check whether double precision support was requested when not available. This
                    // is only done to provide a more helpful error message to callers. If no error was returned, the behavior is the same.
                    // If any error is detected while trying to check for shader support, ignore the value and propagate the current HRESULT.
                    if (hresult == E.E_INVALIDARG && effectContext->IsShaderSupported(pBytecode, bytecode.Length) == S.S_FALSE)
                    {
                        hresult = D2DERR.D2DERR_INSUFFICIENT_DEVICE_CAPABILITIES;
                    }

                    // Finally, assert that we did load the pixel shader correctly
                    hresult.Assert();
                }

                // If loading the bytecode succeeded, set the transform node
                transformGraph->SetSingleTransformNode((ID2D1TransformNode*)&@this->lpVtblForID2D1DrawTransform).Assert();

                // Store the new ID2D1EffectContext object
                ComPtr.CopyTo(effectContext, ref @this->d2D1EffectContext);

                return S.S_OK;
            }
            catch (Exception e)
            {
                return Marshal.GetHRForException(e);
            }
        }

        /// <inheritdoc cref="ID2D1EffectImpl.PrepareForRender"/>
        [UnmanagedCallersOnly]
        public static int PrepareForRender(PixelShaderEffect* @this, D2D1_CHANGE_TYPE changeType)
        {
            try
            {
                // Validate the constant buffer. We either must have a stateless shader, in which case
                // the constant buffer might either be null or empty (both cases are allowed), or we
                // must have a shader with some state and a constant buffer being set. In that case
                // the code setting the constant buffer will have already validated its length.
                if (@this->GetGlobals().ConstantBufferSize > 0 &&
                    @this->constantBuffer is null)
                {
                    return E.E_NOT_VALID_STATE;
                }

                // First, set the constant buffer, if available
                if (@this->constantBuffer is not null)
                {
                    @this->d2D1DrawInfo->SetPixelShaderConstantBuffer(
                        buffer: @this->constantBuffer,
                        bufferCount: (uint)@this->GetGlobals().ConstantBufferSize).Assert();
                }

                ReadOnlySpan<D2D1ResourceTextureDescription> resourceTextureDescriptions = @this->GetGlobals().ResourceTextureDescriptions.Span;

                for (int i = 0; i < resourceTextureDescriptions.Length; i++)
                {
                    using ComPtr<ID2D1ResourceTextureManager> resourceTextureManager = new(@this->resourceTextureManagerBuffer[i]);

                    // If the current resource texture manager is not set, we cannot render, as there's an unbound resource texture
                    if (resourceTextureManager.Get() is null)
                    {
                        return E.E_NOT_VALID_STATE;
                    }

                    using ComPtr<ID2D1ResourceTextureManagerInternal> resourceTextureManagerInternal = default;

                    // Get the ID2D1ResourceTextureManagerInternal object. This cast should always succeed, as when
                    // an input resource texture managers is set it's also checked for ID2D1ResourceTextureManagerInternal,
                    // but still validate for good measure.
                    resourceTextureManager.CopyTo(resourceTextureManagerInternal.GetAddressOf()).Assert();

                    using ComPtr<ID2D1ResourceTexture> d2D1ResourceTexture = default;

                    // Try to get the ID2D1ResourceTexture from the manager
                    resourceTextureManagerInternal.Get()->GetResourceTexture(d2D1ResourceTexture.GetAddressOf()).Assert();

                    ref readonly D2D1ResourceTextureDescription resourceTextureDescription = ref resourceTextureDescriptions[i];

                    // Set the ID2D1ResourceTexture object to the current index in the ID2D1DrawInfo object in use
                    @this->d2D1DrawInfo->SetResourceTexture(
                        textureIndex: (uint)resourceTextureDescription.Index,
                        resourceTexture: d2D1ResourceTexture.Get()).Assert();
                }

                return S.S_OK;
            }
            catch (Exception e)
            {
                return Marshal.GetHRForException(e);
            }
        }

        /// <inheritdoc cref="ID2D1EffectImpl.SetGraph"/>
        [UnmanagedCallersOnly]
        public static int SetGraph(PixelShaderEffect* @this, ID2D1TransformGraph* transformGraph)
        {
            return E.E_NOTIMPL;
        }
    }
}