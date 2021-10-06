// 6. Анализ базового класса Object

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

        public override string ToString()
        {
            return $"\nФамилия: {_lastName}\n"
                 + $"Имя: {_firstName}\n"
                 + $"Дата рождения: {_birthDate.ToShortDateString()}\n";
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

        public override string ToString()
        {
            return base.ToString() + $"Учебное заведение: {_institution}.";
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

        public override string ToString()
        {
            return base.ToString() + $"Учусь в {_groupName} группе.";
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

        public override string ToString()
        {
            return base.ToString() + $"Учусь в {_className} классе.";
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
            Student student = (Student)learners[0];

            foreach (Learner item in learners)
            {
                WriteLine(item);
                item.Think();
                item.Study();
            }

            WriteLine($"\n\nПолное имя типа - {student.GetType().FullName}.");
            WriteLine($"Имя текущего элемента - {student.GetType().Name}.");
            WriteLine($"Базовый класс текущего элемента - {student.GetType().BaseType}.");
            Write("Является ли текущий элемент абстрактным объектом - ");
            WriteLine(student.GetType().IsAbstract);
            WriteLine($"Является ли объект классом - {student.GetType().IsClass}");
            Write("Можно ли получить доступ к объекту из кода за пределами сборки - ");
            WriteLine(student.GetType().IsVisible);

            // пауза программы
            ReadKey();
        }
    }
}
