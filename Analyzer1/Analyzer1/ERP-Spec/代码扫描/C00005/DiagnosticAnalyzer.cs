using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.代码扫描.C00005
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class C00005Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "C00005";

        private const string Category = "代码扫描";

        private static readonly string Title = "CodeAnalyzer.SPEC:C00005";

        private static readonly string MessageFormat = "产品代码禁止设计成静态成员，除非工具方法:{0}";

        private static readonly string Description = "代码扫描>SPEC:C00005;产品代码禁止设计成静态成员，除非工具方法";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description,
            CommonHelper.helpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            if (methodSymbol.IsStatic && 
                methodSymbol.ContainingType.Name.EndsWith("Initializer") == false && 
                methodSymbol.ContainingType.Name.EndsWith("Helper") == false &&
                methodSymbol.ContainingType.Name.EndsWith("Extensions") == false && 
                methodSymbol.ContainingType.Name.EndsWith("Factory") == false &&
                methodSymbol.ContainingType.Name.EndsWith("Manager") == false &&
                methodSymbol.ContainingType.Name.EndsWith("Enum") == false &&
                methodSymbol.ContainingType.Name.EndsWith("LangRes") == false &&
                methodSymbol.ContainingType.Name.EndsWith("Const") == false)
            {
                var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);

                context.ReportDiagnostic(diagnostic);

            }
        }
    }
}