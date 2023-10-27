using System;

namespace ComputeSharp;

/// <summary>
/// An attribute that indicates the compile options that should be used when compiling a given compute shader.
/// <para>
/// This attribute can be used to annotate shader types as follows:
/// <code>
/// [CompileOptions(CompileOptions.IeeeStrictness | CompileOptions.OptimizationLevel3)]
/// struct MyShader : IComputeShader
/// {
/// }
/// </code>
/// </para>
/// <para>
/// This attribute can also be added to a whole assembly, and will be used by default if not overridden by a shader type.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Assembly | AttributeTargets.Method, AllowMultiple = false)]
public sealed class CompileOptionsAttribute : Attribute
{
    /// <summary>
    /// Creates a new <see cref="CompileOptionsAttribute"/> instance with the specified parameters.
    /// </summary>
    /// <param name="options">The compiler options to use to compile the shader.</param>
    public CompileOptionsAttribute(CompileOptions options)
    {
        Options = options;
    }

    /// <summary>
    /// Gets the compile options to use to compile the shader.
    /// </summary>
    public CompileOptions Options { get; }
}