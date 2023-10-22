using System;

namespace ComputeSharp.D2D1;

/// <summary>
/// An attribute for a D2D1 shader indicating the id of the shader effect to create.
/// </summary>
/// <remarks>
/// This only applies to effects created from <see cref="Interop.D2D1PixelShaderEffect"/>.
/// </remarks>
/// <param name="value">The value for the effect id.</param>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
public sealed class D2DEffectIdAttribute(string value) : Attribute
{
    /// <summary>
    /// Gets the effect id value.
    /// </summary>
    public Guid Value { get; } = Guid.Parse(value);
}