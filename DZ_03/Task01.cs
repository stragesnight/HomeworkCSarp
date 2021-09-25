/*
 * 	-= Домашняя Работа №3, Задание №1 =-
 * 		Ученик: Шелест Александр
 *
 * 	Написать программу, которая считывает символы с клавиатуры,
 * 	пока не будет введена точка. 
 * 	Нужно сосчитать количество введённых пробелов.
 */

using System;
using System.Text;

class Task01
{
	static void Main(string[] args)
	{
	  	Console.WriteLine("-= Домашняя Работа №3, Задание №1 =-");
	  	Console.WriteLine("\tУченик: Шелест Александр\n");
	  	Console.WriteLine("Написать программу, которая считывает символы с клавиатуры,");
	  	Console.WriteLine("пока не будет введена точка.");
	  	Console.WriteLine("Нужно сосчитать количество введённых пробелов.");
	 
		// выполнять алгоритм программы, пока
		// пользователь не решит завершить работу
		while (TaskRoutine())
			continue;
	}

	private static bool TaskRoutine()
	{
		Console.Write("\n\nВведите строку, завершающуюся точкой: ");
		// вызов метода чтения строки
		// и создание переменной nSpaces "на месте"
		string str = ReadStringByChars(out int nSpaces);

		Console.WriteLine($"\n\nВаша строка - \"{str}\",");
		Console.WriteLine($"количество пробелов в ней - {nSpaces}.");

		Console.Write("\nПопробовать ещё одну строку? (1 = да, 0 = нет): ");
		return Console.ReadKey().KeyChar != '0';
	}

	// Прочесть строку, завершающуюся точкой, с клавиатуры.
	// - в ссылочный параметр nSpaces записывается количество пробелов в строке.
	private static string ReadStringByChars(out int nSpaces)
	{
		nSpaces = 0;
		char input = '0';
		StringBuilder builder = new StringBuilder();

		while (input != '.')
		{
			// получить кнопку от пользователя и извлечь символ
			input = Console.ReadKey().KeyChar;
			// добавить ввод в конец строки
			builder.Append(input);

			if (input == ' ')
				nSpaces++;
		}

		// превратить строку и возвратить её
		return builder.ToString();
	}
}
