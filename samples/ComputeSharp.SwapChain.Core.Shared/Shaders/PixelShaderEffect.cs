using System;
using ComputeSharp.D2D1;
#if WINDOWS_UWP
using ComputeSharp.D2D1.Uwp;
#else
using ComputeSharp.D2D1.WinUI;
#endif
using Microsoft.Graphics.Canvas;

#nullable enable

namespace ComputeSharp.SwapChain.Core.Shaders;

/// <summary>
/// An base effect for an animated pixel shader.
/// </summary>
public abstract class PixelShaderEffect : CanvasEffect
{
    /// <summary>
    /// The current elapsed time.
    /// </summary>
    private TimeSpan elapsedTime;

    /// <summary>
    /// The current screen width in raw pixels.
    /// </summary>
    private int screenWidth;

    /// <summary>
    /// The current screen height in raw pixels.
    /// </summary>
    private int screenHeight;

    /// <summary>
    /// Gets or sets the total elapsed time.
    /// </summary>
    public TimeSpan ElapsedTime
    {
        get => this.elapsedTime;
        set => SetAndInvalidateEffectGraph(ref this.elapsedTime, value);
    }

    /// <summary>
    /// Gets or sets the screen width in raw pixels.
    /// </summary>
    public int ScreenWidth
    {
        get => this.screenWidth;
        set => SetAndInvalidateEffectGraph(ref this.screenWidth, value);
    }

    /// <summary>
    /// Gets or sets the screen height in raw pixels.
    /// </summary>
    public int ScreenHeight
    {
        get => this.screenHeight;
        set => SetAndInvalidateEffectGraph(ref this.screenHeight, value);
    }

    /// <summary>
    /// An effect for an animated pixel shader.
    /// </summary>
    /// <typeparam name="T">The type of pixel shader to render.</typeparam>
    public sealed class For<T> : PixelShaderEffect
        where T : unmanaged, ID2D1PixelShader
    {
        /// <summary>
        /// The <typeparamref name="T"/> factory to use.
        /// </summary>
        private readonly Factory factory;

        /// <summary>
        /// The <see cref="PixelShaderEffect{T}"/> instance in use.
        /// </summary>
        private PixelShaderEffect<T>? effect;

        /// <summary>
        /// Creates a new <see cref="For{T}"/> instance with the specified parameters.
        /// </summary>
        /// <param name="factory">The input <typeparamref name="T"/> factory.</param>
        public For(Factory factory)
        {
            this.factory = factory;
        }

        /// <inheritdoc/>
        protected override ICanvasImage BuildEffectGraph()
        {
            this.effect = new PixelShaderEffect<T>();

            return this.effect;
        }

        /// <inheritdoc/>
        protected override void ConfigureEffectGraph()
        {
            this.effect!.ConstantBuffer = this.factory(ElapsedTime, ScreenWidth, ScreenHeight);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.effect?.Dispose();
            }
        }

        /// <summary>
        /// A factory of a given shader instance.
        /// </summary>
        /// <param name="elapsedTime">The total elapsed time.</param>
        /// <param name="screenWidth">The screen width in raw pixels.</param>
        /// <param name="screenHeight">The screen height in raw pixels.</param>
        /// <returns>A shader instance to render.</returns>
        public delegate T Factory(TimeSpan elapsedTime, int screenWidth, int screenHeight);
    }
}
