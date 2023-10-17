// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um/d3d12.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Interop.Windows;

namespace ComputeSharp.Win32;

internal partial struct D3D12_FEATURE_DATA_ARCHITECTURE1
{
    public uint NodeIndex;

    public BOOL TileBasedRenderer;

    public BOOL UMA;

    public BOOL CacheCoherentUMA;

    public BOOL IsolatedMMU;
}