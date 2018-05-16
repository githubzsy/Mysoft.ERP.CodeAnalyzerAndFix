using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DiagnosticTools.Utilities
{
    public static class RoslynUtils
    {
        public static ExpressionSyntax GetInstantiationExpressionFromType(string typeName, TypeArgumentListSyntax typeArgumentsSyntax)
        {
            // 0: Name of type
            // 1: (Optional) generic arguments
            // 2: (Optional) arguments
            var result = " new {0}{1}({2})";
            var typeArguments = GetParsedTypeArguments(typeArgumentsSyntax);
            var arguments = ""; // Not implemented until I find out a place where to use them

            return SyntaxFactory.ParseExpression(string.Format(result, typeName, typeArguments, arguments));
        }

        private static string GetParsedTypeArguments(TypeArgumentListSyntax arguments)
        {
            if (arguments.Arguments.Count == 0) { return ""; }

            var sb = new StringBuilder("<");
            for (var i = 0; i < arguments.Arguments.Count; i++)
            {
                sb.Append(arguments.Arguments[i].ToString());
                if (i != arguments.Arguments.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(">");
            return sb.ToString();
        }
    }
}