using System;
using System.Runtime.CompilerServices;
using ComputeSharp.D2D1.Descriptors;
using ComputeSharp.Win32;

namespace ComputeSharp.D2D1.Shaders.Loaders;

/// <summary>
/// A data loader for D2D1 pixel shaders dispatched via <see cref="ID2D1DrawInfo"/>.
/// </summary>
internal unsafe struct D2D1ByteBufferConstantBufferLoader : ID2D1ConstantBufferLoader
{
    /// <summary>
    /// The target buffer to write data to.
    /// </summary>
    private readonly byte* buffer;

    /// <summary>
    /// The length of the buffer pointed to by <see cref="buffer"/>.
    /// </summary>
    private readonly int length;

    /// <summary>
    /// The number of written bytes.
    /// </summary>
    private int writtenBytes;

    /// <summary>
    /// Creates a new <see cref="D2D1ByteBufferConstantBufferLoader"/> instance.
    /// </summary>
    /// <param name="buffer">The target buffer to write data to.</param>
    /// <param name="length">The length of the buffer pointed to by <see cref="buffer"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal D2D1ByteBufferConstantBufferLoader(byte* buffer, int length)
    {
        this.buffer = buffer;
        this.length = length;
        this.writtenBytes = -1;
    }

    /// <summary>
    /// Gets the number of written bytes, or <c>0</c> in case of failure.
    /// </summary>
    /// <param name="writtenBytes">The number of written bytes, of <c>0</c> in case of failure.</param>
    /// <returns>Whether or not the constant buffer was retrieved successfully.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool TryGetWrittenBytes(out int writtenBytes)
    {
        // If the -1 marker is present, it means copying to the target span failed.
        // In this case, the returned number of written bytes is still reported as 0.
        // But, the actual load operation is marked as failed. This allows the data loader
        // to both handle calls with a destination too short and with an empty source buffer.
        if (this.writtenBytes == -1)
        {
            writtenBytes = 0;

            return false;
        }

        writtenBytes = this.writtenBytes;

        return true;
    }

    /// <inheritdoc/>
    void ID2D1ConstantBufferLoader.LoadConstantBuffer(ReadOnlySpan<byte> data)
    {
        if (data.TryCopyTo(new Span<byte>(this.buffer, this.length)))
        {
            this.writtenBytes = data.Length;
        }
    }
}