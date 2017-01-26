// Skeleton written by Joe Zachary for CS 3500, January 2017

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula

    {
        private string output;
        private String lpPattern = @"\(";
        private String rpPattern = @"\)";
        private String opPattern = @"[\+\-*/]";
        private String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
        private String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
        private String altdouble = @"^\d+(,\d+)*(\.\d+(e\d+)?)?$";

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            if (formula == "")
            {
                throw new FormulaFormatException("No Input");
            }

            var Operators = new List<string>();
            var Values = new List<string>();
            IEnumerable<string> tokens = Formula.GetTokens(formula);

            bool failure = true;
            int parcount = 0;

            string first = tokens.ElementAt<string>(0);
            string last = tokens.ElementAt<string>(tokens.Count()-1);
            string previous = "";

            //Checks first element in formula.
            if (Regex.IsMatch(first, rpPattern) || Regex.IsMatch(first, opPattern))
            {
                throw new FormulaFormatException("The first element of the formula is not either a (, number, or variable.");
            }
            
            //Checks last element in formula.
            if (Regex.IsMatch(last, lpPattern) || Regex.IsMatch(last, opPattern))
            {
                throw new FormulaFormatException("The last element of the formula is not either a ), number, or variable.");
            }

           


            foreach (string t in tokens)
            {
                //Checks for invalid input tokens.
                if (!Regex.IsMatch(t, lpPattern) && !Regex.IsMatch(t, rpPattern) && !Regex.IsMatch(t, altdouble)
                    && !Regex.IsMatch(t, varPattern) && !Regex.IsMatch(t, opPattern))
                {
                    throw new FormulaFormatException("Unexpected object in formula: " + t);
                }

                //Checks token for ( and adds to Operator list.
                if (Regex.IsMatch(t,lpPattern))
                {
                    Operators.Add(t);
                    parcount += 1;
                }

                //Checks token for ) and adds to Operator list.
                if (Regex.IsMatch(t, rpPattern))
                {
                    Operators.Add(t);
                    parcount -= 1;
                }

                //Checks token for valid operators and adds to Operator list.
                if (Regex.IsMatch(t, opPattern))
                {
                    Operators.Add(t);
                }

                //Checks token for numbers or variables and adds to Values list.
                if (Regex.IsMatch(t, altdouble) || Regex.IsMatch(t, varPattern))
                {
                    Values.Add(t);
                }

                //Checks proper formatting following ( and operators.
                if (Regex.IsMatch(previous, lpPattern) || Regex.IsMatch(previous, opPattern))
                {
                    if (!Regex.IsMatch(t, varPattern) && !Regex.IsMatch(t, altdouble) && !Regex.IsMatch(t, lpPattern))
                    {
                        throw new FormulaFormatException("Check 5: Formula containing " + previous + " followed by " + t + " is invalid.");
                    }
                }

                //Checks proper formatting following ), numbers, and variables.
                if (Regex.IsMatch(previous, rpPattern) || Regex.IsMatch(previous, altdouble) || Regex.IsMatch(previous, varPattern))
                {
                    if (!Regex.IsMatch(t, opPattern) && !Regex.IsMatch(t, rpPattern))
                    {
                        throw new FormulaFormatException("Check 6: `Formula containing " + previous + " followed by " + t + " is invalid.");
                    }

                    
                }

                //Checks relative parenthesis count.
                if (parcount < 0)
                {
                    throw new FormulaFormatException("Parenthesis mismatch: Formula contains " + parcount + " More ) than ( .");
                }

                previous = t;

            }
            if (parcount != 0)
            {
                throw new FormulaFormatException("Parenthesis mismatch: Formula contains More ( than ) .");
            }

            output = formula;

        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        { 
            var Operators = new Stack<string>();
            var Values = new Stack<string>();
            IEnumerable<string> tokens = Formula.GetTokens(output);

            double FinalAnswer = 0;
            bool keepgoing = true;

            

            foreach (string t in tokens)
            {
                
                //Current item is a double.
                if (Regex.IsMatch(t, altdouble))
                {
                    if (Operators.Count == 0)
                    {
                        Values.Push(t);
                    }

                    if (Operators.Count != 0)
                    {
                        if (Operators.Peek() == "*" || Operators.Peek() == "/")
                        {
                            if (Operators.Peek() == "*")
                            {
                                double result = double.Parse(Values.Pop()) * double.Parse(t);
                                Values.Push(result.ToString());
                                Operators.Pop();
                            }
                            else
                            {
                                if (double.Parse(t) == 0)
                                {
                                    throw new FormulaEvaluationException("Cannot divide by zero.");
                                }

                                double result = double.Parse(Values.Pop()) / double.Parse(t);
                                Values.Push(result.ToString());
                                Operators.Pop();
                                
                            }
                        }
                        else
                        {
                            Values.Push(t);
                        }
                    }                  

                    
                }
                
                if (Regex.IsMatch(t, varPattern))
                {
                    if (double.IsNaN(lookup(t)))
                    {
                        throw new UndefinedVariableException("No specification for variable values.");
                    }

                    if (Operators.Count == 0)
                    {
                        Values.Push(t);
                    }

                    if (Operators.Count != 0)
                    {
                        if (Operators.Peek() == "*" || Operators.Peek() == "/")
                        {
                            if (Operators.Peek() == "*")
                            {
                                if (Regex.IsMatch(Values.Peek(), varPattern))
                                {
                                    double first = lookup(Values.Pop());
                                    double second = lookup(t);
                                    double result = first * second;
                                    Values.Push(result.ToString());
                                    Operators.Pop();
                                }
                                else
                                {
                                    double result = double.Parse(Values.Pop()) * lookup(t);
                                    Values.Push(result.ToString());
                                    Operators.Pop();
                                }
                                
                            }
                            else
                            {
                                if (Regex.IsMatch(Values.Peek(), varPattern))
                                {
                                    double first = lookup(Values.Pop());
                                    double second = lookup(t);
                                    double result = first / second;
                                    Values.Push(result.ToString());
                                    Operators.Pop();
                                }
                                else
                                {
                                    double result = double.Parse(Values.Pop()) / lookup(t);
                                    Values.Push(result.ToString());
                                    Operators.Pop();
                                }
                                
                            }
                        }

                        else
                        {
                            Values.Push(t);
                        }
                    }

                    

                }

                if (t == "+" || t == "-")
                {
                    if (Operators.Count != 0)
                    {
                        if (Operators.Peek() == "+" || Operators.Peek() == "-")
                        {
                            if (Operators.Peek() == "+")
                            {
                                double second = double.Parse(Values.Pop());
                                double first = double.Parse(Values.Pop());
                                double result = first + second;
                                Values.Push(result.ToString());
                                Operators.Pop();
                            }
                            else
                            {
                                double second = double.Parse(Values.Pop());
                                double first = double.Parse(Values.Pop());
                                double result = first - second;
                                Values.Push(result.ToString());
                                Operators.Pop();
                            }

                        }
                    }
                    

                    Operators.Push(t);

                }

                if (t == "*" || t == "/")
                {
                    Operators.Push(t);
                }

                if (Regex.IsMatch(t, lpPattern))
                {
                    Operators.Push(t);
                }

                if (Regex.IsMatch(t, rpPattern))
                {
                    if (Regex.IsMatch(Operators.Peek(), lpPattern))
                    {
                        Operators.Pop();
                        keepgoing = false;
                    }
                    //4 cases. Value contains 2 variables, 1 variable and 1 value, 1 value and 1 variable, or 2 values.
                    while (keepgoing == true)
                    {
                        if (Operators.Peek() == "+" || Operators.Peek() == "-")
                        {
                            if (Operators.Peek() == "+")
                            {
                                while (keepgoing == true)
                                {
                                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), varPattern))
                                    {
                                        double second = lookup(Values.Pop());
                                        double first = lookup(Values.Pop());
                                        double result = first + second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), altdouble))
                                    {
                                        double second = lookup(Values.Pop());
                                        double first = double.Parse(Values.Pop());
                                        double result = first + second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), varPattern))
                                    {
                                        double second = double.Parse(Values.Pop());
                                        double first = lookup(Values.Pop());
                                        double result = first + second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), altdouble))
                                    {
                                        double second = double.Parse(Values.Pop());
                                        double first = double.Parse(Values.Pop());
                                        double result = first + second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                }

                            }
                            else
                            {
                                while (keepgoing == true)
                                {
                                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), varPattern))
                                    {
                                        double second = lookup(Values.Pop());
                                        double first = lookup(Values.Pop());
                                        double result = first - second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), altdouble))
                                    {
                                        double second = lookup(Values.Pop());
                                        double first = double.Parse(Values.Pop());
                                        double result = first - second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), varPattern))
                                    {
                                        double second = double.Parse(Values.Pop());
                                        double first = lookup(Values.Pop());
                                        double result = first - second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), altdouble))
                                    {
                                        double second = double.Parse(Values.Pop());
                                        double first = double.Parse(Values.Pop());
                                        double result = first - second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                }
                            }
                            Operators.Pop();
                        }

                        if (Operators.Peek() == "*" || Operators.Peek() == "/")
                        {
                            if (Operators.Peek() == "*")
                            {
                                while (keepgoing == true)
                                {
                                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), varPattern))
                                    {
                                        double second = lookup(Values.Pop());
                                        double first = lookup(Values.Pop());
                                        double result = first * second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), altdouble))
                                    {
                                        double second = lookup(Values.Pop());
                                        double first = double.Parse(Values.Pop());
                                        double result = first * second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), varPattern))
                                    {
                                        double second = double.Parse(Values.Pop());
                                        double first = lookup(Values.Pop());
                                        double result = first * second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), altdouble))
                                    {
                                        double second = double.Parse(Values.Pop());
                                        double first = double.Parse(Values.Pop());
                                        double result = first * second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                        break;
                                    }
                                }


                            }

                            else
                            {
                                while (keepgoing == true)
                                {
                                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), varPattern))
                                    {
                                        double second = lookup(Values.Pop());
                                        if (second == 0)
                                        {
                                            throw new FormulaEvaluationException("Cannot divide by zero.");
                                        }

                                        double first = lookup(Values.Pop());
                                        double result = first / second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), altdouble))
                                    {
                                        double second = lookup(Values.Pop());

                                        if (second == 0)
                                        {
                                            throw new FormulaEvaluationException("Cannot divide by zero.");
                                        }
                                        double first = double.Parse(Values.Pop());
                                        double result = first / second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), varPattern))
                                    {
                                        double second = double.Parse(Values.Pop());
                                        if (second == 0)
                                        {
                                            throw new FormulaEvaluationException("Cannot divide by zero.");
                                        }

                                        double first = lookup(Values.Pop());
                                        double result = first / second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                    }
                                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(Values.Count - 2), altdouble))
                                    {
                                        double second = double.Parse(Values.Pop());
                                        if (second == 0)
                                        {
                                            throw new FormulaEvaluationException("Cannot divide by zero.");
                                        }

                                        double first = double.Parse(Values.Pop());
                                        double result = first / second;
                                        Values.Push(result.ToString());
                                        //Operators.Pop();
                                        keepgoing = false;
                                    }
                                }

                            }
                        }
                    }
                    
                    
                    
                }
                keepgoing = true;

            }

            if (Operators.Count == 0)
            {
                if (Regex.IsMatch(Values.Peek(), varPattern))
                {
                    FinalAnswer = lookup(Values.Pop());
                }
                else
                {
                    FinalAnswer = double.Parse(Values.Pop());
                }
                
            }
            //4 cases. Value contains 2 variables, 1 variable and 1 value, 1 value and 1 variable, or 2 values.
            if (Operators.Count == 1)
            {
                if (Operators.Peek() == "+")
                {
                    if(Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(0), varPattern))
                    {
                        double second = lookup(Values.Pop());
                        double first = lookup(Values.ElementAt<string>(0));
                        FinalAnswer = first + second;
                    }
                    if(Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(0), altdouble))
                    {
                        double second = lookup(Values.Pop());
                        double first = double.Parse(Values.ElementAt<string>(0));
                        FinalAnswer = first + second;
                    }
                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(0), varPattern))
                    {
                        double second = double.Parse(Values.Pop());
                        double first = lookup(Values.ElementAt<string>(0));
                        FinalAnswer = first + second;
                    }
                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(0), altdouble))
                    {
                        double second = double.Parse(Values.Pop());
                        double first = double.Parse(Values.ElementAt<string>(0));
                        FinalAnswer = first + second;
                    }
                }
                else
                {
                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(0), varPattern))
                    {
                        double second = lookup(Values.Pop());
                        double first = lookup(Values.ElementAt<string>(0));
                        FinalAnswer = first - second;
                    }
                    if (Regex.IsMatch(Values.Peek(), varPattern) && Regex.IsMatch(Values.ElementAt<string>(0), altdouble))
                    {
                        double second = lookup(Values.Pop());
                        double first = double.Parse(Values.ElementAt<string>(0));
                        FinalAnswer = first - second;
                    }
                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(0), varPattern))
                    {
                        double second = double.Parse(Values.Pop());
                        double first = lookup(Values.ElementAt<string>(0));
                        FinalAnswer = first - second;
                    }
                    if (Regex.IsMatch(Values.Peek(), altdouble) && Regex.IsMatch(Values.ElementAt<string>(0), altdouble))
                    {
                        double second = double.Parse(Values.Pop());
                        double first = double.Parse(Values.ElementAt<string>(0));
                        FinalAnswer = first - second;
                    }
                }
                
            }
            
            return FinalAnswer;
        }
        


        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
        
        public double Looker(string var)
        {
            

            return 5;
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);
    
    

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
