// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared/dxgi1_4.h in the Windows SDK for Windows 10.0.20348.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Interop.Windows;

namespace ComputeSharp.Win32;

[Guid("1BC6EA02-EF36-464F-BF0C-21CA39E5168A")]
[NativeTypeName("struct IDXGIFactory4 : IDXGIFactory3")]
[NativeInheritance("IDXGIFactory3")]
internal unsafe partial struct IDXGIFactory4
{
    public void** lpVtbl;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(0)]
    public HRESULT QueryInterface([NativeTypeName("const IID &")] Guid* riid, void** ppvObject)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int>)(lpVtbl[0]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), riid, ppvObject);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(1)]
    [return: NativeTypeName("ULONG")]
    public uint AddRef()
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint>)(lpVtbl[1]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(2)]
    [return: NativeTypeName("ULONG")]
    public uint Release()
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint>)(lpVtbl[2]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(3)]
    public HRESULT SetPrivateData([NativeTypeName("const GUID &")] Guid* Name, uint DataSize, [NativeTypeName("const void *")] void* pData)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, uint, void*, int>)(lpVtbl[3]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Name, DataSize, pData);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(4)]
    public HRESULT SetPrivateDataInterface([NativeTypeName("const GUID &")] Guid* Name, [NativeTypeName("const IUnknown *")] IUnknown* pUnknown)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, IUnknown*, int>)(lpVtbl[4]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Name, pUnknown);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(5)]
    public HRESULT GetPrivateData([NativeTypeName("const GUID &")] Guid* Name, uint* pDataSize, void* pData)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, uint*, void*, int>)(lpVtbl[5]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Name, pDataSize, pData);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(6)]
    public HRESULT GetParent([NativeTypeName("const IID &")] Guid* riid, void** ppParent)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int>)(lpVtbl[6]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), riid, ppParent);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(7)]
    public HRESULT EnumAdapters(uint Adapter, IDXGIAdapter** ppAdapter)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, IDXGIAdapter**, int>)(lpVtbl[7]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Adapter, ppAdapter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(8)]
    public HRESULT MakeWindowAssociation(HWND WindowHandle, uint Flags)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, int>)(lpVtbl[8]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), WindowHandle, Flags);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(9)]
    public HRESULT GetWindowAssociation(HWND* pWindowHandle)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND*, int>)(lpVtbl[9]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), pWindowHandle);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(12)]
    public HRESULT EnumAdapters1(uint Adapter, IDXGIAdapter1** ppAdapter)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, IDXGIAdapter1**, int>)(lpVtbl[12]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), Adapter, ppAdapter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(13)]
    public BOOL IsCurrent()
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, int>)(lpVtbl[13]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(14)]
    public BOOL IsWindowedStereoEnabled()
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, int>)(lpVtbl[14]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(16)]
    public HRESULT CreateSwapChainForCoreWindow(IUnknown* pDevice, IUnknown* pWindow, [NativeTypeName("const DXGI_SWAP_CHAIN_DESC1 *")] DXGI_SWAP_CHAIN_DESC1* pDesc, IDXGIOutput* pRestrictToOutput, IDXGISwapChain1** ppSwapChain)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, IUnknown*, DXGI_SWAP_CHAIN_DESC1*, IDXGIOutput*, IDXGISwapChain1**, int>)(lpVtbl[16]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), pDevice, pWindow, pDesc, pRestrictToOutput, ppSwapChain);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(17)]
    public HRESULT GetSharedResourceAdapterLuid(HANDLE hResource, LUID* pLuid)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, LUID*, int>)(lpVtbl[17]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), hResource, pLuid);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(18)]
    public HRESULT RegisterStereoStatusWindow(HWND WindowHandle, uint wMsg, [NativeTypeName("DWORD *")] uint* pdwCookie)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, uint*, int>)(lpVtbl[18]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), WindowHandle, wMsg, pdwCookie);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(19)]
    public HRESULT RegisterStereoStatusEvent(HANDLE hEvent, [NativeTypeName("DWORD *")] uint* pdwCookie)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, uint*, int>)(lpVtbl[19]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), hEvent, pdwCookie);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(20)]
    public void UnregisterStereoStatus([NativeTypeName("DWORD")] uint dwCookie)
    {
        ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, void>)(lpVtbl[20]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), dwCookie);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(21)]
    public HRESULT RegisterOcclusionStatusWindow(HWND WindowHandle, uint wMsg, [NativeTypeName("DWORD *")] uint* pdwCookie)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HWND, uint, uint*, int>)(lpVtbl[21]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), WindowHandle, wMsg, pdwCookie);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(22)]
    public HRESULT RegisterOcclusionStatusEvent(HANDLE hEvent, [NativeTypeName("DWORD *")] uint* pdwCookie)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, HANDLE, uint*, int>)(lpVtbl[22]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), hEvent, pdwCookie);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(23)]
    public void UnregisterOcclusionStatus([NativeTypeName("DWORD")] uint dwCookie)
    {
        ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint, void>)(lpVtbl[23]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), dwCookie);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(24)]
    public HRESULT CreateSwapChainForComposition(IUnknown* pDevice, [NativeTypeName("const DXGI_SWAP_CHAIN_DESC1 *")] DXGI_SWAP_CHAIN_DESC1* pDesc, IDXGIOutput* pRestrictToOutput, IDXGISwapChain1** ppSwapChain)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, IUnknown*, DXGI_SWAP_CHAIN_DESC1*, IDXGIOutput*, IDXGISwapChain1**, int>)(lpVtbl[24]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), pDevice, pDesc, pRestrictToOutput, ppSwapChain);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(25)]
    public uint GetCreationFlags()
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, uint>)(lpVtbl[25]))((IDXGIFactory4*)Unsafe.AsPointer(ref this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(26)]
    public HRESULT EnumAdapterByLuid(LUID AdapterLuid, [NativeTypeName("const IID &")] Guid* riid, void** ppvAdapter)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, LUID, Guid*, void**, int>)(lpVtbl[26]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), AdapterLuid, riid, ppvAdapter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [VtblIndex(27)]
    public HRESULT EnumWarpAdapter([NativeTypeName("const IID &")] Guid* riid, void** ppvAdapter)
    {
        return ((delegate* unmanaged[Stdcall]<IDXGIFactory4*, Guid*, void**, int>)(lpVtbl[27]))((IDXGIFactory4*)Unsafe.AsPointer(ref this), riid, ppvAdapter);
    }
}