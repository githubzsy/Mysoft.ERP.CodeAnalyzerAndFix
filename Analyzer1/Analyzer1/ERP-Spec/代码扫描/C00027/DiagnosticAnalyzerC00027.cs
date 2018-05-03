using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.代码扫描.C00027
{
    /// <summary>
    /// 每个方法体不允许大于50行（有效代码）
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerC00027: DiagnosticAnalyzer
    {
        /// <summary>
        /// 分析Id
        /// </summary>
        public const string DiagnosticId = "C00027";
        /// <summary>
        /// 分类
        /// </summary>
        private const string Category = "代码扫描";
        /// <summary>
        /// 标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:C00027";
        /// <summary>
        /// 消息内容
        /// </summary>
        private static readonly string MessageFormat = "每个方法体不允许大于50行（有效代码）";
        /// <summary>
        /// 错误描述
        /// </summary>
        private static readonly string Description = "代码扫描>SPEC:C00027; 每个方法体不允许大于50行（有效代码）,当前代码已经有{0}";
        /// <summary>
        /// 校验规则
        /// </summary>
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description,
            CommonHelper.HelpLinkUri);

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.MethodDeclaration);
        }

        private void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var method = (MethodDeclarationSyntax) context.Node;

            if (method?.Body != null && method.Body.Statements.Count > 50)
            {
                var diagnostic = Diagnostic.Create(Rule, method.Body.Statements[method.Body.Statements.Count -1].GetLocation(), method.Body.Statements.Count);
                context.ReportDiagnostic(diagnostic);
            }
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    }
}
