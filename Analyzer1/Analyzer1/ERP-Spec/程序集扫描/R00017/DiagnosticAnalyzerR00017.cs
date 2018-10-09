using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.ERP_Spec.程序集扫描.R00017
{
    /// <summary>
    /// SPEC:R00017; 方法的参数已超过5个
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerR00017 : DiagnosticAnalyzer
    {
        /// <summary>
        /// 分析Id
        /// </summary>
        public const string DiagnosticId = "R00017";

        /// <summary>
        /// 分类
        /// </summary>
        private const string Category = "程序集扫描";

        /// <summary>
        /// 标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:R00017";

        /// <summary>
        /// 消息内容
        /// </summary>
        private static readonly string MessageFormat = "方法的参数已超过5个";

        /// <summary>
        /// 错误描述
        /// </summary>
        private static readonly string Description = "程序集扫描>SPEC:R00017; 方法的参数已超过5个";

        /// <summary>
        /// 校验规则
        /// </summary>
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description,
            CommonHelper.HelpLinkUri);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }
        
        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            IMethodSymbol methodSymbol = (IMethodSymbol)context.Symbol;
            // 如果方法参数大于5个
            if (methodSymbol.Parameters.Length > 5)
            {
                Diagnostic diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    }
}
