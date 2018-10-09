using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.Test
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Test : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RM0015";

        private const string Category = "ERP����淶";

        private static readonly string Title = "61.CodeAnalyzer.SPEC:RM0015";

        private static readonly string MessageFormat = "AppService�ķ���������[ActionDescription]���";

        private static readonly string Description = "ERP����淶>SPEC:RM0015;AppService�ķ���������[ActionDescription]���";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description,
            CommonHelper.HelpLinkUri);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {

                var rule1 = new DiagnosticDescriptor("AA123", "��̬����.123", "��ʽ����Ϣ", "����", DiagnosticSeverity.Error, true, "����", CommonHelper.HelpLinkUri);
                var rule2 = new DiagnosticDescriptor("AA456", "��̬����.456", "��ʽ����Ϣ", "����", DiagnosticSeverity.Error, true, "����", CommonHelper.HelpLinkUri);




                return ImmutableArray.Create(rule1, rule2);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;

            if (methodSymbol.Name.StartsWith("get_") || methodSymbol.Name.StartsWith("set_"))
            {
                return;
            }

            var attributeDatas = methodSymbol.GetAttributes();

            //if (methodSymbol.ContainingType.BaseType != null &&
            //    methodSymbol.ContainingType.BaseType.Name == "AppService" &&
            //    attributeDatas.Any(x => x.AttributeClass.Name == "ActionDescriptionAttribute") == false)
            //{
            if (methodSymbol.DeclaredAccessibility == Accessibility.Public &&
                methodSymbol.ContainingType.Name.EndsWith("AppService") &&
                attributeDatas.Any(x => x.AttributeClass.Name == "ActionDescriptionAttribute") == false &&
                attributeDatas.Any(x => x.AttributeClass.Name == "ForbidHttpAttribute") == false)
            {
                //methodSymbol.ContainingType.BaseType!=null && methodSymbol.ContainingType.BaseType

                var diagnostic = Diagnostic.Create(SupportedDiagnostics.First(x => x.Id == "AA123"), methodSymbol.Locations[0], methodSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}