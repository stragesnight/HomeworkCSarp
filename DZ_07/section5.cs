// 5. Абстрактные классы.

using System;
using static System.Console;

namespace SimpleProject
{
    public abstract class Human
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

        public abstract void Think();

        public virtual void Print()
        {
            WriteLine($"\nФамилия: {_lastName}");
            WriteLine($"Имя: {_firstName}");
            WriteLine($"Дата рождения: {_birthDate.ToShortDateString()}");
        }
    }

    abstract class Learner : Human
    {
        string _institution;

        public Learner(string firstName, string lastName, DateTime date, string institution)
            : base(firstName, lastName, date)
        {
            _institution = institution;
        }

        public abstract void Study();

        public override void Print()
        {
            base.Print();
            WriteLine($"Учебное заведение: {_institution}.");
        }
    }

    class Student : Learner
    {
        string _groupName;

        public Student(string firstName, string lastName, DateTime date, 
                string institution, string groupName)
            : base(firstName, lastName, date, institution)
        {
            _groupName = groupName;
        }

        public override void Think()
        {
            WriteLine("Я думаю как студент.");
        }

        public override void Study()
        {
            WriteLine("Я изучаю предметы в институте.");
        }

        public override void Print()
        {
            base.Print();
            WriteLine($"Учусь в {_groupName} группе.");
        }
    }

    class SchoolChild : Learner
    {
        string _className;

        public SchoolChild(string firstName, string lastName, DateTime date, 
                string institution, string className)
            : base(firstName, lastName, date, institution)
        {
            _className = className;
        }

        public override void Think()
        {
            WriteLine("Я думаю как школьник.");
        }

        public override void Study()
        {
            WriteLine("Я изучаю предметы в школе.");
        }

        public override void Print()
        {
            base.Print();
            WriteLine($"Учусь в {_className} классе.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Learner[] learners = {
                new Student("John", "Doe", new DateTime(1990, 6, 12),"IT Step", "15PPS21"),
                new SchoolChild("Jack", "Smith", new DateTime(2008, 4, 18), "School#154", "1-A")
            };

            foreach (Learner item in learners)
            {
                item.Print();
                item.Think();
                item.Study();
            }

            // пауза программы
            ReadKey();
        }
    }
}
