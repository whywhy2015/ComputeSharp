// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared/dxgi.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Interop.Windows;

namespace ComputeSharp.Win32;

[Guid("29038F61-3839-4626-91FD-086879011A05")]
[NativeTypeName("struct IDXGIAdapter1 : IDXGIAdapter")]
[NativeInheritance("IDXGIAdapter")]
internal unsafe partial struct IDXGIAdapter1
{
    public void** lpVtbl;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(0)]
    public HRESULT QueryInterface([NativeTypeName("const IID &")] Guid* riid, void** ppvObject)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, Guid*, void**, int>)(lpVtbl[0]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), riid, ppvObject);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(1)]
    [return: NativeTypeName("ULONG")]
    public uint AddRef()
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, uint>)(lpVtbl[1]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(2)]
    [return: NativeTypeName("ULONG")]
    public uint Release()
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, uint>)(lpVtbl[2]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(3)]
    public HRESULT SetPrivateData([NativeTypeName("const GUID &")] Guid* Name, uint DataSize, [NativeTypeName("const void *")] void* pData)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, Guid*, uint, void*, int>)(lpVtbl[3]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), Name, DataSize, pData);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(4)]
    public HRESULT SetPrivateDataInterface([NativeTypeName("const GUID &")] Guid* Name, [NativeTypeName("const IUnknown *")] IUnknown* pUnknown)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, Guid*, IUnknown*, int>)(lpVtbl[4]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), Name, pUnknown);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(5)]
    public HRESULT GetPrivateData([NativeTypeName("const GUID &")] Guid* Name, uint* pDataSize, void* pData)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, Guid*, uint*, void*, int>)(lpVtbl[5]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), Name, pDataSize, pData);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(6)]
    public HRESULT GetParent([NativeTypeName("const IID &")] Guid* riid, void** ppParent)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, Guid*, void**, int>)(lpVtbl[6]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), riid, ppParent);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(7)]
    public HRESULT EnumOutputs(uint Output, IDXGIOutput** ppOutput)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, uint, IDXGIOutput**, int>)(lpVtbl[7]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), Output, ppOutput);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(8)]
    public HRESULT GetDesc(DXGI_ADAPTER_DESC* pDesc)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, DXGI_ADAPTER_DESC*, int>)(lpVtbl[8]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), pDesc);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(9)]
    public HRESULT CheckInterfaceSupport([NativeTypeName("const GUID &")] Guid* InterfaceName, LARGE_INTEGER* pUMDVersion)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, Guid*, LARGE_INTEGER*, int>)(lpVtbl[9]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), InterfaceName, pUMDVersion);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(10)]
    public HRESULT GetDesc1(DXGI_ADAPTER_DESC1* pDesc)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIAdapter1*, DXGI_ADAPTER_DESC1*, int>)(lpVtbl[10]))((IDXGIAdapter1*)Unsafe.AsPointer(ref this), pDesc);
    }
}