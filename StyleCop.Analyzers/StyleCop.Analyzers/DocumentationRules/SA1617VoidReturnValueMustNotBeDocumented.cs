﻿namespace StyleCop.Analyzers.DocumentationRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;


    /// <summary>
    /// A C# code element does not contain a return value, or returns <c>void</c>, but the documentation header for the
    /// element contains a <c>&lt;returns&gt;</c> tag.
    /// </summary>
    /// <remarks>
    /// <para>C# syntax provides a mechanism for inserting documentation for classes and elements directly into the
    /// code, through the use of XML documentation headers. For an introduction to these headers and a description of
    /// the header syntax, see the following article:
    /// <see href="http://msdn.microsoft.com/en-us/magazine/cc302121.aspx">XML Comments Let You Build Documentation
    /// Directly From Your Visual Studio .NET Source Files</see>.</para>
    ///
    /// <para>A violation of this rule occurs if an element which returns <c>void</c> contains a <c>&lt;returns&gt;</c>
    /// tag within its documentation header.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1617VoidReturnValueMustNotBeDocumented : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1617VoidReturnValueMustNotBeDocumented"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1617";
        private const string Title = "Void return value must not be documented";
        private const string MessageFormat = "Void return value must not be documented";
        private const string Category = "StyleCop.CSharp.DocumentationRules";
        private const string Description = "A C# code element does not contain a return value, or returns void, but the documentation header for the element contains a <returns> tag.";
        private const string HelpLink = "http://www.stylecop.com/docs/SA1617.html";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description, HelpLink);

        private static readonly ImmutableArray<DiagnosticDescriptor> SupportedDiagnosticsValue =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return SupportedDiagnosticsValue;
            }
        }

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(this.HandleMethodDeclaration, SyntaxKind.MethodDeclaration);
            context.RegisterSyntaxNodeAction(this.HandleDelegateDeclaration, SyntaxKind.DelegateDeclaration);
        }

        private void HandleMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = context.Node as MethodDeclarationSyntax;
            HandleMember(context, methodDeclaration?.ReturnType);
        }

        private void HandleDelegateDeclaration(SyntaxNodeAnalysisContext context)
        {
            var delegateDeclaration = context.Node as DelegateDeclarationSyntax;
            HandleMember(context, delegateDeclaration?.ReturnType);
        }

        private static void HandleMember(SyntaxNodeAnalysisContext context, TypeSyntax returnValue)
        {
            var documentation = XmlCommentHelper.GetDocumentationStructure(context.Node);

            if (context.Node != null && documentation != null)
            {
                var returnType = returnValue as PredefinedTypeSyntax;

                // Check if the return type is void.
                if (returnType != null && returnType.Keyword.IsKind(SyntaxKind.VoidKeyword))
                {
                    // Check if the return value is documented
                    var returnsElement = XmlCommentHelper.GetTopLevelElement(documentation, XmlCommentHelper.ReturnsXmlTag);

                    if (returnsElement != null)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptor, returnsElement.GetLocation()));
                    }
                }
            }
        }
    }
}
