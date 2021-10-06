// 4. Полиморфизм, виртуальные методы

using System;
using static System.Console;

namespace SimpleProject
{
    public class Human
    {
        string _firstName;
        string _lastName;
        DateTime _birthDate;

        public Human(string firstName, string lastName, DateTime date)
        {
            _firstName = firstName;
            _lastName = lastName;
            _birthDate = date;
        }

        public virtual void Print()
        {
            WriteLine($"\nФамилия: {_lastName}");
            WriteLine($"Имя: {_firstName}");
            WriteLine($"Дата рождения: {_birthDate.ToShortDateString()}");
        }
    }

    public class Employee : Human
    {
        double _salary;

        public Employee(string firstName, string lastName, DateTime date, double salary)
            : base(firstName, lastName, date)
        {
            _salary = salary;
        }

        public override void Print()
        {
            base.Print();
            WriteLine($"Заработная плата: {_salary} $");
        }
    }

    class Manager : Employee
    {
        string _fieldActivity;

        public Manager(string firstName, string lastName, DateTime date, 
                double salary, string activity)
            : base(firstName, lastName, date, salary)
        {
            _fieldActivity = activity;
        }

        public override void Print()
        {
            Write($"\nМенеджер. Сфера деятельности: {_fieldActivity}");
            base.Print();
        }
    }

    class Scientist : Employee
    {
        string _scientificDirection;

        public Scientist(string firstName, string lastName, DateTime date,
                double salary, string direction)
            : base(firstName, lastName, date, salary)
        {
            _scientificDirection = direction;
        }

        public override void Print()
        {
            Write($"\nУчёный. Научное направление: {_scientificDirection}");
            base.Print();
        }
    }

    class Specialist : Employee
    {
        string _qualification;

        public Specialist(string firstName, string lastName, DateTime date,
                double salary, string qualification)
            : base(firstName, lastName, date, salary)
        {
            _qualification = qualification;
        }

        public override void Print()
        {
            Write($"\nСпециалист. Квалификация: {_qualification}");
            base.Print();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Human[] people = {
                new Manager("john", "Doe", new DateTime(1995, 7, 23), 3500, "Продукты питания"),
                new Scientist("Jim", "Beam", new DateTime(1956, 3, 15), 4253, "История"),
                new Specialist("Jack", "Smith", new DateTime(1996, 11, 5), 2587.43, "Физика")
            };

            foreach (Human item in people)
                item.Print();

            // пауза программы
            ReadKey();
        }
    }
}
