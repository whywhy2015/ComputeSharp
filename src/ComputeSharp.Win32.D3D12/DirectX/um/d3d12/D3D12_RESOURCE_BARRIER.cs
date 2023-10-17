// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um/d3d12.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace ComputeSharp.Win32;

internal partial struct D3D12_RESOURCE_BARRIER
{
    public D3D12_RESOURCE_BARRIER_TYPE Type;

    public D3D12_RESOURCE_BARRIER_FLAGS Flags;

    [NativeTypeName("D3D12_RESOURCE_BARRIER::(anonymous union at C:/Program Files (x86)/Windows Kits/10/Include/10.0.20348.0/um/d3d12.h:2883:5)")]
    public _Anonymous_e__Union Anonymous;

    [StructLayout(LayoutKind.Explicit)]
    internal partial struct _Anonymous_e__Union
    {
        [FieldOffset(0)]
        public D3D12_RESOURCE_TRANSITION_BARRIER Transition;

        [FieldOffset(0)]
        public D3D12_RESOURCE_ALIASING_BARRIER Aliasing;

        [FieldOffset(0)]
        public D3D12_RESOURCE_UAV_BARRIER UAV;
    }
}