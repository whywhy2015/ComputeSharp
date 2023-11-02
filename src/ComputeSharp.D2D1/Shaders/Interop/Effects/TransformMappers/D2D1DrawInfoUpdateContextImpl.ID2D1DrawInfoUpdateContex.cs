using System;
using System.Runtime.InteropServices;
using ComputeSharp.Win32;

namespace ComputeSharp.D2D1.Shaders.Interop.Effects.TransformMappers;

/// <inheritdoc/>
partial struct D2D1DrawInfoUpdateContextImpl
{
    /// <summary>
    /// The implementation for <see cref="ID2D1DrawInfoUpdateContext"/>.
    /// </summary>
    private static unsafe class ID2D1DrawInfoUpdateContextMethods
    {
        /// <inheritdoc cref="D2D1DrawInfoUpdateContextImpl.QueryInterface"/>
        [UnmanagedCallersOnly]
        public static int QueryInterface(D2D1DrawInfoUpdateContextImpl* @this, Guid* riid, void** ppvObject)
        {
            return @this->QueryInterface(riid, ppvObject);
        }

        /// <inheritdoc cref="D2D1DrawInfoUpdateContextImpl.AddRef"/>
        [UnmanagedCallersOnly]
        public static uint AddRef(D2D1DrawInfoUpdateContextImpl* @this)
        {
            return @this->AddRef();
        }

        /// <inheritdoc cref="D2D1DrawInfoUpdateContextImpl.Release"/>
        [UnmanagedCallersOnly]
        public static uint Release(D2D1DrawInfoUpdateContextImpl* @this)
        {
            return @this->Release();
        }

        /// <inheritdoc cref="ID2D1DrawInfoUpdateContext.GetConstantBufferSize"/>
        [UnmanagedCallersOnly]
        public static int GetConstantBufferSize(D2D1DrawInfoUpdateContextImpl* @this, uint* size)
        {
            if (size is null)
            {
                return E.E_POINTER;
            }

            if (@this->d2D1DrawInfo.Get() is null)
            {
                return RO.RO_E_CLOSED;
            }

            *size = (uint)@this->constantBufferSize;

            return S.S_OK;
        }

        /// <inheritdoc cref="ID2D1DrawInfoUpdateContext.GetConstantBuffer"/>
        [UnmanagedCallersOnly]
        public static int GetConstantBuffer(D2D1DrawInfoUpdateContextImpl* @this, byte* buffer, uint bufferCount)
        {
            if (buffer is null)
            {
                return E.E_POINTER;
            }

            if (bufferCount < @this->constantBufferSize)
            {
                return E.E_NOT_SUFFICIENT_BUFFER;
            }

            if (@this->constantBuffer is null)
            {
                return E.E_NOT_VALID_STATE;
            }

            if (@this->d2D1DrawInfo.Get() is null)
            {
                return RO.RO_E_CLOSED;
            }

            if (@this->constantBufferSize > 0)
            {
                Buffer.MemoryCopy(@this->constantBuffer, buffer, bufferCount, @this->constantBufferSize);
            }

            return S.S_OK;
        }

        /// <inheritdoc cref="ID2D1DrawInfoUpdateContext.SetConstantBuffer"/>
        [UnmanagedCallersOnly]
        public static int SetConstantBuffer(D2D1DrawInfoUpdateContextImpl* @this, byte* buffer, uint bufferCount)
        {
            if (buffer is null)
            {
                return E.E_POINTER;
            }

            if (bufferCount != (uint)@this->constantBufferSize)
            {
                return E.E_INVALIDARG;
            }

            if (@this->d2D1DrawInfo.Get() is null)
            {
                return RO.RO_E_CLOSED;
            }

            // If the buffer is empty, just do nothing
            if (bufferCount == 0)
            {
                return S.S_OK;
            }

            // Copy the buffer to the backing store, so the effect can also access it later
            Buffer.MemoryCopy(buffer, @this->constantBuffer, bufferCount, bufferCount);

            // Propagate it to the ID2D1DrawInfo object in use as well
            return @this->d2D1DrawInfo.Get()->SetPixelShaderConstantBuffer(buffer, bufferCount);
        }
    }
}