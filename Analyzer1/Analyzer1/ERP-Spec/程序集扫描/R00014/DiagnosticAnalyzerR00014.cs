using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.ERP_Spec.程序集扫描.R00014
{
    /// <summary>
    /// SPEC:R00014; 私有实例字段的名称不是以 _ 开头
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerR00014 : DiagnosticAnalyzer
    {
        /// <summary>
        /// 分析Id
        /// </summary>
        public const string DiagnosticId = "R00014";

        /// <summary>
        /// 分类
        /// </summary>
        private const string Category = "程序集扫描";

        /// <summary>
        /// 标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:R00014";

        /// <summary>
        /// 消息内容
        /// </summary>
        private static readonly string MessageFormat = "私有实例字段的名称不是以 _ 开头";

        /// <summary>
        /// 错误描述
        /// </summary>
        private static readonly string Description = "程序集扫描>SPEC:R00014; 私有实例字段的名称不是以 _ 开头";

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
            // 是私有静态字段但是不以s_开头
            if (symbol.DeclaredAccessibility == Accessibility.Private && symbol.IsStatic == false && symbol.Name.StartsWith("_") == false)
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
