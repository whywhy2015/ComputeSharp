// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um/d3d12.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace ComputeSharp.Win32;

internal enum D3D12_CPU_PAGE_PROPERTY
{
    D3D12_CPU_PAGE_PROPERTY_UNKNOWN = 0,
    D3D12_CPU_PAGE_PROPERTY_NOT_AVAILABLE = 1,
    D3D12_CPU_PAGE_PROPERTY_WRITE_COMBINE = 2,
    D3D12_CPU_PAGE_PROPERTY_WRITE_BACK = 3,
}