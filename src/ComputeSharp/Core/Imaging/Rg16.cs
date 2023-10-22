using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable IDE0290

namespace ComputeSharp;

/// <summary>
/// Packed pixel type containing two 8-bit unsigned normalized values ranging from 0 to 255.
/// <para>
/// Ranges from [0, 0, 0, 0] to [1, 1, 0, 0] in vector form.
/// </para>
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Rg16 : IEquatable<Rg16>, IPixel<Rg16, Float2>, ISpanFormattable
{
    /// <summary>
    /// The red component.
    /// </summary>
    public byte R;

    /// <summary>
    /// The green component.
    /// </summary>
    public byte G;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rg16"/> struct.
    /// </summary>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    public Rg16(byte r, byte g)
    {
        this.R = r;
        this.G = g;
    }

    /// <summary>
    /// Gets or sets the packed representation of the <see cref="Rg16"/> struct.
    /// </summary>
    public ushort PackedValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => Unsafe.As<Rg16, ushort>(ref Unsafe.AsRef(in this));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Unsafe.As<Rg16, ushort>(ref this) = value;
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Float2 ToPixel()
    {
        return new(this.R / (float)byte.MaxValue, this.G / (float)byte.MaxValue);
    }

    /// <summary>
    /// Compares two <see cref="Rg16"/> objects for equality.
    /// </summary>
    /// <param name="left">The <see cref="Rg16"/> on the left side of the operand.</param>
    /// <param name="right">The <see cref="Rg16"/> on the right side of the operand.</param>
    /// <returns>
    /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rg16 left, Rg16 right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="Rg16"/> objects for equality.
    /// </summary>
    /// <param name="left">The <see cref="Rg16"/> on the left side of the operand.</param>
    /// <param name="right">The <see cref="Rg16"/> on the right side of the operand.</param>
    /// <returns>
    /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rg16 left, Rg16 right) => !left.Equals(right);

    /// <inheritdoc/>
    public override readonly bool Equals(object? obj)
    {
        return obj is Rg16 rgba32 && Equals(rgba32);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Rg16 other)
    {
        return PackedValue.Equals(other.PackedValue);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode()
    {
        return PackedValue.GetHashCode();
    }

    /// <inheritdoc/>
    public override readonly string ToString()
    {
        return $"{nameof(Rg16)}({this.R}, {this.G})";
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(formatProvider, $"{nameof(Rg16)}({this.R}, {this.G})");
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return destination.TryWrite(provider, $"{nameof(Rg16)}({this.R}, {this.G})", out charsWritten);
    }
}