/*
 *  -= Домашнее Задание №16 =-
 *      Ученик: Шелест Александр
 *
 *  Разработать игру "Тамагочи". Персонаж случайным образом
 *  выдает просьбы - покормить, погулять, уложить спать,
 *  полечить, поиграть. Если просьба не удаовлетворяется трижды,
 *  питомец "заболевает" и просит его полечить, а в случае
 *  отказа - "умирает".
 */

/*
    _/_   _/_    ___"      _/__
    / _   -|-      '   _   /__
     C_   C_>,  \___   /   ___|
*/

using System;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using static System.Console;


namespace TamagochiGame
{
    // делегат, используемый событием из движка игры
    delegate void EngineDelegate();
    // делегат, используемый событием нажатия клавишы
    delegate void KeyboardDelegate(char input);

    // воспомагательный класс для генерации случайных чисел
    internal static class RNG
    {
        public static Random R { get; } = new Random();
    }

    // Экран
    internal static class Screen
    {
        // структура для отображения точки в 2д пространстве
        public struct Vec2D
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Vec2D(int x = 0, int y = 0)
            {
                X = x;
                Y = y;
            }
        }

        // воспомагательная графика игры
        private static readonly string Title = ""
            + "_/_   _/_    ___\"      _/__\n"
            + "/ _   -|-      '   _   /__  \n"
            + " C_   C_>,  \\___   /   ___|\n";

        private static readonly string Buttons = 
              "(F)eed       (W)alk      (S)leep\n"
            + "  (C)ure     (P)lay    (E)xit";

        // константы игрового окна

        private static readonly Vec2D WindowSize = new Vec2D(49, 30);
        private static readonly Vec2D TitleStart = new Vec2D(10, 1);
        private static readonly Vec2D RoomStart = new Vec2D(9, 6);
        private static readonly Vec2D RoomEnd = new Vec2D(37, 18);
        private static readonly Vec2D TomodachiStart = new Vec2D(18, 10);
        private static readonly Vec2D StatusBoxStart = new Vec2D(9, 20);
        private static readonly Vec2D ButtonsStart = new Vec2D(7, 25);

        // Инициализировать игровой экран
        public static void Init()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SetWindowSize(WindowSize.X, WindowSize.Y);
                SetBufferSize(WindowSize.X, WindowSize.Y);
            }

            // подписаться на событие обновления
            Engine.UpdateEvent += OnUpdate;

            Console.Title = "Tamagochi!";
            Console.CursorVisible = false;
            Console.Clear();
            DrawStaticElements();
        }

        // Обновить игровой экран
        private static void OnUpdate()
        {
            // нарисовать персонажа
            FillSpace(TomodachiStart, new Vec2D(
                        TomodachiStart.X + 11, TomodachiStart.Y + 6), ' ');
            DrawTomodachi();

            // нарисовать статус персонажа
            FillSpace(StatusBoxStart, new Vec2D(
                        45, StatusBoxStart.Y + 2), ' ');
            
            Tomodachi tomodachi = Engine.tomodachi;
            string statusString = $"Health: {tomodachi.Health}\n"
                    + $"Current Request: {tomodachi.CurrentRequest}\n"
                    + $"Time left: {tomodachi.CurrentRequestTimer / 1000} sec";
            DrawGraphics(statusString, StatusBoxStart);
        }

        // Нарисовать статические элементы экрана
        private static void DrawStaticElements()
        {
            DrawGraphics(Title, TitleStart);
            DrawBox(RoomStart, RoomEnd, '_', '|');
            DrawGraphics(Buttons, ButtonsStart);
        }

        // Нарисовать персонажа
        private static void DrawTomodachi()
        {
            // определить позицию головы
            Vec2D headPos = new Vec2D(TomodachiStart.X, 
                    TomodachiStart.Y + Engine.tomodachi.FaceOffset);

            DrawGraphics(Engine.tomodachi.BodyGraphics, TomodachiStart);
            DrawGraphics(Engine.tomodachi.FaceGraphics, headPos);
        }

        // Нарисовать коробку указанными символами
        private static void DrawBox(Vec2D ul, Vec2D lr, char h, char v)
        {
            SetCursorPosition(ul.X, ul.Y);

            for (int y = ul.Y; y <= lr.Y; ++y)
            {
                if (y == ul.Y)
                {
                    Write(' ');
                    for (int x = ul.X; x <= lr.X - 2; ++x)
                        Write(h);
                }
                else if (y == ul.Y + 1)
                {
                    Write('/');
                    SetCursorPosition(lr.X, CursorTop);
                    Write('\\');
                }
                else if (y == lr.Y)
                {
                    Write('\\');
                    for (int x = ul.X; x <= lr.X - 2; ++x)
                        Write(h);
                    Write('/');
                }
                else
                {
                    Write(v);
                    SetCursorPosition(lr.X, CursorTop);
                    Write(v);
                }

                SetCursorPosition(ul.X, CursorTop + 1);
            }
        }

        // Нарисовать графический элемент
        private static void DrawGraphics(string graphics, Vec2D offset)
        {
            SetCursorPosition(offset.X, offset.Y);

            foreach (char c in graphics)
            {
                switch (c)
                {
                    case '\n':
                        SetCursorPosition(offset.X, CursorTop + 1);
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

        // Заполнить указанный отрезок экрана указанным символом
        private static void FillSpace(Vec2D ul, Vec2D lr, char c)
        {
            SetCursorPosition(ul.X, ul.Y);

            for (int y = ul.Y; y <= lr.Y; ++y)
            {
                for (int x = ul.X; x <= lr.X; ++x)
                    Write(c);

                SetCursorPosition(ul.X, CursorTop + 1);
            }
        }
    }

    // Клавиатура
    internal static class Keyboard
    {
        // Событие нажатия клавишы
        public static event KeyboardDelegate InputEvent = null;

        // Запустить цикл получения нажатий от пользователя
        public static void GatherInputs()
        {
            while (true)
                InputEvent?.Invoke(ReadKey(true).KeyChar);
        }
    }

    // Игровой движок
    internal static class Engine
    {
        // Событие обновления состояния игры
        public static event EngineDelegate UpdateEvent = null;

        public static Tomodachi tomodachi { get; private set; }
        public static readonly int UpdateDelay = 500;

        private static bool _gameOver = false;

        // Инициализировать игровой движок для указанного персонажа
        public static void Init(Tomodachi _tomodachi)
        {
            tomodachi = _tomodachi;
            // подписка на наэатие пользователем клавишы
            Keyboard.InputEvent += OnInput;
        }

        // Начать цикл обновления игры
        public static void Start()
        {
            while (!_gameOver)
            {
                UpdateEvent?.Invoke(); 
                Thread.Sleep(UpdateDelay);
            }

            Thread.CurrentThread.Join();
        }

        // Прекратить выполнение обновления игры
        public static void GameOver()
        {
            _gameOver = true;
            Environment.Exit(0);
        }

        // Метод, вызываемый при нажатии пользователем клавишы
        private static void OnInput(char input)
        {
            if (Enum.IsDefined(typeof(Tomodachi.Request), (Tomodachi.Request)input))
            {
                if ((Tomodachi.Request)input == tomodachi.CurrentRequest)
                    tomodachi.FulfillRequest();
                else
                    tomodachi.DismissRequest();
            }
            else if (input == 'e')
                GameOver();
        }
    }

    // Игровой персонаж, "Томодачи"
    internal sealed class Tomodachi
    {
        // Список возможных запросов
        public enum Request : byte
        { 
            Feed =  (byte)'f', 
            Walk =  (byte)'w', 
            Sleep = (byte)'s', 
            Cure =  (byte)'c', 
            Play =  (byte)'p',
            None =  255
        };

        // графика персонажа 

        private static readonly string[] _idleBody = {
              @"   v___v   "+'\n'
            + @"  /';;;'\  "+'\n'
            + @" /       \ "+'\n'
            + @" w       w "+'\n'
            + @"  \_____/  "+'\n'
            + @"  </   \>  ",

        '\n'+ @"  v_____v   "+'\n'
            + @" / ';;;' \  "+'\n'
            + @"w         w "+'\n'
            + @" \_______/  "+'\n'
            + @" </     \>  ",
        };

        private static readonly string[] _faces = {
               "\n\n"
            + @"    o o"+'\n'
            + @"     U ",

               "\n\n"
            + @"    o o"+'\n'
            + @"     - ",

               "\n\n"
            + @"    u u"+'\n'
            + @"     ^ ",

               "\n\n"
            + @"    X X"+'\n'
            + @"     ^ ",

               "\n\n"
            + @"   ~^ ^~"+'\n'
            + @"     '",

        };

        // поля для внутреннего использования

        private static readonly int _requestTimer = 120000;
        private static readonly int _faceLockTimer = 2000;
        private static readonly int _healthMax = 3;
        private int _currentRequestTimer = _requestTimer;
        private int _currentFaceLockTimer = _faceLockTimer;
        private int _currentFrame = 0;
        private Request _prevRequest = Request.None;

        // публичные поля

        public Request CurrentRequest { get; private set; } = Request.None;
        public string BodyGraphics { get; private set; } = _idleBody[0];
        public string FaceGraphics { get; private set; } = _faces[0];
        public int CurrentRequestTimer {get { return _currentRequestTimer; }}
        public int FaceOffset { get; private set; } = 0;
        public int Health { get; private set; } = _healthMax;

        // Конструктор
        public Tomodachi()
        {
            // подписка на событие обновления
            Engine.UpdateEvent += OnUpdate;
        }

        // Деструктор
        // Сохраняет персонажа в файл
        ~Tomodachi() => SaveToFile("tomodachi");

        // Сохранить персонажа в файл
        public void SaveToFile(string filename)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Create(filename)))
            {
                bw.Write(Health);
                bw.Write((byte)CurrentRequest);
                bw.Write(_currentRequestTimer);
            }
        }

        // Загрузить персонажа из файла
        public void LoadFromFile(string filename)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(File.OpenRead(filename)))
                {
                    Health = br.ReadInt32();
                    CurrentRequest = (Request)br.ReadByte();
                    _currentRequestTimer = br.ReadInt32();

                    // если сохранённый персонаж более недоступен,
                    // обновить его характеристики
                    if (Health == -1)
                    {
                        Health = _healthMax;
                        CurrentRequest = Request.None;
                        _currentRequestTimer = _requestTimer;
                    }
                }
            }
            catch (IOException)
            {
                return;
            }
        }

        // Восполнить запрос персонажа
        public void FulfillRequest()
        {
            // сбросить текущий запрос
            CurrentRequest = Request.None;
            
            // инкрементировать здоровье
            Health++;
            if (Health > _healthMax)
                Health = _healthMax;

            // активировать специальное выражение лица на 2 секунды
            _currentFaceLockTimer = _faceLockTimer;
            FaceGraphics = _faces[4];
        }

        // Отклонить запрос персонажа
        public void DismissRequest()
        {
            // сбросить текущий запрос
            CurrentRequest = Request.None;

            // декрементировать здоровье
            Health--;
            if (Health == 0)
                CreateNewRequest(Request.Cure);
            else if (Health < 0)
                Engine.GameOver();
        }

        // Метод, вызываемый при обновлении состояния игры
        private void OnUpdate()
        {
            // обновить таймер и счётчик
            _currentRequestTimer -= Engine.UpdateDelay;
            _currentFrame++;

            // выбрать новый кадр для тела
            BodyGraphics = _idleBody[_currentFrame % _idleBody.Length];

            // выбрать новое выражения лица, если нужно
            if (_currentFaceLockTimer < 0)
                FaceGraphics = _faces[_healthMax - Health];
            else
                _currentFaceLockTimer -= Engine.UpdateDelay;

            FaceOffset = _currentFrame % 2;

            // отклонить запрос, если время истекло
            if (_currentRequestTimer <= 0)
                DismissRequest();

            // сгенерировать новй запрос, если текущий запрос пуст
            if (CurrentRequest == Request.None)
                CreateNewRequest(GetRandomRequest());
        }

        // Получить случайный запрос
        private Request GetRandomRequest()
        {
            Request[] values = (Request[])Enum.GetValues(typeof(Request));
            return values[RNG.R.Next() % (values.Length - 1)];
        }

        // Инициализировать новый запрос
        private void CreateNewRequest(Request request)
        {
            // сгенерировать новый запрос, если он совпадает со старым
            while (request == _prevRequest)
                request = GetRandomRequest();

            // присвоить новый запрос и обновить таймер
            CurrentRequest = request;
            _prevRequest = CurrentRequest;
            _currentRequestTimer = _requestTimer;
        }
    }

    static class Program
    {
        static void Main()
        {
            WriteLine("-= Домашнее Задание №16 =-");
            WriteLine("    Ученик: Шелест Александр");
            WriteLine("Разработать игру \"Тамагочи\". Персонаж случайным образом");
            WriteLine("выдает просьбы - покормить, погулять, уложить спать,");
            WriteLine("полечить, поиграть. Если просьба не удаовлетворяется трижды,");
            WriteLine("питомец \"заболевает\" и просит его полечить, а в случае");
            WriteLine("отказа - \"умирает\".\n");
            WriteLine("Что-бы совершить действие, нажмите клавишу, ");
            WriteLine("соответствующую первой букве запроса, например:");
            WriteLine("Запрос Walk - клавиша 'w'");
            WriteLine("Запрос Play - клавиша 'p'");
            WriteLine("Запрос Cure - клавиша 'c'\n");
            WriteLine("Дополнительная информация:");
            WriteLine("- Названия всех запросов показаны внизу игрового экрана");
            WriteLine("- При совершении неправильного действия, тамагочи расстраивается");
            WriteLine("- Используются латинские символы, так что включите англ. раскладку!");
            WriteLine("- При выходе из программы прогресс автоматически сохраняется\n");
            WriteLine("Нажмите любую клавишу для начала игры!");
            ReadKey(true);
 
            // Инициализировать персонажа и попытаться загрузить его из файла
            Tomodachi tomodachi = new Tomodachi();
            tomodachi.LoadFromFile("tomodachi");

            // Инициализировать игровой экран и движок
            Screen.Init();
            Engine.Init(tomodachi);

            // добавить метод для вызова при закрытии программы
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Инициализировать 2 потока:
            // - поток движка для обновления графики и состояния игры
            // - поток клавиатуры для ввода пользователем данных
            Thread engineThread = new Thread(new ThreadStart(Engine.Start));
            Thread keyboardThread = new Thread(new ThreadStart(Keyboard.GatherInputs));
            
            // вызвать оба потока на выполнение
            keyboardThread.Start();
            engineThread.Start();
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Console.CursorVisible = true;
        }
    }
}

