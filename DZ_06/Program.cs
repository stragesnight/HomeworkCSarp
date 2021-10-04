/*
 *  -= Домашнее Задание №6 =-
 *      Ученик: Шелест Александр
 *
 *  Разработать приложение "7 чудес света", где каждое чудо будет 
 *  представлено отдельным классом. С помощью пространства имён обеспечить
 *  возможность взаимодействия классов.
 */

using System;

namespace DZ_06
{
    class Program
    {
        // делегат для функции, которая отображает информация о чуде света
        delegate void WonderInfo(); 
        // делегат для предиката, используется при проверке ввода пользователя
        delegate bool Predicate<T>(T v);


        static void Main(string[] args)
        {
            // создание массива объектов делегата для каждого чуда света
            // обращение к методам классов происходит через их пространство имён
            WonderInfo[] wonders = new WonderInfo[] {
                    new WonderInfo(Egypt.Giza.GreatPyramidOfGiza.DisplayInfo),
                    new WonderInfo(Iraq.Nineveh.HangingGardensOfBabylon.DisplayInfo),
                    new WonderInfo(Turkey.Selchuk.TempleOfArtemisAtEphesus.DisplayInfo),
                    new WonderInfo(Greece.Olympia.StatueOfZeusAtOlympia.DisplayInfo),
                    new WonderInfo(Turkey.Bodrum.MausoleumAtHalicarnassus.DisplayInfo),
                    new WonderInfo(Greece.Rhodes.ColossusOfRhodes.DisplayInfo),
                    new WonderInfo(Egypt.Alexandria.LighthouseOfAlexandria.DisplayInfo)
            };

            byte input = 255;
            while (input != 0)
            {
                Console.Clear();

                Console.WriteLine("\t====================");
                Console.WriteLine("\t||   -- МЕНЮ --   ||");
                Console.WriteLine("\t====================\n");
                Console.WriteLine("Отобразить информацию о:");
                Console.WriteLine(" 1 - Пирамиде Хеопса");
                Console.WriteLine(" 2 - Висячих садах Семирамиды");
                Console.WriteLine(" 3 - Храме Артемиды в Эфесе");
                Console.WriteLine(" 4 - Статуе Зевса в Олимпии");
                Console.WriteLine(" 5 - Мавзолее в Галикарнасе");
                Console.WriteLine(" 6 - Колоссе Родосском");
                Console.WriteLine(" 7 - Александрийском маяке");
                Console.WriteLine(" 8 - всех чудесах света");
                Console.WriteLine("\n0 - выход из программы\n");

                // получить ввод от пользователя и проверить его на корректность
                input = GetUserInput<byte>("Введите действие по его номеру в меню: ",
                        new Predicate<byte>(v => (v <= 8)));

                Console.Clear();

                switch (input)
                {
                    case 8:
                        foreach (var wonder in wonders)
                        {
                            wonder();
                            Console.WriteLine("");
                        }
                        Console.WriteLine("\n\nнажмите любую клавишу для возвращения в меню...");
                        Console.ReadKey();
                        break;

                    case 0:
                        break;

                    default:
                        wonders[input - 1]();
                        Console.WriteLine("\n\nнажмите любую клавишу для возвращения в меню...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static T GetUserInput<T>(string msg, Predicate<T> predicate)
        {
            T result = default(T);

            while (true)
            {
                Console.Write(msg);

                try
                {
                    result = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));

                    if (!predicate(result))
                        continue;
                }
                catch (Exception e)
                {
                    continue;
                }

                return result;
            }
        }
    }
}
