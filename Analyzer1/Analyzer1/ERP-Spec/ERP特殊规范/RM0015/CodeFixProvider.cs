using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Rm0015CodeFixProvider))]
    [Shared]
    public class Rm0015CodeFixProvider : CodeFixProvider
    {
        private const string title = "添加[ActionDescription]标记";

        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(Rm0015Analyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
                .OfType<MethodDeclarationSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title,
                    c => MakeUppercaseAsync(context.Document, declaration, c),
                    title),
                diagnostic);
        }

        private async Task<Document> MakeUppercaseAsync(Document document, MethodDeclarationSyntax methodDeclarationSyntax, CancellationToken cancellationToken)
        {
            //获取目标方法

            var attributeLists = methodDeclarationSyntax.AttributeLists;

            var attributeListSyntax = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(
                SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("ActionDescription"))));  

            var attr = SourceText.From("[ActionDescription(\"\",\"00\")]");

            var newMethodDeclarationSyntax =
                methodDeclarationSyntax.WithAttributeLists(attributeLists.Add(attributeListSyntax));

            var root = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = root.ReplaceNode(methodDeclarationSyntax, newMethodDeclarationSyntax);
            var newDocument = document.WithSyntaxRoot(newRoot);
            return newDocument;
        }
    }
}