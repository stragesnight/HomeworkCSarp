/*
 * 	-= Домашная Работа №2, Задание №2 =-
 * 		Ученик: Шелест Александр
 *
 * 	Проверить, является ли введённый пользователем билет
 * 	"счастливым", т.е. проверить, совпадает ли сумма цифр
 * 	левой части с суммой цифр правой части.
 */

using System;

class Task02
{
	static void Main(string[] args)
	{
		string ticket;

		Console.WriteLine("-= Домашная Работа №2, Задание №2 =-");
		Console.WriteLine("\tУченик: Шелест Александр\n");
		Console.WriteLine("Проверить, является ли введённый пользователем билет");
		Console.WriteLine("\"счастливым\", т.е. проверить, совпадает ли сумма цифр");
		Console.WriteLine("левой части с суммой цифр правой части.");
 
		// ввод пользователем строки с проверкой на корректность
		do
		{
			Console.Write("Введите номер билета (шестизначное число): ");
			ticket = Console.ReadLine();

		} while (ticket.Length != 6 || !IsNumericString(ticket));

		Console.WriteLine("\nИдёт проверка билета...\n");

		// выбор строки в зависимости от результата проверки
		string res = IsLuckyTicket(ticket) 
					? "билет счастилвый!"
					: "билет не счастливый...";

		Console.WriteLine("Результат: {0}", res);

		Console.WriteLine("\nНажмите любю клавишу для выхода из программы...\n");
		Console.ReadKey();
	}

	// Проверить, соответствует ли строка числовому значению
	private static bool IsNumericString(string str)
	{
		for (int i = 0; i < str.Length; i++)
		{
			// если хотя бы один символ не является цифрой,
			// то строка не подходит по критерию
			if (str[i] < '0' || str[i] > '9')
				return false;
		}

		return true;
	}

	// Проверить, является ли билет счастливым
	private static bool IsLuckyTicket(string str)
	{
		ushort sum_left = 0;
		ushort sum_right = 0;

		// так как на, по сути, не важны сами числовые значения строки,
		// можно считать сумму символьных значений
		for (ushort i = 0; i < 3; i++)
			sum_left += str[i];
		for (ushort i = 3; i < 6; i++)
			sum_right += str[i];

		return sum_left == sum_right;
	}
}

