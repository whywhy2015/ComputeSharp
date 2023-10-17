// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from d3d12.h in Microsoft.Direct3D.D3D12 v1.600.10
// Original source is Copyright © Microsoft. Licensed under the MIT license

namespace ComputeSharp.Win32;

internal unsafe partial struct D3D12_AUTO_BREADCRUMB_NODE1
{
    [NativeTypeName("const char *")]
    public sbyte* pCommandListDebugNameA;

    [NativeTypeName("const wchar_t *")]
    public ushort* pCommandListDebugNameW;

    [NativeTypeName("const char *")]
    public sbyte* pCommandQueueDebugNameA;

    [NativeTypeName("const wchar_t *")]
    public ushort* pCommandQueueDebugNameW;
    public ID3D12GraphicsCommandList* pCommandList;
    public ID3D12CommandQueue* pCommandQueue;
    public uint BreadcrumbCount;

    [NativeTypeName("const UINT *")]
    public uint* pLastBreadcrumbValue;

    [NativeTypeName("const D3D12_AUTO_BREADCRUMB_OP *")]
    public D3D12_AUTO_BREADCRUMB_OP* pCommandHistory;

    [NativeTypeName("const struct D3D12_AUTO_BREADCRUMB_NODE1 *")]
    public D3D12_AUTO_BREADCRUMB_NODE1* pNext;
    public uint BreadcrumbContextsCount;
    public D3D12_DRED_BREADCRUMB_CONTEXT* pBreadcrumbContexts;
}