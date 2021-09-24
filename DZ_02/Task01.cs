/*
 * 	-= Домашная Работа №2, Задание №1 =-
 * 		Ученик: Шелест Александр
 *
 * 	Вывести все положительные числа от A до B (A < B).
 * 	Каждое число должно выводится с новой строки, при этом 
 * 	количество повторений числа равно его значению.
 */

using System;

class Task01
{
	static void Main(string[] args)
	{
		const uint d_min = 1;
		const uint d_max = 16;

		uint a;
		uint b;
		
		Console.WriteLine("-= Домашная Работа №2, Задание №1 =-");
		Console.WriteLine("\tУченик: Шелест Александр\n");
		Console.WriteLine("Вывести все положительные числа от A до B (A < B).");
		Console.WriteLine("Каждое число должно выводится с новой строки, при этом");
		Console.WriteLine("количество повторений числа равно его значению.\n");

		// ввод пользователем диапазона с проверкой на корректность
		do
		{
			Console.Write("Введите нижнюю границу диапазона A [{0} .. {1}]: ",
					d_min, d_max - 1);

		} while (!uint.TryParse(Console.ReadLine(), out a) 
				|| !InRange(a, d_min, d_max - 1));

		do
		{
			Console.Write("Введите верхнюю границу диапазона B [{0} .. {1}]: ",
					a, d_max);

		} while (!uint.TryParse(Console.ReadLine(), out b) 
				|| !InRange(b, a, d_max));

		Console.WriteLine("\nРезультирующий диапазон: [{0} .. {1}]", a, b);

		// вывод последовательности чисел
		for (uint i = a; i <= b; i++)
		{
			for (uint j = 0; j < i; j++)
				Console.Write("{0} ", i);
			Console.Write('\n');
		}

		Console.WriteLine("\nНажмите любую клавишу для выхода из программы...");
		Console.ReadKey();
	}

	// Проверить, входит ли число в диапазон
	private static bool InRange(uint v, uint min, uint max)
		=> (v >= min) && (v <= max);
}

