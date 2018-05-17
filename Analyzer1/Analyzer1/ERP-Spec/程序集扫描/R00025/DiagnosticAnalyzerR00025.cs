using System.Collections.Immutable;
using DiagnosticTools.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodeAnalyzer.程序集扫描.R00025
{
    /// <summary>
    ///     SPEC:R00025; 服务类型的公开实例方法需要定义成虚方法
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DiagnosticAnalyzerR00025 : DiagnosticAnalyzer
    {
        /// <summary>
        ///     分析Id
        /// </summary>
        public const string DiagnosticId = "R00025";

        /// <summary>
        ///     分类
        /// </summary>
        private const string Category = "程序集扫描";

        /// <summary>
        ///     标题
        /// </summary>
        private static readonly string Title = "hoof.CodeAnalyzer.SPEC:R00025";

        /// <summary>
        ///     消息内容
        /// </summary>
        private static readonly string MessageFormat = "服务类型的公开实例方法需要定义成虚方法";

        /// <summary>
        ///     错误描述
        /// </summary>
        private static readonly string Description = "程序集扫描>SPEC:R00025; 服务类型的公开实例方法需要定义成虚方法";

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
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            IMethodSymbol methodSymbol = (IMethodSymbol) context.Symbol;
            if (methodSymbol.Name.StartsWith("get_") || methodSymbol.Name.StartsWith("set_"))
            {
                return;
            }
            INamedTypeSymbol baseType = methodSymbol.ContainingType.BaseType;
            if (
                methodSymbol.DeclaredAccessibility == Accessibility.Public &
                methodSymbol.ContainingType.TypeKind == TypeKind.Class && 
                methodSymbol.ContainingType.Name.EndsWith("Service") &&
                methodSymbol.IsStatic == false &&
                CommonHelper.IsVirtualMethod(baseType, methodSymbol) == false
                )
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