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
using System.Threading;
using System.Runtime.InteropServices;
using static System.Console;


namespace TamagochiGame
{
    delegate void EngineDelegate();
    delegate void KeyboardDelegate(char input);

    internal static class RNG
    {
        public static Random R { get; } = new Random();
    }

    internal static class Screen
    {
        public struct Vec2D
        {
            public static Vec2D Identity = new Vec2D { X = 1, Y = 1 };
            public int X { get; set; }
            public int Y { get; set; }
        }

        public static readonly Vec2D WindowSize = new Vec2D { X = 86, Y = 32 };

        public static void Init()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SetWindowSize(WindowSize.X, WindowSize.Y);
                SetBufferSize(WindowSize.X, WindowSize.Y);
            }

            Engine.UpdateEvent += OnUpdate;
            Console.Clear();
        }

        public static void DrawTomodachi(Tomodachi tomodachi)
        {
            DrawGraphics(tomodachi.BodyGraphics, Vec2D.Identity);
            DrawGraphics(tomodachi.FaceGraphics, 
                    new Vec2D {X = 1, Y=tomodachi.FaceOffset+1});
        }

        private static void OnUpdate()
        {

        }

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
    }

    internal static class Keyboard
    {
        public static event KeyboardDelegate InputEvent = null;

        public static void GatherInputs()
        {
            while (true)
                InputEvent?.Invoke(ReadKey(true).KeyChar);
        }
    }

    internal static class Engine
    {
        public static event EngineDelegate UpdateEvent = null;

        public static Tomodachi tomodachi { get; private set; }
        public static readonly int UpdateDelay = 500;

        private static bool _gameOver = false;

        public static void Init(Tomodachi _tomodachi)
        {
            tomodachi = _tomodachi;
            Keyboard.InputEvent += OnInput;
        }

        public static void Start()
        {
            while (!_gameOver)
            {
                Update();
                Thread.Sleep(UpdateDelay);
            }
        }

        public static void GameOver()
        {
            _gameOver = true;
        }

        private static void Update()
        {
            UpdateEvent?.Invoke();
            Console.Clear();
            Screen.DrawTomodachi(tomodachi);


            //Thread.CurrentThread.Join();
        }

        private static void OnInput(char input)
        {
            if (Enum.IsDefined(typeof(Tomodachi.Request), (Tomodachi.Request)input))
            {
                if ((Tomodachi.Request)input == tomodachi.CurrentRequest)
                    tomodachi.FulfillRequest();
                else
                    tomodachi.DismissRequest();
            }
        }
    }

    internal sealed class Tomodachi
    {
        public enum Request : byte
        { 
            Feed =      (byte)'f', 
            GoOutside = (byte)'g', 
            Sleep =     (byte)'s', 
            Cure =      (byte)'c', 
            Play =      (byte)'p',
            None =      255
        };

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
        };

        private static readonly int _requestTimer = 12000;
        private static readonly int _healthMax = 3;
        private int _currentRequestTimer = _requestTimer;
        private int _currentFrame = 0;

        public Request CurrentRequest { get; private set; } = Request.None;
        public string BodyGraphics { get; private set; } = _idleBody[0];
        public string FaceGraphics { get; private set; } = _faces[0];
        public int FaceOffset { get; private set; } = 0;
        public int Health { get; private set; } = _healthMax;

        public Tomodachi()
        {
            Engine.UpdateEvent += OnUpdate;
        }

        public void FulfillRequest()
        {
            CurrentRequest = Request.None;
            
            Health++;
            if (Health > _healthMax)
                Health = _healthMax;
        }

        public void DismissRequest()
        {
            CurrentRequest = Request.None;

            Health--;
            if (Health == 0)
            {
                CurrentRequest = Request.Cure;
                _currentRequestTimer = _requestTimer;
            }
            else if (Health < 0)
                Engine.GameOver();
        }

        private void OnUpdate()
        {
            _currentRequestTimer -= Engine.UpdateDelay;
            _currentFrame++;

            BodyGraphics = _idleBody[_currentFrame % _idleBody.Length];
            FaceGraphics = _faces[_healthMax - Health];
            FaceOffset = _currentFrame % 2;

            if (_currentRequestTimer <= 0)
                DismissRequest();

            if (CurrentRequest == Request.None)
                CreateNewRequest();
        }

        private void CreateNewRequest()
        {
            Request[] values = (Request[])Enum.GetValues(typeof(Request));
            CurrentRequest = values[RNG.R.Next() % (values.Length - 1)];
            _currentRequestTimer = _requestTimer;
        }
    }

    static class Program
    {
        static void Main()
        {
            Screen.Init();
            Engine.Init(new Tomodachi());

            Thread engineThread = new Thread(new ThreadStart(Engine.Start));
            Thread keyboardThread = new Thread(new ThreadStart(Keyboard.GatherInputs));
            
            keyboardThread.Start();
            engineThread.Start();
        }
    }
}

