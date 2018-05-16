using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace DiagnosticTools.Utilities
{
    public static class Extensions
    {
        public static bool IsSubclassOf(this ITypeSymbol symbol, Type type)
        {
            var @base = symbol.BaseType;

            while (@base != null)
            {
                if (@base.Name == type.Name) { return true; }
                @base = @base.BaseType;
            }

            return false;
        }

        public static bool ImplementsInterface(this ITypeSymbol symbol, Type type)
        {
            return symbol.Interfaces.Any(x => x.Name == type.Name);
        }

        public static bool IsCollection(this ITypeSymbol symbol)
        {
            return symbol.Interfaces.Any(x => x.Name == typeof(IEnumerable).Name
                                              || x.Name == typeof(IEnumerable<>).Name);
        }
    }
}