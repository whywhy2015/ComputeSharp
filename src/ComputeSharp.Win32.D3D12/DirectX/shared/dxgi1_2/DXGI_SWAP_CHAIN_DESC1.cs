// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared/dxgi1_2.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Interop.Windows;

namespace ComputeSharp.Win32;

internal partial struct DXGI_SWAP_CHAIN_DESC1
{
    public uint Width;

    public uint Height;

    public DXGI_FORMAT Format;

    public BOOL Stereo;

    public DXGI_SAMPLE_DESC SampleDesc;

    [NativeTypeName("DXGI_USAGE")]
    public uint BufferUsage;

    public uint BufferCount;

    public DXGI_SCALING Scaling;

    public DXGI_SWAP_EFFECT SwapEffect;

    public DXGI_ALPHA_MODE AlphaMode;

    public uint Flags;
}