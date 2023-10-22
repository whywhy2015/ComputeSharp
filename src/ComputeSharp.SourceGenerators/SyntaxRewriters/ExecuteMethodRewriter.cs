using ComputeSharp.SourceGeneration.SyntaxRewriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ComputeSharp.SourceGenerators.SyntaxRewriters;

/// <summary>
/// A custom <see cref="CSharpSyntaxRewriter"/> type that does postprocessing fixups on the shader main method.
/// </summary>
/// <param name="shaderSourceRewriter">The <see cref="SourceGeneration.SyntaxRewriters.ShaderSourceRewriter"/> instance used to process the input tree.</param>
internal abstract class ExecuteMethodRewriter(ShaderSourceRewriter shaderSourceRewriter) : CSharpSyntaxRewriter
{
    /// <summary>
    /// Gets the <see cref="ShaderSourceRewriter"/> instance used to process the input tree.
    /// </summary>
    protected ShaderSourceRewriter ShaderSourceRewriter => shaderSourceRewriter;

    /// <inheritdoc cref="CSharpSyntaxRewriter.Visit(SyntaxNode?)"/>
    public TNode? Visit<TNode>(TNode? node)
        where TNode : SyntaxNode
    {
        return (TNode?)base.Visit(node);
    }

    /// <inheritdoc/>
    public override SyntaxNode? VisitParameterList(ParameterListSyntax node)
    {
        ParameterListSyntax updatedNode = (ParameterListSyntax)base.VisitParameterList(node)!;

        updatedNode = updatedNode.AddParameters(Parameter(Identifier($"uint3 {nameof(ThreadIds)} : SV_DispatchThreadID")));

        if (ShaderSourceRewriter.IsGroupIdsUsed)
        {
            updatedNode = updatedNode.AddParameters(Parameter(Identifier($"uint3 {nameof(GroupIds)} : SV_GroupThreadID")));
        }

        if (ShaderSourceRewriter.IsGroupIdsIndexUsed)
        {
            updatedNode = updatedNode.AddParameters(Parameter(Identifier($"uint __{nameof(GroupIds)}__get_Index : SV_GroupIndex")));
        }

        if (ShaderSourceRewriter.IsGridIdsUsed)
        {
            updatedNode = updatedNode.AddParameters(Parameter(Identifier($"uint3 {nameof(GridIds)} : SV_GroupID")));
        }

        return updatedNode;
    }

    /// <summary>
    /// A custom <see cref="ExecuteMethodRewriter"/> type specialized for compute shaders.
    /// </summary>
    /// <remarks>
    /// Creates a new <see cref="Compute"/> instance with the specified parameters.
    /// </remarks>
    /// <param name="shaderSourceRewriter">The <see cref="SourceGeneration.SyntaxRewriters.ShaderSourceRewriter"/> instance used to process the input tree.</param>
    public sealed class Compute(ShaderSourceRewriter shaderSourceRewriter) : ExecuteMethodRewriter(shaderSourceRewriter)
    {
        /// <inheritdoc/>
        public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            MethodDeclarationSyntax updatedNode = ((MethodDeclarationSyntax)base.VisitMethodDeclaration(node)!).WithModifiers(TokenList());

            // When we're rewriting the main compute shader method, we need to insert a range
            // check to ensure that invocation outside of the requested range are discarded.
            // The following snippet creates this prologue before the user provided body:
            //
            // if (ThreadIds.x < __x &&
            //     ThreadIds.y < __y &&
            //     ThreadIds.z < __z)
            // {
            //     <body>
            // }
            BinaryExpressionSyntax rangeCheckExpression =
                BinaryExpression(SyntaxKind.LogicalAndExpression,
                    BinaryExpression(SyntaxKind.LogicalAndExpression,
                        BinaryExpression(SyntaxKind.LessThanExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(nameof(ThreadIds)),
                                IdentifierName("x")),
                            IdentifierName("__x")),
                        BinaryExpression(SyntaxKind.LessThanExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(nameof(ThreadIds)),
                                IdentifierName("y")),
                            IdentifierName("__y"))),
                    BinaryExpression(SyntaxKind.LessThanExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(nameof(ThreadIds)),
                            IdentifierName("z")),
                        IdentifierName("__z")));

            return updatedNode.WithBody(Block(IfStatement(rangeCheckExpression, updatedNode.Body!)));
        }
    }

    /// <summary>
    /// A custom <see cref="ExecuteMethodRewriter"/> type specialized for compute shaders.
    /// </summary>
    /// <param name="shaderSourceRewriter">The <see cref="SourceGeneration.SyntaxRewriters.ShaderSourceRewriter"/> instance used to process the input tree.</param>
    public sealed class Pixel(ShaderSourceRewriter shaderSourceRewriter) : ExecuteMethodRewriter(shaderSourceRewriter)
    {
        /// <inheritdoc/>
        public override SyntaxNode? VisitReturnStatement(ReturnStatementSyntax node)
        {
            ReturnStatementSyntax updatedNode = (ReturnStatementSyntax)base.VisitReturnStatement(node)!;

            // {
            //     __outputTexture[ThreadIds.xy] = <RETURN_EXPRESSION>;
            //     return;
            // }
            return
                Block(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            ElementAccessExpression(IdentifierName("__outputTexture"))
                            .AddArgumentListArguments(
                                Argument(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("ThreadIds"),
                                        IdentifierName("xy")))),
                            updatedNode.Expression!)),
                    ReturnStatement());
        }

        /// <inheritdoc/>
        public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            MethodDeclarationSyntax updatedNode = ((MethodDeclarationSyntax)base.VisitMethodDeclaration(node)!).WithModifiers(TokenList());

            // Change the return type to void
            updatedNode = updatedNode.WithReturnType(PredefinedType(Token(SyntaxKind.VoidKeyword)));

            // Same as above, but skipping the third check for the Z axis.
            // This is because pixel shaders only ever run on 2D textures.
            BinaryExpressionSyntax rangeCheckExpression =
                BinaryExpression(SyntaxKind.LogicalAndExpression,
                    BinaryExpression(SyntaxKind.LessThanExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(nameof(ThreadIds)),
                            IdentifierName("x")),
                        IdentifierName("__x")),
                    BinaryExpression(SyntaxKind.LessThanExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(nameof(ThreadIds)),
                            IdentifierName("y")),
                        IdentifierName("__y")));

            return updatedNode.WithBody(Block(IfStatement(rangeCheckExpression, updatedNode.Body!)));
        }

        /// <inheritdoc/>
        public override SyntaxNode? VisitIdentifierName(IdentifierNameSyntax node)
        {
            IdentifierNameSyntax updatedNode = (IdentifierNameSyntax)base.VisitIdentifierName(node)!;

            // Since pixel shaders are only ever executed on 2D textures, the special __z field can
            // be removed and replaced with just numeric literal expressions, where the value is 1.
            if (updatedNode.Identifier.Text == "__z")
            {
                return LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(1));
            }

            return updatedNode;
        }
    }
}