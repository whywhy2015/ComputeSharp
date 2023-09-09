using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Interop.Windows;
#if !NET6_0_OR_GREATER
using RuntimeHelpers = ComputeSharp.NetStandard.RuntimeHelpers;
#endif

namespace ComputeSharp.D2D1.Interop.Effects;

/// <inheritdoc/>
unsafe partial struct ComputeShaderEffect
{
    /// <summary>
    /// A generic compute shader implementation.
    /// </summary>
    /// <typeparam name="T">The type of shader.</typeparam>
    public sealed class For<T>
        where T : unmanaged, ID2D1ComputeShader
    {
        /// <inheritdoc cref="PixelShaderEffect.For{T}.effectFactory"/>
        private readonly PixelShaderEffect.FactoryDelegate effectFactory;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.shaderId"/>
        private readonly Guid shaderId;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.constantBufferSize"/>
        private readonly int constantBufferSize;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.inputCount"/>
        private readonly int inputCount;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.inputDescriptionCount"/>
        private readonly int inputDescriptionCount;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.inputDescriptions"/>
        private readonly D2D1InputDescription* inputDescriptions;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.bytecode"/>
        private readonly byte* bytecode;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.bytecodeSize"/>
        private readonly int bytecodeSize;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.bufferPrecision"/>
        private readonly D2D1BufferPrecision bufferPrecision;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.channelDepth"/>
        private readonly D2D1ChannelDepth channelDepth;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.resourceTextureDescriptionCount"/>
        private readonly int resourceTextureDescriptionCount;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.resourceTextureDescriptions"/>
        private readonly D2D1ResourceTextureDescription* resourceTextureDescriptions;

        /// <inheritdoc cref="PixelShaderEffect.For{T}.For"/>
        private For(
            PixelShaderEffect.FactoryDelegate effectFactory,
            Guid shaderId,
            int constantBufferSize,
            int inputCount,
            int inputDescriptionCount,
            D2D1InputDescription* inputDescriptions,
            byte* bytecode,
            int bytecodeSize,
            D2D1BufferPrecision bufferPrecision,
            D2D1ChannelDepth channelDepth,
            int resourceTextureDescriptionCount,
            D2D1ResourceTextureDescription* resourceTextureDescriptions)
        {
            this.effectFactory = effectFactory;
            this.shaderId = shaderId;
            this.constantBufferSize = constantBufferSize;
            this.inputCount = inputCount;
            this.inputDescriptionCount = inputDescriptionCount;
            this.inputDescriptions = inputDescriptions;
            this.bytecode = bytecode;
            this.bytecodeSize = bytecodeSize;
            this.bufferPrecision = bufferPrecision;
            this.channelDepth = channelDepth;
            this.resourceTextureDescriptionCount = resourceTextureDescriptionCount;
            this.resourceTextureDescriptions = resourceTextureDescriptions;
        }

        /// <inheritdoc cref="PixelShaderEffect.For{T}.Instance"/>
        public static For<T> Instance { get; } = CreateInstance();

        /// <inheritdoc cref="PixelShaderEffect.For{T}.CreateInstance"/>
        private static For<T> CreateInstance()
        {
            // Load all shader properties
            Guid shaderId = typeof(T).GUID;
            int constantBufferSize = D2D1ComputeShader.GetConstantBufferSize<T>();
            D2D1BufferPrecision bufferPrecision = D2D1ComputeShader.GetOutputBufferPrecision<T>();
            D2D1ChannelDepth channelDepth = D2D1ComputeShader.GetOutputBufferChannelDepth<T>();
            int inputCount = D2D1ComputeShader.GetInputCount<T>();

            // Prepare the input descriptions
            ReadOnlyMemory<D2D1InputDescription> inputDescriptionsInfo = D2D1ComputeShader.GetInputDescriptions<T>();
            int inputDescriptionCount = inputDescriptionsInfo.Length;
            D2D1InputDescription* inputDescriptions = (D2D1InputDescription*)RuntimeHelpers.AllocateTypeAssociatedMemory(typeof(For<T>), sizeof(D2D1InputDescription) * inputDescriptionCount);

            inputDescriptionsInfo.Span.CopyTo(new Span<D2D1InputDescription>(inputDescriptions, inputDescriptionCount));

            // Prepare the resource texture descriptions
            ReadOnlyMemory<D2D1ResourceTextureDescription> resourceTextureDescriptionsInfo = D2D1ComputeShader.GetResourceTextureDescriptions<T>();
            int resourceTextureDescriptionCount = resourceTextureDescriptionsInfo.Length;
            D2D1ResourceTextureDescription* resourceTextureDescriptions = (D2D1ResourceTextureDescription*)RuntimeHelpers.AllocateTypeAssociatedMemory(typeof(For<T>), sizeof(D2D1ResourceTextureDescription) * resourceTextureDescriptionCount);

            resourceTextureDescriptionsInfo.Span.CopyTo(new Span<D2D1ResourceTextureDescription>(resourceTextureDescriptions, resourceTextureDescriptionCount));

            // Copy the bytecode to the target buffer
            ReadOnlyMemory<byte> bytecodeInfo = D2D1ComputeShader.LoadBytecode<T>();
            int bytecodeSize = bytecodeInfo.Length;
            byte* bytecode = (byte*)RuntimeHelpers.AllocateTypeAssociatedMemory(typeof(For<T>), bytecodeSize);

            bytecodeInfo.Span.CopyTo(new Span<byte>(bytecode, bytecodeSize));

            // Initialize the shared instance with the computed state
            return new(
                CreateEffect,
                shaderId,
                constantBufferSize,
                inputCount,
                inputDescriptionCount,
                inputDescriptions,
                bytecode,
                bytecodeSize,
                bufferPrecision,
                channelDepth,
                resourceTextureDescriptionCount,
                resourceTextureDescriptions);
        }

        /// <summary>
        /// Gets a reference to the id of the effect.
        /// </summary>
        public ref readonly Guid Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this.shaderId;
        }

        /// <summary>
        /// Gets the factory for the current effect.
        /// </summary>
#if NET6_0_OR_GREATER
        public delegate* unmanaged[Stdcall]<IUnknown**, HRESULT> Factory
#else
        public void* Factory
#endif
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET6_0_OR_GREATER
            get => (delegate* unmanaged[Stdcall]<IUnknown**, HRESULT>)Marshal.GetFunctionPointerForDelegate(this.effectFactory);
#else
            get => (void*)Marshal.GetFunctionPointerForDelegate(this.effectFactory);
#endif
        }

        /// <summary>
        /// Gets the number of inputs for the effect.
        /// </summary>
        public int InputCount => this.inputCount;

        /// <inheritdoc cref="PixelShaderEffect.FactoryDelegate"/>
        private static int CreateEffect(IUnknown** effectImpl)
        {
            For<T> instance = Instance;

            return ComputeShaderEffect.Factory(
                instance.shaderId,
                instance.constantBufferSize,
                instance.inputCount,
                instance.inputDescriptionCount,
                instance.inputDescriptions,
                instance.bytecode,
                instance.bytecodeSize,
                instance.bufferPrecision,
                instance.channelDepth,
                instance.resourceTextureDescriptionCount,
                instance.resourceTextureDescriptions,
                effectImpl);
        }
    }
}