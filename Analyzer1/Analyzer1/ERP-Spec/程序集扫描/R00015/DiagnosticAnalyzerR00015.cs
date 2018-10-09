using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using DiagnosticTools.Utilities;

namespace CodeAnalyzer.ERP_Spec.程序集扫描.R00015
{
    /// <summary>
    /// SPEC:R00015; 方法名称的首字母不是大写开头
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerR00015 : DiagnosticAnalyzer
    {
        /// <summary>
        /// 分析Id
        /// </summary>
        public const string DiagnosticId = "R00015";

        /// <summary>
        /// 分类
        /// </summary>
        private const string Category = "程序集扫描";

        /// <summary>
        /// 标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:R00015";

        /// <summary>
        /// 消息内容
        /// </summary>
        private static readonly string MessageFormat = "方法名称的首字母不是大写开头";

        /// <summary>
        /// 错误描述
        /// </summary>
        private static readonly string Description = "程序集扫描>SPEC:R00015; 方法名称的首字母不是大写开头";

        /// <summary>
        /// 校验规则
        /// </summary>
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Info, true, Description,
            CommonHelper.HelpLinkUri);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }
        
        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            IMethodSymbol method = (IMethodSymbol)context.Symbol;

            if (method.Name[0].IsUpper() == false && method.GetAttributes().Any(a => a.AttributeClass.Name == "SpecialName") == false)
            {
                // 此处判断不准确，应当用typeof判断
                if (method.Parameters.Length == 2
                    && method.DeclaredAccessibility != Accessibility.Public
                    && method.Parameters[0].Type.Name == "Object" && method.Parameters[0].Name == "sender"
                    && method.Parameters[1].Type.Name == "EventArgs" && method.Parameters[1].Name == "e")
                {
                    // 这是标准的事件订阅代码，而且还是IDE自动生成的，因此先允许这种代码。
                }
                else
                {
                    Diagnostic diagnostic = Diagnostic.Create(Rule, method.Locations[0], method.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    }
}
