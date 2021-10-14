using static System.Console;

namespace DZ11Section5
{
    static class ExampleExtensions
    {
        public static int NumberWords(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return 0;

            data = System.Text.RegularExpressions.Regex.Replace(data.Trim(), @"\s+", " ");

            return data.Split(' ').Length;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Write("Enter a string: ");
            string str = ReadLine();

            WriteLine($"Number of words in the string: {str.NumberWords()}");
            ReadKey();
        }
    }
}
