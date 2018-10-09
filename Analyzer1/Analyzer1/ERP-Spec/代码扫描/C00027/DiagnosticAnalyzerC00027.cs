using System.Collections.Immutable;
using System.IO;
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
    public class DiagnosticAnalyzerC00027 : DiagnosticAnalyzer
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
            var method = (MethodDeclarationSyntax)context.Node;

            //if (method?.Body != null && method.Body.Statements.Count > 50)
            //{
            //    var diagnostic = Diagnostic.Create(Rule, method.Body.Statements[50 - 1].GetLocation(), method.Body.Statements.Count);
            //    context.ReportDiagnostic(diagnostic);
            //}

            if (method?.Body != null)
            {
                var body = method?.Body.GetText();

                int lineCount = 0;


                //一下代码移植自代码扫描SpecChecker，无任何修改。
                using (StringReader reader = new StringReader(method?.Body.GetText().ToString()))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        if (line.Length < 5) // 忽略无效的代码，例如：花括号
                            continue;

                        if (line == "continue;"
                            || line == "break;"
                        //|| line.EndsWith('=')	// 赋值语句折行
                        //|| line.EndsWith("(")	// 方法调用拆行
                        //|| line.EndsWith(',')	// 赋值或者调用方法拆行						
                        //|| line.StartsWith("&&")	// 判断逻辑拆行
                        //|| line.EndsWith("&&")		// 判断逻辑拆行
                        //|| line.StartsWith("||")	// 判断逻辑拆行
                        //|| line.EndsWith("||")		// 判断逻辑拆行
                        //|| line.StartsWith("?")		// 判断逻辑拆行
                        //|| line.StartsWith(":")		// 判断逻辑拆行
                        //|| (line.IndexOf(" = new ") > 0 && line.IndexOf(',') < 0)	// 创建对象拆行
                        )
                            continue;

                        //if( line.StartsWith("//") ) {   // 注释行
                        //	if( CommentRule.GetWordCount(line) >= 3 )
                        //		commentCount++;
                        //}
                        //else {
                        if (line.StartsWith("if")
                            || line.StartsWith("for")
                            || line.StartsWith("foreach")
                            || line.StartsWith("while")
                            || line.EndsWith(";")
                        )
                            lineCount++;
                        //}

                        // 只要包含合理数量的汉字，就认为是一个有效的【注释行】，
                        // 因为有时候就是处理汉字消息，再写汉字注释就显得多余了
                        //if (CommentRule.GetWordCount(line) >= 3)
                        //    commentCount++;

                    }
                }


                if (lineCount > 50)
                {
                    var diagnostic = Diagnostic.Create(Rule, method.GetLocation(), method.Body.Statements.Count);
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
