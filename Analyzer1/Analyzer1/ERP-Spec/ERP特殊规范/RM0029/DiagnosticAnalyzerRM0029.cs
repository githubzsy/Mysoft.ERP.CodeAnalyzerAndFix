using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

// ReSharper disable once CheckNamespace
namespace CodeAnalyzer
{
    /// <summary>
    ///     SPEC:R00025; 服务类型的公开实例方法需要定义成虚方法
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerRM0029 : DiagnosticAnalyzer
    {
        /// <summary>
        ///     分析Id
        /// </summary>
        public const string DiagnosticId = "RM0029";

        /// <summary>
        ///     分类
        /// </summary>
        private const string Category = "程序集扫描";

        /// <summary>
        ///     标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:RM0029";

        /// <summary>
        ///     消息内容
        /// </summary>
        private static readonly string MessageFormat = "服务类型的公开实例方法不能是封闭方法";

        /// <summary>
        ///     错误描述
        /// </summary>
        private static readonly string Description = "ERP特殊规范>SPEC：RM0029; 服务类型的公开实例方法不能是封闭方法";

        /// <summary>
        ///     校验规则
        /// </summary>
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, true, Description,
            CommonHelper.helpLinkUri);

        /// <summary>
        /// Called once at session start to register actions in the analysis context.
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;
            var baseType = methodSymbol?.ContainingType.BaseType;
            if (baseType != null && baseType.Name == "AppService" && methodSymbol.IsVirtual == false)
            {
                var diagnostic = Diagnostic.Create(Rule, methodSymbol.Locations[0], methodSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
    }
}