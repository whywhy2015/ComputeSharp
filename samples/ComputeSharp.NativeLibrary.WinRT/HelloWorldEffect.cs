using ComputeSharp.D2D1;
using ComputeSharp.D2D1.WinUI;
using Windows.Foundation;

namespace ComputeSharp.NativeLibrary;

/// <summary>
/// A hello world effect that displays a color gradient.
/// </summary>
public sealed partial class HelloWorldEffect : CanvasEffect
{
    /// <summary>
    /// The <see cref="PixelShaderEffect{T}"/> node in use.
    /// </summary>
    private static readonly EffectNode<PixelShaderEffect<Shader>> EffectNode = new();

    /// <inheritdoc cref="Shader.time"/>
    private float time;

    /// <inheritdoc cref="Shader.dispatchSize"/>
    private Rect dispatchArea;

    /// <summary>
    /// Gets or sets the current time since the start of the application.
    /// </summary>
    public float Time
    {
        get => this.time;
        set => SetAndInvalidateEffectGraph(ref this.time, value);
    }

    /// <summary>
    /// Gets or sets the dispatch area for the current output.
    /// </summary>
    public Rect DispatchArea
    {
        get => this.dispatchArea;
        set => SetAndInvalidateEffectGraph(ref this.dispatchArea, value);
    }

    /// <inheritdoc/>
    protected override void BuildEffectGraph(EffectGraph effectGraph)
    {
        effectGraph.RegisterOutputNode(EffectNode, new PixelShaderEffect<Shader>());
    }

    /// <inheritdoc/>
    protected override void ConfigureEffectGraph(EffectGraph effectGraph)
    {
        effectGraph.GetNode(EffectNode).ConstantBuffer = new Shader(
            time: this.time,
            dispatchSize: new int2(
                x: (int)double.Round(this.dispatchArea.Width),
                y: (int)double.Round(this.dispatchArea.Height)));
    }

    /// <summary>
    /// The D2D1 shader for <see cref="HelloWorldEffect"/>.
    /// </summary>
    [D2DEffectDisplayName(nameof(HelloWorldEffect))]
    [D2DEffectDescription("A hello world effect that displays a color gradient.")]
    [D2DEffectCategory("Render")]
    [D2DEffectAuthor("ComputeSharp.D2D1")]
    [D2DInputCount(0)]
    [D2DRequiresScenePosition]
    [D2DShaderProfile(D2D1ShaderProfile.PixelShader50)]
    [D2DGeneratedPixelShaderDescriptor]
    [AutoConstructor]
    internal readonly partial struct Shader : ID2D1PixelShader
    {
        /// <summary>
        /// The current time since the start of the application.
        /// </summary>
        private readonly float time;

        /// <summary>
        /// The dispatch size for the current output.
        /// </summary>
        private readonly int2 dispatchSize;

        /// <inheritdoc/>
        public float4 Execute()
        {
            int2 xy = (int2)D2D.GetScenePosition().XY;
            float2 uv = xy / (float2)this.dispatchSize;
            float3 color = 0.5f + (0.5f * Hlsl.Cos(this.time + new float3(uv, uv.X) + new float3(0, 2, 4)));

            return new(color, 1f);
        }
    }
}
