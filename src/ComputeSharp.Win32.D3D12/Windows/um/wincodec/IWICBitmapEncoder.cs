// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um/wincodec.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ComputeSharp.Win32;

[Guid("00000103-A8F2-4877-BA0A-FD2B6645FB94")]
[NativeTypeName("struct IWICBitmapEncoder : IUnknown")]
[NativeInheritance("IUnknown")]
internal unsafe partial struct IWICBitmapEncoder
{
    public void** lpVtbl;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(0)]
    public HRESULT QueryInterface([NativeTypeName("const IID &")] Guid* riid, void** ppvObject)
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, Guid*, void**, int>)(lpVtbl[0]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this), riid, ppvObject);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(1)]
    [return: NativeTypeName("ULONG")]
    public uint AddRef()
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, uint>)(lpVtbl[1]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(2)]
    [return: NativeTypeName("ULONG")]
    public uint Release()
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, uint>)(lpVtbl[2]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(3)]
    public HRESULT Initialize(IStream* pIStream, WICBitmapEncoderCacheOption cacheOption)
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, IStream*, WICBitmapEncoderCacheOption, int>)(lpVtbl[3]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this), pIStream, cacheOption);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(4)]
    public HRESULT GetContainerFormat(Guid* pguidContainerFormat)
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, Guid*, int>)(lpVtbl[4]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this), pguidContainerFormat);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(7)]
    public HRESULT SetPalette(IWICPalette* pIPalette)
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, IWICPalette*, int>)(lpVtbl[7]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this), pIPalette);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(8)]
    public HRESULT SetThumbnail(IWICBitmapSource* pIThumbnail)
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, IWICBitmapSource*, int>)(lpVtbl[8]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this), pIThumbnail);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(9)]
    public HRESULT SetPreview(IWICBitmapSource* pIPreview)
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, IWICBitmapSource*, int>)(lpVtbl[9]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this), pIPreview);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(10)]
    public HRESULT CreateNewFrame(IWICBitmapFrameEncode** ppIFrameEncode, void** ppIEncoderOptions)
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, IWICBitmapFrameEncode**, void**, int>)(lpVtbl[10]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this), ppIFrameEncode, ppIEncoderOptions);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(11)]
    public HRESULT Commit()
    {
        return ((delegate* unmanaged[Stdcall]<IWICBitmapEncoder*, int>)(lpVtbl[11]))((IWICBitmapEncoder*)Unsafe.AsPointer(ref this));
    }
}