/*
 * 	-= Домашнее Задание №1 =-
 * 		Ученик: Шелест Александр
 *
 * 	Вывести на экран консоли произвольное объявление или стихотворение.
 * 	В данном случае выводится:
 * 	 * история развития технологий программирования;
 * 	 * причины возникновения платформы Microsoft .NET.
 */

using System;

namespace DZ_01
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("-= Домашнее Задание №1 =-");
			Console.WriteLine("\tУченик: Шелест Александр\n");
			Console.WriteLine("Вывести на экран консоли произвольное объявление или стихотворение.");
			Console.WriteLine("В данном случае выводится:");
			Console.WriteLine("\t* история развития технологий программирования;");
			Console.WriteLine("\t* причины возникновения платформы Microsoft .NET.\n");

			Console.WriteLine("Нажмите Enter для продолжения...");
			Console.Read();

			Console.WriteLine("Основные этапы развития технологий программирования:");
			Console.WriteLine("\t* 1837 г. - Аналитическая машина Чарльза Бэббиджа и Ады Лэвлэйс");
			Console.WriteLine("\t* 1943-1946 г. - Система команд ENIAC по трудам Алана Тьюринга");
			Console.WriteLine("\t* 1950 г. - \"Краткий код\" В. Шмидта, А. Б. Тоника и Дж. Р. Логана");
			Console.WriteLine("\t* 1957 г. - Реализация языка \"FORTRAN I\" Джоном Бэкусом");
			Console.WriteLine("\t* 1964 г. - \"BASIC\" Джона Кэмени и Томаса Курца");
			Console.WriteLine("\t* 1972 г. - язык \"С\" Дэнниса Ритчи");
			Console.WriteLine("\t* 1983 г. - язык \"С++\" Бьёрна Страуструпа");
			Console.WriteLine("\t* 1995 г. - создание языка \"Java\" Джеймсом Гослингом и Sun Microsystems");
			Console.WriteLine("\t* 2000 г. - появление языка \"C#\" от Microsoft");
			Console.WriteLine("\t* 2000-... г. - реализация множества (в основном высокоуровневых) языков");

			Console.WriteLine("\nНажмите Enter для продолжения...");
			Console.Read();

			Console.WriteLine("Причина возникновения платформы .NET:");
			Console.WriteLine("Microsoft начала разработку платформы .NET в конце 90-х, как часть");
			Console.WriteLine("своего маркетингового плана \"Microsoft .NET Strategy\".");
			Console.WriteLine("Эта платформа изначально задумывалась как проприетарный набор инструментов");
			Console.WriteLine("для разработки основной масс программ для операционной системы Windows.");
			Console.WriteLine("Со временем же, платформа .NET превратилась в мощный кросс-платформенный");
			Console.WriteLine("инструментарий для различных целей разработки с открытым исходным кодом (.NET Core).");

			Console.WriteLine("\nНажмите Enter для выхода из программы...");
			Console.Read();
		}
	}
}
