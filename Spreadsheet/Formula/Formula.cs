// Change log:
// Last updated: 9/8, updated for non-nullable types

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private string formulaString = ""; //store the formula into syntax correctly string
        private HashSet<String> variables = new();//store all the variables

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            IEnumerable<string> substrings = Formula.GetTokens(formula);
            int count = 0;//number of token
            int leftP = 0;//number of left parentheses
            int rightP = 0;//number of right parentheses
            string temp;//store the normalized string
            bool hasOpenP = false; //check for following problem
            bool extraFollow = false; //check for extra following problem

            //valid variable pattern
            string pattern = "^[A-Za-z_]+[0-9]+$";
            
            foreach (string i in substrings)
            {
                count++;

                double tempDouble;
                //if i is a number
                if (double.TryParse(i, out tempDouble))
                {
                    if (extraFollow)//handle extro follow problem
                    {
                        throw new FormulaFormatException("syntactically incorrect: an opening parenthesis or an operator must follow by a number a variable or an opening parenthesis");
                    }
                    hasOpenP = false;
                    extraFollow = true;
                    formulaString += tempDouble;
                }
                else if (i.Equals("+") || i.Equals("-") || i.Equals("*") || i.Equals("/"))
                {
                    if(count == 1)
                    {
                        throw new FormulaFormatException("syntactically incorrect: formula must start with a number, a varibale or a openthesis");
                    }
                    else if (hasOpenP)
                    {
                        throw new FormulaFormatException("syntactically incorrect: an opening parenthesis or an operator must follow by a number a variable or an opening parenthesis");
                    }
                    formulaString += i;
                    hasOpenP = true;
                    extraFollow = false;
                }
                else if (i.Equals("("))
                {
                    if (extraFollow)
                    {
                        throw new FormulaFormatException("syntactically incorrect: an opening parenthesis or an operator must follow by a number a variable or an opening parenthesis");
                    }
                    leftP++;
                    hasOpenP = true;
                    formulaString += i;
                }
                else if (i.Equals(")"))
                {
                    if (count == 1)
                    {
                        throw new FormulaFormatException("syntactically incorrect: formula must start with a number, a varibale or a openthesis");
                    }else if (hasOpenP)
                    {
                        throw new FormulaFormatException("syntactically incorrect: an opening parenthesis or an operator must follow by a number a variable or an opening parenthesis");
                    }
                    extraFollow = true;
                    rightP++;
                    formulaString += i;
                }
                else if(isValid(i)&&Regex.IsMatch(i, pattern))//if it's a valid variable
                {
                    if (extraFollow)
                    {
                        throw new FormulaFormatException("syntactically incorrect: an opening parenthesis or an operator must follow by a number a variable or an opening parenthesis");
                    }
                    temp = normalize(i);
                    if (isValid(temp))
                    {
                        variables.Add(temp);
                        formulaString += temp;
                        hasOpenP = false;
                        extraFollow = true;
                    }
                    else
                    {
                        throw new FormulaFormatException("syntactically incorrect: illegal vaiable after apply normalizer");
                    }
                }
                else//it's not a valid token
                {
                    throw new FormulaFormatException("syntactically incorrect");
                }

            }

            if (count == 0)//if it's an empty string
            {
                throw new FormulaFormatException("syntactically incorrect: empty formula");
            }

            
            if (leftP != rightP)//unbalance parenterness problem
            {
                throw new FormulaFormatException("syntactically incorrect: number of parentheses is not eqaul");
            }else 
            {
                string last = formulaString[formulaString.Length - 1] + "";//check last token
                if (last.Equals("(") || last.Equals("+") || last.Equals("-") || last.Equals("*") || last.Equals("/"))
                {
                    throw new FormulaFormatException("syntactically incorrect: formula must end with a number, a variable or a closing parenthesis");
                }
                    
            }

        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            //this is the driver's method
            //evaluate is the calculating method
            //evaluate may involve in recursion depends on parentheses
            //object result = evaluate(formulaString, lookup);
            //return result;
            //create two stacks
            Stack<double> valueStack = new Stack<double>();
            Stack<String> operatorStack = new Stack<String>();

            //split the exp into string array
            String[] substrings = Regex.Split(formulaString, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //loop through the string array
            for (int i = 0; i < substrings.Length; i++)
            {

                //skip the empty space
                if (substrings[i].Equals("") || substrings[i].Equals(" "))
                {
                    continue;
                }

                double tempDouble;
                //if t is a number
                if (double.TryParse(substrings[i], out tempDouble))
                {
                    object value;
                    if (popOnce(operatorStack, valueStack, tempDouble, out value)) { }
                    else
                    {
                        return value;
                    }

                }
                //if t is + or -
                else if (substrings[i].Equals("+") || substrings[i].Equals("-"))
                {
                    //if top is + or -
                    if (checkOperator("+", "-", operatorStack))
                    {
                        object value;
                        if (popTwice(operatorStack, valueStack, out value))
                        {
                            double temp = (double)value;
                            valueStack.Push(temp);
                            operatorStack.Push(substrings[i]);
                        }
                        else
                        {
                            return value;
                        }

                    }
                    else
                    {
                        operatorStack.Push(substrings[i]);
                    }
                }
                // if t is * / or (
                else if (substrings[i].Equals("*") || substrings[i].Equals("/"))
                {
                    //if top is + or -
                    if (checkOperator("*", "/", operatorStack))
                    {
                        object value;
                        if (popTwice(operatorStack, valueStack, out value))
                        {
                            double temp = (double)value;
                            valueStack.Push(temp);
                            operatorStack.Push(substrings[i]);
                        }
                        else
                        {
                            return value;
                        }
                    }
                    else
                    {
                        operatorStack.Push(substrings[i]);
                    }
                }
                else if (substrings[i].Equals("("))
                {
                    operatorStack.Push(substrings[i]);

                }
                else if (substrings[i].Equals(")"))
                {
                    if (checkOperator("*", "/", operatorStack))
                    {
                        object value;
                        if (popTwice(operatorStack, valueStack, out value))
                        {
                            double temp = (double)value;
                            valueStack.Push(temp);
                            operatorStack.Push(substrings[i]);
                        }
                        else
                        {
                            return value;
                        }
                        operatorStack.Pop();
                    }
                    else
                    {
                        object value;
                        if (popTwice(operatorStack, valueStack, out value))
                        {
                            double temp = (double)value;
                            valueStack.Push(temp);
                            operatorStack.Pop();
                        }
                        else
                        {
                            return value;
                        }
                    }
                }
                else//if t is a varaible
                {
                    try
                    {
                        double variable = lookup(substrings[i]);
                        object value;
                        if (popOnce(operatorStack, valueStack, variable, out value))
                        {

                        }
                        else
                        {
                            return value;
                        }

                    }
                    catch (ArgumentException)//the variable dose not contain a value or it's a illegal token
                    {
                        return new FormulaError("lookup can't find the variable");
                    }
                }
            }

            //pop the remaining values
            while (operatorStack.Count > 0)
            {
                object value;
                if (popTwice(operatorStack, valueStack, out value))
                {
                    double temp = (double)value;
                    valueStack.Push(temp);
                }
                else
                {
                    return value;
                }
            }

            return valueStack.Pop();

        }

        /// <summary>
        /// This method check the top operator in the operatorStack
        /// and check if it matches the given operators
        /// </summary>
        /// <param name="oper1"></param>
        /// <param name="oper2"></param>
        /// <param name="operatorStack"></param>
        /// <returns></returns>
        private static bool checkOperator(String oper1, String oper2, Stack<String> operatorStack)
        {
            try
            {
                if (operatorStack.Peek().Equals(oper1) || operatorStack.Peek().Equals(oper2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //handle the case when the operator stack is empty
            catch (System.InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Apply the string operator into numbers
        /// </summary>
        /// <param name="x">number on the left of the operator</param>
        /// <param name="y">number on the right of the operator</param>
        /// <param name="oper"></param>
        /// <returns>the calculate result with the given operator</returns>
        /// <exception cref="ArgumentException"></exception>
        private static bool Operating(double x, double y, String oper, out object value)
        {
            if (oper.Equals("*"))
            {
                value = x * y;
                return true;
            }else if (oper.Equals("/"))
            {
                if (y == 0)
                {
                    value = new FormulaError("dividing by zero occur");
                    return false;
                }
                else
                {
                    value = x / y;
                    return true;
                }
            }else if (oper.Equals("+"))
            {
                value = x + y;
                return true;
            }else
            {
                value = x - y;
                return true;
            }
        }

        /// <summary>
        /// This method pop the value stack twice and operator stack once, then
        /// apply the calculation
        /// </summary>
        /// <param name="operatorStack"></param>
        /// <param name="valueStack"></param>
        /// <returns>the calculate result</returns>
        private static bool popTwice(Stack<String> operatorStack, Stack<double> valueStack,out object result)
        {
            double right = valueStack.Pop();
            double left = valueStack.Pop();
            string oper = operatorStack.Pop();
            //object result;
            if(Operating(left, right, oper, out result))
            {
                //value = result;
                return true;
            }
            else
            {
                //value = result;
                return false;
            }
            

        }

        /// <summary>
        /// This method pop the value stack twice and operator stack once, then
        /// apply the calculation
        /// </summary>
        /// <param name="operatorStack"></param>
        /// <param name="valueStack"></param>
        /// <returns>the calculate result</returns>
        private static bool popOnce(Stack<String> operatorStack, Stack<double> valueStack, double right,out object result)
        {
            if (checkOperator("*", "/", operatorStack))
            {
                double left = valueStack.Pop();
                string oper = operatorStack.Pop();
                //object result;
                if (Operating(left, right, oper, out result))
                {
                    double value = (double)result;
                    valueStack.Push(value);
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                result = 0;
                valueStack.Push(right);
                return true;
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<String> s = variables;
            return s;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return formulaString;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if(obj == null)//check if it's null
            {
                return false;
            }else if(obj.GetType() != typeof(Formula))//check if it's not Formula type
            {
                return false;
            }
            else
            {
                string? str = obj.ToString();

                if (formulaString==str)//check if the string is equal
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that f1 and f2 cannot be null, because their types are non-nullable
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            string str1 = f1.ToString();
            string str2 = f2.ToString();

            if (str1.Equals(str2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that f1 and f2 cannot be null, because their types are non-nullable
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if(f1 == f2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            //formulaString is a syntax correctly string, so the GetHashCode will be same if the string is the same
            return formulaString.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }


}
