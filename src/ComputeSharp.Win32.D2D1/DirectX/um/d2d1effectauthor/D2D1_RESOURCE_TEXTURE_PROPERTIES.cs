// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um/d2d1effectauthor.h in the Windows SDK for Windows 10.0.22000.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace ComputeSharp.Win32;

internal unsafe partial struct D2D1_RESOURCE_TEXTURE_PROPERTIES
{
    [NativeTypeName("const UINT32 *")]
    public uint* extents;

    [NativeTypeName("UINT32")]
    public uint dimensions;

    public D2D1_BUFFER_PRECISION bufferPrecision;

    public D2D1_CHANNEL_DEPTH channelDepth;

    public D2D1_FILTER filter;

    [NativeTypeName("const D2D1_EXTEND_MODE *")]
    public D2D1_EXTEND_MODE* extendModes;
}