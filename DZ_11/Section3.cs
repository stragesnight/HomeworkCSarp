using System;
using static System.Console;

namespace DZ11Section3
{
    public delegate double AnonymousDelegateDouble(double x, double y);
    public delegate void AnonymousDelegateInt(int n);
    public delegate void AnonymousDelegateVoid();

    class Dispatcher
    {
        public event AnonymousDelegateDouble eventDouble;
        public event AnonymousDelegateInt eventInt;

        public double? OnEventDouble(double x, double y)
            => eventDouble?.Invoke(x, y);

        public void OnEventInt(int n = 0) => eventInt?.Invoke(n);
    }

    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("\tThe use of events");
            Dispatcher dispatcher = new Dispatcher();

            dispatcher.eventDouble += delegate(double a, double b)
            {
                if (b == 0)
                    throw new DivideByZeroException();
                return a / b;
            };

            double n1 = 5.7, n2 = 3.2;
            WriteLine($"{n1} / {n2} = {dispatcher.OnEventDouble(n1, n2)}");

            WriteLine("\tUsing a local variable");
            int number = 5;

            dispatcher.eventInt += delegate(int n)
            {
                WriteLine($"{number} + {n} = {number + n}");
            };

            dispatcher.OnEventInt();
            dispatcher.OnEventInt(6);

            WriteLine("\tThe use of a delegate");

            AnonymousDelegateVoid voidDel = delegate { WriteLine("Ok!"); };

            voidDel += delegate { WriteLine("Bye!"); };
            voidDel?.Invoke();

            ReadKey();
        }
    }
}
