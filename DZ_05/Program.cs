/*
 *  -= Домашнее Задание №5 =-
 *      Ученик: Шелест Александр
 *
 *  Сравнить население столиц трёх столиц разных стран, обозначив 
 *  страну пространством имён, а город - классом с соответствующим именем.
 */

using System;

// порстранство имён для Украины
namespace Ukraine
{
    // Класс Киев
    class Kyiv
    {
        // приватные статические поля
        
        private static string name = "Киев";
        private static ulong population = 2962180;

        // Получить информацию о столице
        public static string GetInfo() 
        {
            // получить сокращённое население
            string populationM = ((float)population / 1000000f).ToString().Substring(0, 5);
            // возвратить форматированную строку
            return $"Столица: {name}, Население: {population} человек ({populationM} млн)";
        }
    }
}

// пространство имён для Японии
namespace Japan
{
    // Класс Токио
    class Tokyo
    {
        // приватные статические поля

        private static string name = "Токио";
        private static ulong population = 14043239;

        // Получить информацию о столице
        public static string GetInfo() 
        {
            // получить сокращённое население
            string populationM = ((float)population / 1000000f).ToString().Substring(0, 5);
            // возвратить форматированную строку
            return $"Столица: {name}, Население: {population} человек ({populationM} млн)";
        }
    }
}

// пространство имён для Финляндии
namespace Finland
{
    // Класс Хельсинки
    class Helsinki
    {
        // приватные статические поля

        private static string name = "Хельсинки";
        private static ulong population = 656250;

        // Получить информацию о столице
        public static string GetInfo() 
        {
            // получить сокращённое население
            string populationM = ((float)population / 1000f).ToString().Substring(0, 5);
            // возвратить форматированную строку
            return $"Столица: {name}, Население: {population} человек ({populationM} тыс.)";
        }
    }
}

// пространство имён для главной программы
namespace DZ_05
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-= Домашнее Задание №5 =-");
            Console.WriteLine("\tУченик: Шелест Александр\n");
            Console.WriteLine("Сравнить население столиц трёх столиц разных стран, обозначив ");
            Console.WriteLine("страну пространством имён, а город - классом с соответствующим именем.\n");
 
            // вывод информации о столицах в порядке возрастания населения
            Console.WriteLine("Финляндия:");
            Console.WriteLine(Finland.Helsinki.GetInfo());
            Console.WriteLine("\nУкраина:");
            Console.WriteLine(Ukraine.Kyiv.GetInfo());
            Console.WriteLine("\nЯпония:");
            Console.WriteLine(Japan.Tokyo.GetInfo());

            // пауза и выход из программы
            Console.WriteLine("\nНажмите любую клавишу для выхода из программы...");
            Console.ReadKey();
        }
    }
}

