using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ComputeSharp.Descriptors;
using ComputeSharp.Graphics.Commands;
using ComputeSharp.Graphics.Extensions;
using ComputeSharp.Resources.Interop;
using ComputeSharp.Shaders.Dispatching;
using ComputeSharp.Shaders.Loading;
using ComputeSharp.Win32;

namespace ComputeSharp;

/// <summary>
/// A context to batch compute operations in a single invocation, minimizing GPU overhead.
/// </summary>
/// <remarks>
/// <para>
/// This type must always be used in a <see langword="using"/> statement and disposed properly.
/// Not doing so is undefined behavior and may result in the target device not being disposed correctly.
/// </para>
/// <para>
/// For more documentation on this, see the remarks in <see cref="GraphicsDeviceExtensions.CreateComputeContext(ComputeSharp.GraphicsDevice)"/>.
/// </para>
/// </remarks>
public struct ComputeContext : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// The <see cref="GraphicsDevice"/> instance owning the current context.
    /// </summary>
    private GraphicsDevice? device;

    /// <summary>
    /// The current <see cref="CommandList"/> instance used to dispatch shaders.
    /// </summary>
    private CommandList commandList;

    /// <summary>
    /// Creates a new <see cref="ComputeContext"/> instance with the specified parameters.
    /// </summary>
    /// <param name="device">The <see cref="GraphicsDevice"/> instance owning the current context.</param>
    internal ComputeContext(GraphicsDevice device)
    {
        this.device = device;
        this.commandList = default;

        // Increment the reference count for the device. This has to be released when disposing the context.
        // Not disposing the context is undefined behavior, so we can rely on that to release the reference.
        device.GetReferenceTracker().DangerousAddRef();
    }

    /// <summary>
    /// Gets the <see cref="ComputeSharp.GraphicsDevice"/> associated with the current instance.
    /// </summary>
    public readonly GraphicsDevice GraphicsDevice
    {
        get
        {
            default(InvalidOperationException).ThrowIf(this.device is null);

            return this.device;
        }
    }

    /// <summary>
    /// Inserts a resource barrier for a specific resource.
    /// </summary>
    /// <param name="d3D12Resource">The <see cref="ID3D12Resource"/> to insert the barrier for.</param>
    internal readonly unsafe void Barrier(ID3D12Resource* d3D12Resource)
    {
        default(InvalidOperationException).ThrowIf(this.device is null);

        ref CommandList commandList = ref GetCommandList();

        commandList.D3D12GraphicsCommandList->UnorderedAccessViewBarrier(d3D12Resource);
    }

    /// <summary>
    /// Clears a specific resource.
    /// </summary>
    /// <param name="d3D12Resource">The <see cref="ID3D12Resource"/> to clear.</param>
    /// <param name="d3D12GpuDescriptorHandle">The <see cref="D3D12_GPU_DESCRIPTOR_HANDLE"/> value for the target resource.</param>
    /// <param name="d3D12CpuDescriptorHandle">The <see cref="D3D12_CPU_DESCRIPTOR_HANDLE"/> value for the target resource.</param>
    /// <param name="isNormalized">Indicates whether the target resource uses a normalized format.</param>
    internal readonly unsafe void Clear(
        ID3D12Resource* d3D12Resource,
        D3D12_GPU_DESCRIPTOR_HANDLE d3D12GpuDescriptorHandle,
        D3D12_CPU_DESCRIPTOR_HANDLE d3D12CpuDescriptorHandle,
        bool isNormalized)
    {
        default(InvalidOperationException).ThrowIf(this.device is null);

        ref CommandList commandList = ref GetCommandList(pipelineState: null);

        commandList.D3D12GraphicsCommandList->ClearUnorderedAccessView(d3D12Resource, d3D12GpuDescriptorHandle, d3D12CpuDescriptorHandle, isNormalized);
    }

    /// <summary>
    /// Fills a specific resource.
    /// </summary>
    /// <param name="d3D12Resource">The <see cref="ID3D12Resource"/> to fill.</param>
    /// <param name="d3D12GpuDescriptorHandle">The <see cref="D3D12_GPU_DESCRIPTOR_HANDLE"/> value for the target resource.</param>
    /// <param name="d3D12CpuDescriptorHandle">The <see cref="D3D12_CPU_DESCRIPTOR_HANDLE"/> value for the target resource.</param>
    /// <param name="value">The value to use to fill the resource.</param>
    internal readonly unsafe void Fill(
        ID3D12Resource* d3D12Resource,
        D3D12_GPU_DESCRIPTOR_HANDLE d3D12GpuDescriptorHandle,
        D3D12_CPU_DESCRIPTOR_HANDLE d3D12CpuDescriptorHandle,
        Float4 value)
    {
        default(InvalidOperationException).ThrowIf(this.device is null);

        ref CommandList commandList = ref GetCommandList(pipelineState: null);

        commandList.D3D12GraphicsCommandList->FillUnorderedAccessView(d3D12Resource, d3D12GpuDescriptorHandle, d3D12CpuDescriptorHandle, value);
    }

    /// <summary>
    /// Runs the input shader with the specified parameters.
    /// </summary>
    /// <param name="x">The number of iterations to run on the X axis.</param>
    /// <param name="shader">The input <typeparamref name="T"/> instance representing the compute shader to run.</param>
    internal readonly unsafe void Run<T>(int x, in T shader)
        where T : struct, IComputeShader, IComputeShaderDescriptor<T>
    {
        Run(x, 1, 1, in shader);
    }

    /// <summary>
    /// Runs the input shader with the specified parameters.
    /// </summary>
    /// <param name="x">The number of iterations to run on the X axis.</param>
    /// <param name="y">The number of iterations to run on the Y axis.</param>
    /// <param name="shader">The input <typeparamref name="T"/> instance representing the compute shader to run.</param>
    internal readonly unsafe void Run<T>(int x, int y, in T shader)
        where T : struct, IComputeShader, IComputeShaderDescriptor<T>
    {
        Run(x, y, 1, in shader);
    }

    /// <summary>
    /// Runs the input shader with the specified parameters.
    /// </summary>
    /// <typeparam name="T">The type of compute shader to run.</typeparam>
    /// <param name="x">The number of iterations to run on the X axis.</param>
    /// <param name="y">The number of iterations to run on the Y axis.</param>
    /// <param name="z">The number of iterations to run on the Z axis.</param>
    /// <param name="shader">The input <typeparamref name="T"/> instance representing the compute shader to run.</param>
    internal readonly unsafe void Run<T>(int x, int y, int z, in T shader)
        where T : struct, IComputeShader, IComputeShaderDescriptor<T>
    {
        default(InvalidOperationException).ThrowIf(this.device is null);
        default(ArgumentOutOfRangeException).ThrowIfNegativeOrZero(x);
        default(ArgumentOutOfRangeException).ThrowIfNegativeOrZero(y);
        default(ArgumentOutOfRangeException).ThrowIfNegativeOrZero(z);

        int groupsX = Math.DivRem(x, T.ThreadsX, out int modX) + (modX == 0 ? 0 : 1);
        int groupsY = Math.DivRem(y, T.ThreadsY, out int modY) + (modY == 0 ? 0 : 1);
        int groupsZ = Math.DivRem(z, T.ThreadsZ, out int modZ) + (modZ == 0 ? 0 : 1);

        default(ArgumentOutOfRangeException).ThrowIfNotBetweenOrEqual(groupsX, 1, D3D11.D3D11_CS_DISPATCH_MAX_THREAD_GROUPS_PER_DIMENSION);
        default(ArgumentOutOfRangeException).ThrowIfNotBetweenOrEqual(groupsY, 1, D3D11.D3D11_CS_DISPATCH_MAX_THREAD_GROUPS_PER_DIMENSION, nameof(groupsX));
        default(ArgumentOutOfRangeException).ThrowIfNotBetweenOrEqual(groupsZ, 1, D3D11.D3D11_CS_DISPATCH_MAX_THREAD_GROUPS_PER_DIMENSION, nameof(groupsX));

        PipelineData pipelineData = PipelineDataLoader<T>.GetPipelineData(this.device);

        ref CommandList commandList = ref GetCommandList(pipelineData.D3D12PipelineState);

        commandList.D3D12GraphicsCommandList->SetComputeRootSignature(pipelineData.D3D12RootSignature);

        D3D12GraphicsCommandListConstantBufferLoader dataLoader = new(commandList.D3D12GraphicsCommandList);

        T.LoadConstantBuffer(in shader, ref dataLoader, x, y, z);

        D3D12GraphicsCommandListGraphicsResourceLoader graphicsResourceLoader = new(commandList.D3D12GraphicsCommandList, this.device, rootParameterOffset: 1);

        T.LoadGraphicsResources(in shader, ref graphicsResourceLoader);

        commandList.D3D12GraphicsCommandList->Dispatch((uint)groupsX, (uint)groupsY, (uint)groupsZ);
    }

    /// <summary>
    /// Runs the input shader with the specified parameters.
    /// </summary>
    /// <typeparam name="T">The type of pixel shader to run.</typeparam>
    /// <typeparam name="TPixel">The type of pixel to work on.</typeparam>
    /// <param name="texture">The target texture to invoke the pixel shader upon.</param>
    /// <param name="shader">The input <typeparamref name="T"/> instance representing the pixel shader to run.</param>
    internal readonly unsafe void Run<T, TPixel>(IReadWriteNormalizedTexture2D<TPixel> texture, in T shader)
        where T : struct, IComputeShader<TPixel>, IComputeShaderDescriptor<T>
        where TPixel : unmanaged
    {
        default(InvalidOperationException).ThrowIf(this.device is null);

        int x = texture.Width;
        int y = texture.Height;
        int groupsX = Math.DivRem(x, T.ThreadsX, out int modX) + (modX == 0 ? 0 : 1);
        int groupsY = Math.DivRem(y, T.ThreadsY, out int modY) + (modY == 0 ? 0 : 1);

        default(ArgumentOutOfRangeException).ThrowIfNotBetweenOrEqual(groupsX, 1, D3D11.D3D11_CS_DISPATCH_MAX_THREAD_GROUPS_PER_DIMENSION);
        default(ArgumentOutOfRangeException).ThrowIfNotBetweenOrEqual(groupsY, 1, D3D11.D3D11_CS_DISPATCH_MAX_THREAD_GROUPS_PER_DIMENSION, nameof(groupsX));

        PipelineData pipelineData = PipelineDataLoader<T>.GetPipelineData(this.device);

        ref CommandList commandList = ref GetCommandList(pipelineData.D3D12PipelineState);

        commandList.D3D12GraphicsCommandList->SetComputeRootSignature(pipelineData.D3D12RootSignature);

        D3D12GraphicsCommandListConstantBufferLoader constantBufferLoader = new(commandList.D3D12GraphicsCommandList);

        T.LoadConstantBuffer(in shader, ref constantBufferLoader, x, y, 1);

        D3D12GraphicsCommandListGraphicsResourceLoader graphicsResourceLoader = new(commandList.D3D12GraphicsCommandList, this.device, rootParameterOffset: 2);

        T.LoadGraphicsResources(in shader, ref graphicsResourceLoader);

        // Load the implicit output texture
        commandList.D3D12GraphicsCommandList->SetComputeRootDescriptorTable(
            1,
            ((ID3D12ReadOnlyResource)texture).ValidateAndGetGpuDescriptorHandle(this.device));

        commandList.D3D12GraphicsCommandList->Dispatch((uint)groupsX, (uint)groupsY, 1);
    }

    /// <summary>
    /// Inserts a transition for a specific resource.
    /// </summary>
    /// <param name="d3D12Resource">The <see cref="ID3D12Resource"/> to change state for.</param>
    /// <param name="d3D12ResourceStatesBefore">The starting <see cref="D3D12_RESOURCE_STATES"/> value for the transition.</param>
    /// <param name="d3D12ResourceStatesAfter">The destnation <see cref="D3D12_RESOURCE_STATES"/> value for the transition.</param>
    internal readonly unsafe void Transition(
        ID3D12Resource* d3D12Resource,
        D3D12_RESOURCE_STATES d3D12ResourceStatesBefore,
        D3D12_RESOURCE_STATES d3D12ResourceStatesAfter)
    {
        default(InvalidOperationException).ThrowIf(this.device is null);

        ref CommandList commandList = ref GetCommandList(pipelineState: null);

        commandList.D3D12GraphicsCommandList->TransitionBarrier(d3D12Resource, d3D12ResourceStatesBefore, d3D12ResourceStatesAfter);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        default(InvalidOperationException).ThrowIf(this.device is null);

        GraphicsDevice device = this.device;

        this.device = null;

        if (!this.commandList.IsAllocated)
        {
            device.GetReferenceTracker().DangerousRelease();

            return;
        }

        try
        {
            this.commandList.ExecuteAndWaitForCompletion();
        }
        finally
        {
            device.GetReferenceTracker().DangerousRelease();
        }
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask DisposeAsync()
    {
        default(InvalidOperationException).ThrowIf(this.device is null);

        GraphicsDevice device = this.device;

        this.device = null;

        if (!this.commandList.IsAllocated)
        {
            device.GetReferenceTracker().DangerousRelease();

            return ValueTask.CompletedTask;
        }

        try
        {
            return this.commandList.ExecuteAndWaitForCompletionAsync();
        }
        finally
        {
            device.GetReferenceTracker().DangerousRelease();
        }
    }

    /// <summary>
    /// Gets the current <see cref="CommandList"/> instance.
    /// </summary>
    /// <returns>A reference to the <see cref="CommandList"/> instance to use.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the <see cref="CommandList"/> has not been initialized yet.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [UnscopedRef]
    private readonly unsafe ref CommandList GetCommandList()
    {
        // This method has to take the context by readonly reference to allow callers to be marked as readonly.
        // This is needed to skip the hidden copies done by Roslyn, which would break the dispatching, as the
        // original context would not see the changes done by the following queued dispatches.
        ref CommandList commandList = ref Unsafe.AsRef(in this.commandList);

        default(InvalidOperationException).ThrowIf(!commandList.IsAllocated);

        return ref commandList;
    }

    /// <summary>
    /// Gets the current <see cref="CommandList"/> instance, and initializes it as needed.
    /// </summary>
    /// <param name="pipelineState">The input <see cref="ID3D12PipelineState"/> to load.</param>
    /// <returns>A reference to the <see cref="CommandList"/> instance to use.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [UnscopedRef]
    internal readonly unsafe ref CommandList GetCommandList(ID3D12PipelineState* pipelineState)
    {
        ref CommandList commandList = ref Unsafe.AsRef(in this.commandList);

        if (commandList.IsAllocated)
        {
            // Skip setting the pipeline state if the new state is null. This is the case when the upcoming
            // operation is not a shader dispatch, but just a resource clear. In this case there is no state.
            if (pipelineState is not null)
            {
                commandList.D3D12GraphicsCommandList->SetPipelineState(pipelineState);
            }
        }
        else
        {
            commandList = new CommandList(this.device!, pipelineState);
        }

        return ref commandList;
    }
}