/*
 *  -= Домашнее Задание №8 =-
 *      Ученик: Шелест Александр
 *
 *  Реализовать программу "Строительство дома":
 *  - классы:
 *    - House, Basement, Wall, Door, Window, Roof
 *    - Team, Worker, TeamLeader
 *  - интерфейсы:
 *    - IWorker, IPart
 *  
 *  Согласно проекту, дом состоит из: 
 *    1 фундамента, 4 стен, 1 двери, 4 окон и 1 крыши
 */

using System;
using System.Threading;
using System.Collections.Generic;
using static System.Console;


namespace DZ_08
{
    // Воспомагательный класс для генерации случайных чисел
    class RNG
    {
        public static Random r;
        public static void Init() => r = new Random();
    }

    // Воспомагательный класс для вывода сообщений и графики на экран
    class Drawer
    {
        private static readonly int _offsetLeft = 1;
        private static readonly int _offsetTop = 1;

        // Вывести графику на экран консоли
        public static void Draw(string str)
        {
            SetCursorPosition(_offsetLeft, _offsetTop);

            foreach(char c in str)
            {
                switch (c)
                {
                    case '\n':
                        SetCursorPosition(_offsetLeft, CursorTop + 1);
                        break;
                    case ' ':
                        SetCursorPosition(CursorLeft + 1, CursorTop);
                        break;
                    default:
                        Write(c);
                        break;
                }
            }
        }

        // Вывести прогресс постройки дома на экран консоли
        public static void LogProgress(string msg)
        {
            SetCursorPosition(1, WindowHeight - 2);
            for (int i = 0; i < WindowWidth - 1; ++i)
                Write(" ");
            SetCursorPosition(1, WindowHeight - 2);
            Write($"* Прогресс постройки дома: {msg}");
        }

        // вывести мысли работника на экран консоли
        public static void LogWorkerWords(string msg)
        {
            SetCursorPosition(1, WindowHeight - 1);
            for (int i = 0; i < WindowWidth - 1; ++i)
                Write(" ");
            SetCursorPosition(1, WindowHeight - 1);
            Write($"* Мысли работника: {msg}");
        }
    }

    // Интерфейс для части дома
    interface IPart
    {
        // Узнать, закончена ли построка части
        bool IsDone { get; }
        // Начать строительство части
        void Build();
        // Нарисовать часть на экране консоли
        void Draw();
    }

    // Интерфейс для строителя
    interface IWorker
    {
        // Начать/продолжить работу над строительством
        void Work(ref House house);
    }

    // Перечисление возможных сторон (для стен и окон)
    enum Side { Front, Left, Right, Back };

    // Фундамент
    class Basement : IPart
    {
        private bool _isDone = false;
        public bool IsDone { get { return _isDone; } }

        private static readonly string _graphics =
            "\n\n\n\n\n\n\n"
            + "           ,:x::,,\n"
            + "     ,;H############hh::,,\n"
            + "    ########################\n"
            + "      ''::YY############Y:'\n"
            + "              ''\"\"V\"'\n";

        // Построить фундамент
        public void Build()
        {
            string msg = "Постройка фунтамента";

            // имитировать процесс постройки
            for (int i = 0; i < 12; ++i)
            {
                Drawer.LogProgress(msg);

                for (int j = 0; j < (i % 3 + 1); ++j)
                    Write('.');

                Thread.Sleep(250);
            }

            // изменить состояние части и вывести сообщение на экран
            Drawer.LogProgress("Фундамент готов!");
            _isDone = true;
        }

        // Нарисовать фундамент
        public void Draw()
        {
            Drawer.Draw(_graphics);
        }
    }

    // Стена
    class Wall : IPart
    {
        private Side _side;     // сторона дома, в которой находится стена
        private bool _isDone;
        public bool IsDone { get { return _isDone; } }
        
        private static readonly string _backGraphics = 
            "\n\n\n\n"
            + "             |':.\n"
            + "             |''''''::..\n"
            + "             |'''''''''''''|\n"
            + "              '''''''''''''|\n"
            + "                      '''''|\n";

        private static readonly string _rightGraphics = 
            "\n\n\n\n"
            + "          .:'|\n"
            + "      .::::::|\n"
            + "    |::;:::::|\n"
            + "    |:::;::,:\n"
            + "    |:'\n";

        private static readonly string _leftGraphics =
            "\n\n\n\n\n\n"
            + "                       .::'|\n"
            + "                     .:::::|\n"
            + "                  |::::::::|\n"
            + "                  |:::::::'\n"
            + "                  |::::'\n";

        private static readonly string _frontGraphics = 
            "\n\n\n\n\n\n"
            + "    |':..\n"
            + "    |'''''''::..\n"
            + "    |'''''''''''''|\n"
            + "    #'';;'''''''''|\n"
            + "         ''';;''''|\n"
            + "              ''\"\"#\n";


        public Wall(Side side)
        {
            _side = side;
            _isDone = false;
        }

        // Построить стену
        public void Build()
        {
            string msg = "Постройка стены";
            switch(_side)
            {
                case Side.Back:
                    msg += " (задней)";
                    break;
                case Side.Right:
                    msg += " (правой)";
                    break;
                case Side.Left:
                    msg += " (левой)";
                    break;
                case Side.Front:
                    msg += " (передней)";
                    break;
                default:
                    break;
            }

            // имитировать процесс строительства
            for (int i = 0; i < 12; ++i)
            {
                Drawer.LogProgress(msg);

                for (int j = 0; j < (i % 3 + 1); ++j)
                    Write('.');

                Thread.Sleep(250);
            }

            // изменить состояние и вывести сообщение
            Drawer.LogProgress("Стена готова!");
            _isDone = true;
        }

        // Нарисовать стену
        public void Draw()
        {
            // выбрать нужную графику в зависимости от расположения стены
            switch(_side)
            {
                case Side.Back:
                    Drawer.Draw(_backGraphics);
                    break;
                case Side.Right:
                    Drawer.Draw(_rightGraphics);
                    break;
                case Side.Left:
                    Drawer.Draw(_leftGraphics);
                    break;
                case Side.Front:
                    Drawer.Draw(_frontGraphics);
                    break;
                default:
                    break;
            }
        }
    }

    // Дверь
    class Door : IPart
    {
        private bool _isDone = false;
        public bool IsDone { get { return _isDone; } }

        private static readonly string _graphics = 
            "\n\n\n\n\n\n\n"
            + "      |vv|\n"
            + "      |_,|\n"
            + "      |__|\n"
            + "      /_/\n";

        // Установить дверь
        public void Build()
        {
            string msg = "Установка входной двери";

            // имитировать процесс постройки
            for (int i = 0; i < 9; ++i)
            {
                Drawer.LogProgress(msg);

                for (int j = 0; j < (i % 3 + 1); ++j)
                    Write('.');

                Thread.Sleep(250);
            }

            // изменить состояние двери и вывести сообщение
            Drawer.LogProgress("Дверь установлена!");
            _isDone = true;
        }

        // Нарисовать дверь
        public void Draw()
        {
            Drawer.Draw(_graphics);
        }
    }

    // Окно
    class Window : IPart
    {
        private Side _side;     // часть дома, в которой находится окно
        private bool _isDone;
        public bool IsDone { get { return _isDone; } }

        private static readonly string _backGraphics = 
            "\n\n\n\n\n"
            + "                  ///|\n"
            + "                  |__|\n"
            + "                  ^^^^\n";

        private static readonly string _rightGraphics = 
            "\n\n\n\n\n"
            + "        \\\\\\\n"
            + "        |_|\n"
            + "        ^^^\n";

        private static readonly string _leftGraphics =
            "\n\n\n\n\n\n\n"
            + "                      \\\\\\\n"
            + "                      |_|\n"
            + "                      ^^^\n";

        private static readonly string _frontGraphics = 
            "\n\n\n\n\n\n\n\n"
            + "            ////\n"
            + "            |__|\n"
            + "            ^^^^\n";


        public Window(Side side)
        {
            _side = side;
        }

        // Кстановить окно
        public void Build()
        {
            string msg = "Установка окна";
            switch(_side)
            {
                case Side.Back:
                    msg += " (заднего)";
                    break;
                case Side.Right:
                    msg += " (правого)";
                    break;
                case Side.Left:
                    msg += " (левого)";
                    break;
                case Side.Front:
                    msg += " (переднего)";
                    break;
                default:
                    break;
            }

            // имитировать процесс постройки
            for (int i = 0; i < 9; ++i)
            {
                Drawer.LogProgress(msg);

                for (int j = 0; j < (i % 3 + 1); ++j)
                    Write('.');

                Thread.Sleep(250);
            }

            // изменить состояние окна и вывести сообщение
            Drawer.LogProgress("Окно установлено!");
            _isDone = true;
        }

        // Нарисовать окно
        public void Draw()
        {
            // выбрать нужную графику в зависимости от расположения окна
            switch(_side)
            {
                case Side.Back:
                    Drawer.Draw(_backGraphics);
                    break;
                case Side.Right:
                    Drawer.Draw(_rightGraphics);
                    break;
                case Side.Left:
                    Drawer.Draw(_leftGraphics);
                    break;
                case Side.Front:
                    Drawer.Draw(_frontGraphics);
                    break;
                default:
                    break;
            }
        }
    }

    // Крыша
    class Roof : IPart
    {
        private bool _isDone = false;
        public bool IsDone { get { return _isDone; } }

        private static readonly string _graphics =
          "        ^___\n"
        + "       /,/,/*^^^^____\n"
        + "      /,/,/,/,/,/,/,/*^^^^ \n"
        + "     /,/,/,/,/,/,/,/,/,/nn\\\n"
        + "    /,/,/,/,/,/,/,/,/,/oonn\\\n"
        + "   V^^^^,/,/,/,/,/,/,/oooonnV\n"
        + "        ^^^^/,/,/,/,/ooooY  \n"
        + "             ^^^^^,/ooY     \n"
        + "                  V         \n";

        // Установить крышу
        public void Build()
        {
            string msg = "Установка крыши";

            // имитировать процесс постройки
            for (int i = 0; i < 15; ++i)
            {
                Drawer.LogProgress(msg);

                for (int j = 0; j < (i % 3 + 1); ++j)
                    Write('.');

                Thread.Sleep(250);
            }

            // изменить состояние крыши и вывести сообщение
            Drawer.LogProgress("Крыша установлена!");
            _isDone = true;
        }

        // Нарисовать крышу
        public void Draw()
        {
            Drawer.Draw(_graphics);
        }
    }

    // Дом
    class House
    {
        private bool _isDone;
        public bool IsDone { get { return _isDone; } }
        private List<IPart> _parts;

        public House(List<IPart> parts)
        {
            _parts = parts;
            _isDone = _parts.TrueForAll(p => p.IsDone);
        }

        // Проверить, какие части дома уже построены и возвратить следующую
        public IPart GetNextPart()
        {
            foreach(IPart p in _parts)
            {
                if (!p.IsDone)
                    return p;
            }

            // дом готов, если все его части готовы
            _isDone = true;
            return null;
        }

        // Нарисовать текущее состояние дома
        public void Draw()
        {
            foreach(IPart p in _parts)
            {
                // нарисовать часть дома, если она достроена
                if (p.IsDone)
                    p.Draw();
            }
        }
    }


    // Работник
    class Worker : IWorker
    {
        // массив возможных фраз
        string[] phrases = { 
            "Опять работа...", 
            "Ура! Сверхурочные!",
            "Говорила же мама в программисты идти...",
            "*неразборчивое мычание*",
            "Да **б его на%№!&&*!!",
            "Кто это %?@** построил?! А, это же я...",
            "Бетономешалка! Мешает бетон!",
            "Вот приду с работы и буду телек смотреть...",
            "Мужчина в жизни должен сделать 3 вещи..."
        };

        // Получить случайную фразу
        private string GetRandomPhrase()
        {
            return phrases[RNG.r.Next(phrases.Length)];
        }

        // Работать над домом
        public void Work(ref House house)
        {
            Drawer.LogWorkerWords(GetRandomPhrase());

            // получить следующую часть постройки
            IPart nextPart = house.GetNextPart();
            // если ещё есть недостроенная часть, построить её
            nextPart?.Build();
        }
    }

    // Лидер комманды строителей
    class TeamLeader : IWorker
    {
        // список строителей
        private List<IWorker> _workers;

        public TeamLeader(List<IWorker> workers)
        {
            _workers = workers;
        }

        // Построить дом
        public void Work(ref House house)
        {
            // ничего не делать, если нету работников
            if (_workers.Count == 0)
                return;

            // пока дом не будет достроен...
            do
            {
                // выбрать случайного работника
                IWorker randomWorker = _workers[RNG.r.Next(_workers.Count)];

                // дать работнику комманду строить дом
                randomWorker.Work(ref house);
                // нарисовать текущее состояние дома
                house.Draw();

                Thread.Sleep(1000);

            } while (!house.IsDone);

            Drawer.LogProgress("Постройка дома завершена! Нажмите любую клавишу для выхода");
        }
    }

    // Команда строителей
    class Team : IWorker
    {
        private IWorker _leader;

        public Team(IWorker leader)
        {
            _leader = leader;
        }

        // Построить дом
        public void Work(ref House house)
        {
            _leader.Work(ref house);
        }
    }

    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            // инициализировать генератор случайных чисел
            RNG.Init();

            // спрятать курсор
            CursorVisible = false;

            WriteLine("-= Домашнее Задание №8 =-");
            WriteLine("\tУченик: Шелест Александр\n");
            WriteLine("Реализовать программу \"Строительство дома\":");
            WriteLine("- классы:");
            WriteLine("  - House, Basement, Wall, Door, Window, Roof");
            WriteLine("  - Team, Worker, TeamLeader");
            WriteLine("- интерфейсы:");
            WriteLine("  - IWorker, IPart");
            WriteLine("\nСогласно проекту, дом состоит из: ");
            WriteLine("  1 фундамента, 4 стен, 1 двери, 4 окон и 1 крыши");

            WriteLine("\nНажмите любую клавишу для начала строительства!");
            ReadKey();

            // очистить экран
            Clear();

            // инициализировать дом со списком его частей
            House house = new House(new List<IPart> {
                    new Basement(),
                    new Wall(Side.Back),
                    new Window(Side.Back),
                    new Wall(Side.Right),
                    new Window(Side.Right),
                    new Wall(Side.Left),
                    new Window(Side.Left),
                    new Wall(Side.Front),
                    new Window(Side.Front),
                    new Door(),
                    new Roof()
                }
            );

            // создать список работников
            List<IWorker> workers = new List<IWorker>();
            for (int i = 0; i < 5; ++i)
                workers.Add(new Worker());

            // собрать комманду работников
            Team team = new Team(new TeamLeader(workers));

            // построить дом
            team.Work(ref house);

            // пауза программы
            Console.ReadKey();
        }
    }
}

