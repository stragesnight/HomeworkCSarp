using System;
using static System.Console;

namespace DZ11Section4
{
    class Program
    {
        class ExampleCalc
        {
            public string CurrentDate 
                => $"\tCurrent date is {DateTime.Now.ToLongDateString()}\n";
            public static void AddVoid(int x, int y)
                => WriteLine($"{x} + {y} = {x + y}");
            public int AddInt(int x, int y) => x + y;
        }

        static void Main(string[] args)
        {
            ExampleCalc calc = new ExampleCalc();

            WriteLine(calc.CurrentDate);

            try
            {
                Write("Enter an integer: ");
                int n1 = int.Parse(ReadLine());

                Write("Enter an integer: ");
                int n2 = int.Parse(ReadLine());

                WriteLine($"{n1} + {n2} = {calc.AddInt(n1, n2)}");
                ExampleCalc.AddVoid(n1, n2);
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }

            ReadKey();
        }
    }
}
