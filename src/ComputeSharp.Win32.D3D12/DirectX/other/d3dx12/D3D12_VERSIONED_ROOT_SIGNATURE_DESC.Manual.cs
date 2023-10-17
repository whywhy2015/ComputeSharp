// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from d3dx12.h in DirectX-Graphics-Samples commit a7a87f1853b5540f10920518021d91ae641033fb
// Original source is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using static TerraFX.Interop.DirectX.D3D_ROOT_SIGNATURE_VERSION;
using static TerraFX.Interop.DirectX.D3D12_ROOT_SIGNATURE_FLAGS;

namespace ComputeSharp.Win32;

internal unsafe partial struct D3D12_VERSIONED_ROOT_SIGNATURE_DESC
{
    public static void Init_1_0([NativeTypeName("D3D12_VERSIONED_ROOT_SIGNATURE_DESC &")] out D3D12_VERSIONED_ROOT_SIGNATURE_DESC desc, uint numParameters, [NativeTypeName("const D3D12_ROOT_PARAMETER *")] D3D12_ROOT_PARAMETER* _pParameters, uint numStaticSamplers = 0, [NativeTypeName("const D3D12_STATIC_SAMPLER_DESC *")] D3D12_STATIC_SAMPLER_DESC* _pStaticSamplers = null, D3D12_ROOT_SIGNATURE_FLAGS flags = D3D12_ROOT_SIGNATURE_FLAG_NONE)
    {
        desc = default;

        desc.Version = D3D_ROOT_SIGNATURE_VERSION_1_0;
        desc.Anonymous.Desc_1_0.NumParameters = numParameters;
        desc.Anonymous.Desc_1_0.pParameters = _pParameters;
        desc.Anonymous.Desc_1_0.NumStaticSamplers = numStaticSamplers;
        desc.Anonymous.Desc_1_0.pStaticSamplers = _pStaticSamplers;
        desc.Anonymous.Desc_1_0.Flags = flags;
    }

    public static void Init_1_1([NativeTypeName("D3D12_VERSIONED_ROOT_SIGNATURE_DESC &")] out D3D12_VERSIONED_ROOT_SIGNATURE_DESC desc, uint numParameters, [NativeTypeName("const D3D12_ROOT_PARAMETER1 *")] D3D12_ROOT_PARAMETER1* _pParameters, uint numStaticSamplers = 0, [NativeTypeName("const D3D12_STATIC_SAMPLER_DESC *")] D3D12_STATIC_SAMPLER_DESC* _pStaticSamplers = null, D3D12_ROOT_SIGNATURE_FLAGS flags = D3D12_ROOT_SIGNATURE_FLAG_NONE)
    {
        desc = default;

        desc.Version = D3D_ROOT_SIGNATURE_VERSION_1_1;
        desc.Anonymous.Desc_1_1.NumParameters = numParameters;
        desc.Anonymous.Desc_1_1.pParameters = _pParameters;
        desc.Anonymous.Desc_1_1.NumStaticSamplers = numStaticSamplers;
        desc.Anonymous.Desc_1_1.pStaticSamplers = _pStaticSamplers;
        desc.Anonymous.Desc_1_1.Flags = flags;
    }
}