/*
 *  -= Домашнее Задание №10 =-
 *      Ученик: Шелест Александр
 *
 *  Разработать игру "Автомобильные гонки" с использованием делегатов.
 *   - использовать несколько типов автомобилей
 *   - реализовать игру в гонки:
 *     - автомобили двигаются от старта к финишу
 *     - игра завершается, когда первый автомобиль достигает финиша
 *     - автомобили меняют свои скорости случайным образом в диапазоне 
 */

using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using static System.Console;

namespace DZ_10
{
    // Делегат для события, создаваемого автомобилем
    delegate void CarEvent(Car car);
    // Делегат для события гонки
    delegate void RaceEvent();

    // Воспомагательный класс для генерации случайных чисел
    internal sealed class RNG
    {
        private static Random r = null;

        public static Random R
        {
            get
            {
                if (r == null)
                    r = new Random();
                return r;
            }
        }

        // Сгенерировать случайный байт вокруг заданного значения
        public static byte NextByte(byte v, byte spray = 0)
        {
            byte offset = (spray == 0) 
                ? (byte)(R.Next() % (v / 2))
                : (byte)(R.Next() % spray);

            bool offsetPositive = (R.Next() & 1) == 0;
            return offsetPositive ? (byte)(v + offset) : (byte)(v - offset);
        }
    }

    // Воспомагательный класс для отрисовки графики в консоли
    internal class Screen
    {
        public static readonly byte _TRACK_WIDTH = 72;
        public static readonly byte _TRACK_HEIGHT = 1;
        private static readonly byte _LEFT_OFFSET = 2;
        private static readonly byte _TOP_OFFSET = 2;

        // Нарисовать гоночную дорожку для машины
        public static void DrawTrack(byte nTrack)
        {
            byte top = (byte)(((nTrack - 1) * _TRACK_HEIGHT) + _TOP_OFFSET);
            SetCursorPosition(_LEFT_OFFSET, top);

            Write('|');
            for (byte i = 0, len = (byte)(_TRACK_WIDTH - 3); i < len; ++i)
                Write('_');
            
            if ((nTrack & 1) == 0)
                Write("|#.");
            else
                Write("|.#");
        }

        // Нарисовать машину на заданной дорожке
        public static void DrawCar(Car car, byte nTrack)
        {
            // вычислить позицию машини на экране
            ushort traveled = (ushort)(car.TraveledDistance / RacingGame.GridSize);
            byte left = (byte)(_LEFT_OFFSET + traveled);
            byte top = (byte)(((nTrack - 1) * _TRACK_HEIGHT) + _TOP_OFFSET);

            SetCursorPosition(left, top);
            Write(car.Graphics);
        }

        // Нарисовать информацию о гонке
        public static void DrawRaceInfo(List<Car> cars)
        {
            byte top = (byte)(WindowHeight - _TOP_OFFSET - 1);
            top -= RacingGame.NumberOfCars;

            List<Car> sorted = cars.OrderBy(x => x.TraveledDistance).ToList();
            sorted.Reverse();

            SetCursorPosition(_LEFT_OFFSET, top);
            Write("/-------------------------------------------------\\");
            SetCursorPosition(_LEFT_OFFSET, CursorTop + 1);
            Write("|{0, -4}| {1, -16} | {2, -12} | {3, -8} |",
                    "RANK", "CAR NAME", "CAR MODEL", "SPEED");
            SetCursorPosition(_LEFT_OFFSET, CursorTop + 1);

            for (byte i = 0; i < RacingGame.NumberOfCars; ++i)
            {
                string fmt = String.Format(
                    "| №{0, -1} | {1, -16} | {2, -12} | {3, -3} km/h |",
                    i + 1, sorted[i].Name, sorted[i].Model, sorted[i].CurrentSpeed
                );

                Write(fmt);
                SetCursorPosition(_LEFT_OFFSET, CursorTop + 1);
            }

            Write("\\-------------------------------------------------/");
        }

        // Нарисовать произовольное сообщение на экране
        public static void DrawMessage(string msg)
        {
            byte top = (byte)((RacingGame.NumberOfCars * _TRACK_HEIGHT) + _TOP_OFFSET);
            SetCursorPosition(_LEFT_OFFSET, top + 2);
            for (byte i = 0; i < _TRACK_WIDTH; ++i)
                Write(' ');
            SetCursorPosition(_LEFT_OFFSET, top + 2);

            Write($"Latest Info: {msg}");
        }
    }

    // Абстрактный класс Машины
    internal abstract class Car
    {
        // Событие достижения машиной финишной линии
        public event CarEvent FinishedEvent;
        // Событие достижения машиной максимальной скорости
        public event CarEvent MaxSpeedEvent;
        // Событие для изменения машиной позиции на экране
        public event CarEvent PositionChangedEvent;

        // Публичные поля

        public string Name { get; protected set; }
        public string Model { get; protected set; }
        public byte Speed { get; protected set; }
        public string Graphics { get; protected set; }
        public ushort TraveledDistance { get; protected set; }
        public byte CurrentSpeed { get; protected set; }

        // Поля для внутреннего использования

        protected byte _speedRange;
        protected byte _accelerationSpeed;
        protected bool _reachedMaxSpeed;

        // Конструктор с параметрами
        public Car(string name, string model, byte speed)
        {
            Name = name;
            Model = model;
            Speed = speed;
            TraveledDistance = 0;
            CurrentSpeed = 0;
            _reachedMaxSpeed = false;

            // подписка на события гонки: её начало и обновление
            RacingGame.Instance.StartEvent += Start;
            RacingGame.Instance.UpdateEvent += Update;
        }

        // Изменить текущую скорость машины
        private void ChangeSpeed()
        {
            CurrentSpeed += _accelerationSpeed;

            byte delta = (byte)(RNG.R.Next() % _speedRange);
            bool deltaPositive = (RNG.R.Next() & 1) == 0;

            if (deltaPositive)
                CurrentSpeed += delta;
            else if (CurrentSpeed > delta)
                CurrentSpeed -= delta;
        }

        // Начать гонку
        protected virtual void Start()
        {
            Screen.DrawMessage($"\"{Name}\" started!");
        }

        // Обноаить состояние машины
        protected virtual void Update()
        {
            ChangeSpeed();

            // вызвать событие, если была достигнута максимальная скорость
            if (CurrentSpeed >= Speed)
            {
                CurrentSpeed = Speed;
                
                if (!_reachedMaxSpeed)
                {
                    MaxSpeedEvent?.Invoke(this);
                    _reachedMaxSpeed = true;
                }
            }

            ushort oldDist = (ushort)(TraveledDistance / RacingGame.GridSize);
            TraveledDistance += CurrentSpeed;
            ushort newDist = (ushort)(TraveledDistance / RacingGame.GridSize);

            // вызвать событие, если позиция машины на экране должна быть обновлена
            if (newDist != oldDist)
                PositionChangedEvent?.Invoke(this);

            // вызвать событие, если машина достигла линии финиша
            if (TraveledDistance >= RacingGame.TrackLength)
                FinishedEvent?.Invoke(this);
        }
    }

    // Классы, реализующие конкретные типы машин

    internal class PassengerCar : Car
    {
        public PassengerCar(string name, string model, byte speed)
            : base(name, model, speed)
        {
            _speedRange = RNG.NextByte(3);
            _accelerationSpeed = RNG.NextByte(6);
            Graphics = "EH3";
        }
    }

    internal class Truck : Car
    {
        public Truck(string name, string model, byte speed)
            : base(name, model, speed)
        {
            _speedRange = RNG.NextByte(2);
            _accelerationSpeed = RNG.NextByte(3);
            Graphics = "###-D";
        }
    }

    internal class Bus : Car
    {
        public Bus(string name, string model, byte speed)
            : base(name, model, speed)
        {
            _speedRange = RNG.NextByte(2);
            _accelerationSpeed = RNG.NextByte(4);
            Graphics = "EO0)";
        }
    }

    internal class RacingCar : Car
    {
        public RacingCar(string name, string model, byte speed)
            : base(name, model, speed)
        {
            _speedRange = RNG.NextByte(4);
            _accelerationSpeed = RNG.NextByte(8);
            Graphics = "|O->";
        }
    }

    // Класс управление гонкой
    class RacingGame
    {
        // Событие начала гонки
        public event RaceEvent StartEvent;
        // Событие обновления гонки
        public event RaceEvent UpdateEvent;

        // паттерн "Singleton"
        private static RacingGame _instance = null;
        public static RacingGame Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RacingGame();
                return _instance;
            }
        }

        // Публичные статические поля

        public static readonly byte NumberOfCars = 4;
        public static readonly ushort TrackLength = 8000;
        public static readonly ushort GridSize = (ushort)(TrackLength / Screen._TRACK_WIDTH);

        // Поля для внутреннего пользования

        private List<Car> _cars;
        private bool _raceFinished = false;

        // Подготовить машины к гонке
        public void PrepareCars()
        {
            // инициализировать список машин
            _cars = new List<Car> {
                new PassengerCar("Dad's Jiguli", "Shestyorka", RNG.NextByte(180, 20)),
                new Truck("Contra de Banda", "ABC-123", RNG.NextByte(150, 25)),
                new Bus("80 grn to Kyiv", "8qX221", RNG.NextByte(190, 30)),
                new RacingCar("Need for Speeds", "Y0L0", RNG.NextByte(210, 35)),
            };

            for (byte i = 0; i < _cars.Count; ++i)
            {
                // подписаться на события машин
                _cars[i].MaxSpeedEvent += OnMaxSpeed;
                _cars[i].PositionChangedEvent += OnCarPositionChanged;
                _cars[i].FinishedEvent += OnFinish;

                // нарисовать начальное состояние гонки
                Screen.DrawTrack((byte)(i + 1));
                Screen.DrawCar(_cars[i], (byte)(i + 1));
            }

            // нарисовать начальную статистику гонки
            Screen.DrawRaceInfo(_cars);
        }

        // Начать гонку
        public void Start()
        {
            _raceFinished = false;

            // отсчёт до начала гонки
            for (byte i = 3; i > 0; --i)
            {
                Screen.DrawMessage($"Race starts in {i}...");
                Thread.Sleep(1000);
            }

            // вызвать событие начала гонки
            StartEvent?.Invoke();
            Screen.DrawMessage("Race started!!");

            // обновлять состояние гонки, пока она не будет завершена
            while(!_raceFinished)
                Update();
        }

        // Обновить состояние гонки
        private void Update()
        {
            // вызвать событие обновления
            UpdateEvent?.Invoke();
            // задержка программы
            Thread.Sleep(200);
        }

        // Метод, исполняемый при достижении машиной максимальной скорости
        private void OnMaxSpeed(Car c)
        {
            if (!_raceFinished)
                Screen.DrawMessage($"\"{c.Name}\" reached maximum speed! ({c.Speed} km/h)");
        }

        // Метод, исполняемый при необходимости перерисовать машину
        private void OnCarPositionChanged(Car c)
        {
            byte track = (byte)(_cars.IndexOf(c) + 1);
            Screen.DrawTrack(track);
            Screen.DrawCar(c, track);
            Screen.DrawRaceInfo(_cars);
        }

        // Метод, вызываемый по завершению гонки
        private void OnFinish(Car c)
        {
            Screen.DrawMessage($"\"{c.Name}\" reached finish line! Race finished!!!");
            _raceFinished = true;
        }
    }

    class Program
    {
        // Главная программы
        static void Main(string[] args)
        {
            // изменить размер окна консоли
            Console.SetWindowSize(86, 28);

            // очистить консоль и спрятать курсор
            Console.Clear();
            Console.CursorVisible = false;

            // вывести начальный текст на экран
            WriteLine("-= Домашнее Задание №10 =-");
            WriteLine("\tУченик: Шелест Александр\n");
            WriteLine("Разработать игру \"Автомобильные гонки\" с использованием делегатов.");
            WriteLine(" - использовать несколько типов автомобилей");
            WriteLine(" - реализовать игру в гонки:");
            WriteLine("   - автомобили двигаются от старта к финишу");
            WriteLine("   - игра завершается, когда первый автомобиль достигает финиша");
            WriteLine("   - автомобили меняют свои скорости случайным образом в диапазоне");

            WriteLine("\nнажмите любую клавишу для начала гонки...");
            Console.ReadKey();
            Console.Clear();

            // приготовить машины к гонке
            RacingGame.Instance.PrepareCars();
            // начать гонку
            RacingGame.Instance.Start();

            // пауза программы перед её завершением
            Console.ReadKey();
            Console.CursorVisible = true;
        }
    }
}

