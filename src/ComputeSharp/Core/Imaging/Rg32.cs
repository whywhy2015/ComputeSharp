using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable IDE0290

namespace ComputeSharp;

/// <summary>
/// Packed pixel type containing two 16-bit unsigned normalized values ranging from 0 to 255.
/// <para>
/// Ranges from [0, 0, 0, 0] to [1, 1, 0, 0] in vector form.
/// </para>
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Rg32 : IEquatable<Rg32>, IPixel<Rg32, Float2>, ISpanFormattable
{
    /// <summary>
    /// The red component.
    /// </summary>
    public ushort R;

    /// <summary>
    /// The green component.
    /// </summary>
    public ushort G;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rg32"/> struct.
    /// </summary>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    public Rg32(ushort r, ushort g)
    {
        this.R = r;
        this.G = g;
    }

    /// <summary>
    /// Gets or sets the packed representation of the <see cref="Rg32"/> struct.
    /// </summary>
    public uint PackedValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => Unsafe.As<Rg32, uint>(ref Unsafe.AsRef(in this));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Unsafe.As<Rg32, uint>(ref this) = value;
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Float2 ToPixel()
    {
        return new(this.R / (float)ushort.MaxValue, this.G / (float)ushort.MaxValue);
    }

    /// <summary>
    /// Compares two <see cref="Rg32"/> objects for equality.
    /// </summary>
    /// <param name="left">The <see cref="Rg32"/> on the left side of the operand.</param>
    /// <param name="right">The <see cref="Rg32"/> on the right side of the operand.</param>
    /// <returns>
    /// True if the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter; otherwise, false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rg32 left, Rg32 right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="Rg32"/> objects for equality.
    /// </summary>
    /// <param name="left">The <see cref="Rg32"/> on the left side of the operand.</param>
    /// <param name="right">The <see cref="Rg32"/> on the right side of the operand.</param>
    /// <returns>
    /// True if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter; otherwise, false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rg32 left, Rg32 right) => !left.Equals(right);

    /// <inheritdoc/>
    public override readonly bool Equals(object? obj)
    {
        return obj is Rg32 rgba32 && Equals(rgba32);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Rg32 other)
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
        return $"{nameof(Rg32)}({this.R}, {this.G})";
    }

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(formatProvider, $"{nameof(Rg32)}({this.R}, {this.G})");
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return destination.TryWrite(provider, $"{nameof(Rg32)}({this.R}, {this.G})", out charsWritten);
    }
}