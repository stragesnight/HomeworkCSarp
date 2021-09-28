/*
 *  -= Домашнее Задание №4 =-
 *      Ученик: Шелест Александр
 *
 *  Вариант №10: Разработать класс Зажигалка, реализовав:
 *      - не менее пяти полей
 *      - не менее трёх методов управления классом и доступа к полям
 *      - метод с передачей параметра по ссылке
 *      - не менее двух статических полей, представляюшие общие
 *          характеристики класса
 */

using System;
using System.Collections.Generic;

namespace DZ_04
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            string obj = "Салфетка";

            Lighter lighter1 = new Lighter(1.23, 10f, "Lighters Co.", true);
            Console.WriteLine(lighter1.ToString());

            Console.WriteLine("Инициализирована попытка поджечь салфетку...");
            lighter1.LightUp(ref obj);

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();

            Console.WriteLine("\nСостояние зажигалки после подожжения:\n");
            Console.WriteLine(lighter1.ToString());

            Console.WriteLine($"\nСалфетка после поджигания: {obj}");

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();

            Console.WriteLine("\nСоздание некоторого количества зажигалок...");
            List<Lighter> lighters = new List<Lighter>();

            for (int i = 0; i < 8; ++i)
            {
                lighters.Add(new Lighter(r.NextDouble() * 10D, 
                            (float)r.NextDouble() * 10f, 
                            "Lighters Co.",
                            (r.Next() & 1) == 0));

                //Console.WriteLine("\nНовая зажигалка: ");
                //Console.WriteLine(lighters[lighters.Count - 1].ToString());
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();

            double revenue = Lighter.GetTotalRevenue();
            uint activeLighterCount = Lighter.GetTotalActiveLighterCount();

            Console.WriteLine("\nЗначения статических полей класса Lighter:");
            Console.WriteLine($"\tОбщий доход: ${revenue}");
            Console.WriteLine($"\tКоличество активных зажигалок: {activeLighterCount}");
            
            Console.WriteLine("\nНажмите любую клавишу для выхода из программы...");
            Console.ReadKey();
        }
    }
}
