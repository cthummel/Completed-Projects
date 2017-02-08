// Written by Joe Zachary for CS 3500, January 2017.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Unexpected input parameter.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("3 + 4(");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("((6*(5-4)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {
            Formula f = new Formula("(6 * (5.0))) - 4");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct8()
        {
            Formula f = new Formula(")+0");
        }


        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2 + 3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        [TestMethod]
        public void Evaluate1a()
        {
            Formula f = new Formula("2 + 3 + 4 - 5 + 5 + 6");
            Assert.AreEqual(f.Evaluate(v => 0), 15.0, 1e-6);
        }
        [TestMethod]
        public void Evaluate1b()
        {
            Formula f = new Formula("10*10/100*90 / 3");
            Assert.AreEqual(f.Evaluate(v => 0), 30.0, 1e-6);
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate1c()
        {
            Formula f = new Formula("3 * 3 / 0");
            f.Evaluate(v => 0);
            
        }
        [TestMethod]
        public void Evaluate1d()
        {
            Formula f = new Formula("x * 5");
            Assert.AreEqual(f.Evaluate(v => 5), 25.0, 1e-6);
        }
        [TestMethod]
        public void Evaluate1e()
        {
            Formula f = new Formula("x / 5");
            Assert.AreEqual(f.Evaluate(v => 5), 1.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }
        [TestMethod]
        public void Evaluate4a()
        {
            Formula f = new Formula("x * y");
            Assert.AreEqual(f.Evaluate(Lookup4), 24.0, 1e-6);
        }
        [TestMethod]
        public void Evaluate4b()
        {
            Formula f = new Formula("x / y");
            Assert.AreEqual(f.Evaluate(Lookup4), .66, .1);
        }
        

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5 ()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }
        [TestMethod]
        public void Evaluate5a()
        {
            Formula f = new Formula("(x * y) * (z / x)");
            Assert.AreEqual(f.Evaluate(Lookup4), 48.0, 1e-6);
        }

        

        [TestMethod]
        public void Evaluate5b()
        {
            Formula f = new Formula("(x + y) / (z - x)");
            Assert.AreEqual(f.Evaluate(Lookup4), 2.5, 1e-6);
        }
        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        [TestMethod]
        public void Evaluate5c()
        {
            Formula f = new Formula("(x * 5e2) * (3 / 3)");
            Assert.AreEqual(f.Evaluate(v => 2), 1000.0, 1e-6);
        }




        /// <summary>
        /// Basic checking of null arguments.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PS4aTest0a()
        {
            string form = null;
            Formula f = new Formula(form);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PS4aTest0b()
        {
            string form = null;
            Formula f = new Formula(form, s => s.ToUpper(), v => Regex.IsMatch(v, "^[A-Z][0-9]$"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PS4aTest0c()
        {
            string form = "4 + 6 + 8 * 9";
            Formula f = new Formula(form, null, v => Regex.IsMatch(v, "^[A-Z][0-9]$"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PS4aTest0d()
        {
            string form = "4 + 6 + 8 * 9";
            Formula f = new Formula(form, s => s.ToUpper(), null);
        }

        /// <summary>
        /// Checking the empty constructor provided by compiler.
        /// </summary>
        [TestMethod]
        public void PS4aTest1()
        {
            Formula f = new Formula();
            Assert.AreEqual("0", f.ToString());
        }
        [TestMethod]
        public void PS4aTest1a()
        {
            Formula f = new Formula();
            var Variables = new List<string>();
            foreach (string var in f.GetVariables())
            {
                Variables.Add(var);
            }

            Assert.AreEqual("0", f.ToString());
            Assert.AreEqual(0, Variables.Count);
        }

        /// <summary>
        /// Testing the 3 element constuctor works like the 1 element constructor with trivial N and V.
        /// </summary>
        [TestMethod]
        public void PS4aTest2()
        {
            Formula f = new Formula("2 + 3 + 5", s => s, s => true);
            Assert.AreEqual(f.Evaluate(v => 2), 10, 1e-6);
        }
        [TestMethod]
        public void PS4aTest3()
        {
            Formula f = new Formula("2 + 3 + 5");
            Assert.AreEqual(f.Evaluate(v => 2), 10, 1e-6);
        }
        
        [TestMethod]
        public void PS4aTest4()
        {
            Formula f = new Formula("0");
            Formula f1 = new Formula();
            
            Assert.AreEqual(f.Evaluate(v => 2), 0, 1e-6);
            Assert.AreEqual(f1.Evaluate(v => 2), 0, 1e-6);
            
        }
        [TestMethod]
        public void PS4aTest5()
        {
            Formula f = new Formula("x2+y3", s => s.ToUpper(), v => Regex.IsMatch(v, "^[A-Z][0-9]$"));

            Assert.AreEqual("X2+Y3", f.ToString());
            Assert.AreNotEqual("x2+Y3", f.ToString());
            Assert.AreNotEqual("X2+y3", f.ToString());
            Assert.AreNotEqual("x2+y3", f.ToString());
            Assert.AreEqual(f.Evaluate(v => 2), 4, 1e-6);
        }
        [TestMethod]
        public void PS4aTest6()
        {
            Formula f = new Formula("x2+y3", s => s.ToUpper(), v => Regex.IsMatch(v, "^[A-Z][0-9]$"));
            var Variables = new List<string>();
            foreach (string var in f.GetVariables())
            {
                Variables.Add(var);
            }

            Assert.AreEqual("X2+Y3", f.ToString());
            Assert.AreNotEqual("x2+Y3", f.ToString());
            Assert.AreNotEqual("X2+y3", f.ToString());
            Assert.AreNotEqual("x2+y3", f.ToString());
            Assert.AreEqual(f.Evaluate(v => 2), 4, 1e-6);
            Assert.AreEqual(Variables.ElementAt(0), "X2");
            Assert.AreEqual(Variables.ElementAt(1), "Y3");
            Assert.AreNotEqual(Variables.ElementAt(0), "x2");
            Assert.AreNotEqual(Variables.ElementAt(1), "y3");
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PS4aTest7()
        {
            Formula f = new Formula("x2+y3", s => s.ToUpper(), v => Regex.IsMatch(v, "^[a-z][0-9]$"));
        }
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PS4aTest8()
        {
            Formula f = new Formula("x2+y3", s => "++++++", v => Regex.IsMatch(v, "^[A-Z][0-9]$"));
        }

    }
}
