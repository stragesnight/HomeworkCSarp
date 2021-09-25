/*
 * 	-= Домашняя Работа №3, Задание №2 =-
 * 		Ученик: Шелест Александр
 *
 * 	Проверить введённый пользователем номер билета на то,
 * 	является ли он "счастливым", т.е. совпадает ли сумма
 * 	цифр левой части с суммой цифр правой части.
 */

using System;
using System.Text;

class Task01
{
	static void Main(string[] args)
	{
		Console.WriteLine("-= Домашняя Работа №3, Задание №2 =-");
		Console.WriteLine("\tУченик: Шелест Александр\n");
		Console.WriteLine("Проверить введённый пользователем номер билета на то,");
		Console.WriteLine("является ли он \"счастливым\", т.е. совпадает ли сумма");
		Console.WriteLine("цифр левой части с суммой цифр правой части.");

		// выполнять алгоритм программы, пока
		// пользователь не решит завершить работу
		while (TaskRoutine())
			continue;
	}

	// Проверить, является ли счастливым введённый пользователем номер билета
	// возвращает true, если пользователь желает проверить ещё один билет.
	private static bool TaskRoutine()
	{
		int[] ticket;
		do
		{
			Console.WriteLine("\n\n*ввод автоматически завершится по достижению шести цифр*");
			Console.Write("Введите номер трамвайного билета (шестизначное число): ");

		} while (!TryReadTicketNumber(out ticket));

		Console.WriteLine("\n\nПроверка вашего билета...");
		Console.WriteLine($"Результат: ваш билет {IsLucky(ticket) ? "счастливый!" : "не счастливый..."}\n");

		Console.Write("Попробовать ещё раз? (1 = да, 0 = нет): ");
		
		return Console.ReadKey().KeyChar != '0';
	}

	// Попробовать прочесть номер билета от пользователя посимвольно
	// возвращает true, если введна корректная строка (т.е. она состоит только из цифр)
	private static bool TryReadTicketNumber(out int[] ticket)
	{
		ticket = new int[6];

		for (byte i = 0; i < 6; ++i)
		{
			char input = Console.ReadKey().KeyChar;

			// завершить выполнение, если введён некорректный символ
			if (input < '0' || input > '9')
				return false;

			ticket[i]  = (int)input - '0';
		}

		return true;
	}

	// Проверить, является ли счастилвым билет
	private static bool IsLucky(int[] ticket)
	{
		if (ticket.Length != 6)
			return false;

		int sumLeft = 0;
		int sumRight = 0;

		for (byte i = 0; i < 3; ++i)
			sumLeft += ticket[i];
		for (byte i = 3; i < 6; ++i)
			sumRight += ticket[i];

		return sumLeft == sumRight;
	}
}
