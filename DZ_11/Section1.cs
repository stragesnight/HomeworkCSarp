using System;
using static System.Console;

namespace DZ11Section1
{
    public delegate T CalcDelegate<T>(T x, T y);

    public class Calculator
    {
        public static double Add(double x, double y)
        {
            double res = x + y;
            WriteLine($"{x} + {y} = {res}");
            return res;
        }

        public static double Sub(double x, double y)
        {
            double res = x - y;
            WriteLine($"{x} - {y} = {res}");
            return res;
        }

        public static double Mul(double x, double y)
        {
            double res = x * y;
            WriteLine($"{x} * {y} = {res}");
            return res;
        }

        public static double Div(double x, double y)
        {
            if (y == 0)
                throw new DivideByZeroException();
            double res = x / y;
            WriteLine($"{x} / {y} = {res}");
            return res;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Write("Enter an expression: ");
            string expr = ReadLine();
            char sign = ' ';

            foreach(char c in expr)
            {
                if (MatchAny(c, new char[4]{'+', '-', '*', '/'}))
                {
                    sign = c;
                    break;
                }
            }

            try
            {
                string[] numbers = expr.Split(sign);

                double x = double.Parse(numbers[0]);
                double y = double.Parse(numbers[1]);

                CalcDelegate<double> calcDel;
                switch (sign)
                {
                    case '+':
                        calcDel = Calculator.Add;
                        break;
                    case '-':
                        calcDel = Calculator.Sub;
                        break;
                    case '*':
                        calcDel = Calculator.Mul;
                        break;
                    case '/':
                        calcDel = Calculator.Div;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
                
                Write("Result: ");
                calcDel(x, y);
                
                calcDel = Calculator.Add;
                calcDel += Calculator.Sub;
                calcDel += Calculator.Mul;
                calcDel += Calculator.Div;
                WriteLine("\nOther operations:");
                calcDel(x, y);
            }
            catch (Exception e)
            {
                WriteLine($"Exception caught: {e.Message}");
            }

            ReadKey();
        }

        static bool MatchAny(char c, char[] arr)
        {
            foreach(char i in arr)
            {
                if (c == i)
                    return true;
            }

            return false;
        }
    }
}
