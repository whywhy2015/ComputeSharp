using System.Collections.Immutable;
using ComputeSharp.SourceGeneration.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ComputeSharp.SourceGeneration.Diagnostics;

/// <summary>
/// A diagnostic analyzer that generates an error if a field in a target shader type is of a type that is not accessible from its containing assembly.
/// </summary>
public abstract class NotAccessibleFieldTypeInGeneratedShaderDescriptorAttributeTargetAnalyzerBase : DiagnosticAnalyzer
{
    /// <summary>
    /// The <see cref="DiagnosticDescriptor"/> instance to use.
    /// </summary>
    private readonly DiagnosticDescriptor diagnosticDescriptor;

    /// <summary>
    /// The fully qualified type name of the target attribute.
    /// </summary>
    private readonly string generatedShaderDescriptorFullyQualifiedTypeName;

    /// <summary>
    /// Creates a new <see cref="NotAccessibleFieldTypeInGeneratedShaderDescriptorAttributeTargetAnalyzerBase"/> instance with the specified arguments.
    /// </summary>
    /// <param name="diagnosticDescriptor">The <see cref="DiagnosticDescriptor"/> instance to use.</param>
    /// <param name="generatedShaderDescriptorFullyQualifiedTypeName">The fully qualified type name of the target attribute.</param>
    private protected NotAccessibleFieldTypeInGeneratedShaderDescriptorAttributeTargetAnalyzerBase(
        DiagnosticDescriptor diagnosticDescriptor,
        string generatedShaderDescriptorFullyQualifiedTypeName)
    {
        this.diagnosticDescriptor = diagnosticDescriptor;
        this.generatedShaderDescriptorFullyQualifiedTypeName = generatedShaderDescriptorFullyQualifiedTypeName;

        SupportedDiagnostics = ImmutableArray.Create(diagnosticDescriptor);
    }

    /// <inheritdoc/>
    public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

    /// <inheritdoc/>
    public sealed override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(context =>
        {
            // Get the symbol for the target attribute type
            if (context.Compilation.GetTypeByMetadataName(this.generatedShaderDescriptorFullyQualifiedTypeName) is not { } generatedShaderDescriptorAttributeSymbol)
            {
                return;
            }

            context.RegisterSymbolAction(context =>
            {
                // Only struct types are possible targets
                if (context.Symbol is not INamedTypeSymbol { TypeKind: TypeKind.Struct } typeSymbol)
                {
                    return;
                }

                // If the target type is not using the attribute, there's nothing left to do
                if (!typeSymbol.TryGetAttributeWithType(generatedShaderDescriptorAttributeSymbol, out AttributeData? attribute))
                {
                    return;
                }

                foreach (ISymbol memberSymbol in typeSymbol.GetMembers())
                {
                    // Only select allowed fields (ie. skip constants, static fields, etc.)
                    if (memberSymbol is not IFieldSymbol { Type: INamedTypeSymbol fieldTypeSymbol, IsConst: false, IsStatic: false, IsFixedSizeBuffer: false, IsImplicitlyDeclared: false })
                    {
                        continue;
                    }

                    // Emit a diagnostic if the field type is not accessible
                    if (!fieldTypeSymbol.IsAccessibleFromCompilationAssembly(context.Compilation))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            this.diagnosticDescriptor,
                            attribute.GetLocation(),
                            typeSymbol,
                            memberSymbol.Name,
                            fieldTypeSymbol));
                    }
                }
            }, SymbolKind.NamedType);
        });
    }
}