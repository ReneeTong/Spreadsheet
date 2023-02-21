using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator;
/// <summary>
/// This is the main class, it evaluate a math functions and return the result
/// </summary>
public static class Evaluator
{
    //the delegate will either return an int (the value of the variable)
    //or throw an ArgumentException (if the variable has no value).
    public delegate int Lookup(String v);

    /// <summary>
    /// This method takes in the equation and return the calculate result
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="variableEvaluator"></param>
    /// <returns>a calculated result</returns>
    public static int Evaluate(String exp, Lookup variableEvaluator)
    {
        //handle null argument
        if ((exp == null)||(variableEvaluator == null))
        {
            throw new ArgumentException();
        }

        //string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        string result = evaluate(exp, variableEvaluator);
        int tempInt;
        int.TryParse(result, out tempInt);

        return tempInt;
    }

    private static string evaluate(String exp, Lookup variableEvaluator)
    {

        //create two stacks
        Stack<int> valueStack = new Stack<int>();
        Stack<String> operatorStack = new Stack<String>();

        int pCount = 0;//number of parenthesis

        //temperate calculating number holder
        int result = 0;

        //split the exp into string array
        String[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        String show = "";

        //loop through the string array
        for (int i = 0; i < substrings.Length; i++)
        {
            show = substrings[i];

            //skip the empty space
            if (substrings[i].Equals("") || substrings[i].Equals(" "))
            {
                continue;
            }

            int tempInt;
            //if t is a integer
            if (int.TryParse(substrings[i], out tempInt))
            {
                popOnce(operatorStack, valueStack, tempInt);
            }
            //if t is + or -
            else if (substrings[i].Equals("+") || substrings[i].Equals("-"))
            {
                //if top is + or -
                if (checkOperator("+", "-", operatorStack))
                {
                    result = popTwice(operatorStack, valueStack);
                    valueStack.Push(result);
                    operatorStack.Push(substrings[i]);
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
                    result = popTwice(operatorStack, valueStack);
                    valueStack.Push(result);
                    operatorStack.Push(substrings[i]);
                }
                else
                {
                    operatorStack.Push(substrings[i]);
                }
            }else if (substrings[i].Equals("("))
            {
                pCount++;
                string tempString = "";
                i++;
                while ((pCount != 0))
                {
                    if(i > substrings.Length)
                    {
                        throw new ArgumentException();
                    }

                    if (substrings[i].Equals("("))
                    {
                        pCount++;
                    }else if (substrings[i].Equals(")"))
                    {
                        pCount--;
                    }

                    if (pCount == 0)
                    {
                        break;
                    }
                    tempString += substrings[i];
                    i++;
                }
                string tempresult = evaluate(tempString, variableEvaluator);
                if(int.TryParse(tempresult, out tempInt))
                {
                    valueStack.Push(tempInt);
                }

            }

            else//if t is a varaible
            {
                if (checkToken(substrings[i]))
                {
                    try
                    {
                        int variable = variableEvaluator(substrings[i]);
                        popOnce(operatorStack, valueStack, variable);
                    }
                    catch (ArgumentException)//the variable dose not contain a value or it's a illegal token
                    {
                        throw new ArgumentException();
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
        if ((valueStack.Count - operatorStack.Count) > 1)
        {
            throw new ArgumentException();
        }


        //check if operator stack is empty
        if (operatorStack.Count > 0)
        {
            while (operatorStack.Count > 0)
            {
                //check if the last operator is either + or -
                if (checkOperator("+", "-", operatorStack) || checkOperator("*", "/", operatorStack))
                {
                    result = popTwice(operatorStack, valueStack);
                    valueStack.Push(result);

                }
                else
                {
                    break;
                }
            }

        }
        else
        {
            //when there's more then one value in value stack
            if ((valueStack.Count > 1) || (valueStack.Count == 0))
            {
                throw new ArgumentException();
            }
            result = valueStack.Pop();
        }

        //when there's still operation left after calculation
        if (operatorStack.Count > 0)
        {
            throw new ArgumentException();
        }
        return result.ToString();
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
    private static int Operating(int x, int y, String oper)
    {
        switch (oper)
        {
            case "*": return x * y;
            case "/": return (y == 0)? throw new ArgumentException() : x / y;
            case "+": return x + y;
            case "-": return x - y;
            
            default: throw new ArgumentException();
        }
    }
    /// <summary>
    /// This method pop the value stack twice and operator stack once, then
    /// apply the calculation
    /// </summary>
    /// <param name="operatorStack"></param>
    /// <param name="valueStack"></param>
    /// <returns>the calculate result</returns>
    private static int popTwice(Stack<String> operatorStack, Stack<int> valueStack)
    {
        //if value stack contain fewer than 2 value
        if (valueStack.Count > 1)
        {
            int right = valueStack.Pop();
            int left = valueStack.Pop();
            String oper = operatorStack.Pop();
            int result = Operating(left, right, oper);
            return result;
        }
        else
        {
            throw new ArgumentException();
        }

    }

    /// <summary>
    /// This method pop the value stack twice and operator stack once, then
    /// apply the calculation
    /// </summary>
    /// <param name="operatorStack"></param>
    /// <param name="valueStack"></param>
    /// <returns>the calculate result</returns>
    private static void popOnce(Stack<String> operatorStack, Stack<int> valueStack,int right)
    {
        if (checkOperator("*", "/", operatorStack))
        {
            //when the value stack is empty
            if(valueStack.Count > 0)
            {
                int left = valueStack.Pop();
                String oper = operatorStack.Pop();
                int result = Operating(left, right, oper);
                valueStack.Push(result);
            }
            else
            {
                throw new ArgumentException();
            }
        }
        else
        {
            valueStack.Push(right);
        }
    }
    /// <summary>
    /// This method check if the token is a valid variable which follow the format:
    /// one or more letters + one or more digit
    /// </summary>
    /// <param name="token"></param>
    /// <returns>true if it's in valid format, false if not</returns>
    private static bool checkToken(String token)
    {
        string pattern = "^[a-zA-Z]+[0-9]+$";
        return Regex.IsMatch(token, pattern);
    }

}

