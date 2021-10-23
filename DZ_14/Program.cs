/*
 *  -= Домашнее Задание №14 =-
 *      Ученик: Шелест Александр
 *
 *  Разработать класс "Счёт для оплаты", предусмотрев соответствующие поля.
 *  Продемонстрировать работу класса с записью и чтением данных из файла.
 */

using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace DZ_14
{
    // Счёт для оплаты
    internal sealed class PaymentInvoice
    {
        // внутренние поля для чисел с плавающей точкой
        
        private float _paymentPerDay = 0;
        private float _penaltyPerDay = 0;

        // Оплата за день
        public float PaymentPerDay
        {
            get { return _paymentPerDay; }
            // присваивание значения с проверкой на знак
            set { _paymentPerDay = MathF.Abs(value); }
        }

        // Штраф за день
        public float PenaltyPerDay
        {
            get { return _penaltyPerDay; }
            // присваивание значения с проверкой на знак
            set { _penaltyPerDay = MathF.Abs(value); }
        }

        // Количество дней оплаты
        public ushort Days { get; set; }
        // Количество дней задержки оплаты
        public ushort OverdueDays { get; set; } 

        // Базовая сумма к оплате
        public float PenaltylessPaymentAmount
        {
            get { return _paymentPerDay * Days; }
        }

        // Общий штраф к оплате
        public float Penalty
        {
            get { return _penaltyPerDay * OverdueDays; }
        }

        // Общая сумма к оплате (базовая + штраф)
        public float TotalPaymentAmount
        {
            get { return PenaltylessPaymentAmount + Penalty; }
        }

        // перегрузка метода ToString()
        public override string ToString()
        {
            return string.Format(
                    "Оплата за день: ${0:N2},\nШтраф за день: ${1:N2},\n"
                  + "Дней оплаты: {2},\nДней задержки оплаты: {3}\n"
                  + "Сумма к оплате: ${4:N2} (${5:N2} + ${6:N2} штрафа)",
                PaymentPerDay, PenaltyPerDay, Days, OverdueDays, 
                TotalPaymentAmount, PenaltylessPaymentAmount, Penalty);
        }
    }

    // Воспомагательный класс расширения, содержащий методы для
    // записи и чтения счетов оплаты в бинарные файлы
    internal static class PaymentInvoiceExtensions
    {
        // Записать счет для оплаты в бинарный файл
        public static void Write(this BinaryWriter bw, PaymentInvoice invoice)
        {
            bw.Write(invoice.PaymentPerDay);
            bw.Write(invoice.PenaltyPerDay);
            bw.Write(invoice.Days);
            bw.Write(invoice.OverdueDays);
        }

        // Прочесть счет для оплаты из бинарного файла
        public static PaymentInvoice ReadPaymentInvoice(this BinaryReader br)
        {
            try
            {
                return new PaymentInvoice
                {
                    PaymentPerDay = br.ReadSingle(),
                    PenaltyPerDay = br.ReadSingle(),
                    Days = br.ReadUInt16(),
                    OverdueDays = br.ReadUInt16()
                };
            }
            // если возникло исключение во время чтения, значит курсор
            // достиг конца файла
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

            Console.WriteLine("-= Домашнее Задание №14 =-");
            Console.WriteLine("    Ученик: Шелест Александр\n");
            Console.WriteLine("Разработать класс \"Счёт для оплаты\", "
                            + "предусмотрев соответствующие поля.");
            Console.WriteLine("Продемонстрировать работу класса "
                            + "с записью и чтением данных из файла.\n");

            Console.WriteLine("Нажмите любую клавишу для начала...");
            Console.ReadKey();

            // генерация списка счетов и их запись в файл
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Create("payments")))
                {
                    Console.WriteLine("Создание списка счетов для оплаты...");

                    List<PaymentInvoice> payments = new List<PaymentInvoice>();
                    for (int i = 0; i < 8; ++i)
                    {
                        // добавление нового счёта в список
                        payments.Add(GenerateRandomPayment());

                        // вывод счёта на экран
                        Console.WriteLine(payments[i]);
                        Console.WriteLine();
                        // пауза программы
                        Thread.Sleep(500);
                    }

                    Console.WriteLine("Запись списка счетов в файл \"payments\"...");

                    // использользование расширенного метода для прамой записи счёта в файл
                    foreach (PaymentInvoice pi in payments)
                        bw.Write(pi);

                    Console.WriteLine("Запись списка счетов в файл успешно завершена");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Невозможно создать файл для записи");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();

            try
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead("payments")))
                {
                    List<PaymentInvoice> payments = new List<PaymentInvoice>();
                    PaymentInvoice tmp = null;

                    Console.WriteLine("Чтение счетов для оплаты из файла...");

                    // чтение всех счетов для оплаты из файла и добавление их в список
                    // используя метод расширения
                    while ((tmp = br.ReadPaymentInvoice()) != null)
                        payments.Add(tmp);

                    Console.WriteLine("Чтение счетов успешно завершено");
                    Console.WriteLine("\nПолученный список счетов:");

                    // вывод прочитанных из файла счетов
                    foreach (PaymentInvoice pi in payments)
                    {
                        Console.WriteLine(pi);
                        Console.WriteLine();
                        Thread.Sleep(500);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Невозможно открыть файл для чтения");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Нажмите любую клавишу для выхода из программы...");
            Console.ReadKey();
        }

        // Сгенерировать случайный счёт для оплаты
        private static PaymentInvoice GenerateRandomPayment()
        {
            return new PaymentInvoice {
                PaymentPerDay = (float)(r.Next() % 100) * 1.1231241f,
                PenaltyPerDay = (float)(r.Next() % 15) * 1.1231241f,
                Days = (ushort)(r.Next() % 180),
                OverdueDays = (ushort)(r.Next() % 43)
            };
        }
    }
}

