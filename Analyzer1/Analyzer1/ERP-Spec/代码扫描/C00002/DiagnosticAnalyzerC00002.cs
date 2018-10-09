using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.代码扫描.C00002
{
    /// <summary>
    /// SPEC:C00002; 判断字符串是否为空，必须使用string.IsNullOrEmpty这个静态方法
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerC00002 : DiagnosticAnalyzer
    {
        /// <summary>
        ///     分析Id
        /// </summary>
        public const string DiagnosticId = "C00002";

        /// <summary>
        ///     分类
        /// </summary>
        private const string Category = "代码扫描";

        /// <summary>
        ///     标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:C00002";

        /// <summary>
        ///     消息内容
        /// </summary>
        private static readonly string MessageFormat = "判断字符串是否为空，必须使用string.IsNullOrEmpty这个静态方法";

        /// <summary>
        ///     错误描述
        /// </summary>
        private static readonly string Description = "代码扫描>SPEC：C00002; 判断字符串是否为空，必须使用string.IsNullOrEmpty这个静态方法";

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
                    if (Regex.IsMatch(statementSyntax.GetText().ToString(),"==\\s*\"\""))
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