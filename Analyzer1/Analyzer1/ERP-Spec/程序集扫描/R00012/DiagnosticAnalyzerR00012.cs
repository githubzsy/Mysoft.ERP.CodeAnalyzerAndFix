using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using DiagnosticTools.Utilities;

namespace CodeAnalyzer.ERP_Spec.程序集扫描.R00012
{
    /// <summary>
    /// SPEC:R00012; 公开（或常量）字段的名称的首字母不是大写开头
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerR00012 : DiagnosticAnalyzer
    {
        /// <summary>
        /// 分析Id
        /// </summary>
        public const string DiagnosticId = "R00012";

        /// <summary>
        /// 分类
        /// </summary>
        private const string Category = "程序集扫描";

        /// <summary>
        /// 标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:R00012";

        /// <summary>
        /// 消息内容
        /// </summary>
        private static readonly string MessageFormat = "公开（或常量）字段的名称的首字母不是大写开头";

        /// <summary>
        /// 错误描述
        /// </summary>
        private static readonly string Description = "程序集扫描>SPEC:R00012; 公开（或常量）字段的名称的首字母不是大写开头";

        /// <summary>
        /// 校验规则
        /// </summary>
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Info, true, Description,
            CommonHelper.HelpLinkUri);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeField, SymbolKind.Field);
        }

        /// <summary>
        /// 验证字段
        /// </summary>
        /// <param name="context"></param>
        private void AnalyzeField(SymbolAnalysisContext context)
        {
            IFieldSymbol symbol = (IFieldSymbol)context.Symbol;
            if (symbol.DeclaredAccessibility == Accessibility.Public && symbol.Name[0].IsUpper() == false)
            {
                Diagnostic diagnostic = Diagnostic.Create(Rule, symbol.Locations[0], symbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    }
}
