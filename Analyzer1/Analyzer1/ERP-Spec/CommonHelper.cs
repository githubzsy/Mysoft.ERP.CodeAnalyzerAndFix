using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace CodeAnalyzer
{
    public class CommonHelper
    {
        public static readonly string HelpLinkUri = @"http://spec.mysoft.com.cn:55768/static/page/erp-spec.html";


        /// <summary>
        /// 是否为虚方法
        /// </summary>
        /// <param name="baseType">基类</param>
        /// <param name="methodSymbol">当前方法</param>
        /// <returns>是否为虚方法</returns>
        public static bool IsVirtualMethod(INamedTypeSymbol baseType, IMethodSymbol methodSymbol)
        {
            bool isVirtual = methodSymbol.IsVirtual;

            if (isVirtual == true || baseType == null)
            {
                return isVirtual;
            }

            var methods = baseType.GetMembers(methodSymbol.Name).ToImmutableList();

            if (methods.Exists(f => f.IsVirtual == true))
            {
                isVirtual = true;
            }
            else
            {
                if (IsVirtualMethod(baseType.BaseType, methodSymbol))
                {
                    isVirtual = true;
                }
            }

            return isVirtual;
        }
    }
}