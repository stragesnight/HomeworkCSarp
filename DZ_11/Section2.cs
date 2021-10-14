using System;
using System.Collections.Generic;
using static System.Console;

namespace DZ11Section2
{
    public delegate void ExamDelegate(string t);

    class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public void Exam(string task)
        {
            WriteLine($"Student {LastName} solved the {task}");
        }
    }

    class Teacher
    {
        SortedList<int, ExamDelegate> _sortedEvents = new SortedList<int, ExamDelegate>();
        Random _rand = new Random();

        public event ExamDelegate examEvent
        {
            add
            {
                for (int key;;)
                {
                    key = _rand.Next();
                    if (!_sortedEvents.ContainsKey(key))
                    {
                        _sortedEvents.Add(key, value);
                        break;
                    }
                }
            }
            remove
            {
                _sortedEvents.RemoveAt(_sortedEvents.IndexOfValue(value));
            }
        }

        public void Exam(string task)
        {
            foreach (int item in _sortedEvents.Keys)
                _sortedEvents[item]?.Invoke(task);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Student> _group = new List<Student> {
                new Student {
                    FirstName = "John",
                    LastName = "Miller",
                    BirthDate = new DateTime(1997, 3, 12)
                },
                new Student {
                    FirstName = "Candice",
                    LastName = "Lenan",
                    BirthDate = new DateTime(1998, 7, 22)
                },
                new Student {
                    FirstName = "Joey",
                    LastName = "Finch",
                    BirthDate = new DateTime(1996, 11, 30)
                },
                new Student {
                    FirstName = "Nicole",
                    LastName = "Taylor",
                    BirthDate = new DateTime(1996, 5, 10)
                }
            };

            Teacher teacher = new Teacher();

            foreach (Student s in _group)
                teacher.examEvent += s.Exam;

            Student stud = new Student {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1998, 10, 12)
            };

            teacher.examEvent += stud.Exam;
            teacher.Exam("Task #1");
            WriteLine();
            teacher.examEvent -= stud.Exam;
            teacher.Exam("Task #2");

            ReadKey();
        }
    }
}

