using System;
using ComputeSharp.Win32;

namespace ComputeSharp.Graphics.Extensions;

/// <summary>
/// A <see langword="class"/> with extensions for the <see cref="ID3D12GraphicsCommandList"/> type.
/// </summary>
internal static unsafe class ID3D12GraphicsCommandListExtensions
{
    /// <summary>
    /// Copies a texture memory region from one resource (a texture) to another (a buffer).
    /// </summary>
    /// <param name="d3D12GraphicsCommandList">The <see cref="ID3D12GraphicsCommandList"/> instance in use.</param>
    /// <param name="d3D12ResourceDestination">The destination <see cref="ID3D12Resource"/> (a buffer) to write to.</param>
    /// <param name="d3D12PlacedSubresourceFootprintDestination">The <see cref="D3D12_PLACED_SUBRESOURCE_FOOTPRINT"/> value describing <paramref name="d3D12ResourceDestination"/>.</param>
    /// <param name="destinationX">The horizontal offset in the destination buffer.</param>
    /// <param name="destinationY">The vertical offset in the destination buffer.</param>
    /// <param name="destinationZ">The depthwise offset in the destination buffer.</param>
    /// <param name="d3D12ResourceSource">The source <see cref="ID3D12Resource"/> (a texture) to read from.</param>
    /// <param name="sourceX">The horizontal offset in the source texture.</param>
    /// <param name="sourceY">The vertical offset in the source texture.</param>
    /// <param name="sourceZ">The depthwise offset in the source texture.</param>
    /// <param name="width">The width of the memory area to read from.</param>
    /// <param name="height">The height of the memory area to read from.</param>
    /// <param name="depth">The depth of the memory area to read from.</param>
    public static void CopyTextureRegion(
        this ref ID3D12GraphicsCommandList d3D12GraphicsCommandList,
        ID3D12Resource* d3D12ResourceDestination,
        D3D12_PLACED_SUBRESOURCE_FOOTPRINT* d3D12PlacedSubresourceFootprintDestination,
        uint destinationX,
        uint destinationY,
        ushort destinationZ,
        ID3D12Resource* d3D12ResourceSource,
        uint sourceX,
        uint sourceY,
        ushort sourceZ,
        uint width,
        uint height,
        ushort depth)
    {
        D3D12_RESOURCE_DESC d3D12ResourceDescriptionSource = d3D12ResourceSource->GetDesc();
        D3D12_TEXTURE_COPY_LOCATION d3D12TextureCopyLocationDestination = new(d3D12ResourceDestination, in *d3D12PlacedSubresourceFootprintDestination);
        D3D12_TEXTURE_COPY_LOCATION d3D12TextureCopyLocationSource = new(d3D12ResourceSource, 0);
        D3D12_BOX d3D12Box;
        D3D12_BOX* pD3D12Box;

        if (sourceX == 0 &&
            sourceY == 0 &&
            sourceZ == 0 &&
            width == d3D12ResourceDescriptionSource.Width &&
            height == d3D12ResourceDescriptionSource.Height &&
            depth == d3D12ResourceDescriptionSource.Depth)
        {
            pD3D12Box = null;
        }
        else
        {
            d3D12Box = new D3D12_BOX((int)sourceX, (int)sourceY, sourceZ, (int)(sourceX + width), (int)(sourceY + height), sourceZ + depth);
            pD3D12Box = &d3D12Box;
        }

        d3D12GraphicsCommandList.CopyTextureRegion(
            &d3D12TextureCopyLocationDestination,
            destinationX,
            destinationY,
            destinationZ,
            &d3D12TextureCopyLocationSource,
            pD3D12Box);
    }

    /// <summary>
    /// Copies a texture memory region from one resource (a buffer) to another (a texture).
    /// </summary>
    /// <param name="d3D12GraphicsCommandList">The <see cref="ID3D12GraphicsCommandList"/> instance in use.</param>
    /// <param name="d3D12ResourceDestination">The destination <see cref="ID3D12Resource"/> (a texture) to write to.</param>
    /// <param name="destinationX">The horizontal offset in the destination texture.</param>
    /// <param name="destinationY">The vertical offset in the destination texture.</param>
    /// <param name="destinationZ">The depthwise offset in the destination texture.</param>
    /// <param name="d3D12ResourceSource">The source <see cref="ID3D12Resource"/> (a buffer) to read from.</param>
    /// <param name="d3D12PlacedSubresourceFootprintSource">The <see cref="D3D12_PLACED_SUBRESOURCE_FOOTPRINT"/> value describing <paramref name="d3D12ResourceSource"/>.</param>
    /// <param name="sourceX">The horizontal offset in the source buffer.</param>
    /// <param name="sourceY">The vertical offset in the source buffer.</param>
    /// <param name="sourceZ">The depthwise offset in the source buffer.</param>
    /// <param name="width">The width of the memory area to read from.</param>
    /// <param name="height">The height of the memory area to read from.</param>
    /// <param name="depth">The depth of the memory area to read from.</param>
    public static void CopyTextureRegion(
        this ref ID3D12GraphicsCommandList d3D12GraphicsCommandList,
        ID3D12Resource* d3D12ResourceDestination,
        uint destinationX,
        uint destinationY,
        ushort destinationZ,
        ID3D12Resource* d3D12ResourceSource,
        D3D12_PLACED_SUBRESOURCE_FOOTPRINT* d3D12PlacedSubresourceFootprintSource,
        uint sourceX,
        uint sourceY,
        ushort sourceZ,
        uint width,
        uint height,
        ushort depth)
    {
        D3D12_TEXTURE_COPY_LOCATION d3D12TextureCopyLocationDestination = new(d3D12ResourceDestination, 0);
        D3D12_TEXTURE_COPY_LOCATION d3D12TextureCopyLocationSource = new(d3D12ResourceSource, in *d3D12PlacedSubresourceFootprintSource);
        D3D12_BOX d3D12Box;
        D3D12_BOX* pD3D12Box;

        if (sourceX == 0 &&
            sourceY == 0 &&
            sourceZ == 0 &&
            width == d3D12PlacedSubresourceFootprintSource->Footprint.Width &&
            height == d3D12PlacedSubresourceFootprintSource->Footprint.Height &&
            depth == d3D12PlacedSubresourceFootprintSource->Footprint.Depth)
        {
            pD3D12Box = null;
        }
        else
        {
            d3D12Box = new D3D12_BOX((int)sourceX, (int)sourceY, sourceZ, (int)(sourceX + width), (int)(sourceY + height), sourceZ + depth);
            pD3D12Box = &d3D12Box;
        }

        d3D12GraphicsCommandList.CopyTextureRegion(
            &d3D12TextureCopyLocationDestination,
            destinationX,
            destinationY,
            destinationZ,
            &d3D12TextureCopyLocationSource,
            pD3D12Box);
    }

    /// <summary>
    /// Copies a texture memory region from one texture to another.
    /// </summary>
    /// <param name="d3D12GraphicsCommandList">The <see cref="ID3D12GraphicsCommandList"/> instance in use.</param>
    /// <param name="d3D12ResourceDestination">The destination <see cref="ID3D12Resource"/> (a buffer) to write to.</param>
    /// <param name="destinationX">The horizontal offset in the destination buffer.</param>
    /// <param name="destinationY">The vertical offset in the destination buffer.</param>
    /// <param name="destinationZ">The depthwise offset in the destination buffer.</param>
    /// <param name="d3D12ResourceSource">The source <see cref="ID3D12Resource"/> (a texture) to read from.</param>
    /// <param name="sourceX">The horizontal offset in the source texture.</param>
    /// <param name="sourceY">The vertical offset in the source texture.</param>
    /// <param name="sourceZ">The depthwise offset in the source texture.</param>
    /// <param name="width">The width of the memory area to read from.</param>
    /// <param name="height">The height of the memory area to read from.</param>
    /// <param name="depth">The depth of the memory area to read from.</param>
    public static void CopyTextureRegion(
        this ref ID3D12GraphicsCommandList d3D12GraphicsCommandList,
        ID3D12Resource* d3D12ResourceDestination,
        uint destinationX,
        uint destinationY,
        ushort destinationZ,
        ID3D12Resource* d3D12ResourceSource,
        uint sourceX,
        uint sourceY,
        ushort sourceZ,
        uint width,
        uint height,
        ushort depth)
    {
        D3D12_RESOURCE_DESC d3D12ResourceDescriptionSource = d3D12ResourceSource->GetDesc();
        D3D12_TEXTURE_COPY_LOCATION d3D12TextureCopyLocationDestination = new(d3D12ResourceDestination, 0);
        D3D12_TEXTURE_COPY_LOCATION d3D12TextureCopyLocationSource = new(d3D12ResourceSource, 0);
        D3D12_BOX d3D12Box;
        D3D12_BOX* pD3D12Box;

        if (sourceX == 0 &&
            sourceY == 0 &&
            sourceZ == 0 &&
            width == d3D12ResourceDescriptionSource.Width &&
            height == d3D12ResourceDescriptionSource.Height &&
            depth == d3D12ResourceDescriptionSource.Depth)
        {
            pD3D12Box = null;
        }
        else
        {
            d3D12Box = new D3D12_BOX((int)sourceX, (int)sourceY, sourceZ, (int)(sourceX + width), (int)(sourceY + height), sourceZ + depth);
            pD3D12Box = &d3D12Box;
        }

        d3D12GraphicsCommandList.CopyTextureRegion(
            &d3D12TextureCopyLocationDestination,
            destinationX,
            destinationY,
            destinationZ,
            &d3D12TextureCopyLocationSource,
            pD3D12Box);
    }

    /// <summary>
    /// Creates a resource barrier to transition a resource to a specific state.
    /// </summary>
    /// <param name="d3D12GraphicsCommandList">The <see cref="ID3D12GraphicsCommandList"/> instance in use.</param>
    /// <param name="d3D12Resource">The <see cref="ID3D12Resource"/> to change state for.</param>
    /// <param name="d3D12ResourceStatesBefore">The starting <see cref="D3D12_RESOURCE_STATES"/> value for the transition.</param>
    /// <param name="d3D12ResourceStatesAfter">The destnation <see cref="D3D12_RESOURCE_STATES"/> value for the transition.</param>
    public static void TransitionBarrier(
        this ref ID3D12GraphicsCommandList d3D12GraphicsCommandList,
        ID3D12Resource* d3D12Resource,
        D3D12_RESOURCE_STATES d3D12ResourceStatesBefore,
        D3D12_RESOURCE_STATES d3D12ResourceStatesAfter)
    {
        D3D12_RESOURCE_BARRIER d3D12ResourceBarrier = D3D12_RESOURCE_BARRIER.InitTransition(d3D12Resource, d3D12ResourceStatesBefore, d3D12ResourceStatesAfter);

        d3D12GraphicsCommandList.ResourceBarrier(1, &d3D12ResourceBarrier);
    }

    /// <summary>
    /// Inserts a UAV resource barrier for a specific resource.
    /// </summary>
    /// <param name="d3D12GraphicsCommandList">The <see cref="ID3D12GraphicsCommandList"/> instance in use.</param>
    /// <param name="d3D12Resource">The <see cref="ID3D12Resource"/> to insert the barrier for.</param>
    public static void UnorderedAccessViewBarrier(this ref ID3D12GraphicsCommandList d3D12GraphicsCommandList, ID3D12Resource* d3D12Resource)
    {
        D3D12_RESOURCE_BARRIER d3D12ResourceBarrier = D3D12_RESOURCE_BARRIER.InitUAV(d3D12Resource);

        d3D12GraphicsCommandList.ResourceBarrier(1, &d3D12ResourceBarrier);
    }

    /// <summary>
    /// Clears a target UAV resource.
    /// </summary>
    /// <param name="d3D12GraphicsCommandList">The <see cref="ID3D12GraphicsCommandList"/> instance in use.</param>
    /// <param name="d3D12Resource">The <see cref="ID3D12Resource"/> to clear.</param>
    /// <param name="d3D12GpuDescriptorHandle">The <see cref="D3D12_GPU_DESCRIPTOR_HANDLE"/> value for the target resource.</param>
    /// <param name="d3D12CpuDescriptorHandle">The <see cref="D3D12_CPU_DESCRIPTOR_HANDLE"/> value for the target resource.</param>
    /// <param name="isNormalized">Indicates whether the target resource uses a normalized format.</param>
    public static unsafe void ClearUnorderedAccessView(
        this ref ID3D12GraphicsCommandList d3D12GraphicsCommandList,
        ID3D12Resource* d3D12Resource,
        D3D12_GPU_DESCRIPTOR_HANDLE d3D12GpuDescriptorHandle,
        D3D12_CPU_DESCRIPTOR_HANDLE d3D12CpuDescriptorHandle,
        bool isNormalized)
    {
        if (isNormalized)
        {
            Float4 values = default;

            d3D12GraphicsCommandList.ClearUnorderedAccessViewFloat(
                ViewGPUHandleInCurrentHeap: d3D12GpuDescriptorHandle,
                ViewCPUHandle: d3D12CpuDescriptorHandle,
                pResource: d3D12Resource,
                Values: (float*)&values,
                NumRects: 0,
                pRects: null);
        }
        else
        {
            UInt4 values = default;

            d3D12GraphicsCommandList.ClearUnorderedAccessViewUint(
                ViewGPUHandleInCurrentHeap: d3D12GpuDescriptorHandle,
                ViewCPUHandle: d3D12CpuDescriptorHandle,
                pResource: d3D12Resource,
                Values: (uint*)&values,
                NumRects: 0,
                pRects: null);
        }
    }

    /// <summary>
    /// Fills a target UAV resource.
    /// </summary>
    /// <param name="d3D12GraphicsCommandList">The <see cref="ID3D12GraphicsCommandList"/> instance in use.</param>
    /// <param name="d3D12Resource">The <see cref="ID3D12Resource"/> to fill.</param>
    /// <param name="d3D12GpuDescriptorHandle">The <see cref="D3D12_GPU_DESCRIPTOR_HANDLE"/> value for the target resource.</param>
    /// <param name="d3D12CpuDescriptorHandle">The <see cref="D3D12_CPU_DESCRIPTOR_HANDLE"/> value for the target resource.</param>
    /// <param name="value">The value to use to fill the view.</param>
    public static unsafe void FillUnorderedAccessView(
        this ref ID3D12GraphicsCommandList d3D12GraphicsCommandList,
        ID3D12Resource* d3D12Resource,
        D3D12_GPU_DESCRIPTOR_HANDLE d3D12GpuDescriptorHandle,
        D3D12_CPU_DESCRIPTOR_HANDLE d3D12CpuDescriptorHandle,
        Float4 value)
    {
        d3D12GraphicsCommandList.ClearUnorderedAccessViewFloat(
            ViewGPUHandleInCurrentHeap: d3D12GpuDescriptorHandle,
            ViewCPUHandle: d3D12CpuDescriptorHandle,
            pResource: d3D12Resource,
            Values: (float*)&value,
            NumRects: 0,
            pRects: null);
    }

    /// <summary>
    /// Binds an input constant buffer to the first root parameter.
    /// </summary>
    /// <param name="d3D12GraphicsCommandList">The <see cref="ID3D12GraphicsCommandList"/> instance in use.</param>
    /// <param name="data">The input buffer with the constant data to bind.</param>
    public static void SetComputeRoot32BitConstants(this ref ID3D12GraphicsCommandList d3D12GraphicsCommandList, ReadOnlySpan<byte> data)
    {
        fixed (byte* p = data)
        {
            d3D12GraphicsCommandList.SetComputeRoot32BitConstants(0, (uint)data.Length / sizeof(uint), p, 0);
        }
    }
}