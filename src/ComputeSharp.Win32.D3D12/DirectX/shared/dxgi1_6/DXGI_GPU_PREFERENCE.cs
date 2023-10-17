// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared/dxgi1_6.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace ComputeSharp.Win32;

internal enum DXGI_GPU_PREFERENCE
{
    DXGI_GPU_PREFERENCE_UNSPECIFIED = 0,
    DXGI_GPU_PREFERENCE_MINIMUM_POWER = (DXGI_GPU_PREFERENCE_UNSPECIFIED + 1),
    DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE = (DXGI_GPU_PREFERENCE_MINIMUM_POWER + 1),
}