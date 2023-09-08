using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ComputeSharp.D2D1.Extensions;
using TerraFX.Interop.Windows;

namespace ComputeSharp.D2D1.Shaders.Interop.Effects.TransformMappers;

/// <inheritdoc/>
unsafe partial struct D2D1TransformMapperImpl
{
    /// <summary>
    /// The implementation for <see cref="ID2D1TransformMapper"/>.
    /// </summary>
    private static class ID2D1TransformMapperMethods
    {
#if !NET6_0_OR_GREATER
        /// <inheritdoc cref="ID2D1TransformMapper.MapInputRectsToOutputRect"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int MapInputRectsToOutputRectDelegate(
            D2D1TransformMapperImpl* @this,
            ID2D1DrawInfoUpdateContext* d2D1DrawInfoUpdateContext,
            RECT* inputRects,
            RECT* inputOpaqueSubRects,
            uint inputRectCount,
            RECT* outputRect,
            RECT* outputOpaqueSubRect);

        /// <inheritdoc cref="ID2D1TransformMapper.MapOutputRectToInputRects"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int MapOutputRectToInputRectsDelegate(
            D2D1TransformMapperImpl* @this,
            RECT* outputRect,
            RECT* inputRects,
            uint inputRectsCount);

        /// <inheritdoc cref="ID2D1TransformMapper.MapInvalidRect"/>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int MapInvalidRectDelegate(
            D2D1TransformMapperImpl* @this,
            uint inputIndex,
            RECT invalidInputRect,
            RECT* invalidOutputRect);

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
        /// A cached <see cref="MapInputRectsToOutputRectDelegate"/> instance wrapping <see cref="MapInputRectsToOutputRect"/>.
        /// </summary>
        public static readonly MapInputRectsToOutputRectDelegate MapInputRectsToOutputRectWrapper = MapInputRectsToOutputRect;

        /// <summary>
        /// A cached <see cref="MapOutputRectToInputRectsDelegate"/> instance wrapping <see cref="MapOutputRectToInputRects"/>.
        /// </summary>
        public static readonly MapOutputRectToInputRectsDelegate MapOutputRectToInputRectsWrapper = MapOutputRectToInputRects;

        /// <summary>
        /// A cached <see cref="MapInvalidRectDelegate"/> instance wrapping <see cref="MapInvalidRect"/>.
        /// </summary>
        public static readonly MapInvalidRectDelegate MapInvalidRectWrapper = MapInvalidRect;
#endif

        /// <inheritdoc cref="D2D1TransformMapperImpl.QueryInterface"/>
        [UnmanagedCallersOnly]
        public static int QueryInterface(D2D1TransformMapperImpl* @this, Guid* riid, void** ppvObject)
        {
            return @this->QueryInterface(riid, ppvObject);
        }

        /// <inheritdoc cref="D2D1TransformMapperImpl.AddRef"/>
        [UnmanagedCallersOnly]
        public static uint AddRef(D2D1TransformMapperImpl* @this)
        {
            return @this->AddRef();
        }

        /// <inheritdoc cref="D2D1TransformMapperImpl.Release"/>
        [UnmanagedCallersOnly]
        public static uint Release(D2D1TransformMapperImpl* @this)
        {
            return @this->Release();
        }

        /// <inheritdoc cref="ID2D1TransformMapper.MapInputRectsToOutputRect"/>
        [UnmanagedCallersOnly]
        public static int MapInputRectsToOutputRect(
            D2D1TransformMapperImpl* @this,
            ID2D1DrawInfoUpdateContext* d2D1DrawInfoUpdateContext,
            RECT* inputRects,
            RECT* inputOpaqueSubRects,
            uint inputRectCount,
            RECT* outputRect,
            RECT* outputOpaqueSubRect)
        {
            Span<Rectangle> inputs = stackalloc Rectangle[8].Slice(0, (int)inputRectCount);
            Span<Rectangle> opaqueInputs = stackalloc Rectangle[8].Slice(0, (int)inputRectCount);

            for (int i = 0; i < (int)inputRectCount; i++)
            {
                inputs[i] = inputRects[i].ToRectangle();
                opaqueInputs[i] = inputOpaqueSubRects[i].ToRectangle();
            }

            Rectangle output;
            Rectangle opaqueOutput;

            // Invoke MapInputsToOutput and handle exceptions so they don't cross the ABI boundary
            try
            {
                Unsafe.As<ID2D1TransformMapperInterop>(@this->transformMapperHandle.Target!).MapInputsToOutput(
                    d2D1DrawInfoUpdateContext,
                    inputs,
                    opaqueInputs,
                    out output,
                    out opaqueOutput);
            }
            catch (Exception e)
            {
                return e.HResult;
            }

            *outputRect = output.ToRECT();
            *outputOpaqueSubRect = opaqueOutput.ToRECT();

            return S.S_OK;
        }

        /// <inheritdoc cref="ID2D1TransformMapper.MapOutputRectToInputRects"/>
        [UnmanagedCallersOnly]
        public static int MapOutputRectToInputRects(
            D2D1TransformMapperImpl* @this,
            RECT* outputRect,
            RECT* inputRects,
            uint inputRectsCount)
        {
            Rectangle output = outputRect->ToRectangle();
            Span<Rectangle> inputs = stackalloc Rectangle[8].Slice(0, (int)inputRectsCount);

            for (int i = 0; i < (int)inputRectsCount; i++)
            {
                inputs[i] = inputRects[i].ToRectangle();
            }

            // Handle exceptions, as mentioned above
            try
            {
                Unsafe.As<ID2D1TransformMapperInterop>(@this->transformMapperHandle.Target!).MapOutputToInputs(in output, inputs);
            }
            catch (Exception e)
            {
                return e.HResult;
            }

            for (int i = 0; i < (int)inputRectsCount; i++)
            {
                inputRects[i] = inputs[i].ToRECT();
            }

            return S.S_OK;
        }

        /// <inheritdoc cref="ID2D1TransformMapper.MapInvalidRect"/>
        [UnmanagedCallersOnly]
        public static int MapInvalidRect(
            D2D1TransformMapperImpl* @this,
            uint inputIndex,
            RECT invalidInputRect,
            RECT* invalidOutputRect)
        {
            Rectangle invalidInput = invalidInputRect.ToRectangle();
            Rectangle invalidOutput;

            // Handle exceptions once again
            try
            {
                Unsafe.As<ID2D1TransformMapperInterop>(@this->transformMapperHandle.Target!).MapInvalidOutput((int)inputIndex, invalidInput, out invalidOutput);
            }
            catch (Exception e)
            {
                return e.HResult;
            }

            *invalidOutputRect = invalidOutput.ToRECT();

            return S.S_OK;
        }
    }
}