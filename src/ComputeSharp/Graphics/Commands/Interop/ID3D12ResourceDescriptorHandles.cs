using ComputeSharp.Win32;

namespace ComputeSharp.Graphics.Commands.Interop;

/// <summary>
/// A type representing a bundle of reusable resource descriptor handles.
/// </summary>
/// <param name="d3D12CpuDescriptorHandle">The <see cref="D3D12_CPU_DESCRIPTOR_HANDLE"/> value to wrap.</param>
/// <param name="d3D12GpuDescriptorHandle">The <see cref="D3D12_GPU_DESCRIPTOR_HANDLE"/> value to wrap.</param>
/// <param name="d3D12CpuDescriptorHandleNonShaderVisible">The non shader visible <see cref="D3D12_CPU_DESCRIPTOR_HANDLE"/> value to wrap.</param>
internal readonly struct ID3D12ResourceDescriptorHandles(
    D3D12_CPU_DESCRIPTOR_HANDLE d3D12CpuDescriptorHandle,
    D3D12_GPU_DESCRIPTOR_HANDLE d3D12GpuDescriptorHandle,
    D3D12_CPU_DESCRIPTOR_HANDLE d3D12CpuDescriptorHandleNonShaderVisible)
{
    /// <summary>
    /// The <see cref="D3D12_GPU_DESCRIPTOR_HANDLE"/> value for the current entry.
    /// </summary>
    public readonly D3D12_CPU_DESCRIPTOR_HANDLE D3D12CpuDescriptorHandle = d3D12CpuDescriptorHandle;

    /// <summary>
    /// The <see cref="D3D12_GPU_DESCRIPTOR_HANDLE"/> value for the current entry.
    /// </summary>
    public readonly D3D12_GPU_DESCRIPTOR_HANDLE D3D12GpuDescriptorHandle = d3D12GpuDescriptorHandle;

    /// <summary>
    /// The <see cref="D3D12_CPU_DESCRIPTOR_HANDLE"/> value for the current entry, non shader visible.
    /// </summary>
    public readonly D3D12_CPU_DESCRIPTOR_HANDLE D3D12CpuDescriptorHandleNonShaderVisible = d3D12CpuDescriptorHandleNonShaderVisible;
}