using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.代码扫描.C00011
{
    /// <summary>
    ///     SPEC:C00011; 判断是否为逻辑假，不得使用if(!bb)形式，必须写成if(bb == false)
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerC00011: DiagnosticAnalyzer
    {
        /// <summary>
        ///     分析Id
        /// </summary>
        public const string DiagnosticId = "C00011";

        /// <summary>
        ///     分类
        /// </summary>
        private const string Category = "代码扫描";

        /// <summary>
        ///     标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:C00011";

        /// <summary>
        ///     消息内容
        /// </summary>
        private static readonly string MessageFormat = "判断是否为逻辑假，不得使用if(!bb)形式，必须写成if(bb == false)";

        /// <summary>
        ///     错误描述
        /// </summary>
        private static readonly string Description = "代码扫描>SPEC:C00011; 判断是否为逻辑假，不得使用if(!bb)形式，必须写成if(bb == false)";

        /// <summary>
        ///     校验规则
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
            var method = context.Node as MethodDeclarationSyntax;

            if (method?.Body != null && method.Body.Statements.Count > 0)
            {
                foreach (var statementSyntax in method.Body.Statements)
                {
                    if (Regex.IsMatch(statementSyntax.GetText().ToString(), @"!(\w|\()"))
                    {
                        var diagnostic = Diagnostic.Create(Rule, statementSyntax.GetLocation(), method.Identifier.Text);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    }
}