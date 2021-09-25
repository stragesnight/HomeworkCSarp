/*
 * 	-= Домашняя Работа №3, Задание №3 =-
 * 		Ученик: Шелест Александр
 *
 * 	Написать программу для изменения регистра строки,
 * 	введённой пользователем.
 */

using System;
using System.Text;

class Task01
{
	static void Main(string[] args)
	{
  		Console.WriteLine("-= Домашняя Работа №3, Задание №3 =-");
  		Console.WriteLine("\tУченик: Шелест Александр\n");
  		Console.WriteLine("Написать программу для изменения регистра строки,");
  		Console.WriteLine("введённой пользователем.");

		// выполнять алгоритм чтения и проверки,
		// пока пользователь не решит выйти
		while (TaskRoutine())
			continue;
	 
		Console.WriteLine("\nНажмите любую клавишу для выхода из программы...\n");
		Console.ReadKey();
	}

	private static bool TaskRoutine()
	{
		Console.Write("\n\nВведите строку для изменения регистра: ");
		string input = Console.ReadLine();

		string result = SwapStringRegister(input);
		Console.WriteLine($"Изменённая строка: {result}");

		Console.Write("\nПродолжить? (1 = да, 0 = нет): ");
		return Console.ReadKey().KeyChar != '0';
	}

	// Перевернуть регистр символов в строке
	private static string SwapStringRegister(string str)
	{
		StringBuilder builder = new StringBuilder();

		for (int i = 0; i < str.Length; ++i)
		{
			int delta = 0;

			// сдвиг символа на 32 значения вверх, если символ в верхнем регистре
			if ((str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= 'А' && str[i] <= 'Я'))
				delta = 32;
			// сдвиг символа на 32 значения вниз, если символ в нижнем регистре
			else if ((str[i] >= 'a' && str[i] <= 'z') || (str[i] >= 'а' && str[i] <= 'я'))
				delta = -32;

			builder.Append((char)(str[i] + delta));
		}

		// превратить строку и возвратить её
		return builder.ToString();
	}
}
