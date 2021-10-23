/*
 *  -= Домашнее Задание №15 =-
 *      Ученик: Шелест Александр
 *
 *  Используя классы BinaryReader и BinaryWriter реализовать
 *  программу для учёта товарных запасов.
 */

using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace DZ_15
{
    // Товар
    internal sealed class Article
    {
        // Имя товара
        public string Name { get; set; }
        // Количество товара на складе
        public uint Amount { get; set; }

        // внутреннее поле цены товара
        private float _price;
        // Цена товара
        public float Price
        {
            get { return _price; }
            // присваивание значения с проверкой на знак
            set { _price = MathF.Abs(value); }
        }

        // Общая стоимость товара
        public float TotalCost
        {
            get { return Price * Amount; }
        }

        // перегрузка метода ToString()
        public override string ToString()
        {
            return string.Format("Наименование: \"{0}\",\n"
                               + "Количество: {1} шт.,\n"
                               + "Цена: ${2:N2},\n"
                               + "Общая стоимость: ${3:N2}",
                    Name, Amount, Price, TotalCost);
        }
    }

    // Воспомагательный класс расширения для записи и чтения товаров в бинарные файлы
    internal static class ArticleExtensions
    {
        // Записать Товар в бинарный файл
        public static void Write(this BinaryWriter bw, Article article)
        {
            bw.Write(article.Name);
            bw.Write(article.Amount);
            bw.Write(article.Price);
        }

        // Прочесть товар из бинарного файла
        public static Article ReadArticle(this BinaryReader br)
        {
            try
            {
                return new Article {
                    Name = br.ReadString(),
                    Amount = br.ReadUInt32(),
                    Price = br.ReadSingle()
                };
            }
            // если во время чтения возникло исключение, значит
            // курсор достиг конца файла
            catch (IOException)
            {
                return null;
            }
        }
    }

    static class Program
    {
        private static Random r = new Random();

        static void Main()
        {
            Console.WriteLine("-= Домашнее Задание №15 =-");
            Console.WriteLine("    Ученик: Шелест Александр\n");
            Console.WriteLine("Используя классы BinaryReader и BinaryWriter реализовать");
            Console.WriteLine("программу для учёта товарных запасов.");

            Console.WriteLine("\nНажмите любую клавишу для начала...");
            Console.ReadKey();
 
            // запись списка товаров в файл
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Create("articles")))
                {
                    // список имён для товаров
                    string[] names = { "Отвёртка", "Молоток", "Гвозди (100 шт.)",
                        "Дрель (тихая)", "Дрель (громкая)", "Пласкогубцы",
                        "Пила", "Набор для рыбалки", "Измеритель скорости света"
                    };

                    Console.WriteLine("Запись списка товаров в файл...");

                    // сгенерировать товар для каждого имени и записать его в файл
                    // используя метод расширения
                    foreach (string name in names)
                        bw.Write(GenerateRandomArticle(name));
                    
                    Console.WriteLine("Запись списка товаров в завершена успешно");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Невозможно создать файл для записи");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();

            // список товаров, прочтённых из файла
            List<Article> articlesFromFile = new List<Article>();
            // чтение товаров из файла в список
            try
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead("articles")))
                {
                    Console.WriteLine("Чтение списка товаров из файла...\n");

                    Article tmp = null;
                    // чтение товара из бинарного файла при помощи метода расширения
                    while ((tmp = br.ReadArticle()) != null)
                    {
                        articlesFromFile.Add(tmp);
                        Console.WriteLine(tmp);
                        Console.WriteLine();
                        Thread.Sleep(500);
                    }

                    Console.WriteLine("Чтение списка товаров из завершена успешно");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Невозможно открыть файл для чтения");
                Console.WriteLine(ex.Message);
            }

            string input = "";
            // основной цикл выполнения
            while (true)
            {
                // ввод пользователем строки
                Console.Write("\nВведите наименование товара для поиска: ");
                int left = Console.CursorLeft;
                Console.Write("\n(или пустую строку для выхода)");
                Console.SetCursorPosition(left, Console.CursorTop - 1);

                input = Console.ReadLine();
                Console.WriteLine("                              ");

                // проверка на условие выхода
                if (input == "")
                    return;

                // поиск товара в списке при помощи метода Find() и лябда-выражения
                Article found = articlesFromFile.Find((x) => (x.Name == input));

                // выывод результата поиска на экран
                if (found != null)
                {
                    Console.WriteLine("Товар по вашему запросу: ");
                    Console.WriteLine(found);
                }
                else
                {
                    Console.WriteLine("Товар по вашему запросу не был найден");
                }
            }
        }

        // Сгенерировать случайный товар с заданных именем 
        private static Article GenerateRandomArticle(string name)
        {
            return new Article {
                Name = name,
                Amount = (uint)(r.Next() % 100),
                Price = (float)(r.Next() % 50) * 1.123456f
            };
        }
    }
}

