using CodeAnalyzer.ERP特殊规范.RM0015;
using CodeAnalyzer.Test.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeAnalyzer.Test.UnitTests
{
    [TestClass]
    public class Rm0015UnitTest : Helpers.Verifiers.CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
      public class TypeNameAppService
        {   
            public void Method1()
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "RM0015",
                Message = "AppService的方法必须用[ActionDescription]标记",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 13, 25)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            //        var fixtest = @"
            //using System;
            //using System.Collections.Generic;
            //using System.Linq;
            //using System.Text;
            //using System.Threading.Tasks;
            //using System.Diagnostics;

            //namespace ConsoleApplication1
            //{
            //  public class TypeNameAppService
            //    {   
            //        [ActionDescription]
            //        public void Method1()
            //    }
            //}";
            //        VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new Rm0015CodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Rm0015DiagnosticAnalyzer();
        }
    }
}