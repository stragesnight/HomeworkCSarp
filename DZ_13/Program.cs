/*
 *  -= Домашнее Задание №13 =-
 *      Ученик: Шелест Александр
 *
 *  Создать модель карточной игры.
 *  Классы:
 *   * Game - формирует список игроков, тасует карты,
 *            раздает карты игрокам и управляет игровым процессом.
 *   * Player - список имеющихся карт, вывод информации
 *   * Card - игровая карта, имеет масть и тип.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static System.Console;

namespace DZ_13
{
    // Воспомагательный класс для генерации случайных чисел
    internal static class RNG
    {
        private static Random _r = new Random();
        public static Random R { get { return _r; } }
    }

    // Воспомагательный класс для рисования на экране
    internal static class Screen
    {
        // размеры окна

        public static readonly int WindowWidth = 72;
        public static readonly int WindowHeight = 22;

        // поля для разметки текста на экране

        private static int _margin = 1;
        private static int _playersStartLeft = _margin;
        private static int _playersStartTop = _margin + 3;
        private static int _cardListStartLeft = WindowWidth - (WindowWidth / 3) + _margin;
        private static int _cardListStartTop = _margin + 3;
        private static int _gameInfoStartLeft = _margin;
        private static int _gameInfoStartTop = (WindowHeight / 2) + _margin;
        private static int _gameInfoTextWidth = WindowWidth - (WindowWidth / 3) - (_margin * 2);
        private static int _statusLineStartLeft = _margin;
        private static int _statusLineStartTop = WindowHeight - _margin - 1;

        // Инициализировать игровое окно
        public static void Initialize()
        {
            // изменить размеры окна и подготовить консоль
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SetWindowSize(WindowWidth, WindowHeight);
                SetBufferSize(WindowWidth, WindowHeight);
            }
            Console.Clear();
            CursorVisible = false;

            int halfWidth = WindowWidth / 2;

            // название игры
            string msg = ".-=  CARD GAME  =-.";
            SetCursorPosition(halfWidth - (msg.Length / 2) - 1, 0);
            Write(msg);

            // список игроков
            SetCursorPosition(_playersStartLeft, _playersStartTop - 2);
            Write("Players:");

            // игровая информация
            SetCursorPosition(_gameInfoStartLeft, _gameInfoStartTop - 2);
            for (int i = 0; i < _cardListStartLeft - (_margin * 2); i++)
                Write('-');
            SetCursorPosition(_gameInfoStartLeft, _gameInfoStartTop - 1);
            Write("Game info:");

            // разделение окна на 2 части
            SetCursorPosition(_cardListStartLeft - 2, _margin);
            for (int i = 0; i < WindowHeight - (_margin * 3); ++i)
            {
                Write('|');
                SetCursorPosition(_cardListStartLeft - 2, CursorTop + 1);
            }

            // статусная линия внизу
            SetCursorPosition(_statusLineStartLeft, _statusLineStartTop - 1);
            for (int i = 0; i < WindowWidth - (_margin * 2); i++)
                Write('-');
        }

        // Нарисовать информацию об игроке
        public static void DrawPlayer(Player player, int position, bool isAI = true)
        {
            SetCursorPosition(_playersStartLeft, _playersStartTop + position);
            Write($"{position + 1}. {player.ToString()}{(isAI ? "" : " (you)")}");
        }

        // Нарисовать информацию об игроках
        public static void DrawPlayers(List<Player> players)
        {
            for (int i = 0; i < players.Count; ++i)
                DrawPlayer(players[i], i);
        }

        // Нарисовать руку игрока (список его карт)
        public static void DrawPlayerHand(Player player)
        {
            SetCursorPosition(_cardListStartLeft, _cardListStartTop - 2);
            Write($"{player.Name}'s hand: ");

            SetCursorPosition(_cardListStartLeft, _cardListStartTop);
            for (int i = 0, len = player.Hand.Count; i < len; ++i)
            {
                Write($"{i + 1}. {player.Hand[i].ToString()}");
                SetCursorPosition(_cardListStartLeft, CursorTop + 1);
            }
        }

        // Нарисовать информацию об игре
        public static void DrawGameInfo(string msg)
        {
            SetCursorPosition(_gameInfoStartLeft, _gameInfoStartTop);

            foreach (char c in msg)
            {
                // сместить курсор если он вышел за пределы рамки
                if ((CursorLeft - _gameInfoStartLeft) >= _gameInfoTextWidth)
                    SetCursorPosition(_gameInfoStartLeft, CursorTop + 1);

                if(c == '\n')
                    SetCursorPosition(_gameInfoStartLeft, CursorTop + 1);
                else
                    Write(c);
            }
        }

        // Нарисовать статусную строку
        public static void DrawStatusLine(string msg)
        {
            SetCursorPosition(_statusLineStartLeft, _statusLineStartTop);
            for (int i = 0; i < WindowWidth - (_margin * 2); ++i)
                Write(' ');
            SetCursorPosition(_statusLineStartLeft, _statusLineStartTop);
            Write(msg);
        }
    }

    // Класс карты, которую можно сравнивать с другими
    internal sealed class Card : IComparable<Card>
    {
        // Масть карты
        public enum Suit : byte { Diamonds, Clubs, Hearts, Spades };
        // Тип карты
        public enum Type : byte { Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };

        public Suit Suit_ { get; set; }
        public Type Type_ { get; set; }

        // Сравнить карту с другой
        public int CompareTo(Card other)
        {
            // шестёрка забирает туза
            if (Type_ == Type.Six && other.Type_ == Type.Ace)
                return 1;
            // преобразовать перечисление в число и сравнить
            if ((int)Type_ > (int)other.Type_)
                return 1;
            else if ((int)Type_ < (int)other.Type_)
                return -1;
            else
                return 0;
        }

        // Превратить карту в строку
        public override string ToString()
            => $"{Type_.ToString()} of {Suit_.ToString()}";
    }

    // Класс игрока
    internal class Player
    {
        public string Name { get; set; }

        protected List<Card> _hand = new List<Card>();
        public List<Card> Hand 
        { 
            get { return _hand; } 
            protected set { _hand = value; } 
        }

        
        // Выбрать карту и положить её на игровое поле
        public virtual Card PutCard()
        {
            if (Hand.Count == 0)
                return null;

            Card buff = Hand[RNG.R.Next() % Hand.Count];
            Hand.Remove(buff);
            return buff;
        }

        // Взять карты и положить их себе в руку
        public void TakeCard(Card card) => Hand.Add(card);


        // Преобразовать игрока в строку
        public override string ToString()
            => $"{Name}, card count: {Hand.Count}";

        // Проверить, равны ли объекты
        public override bool Equals(object obj)
            => (obj as Player).Name == this.Name;
    }

    // Игрок-человек
    internal sealed class HumanPlayer : Player
    {
        private int GetCardIndex(string msg)
        {
            int res = 0;

            do
            {
                Screen.DrawStatusLine(msg);

            } while (!(int.TryParse(ReadLine(), out res)
                        && (res > 0 && res <= Hand.Count)));

            return res - 1;
        }

        // Положить на стол карту, выбираемую пользователем
        public override Card PutCard()
        {
            if (Hand.Count == 0)
                return null;

            int index = GetCardIndex("Choose a card to put by its number in a list: ");
            Card buff = Hand[index];
            Hand.RemoveAt(index);
            return buff;
        }
    }

    // Класс игрового менеджера
    internal sealed class Game
    {
        public List<Player> Players { get; set; }
        public Player HPlayer { get; set; }

        private List<Card> _deck;
        private Player _winner = null;
        private string _gameInfo = "";
        private readonly int _cardsInHand = 5;
        private int _turn = 0;

        // Перетасовать колоду
        private void ShuffleDeck()
        {
            for (int i = 0, len = RNG.R.Next() % _deck.Count; i < len; ++i)
            {
                int r1 = RNG.R.Next() % _deck.Count;
                int r2 = RNG.R.Next() % _deck.Count;

                Card buff = _deck[r1];
                _deck[r1] = _deck[r2];
                _deck[r2] = buff;
            }
        }

        // Выдать карты игрокам
        private void HandOutCards(int amount)
        {
            if (amount > (_deck.Count / Players.Count))
                amount = _deck.Count / Players.Count;

            foreach (Player p in Players)
            {
                for (int i = 0; i < amount; ++i)
                {
                    p.Hand.Add(_deck[_deck.Count - 1]);
                    _deck.RemoveAt(_deck.Count - 1);
                }
            }
        }

        // Проверить руку игрока на условия победы или поражения
        private void CheckPlayersHand(Player p)
        {
            if (p.Hand.Count == 0)
            {
                _gameInfo += $"{p.Name} has no cards left!\n";
                Players.Remove(p);
            }
            else if (Players.Count == 1 && Players.Contains(p))
            {
                _gameInfo += $"{p.Name} wins the game!\n";
                _winner = p;
            }
        }

        // Провести один игровой ход
        private void TakeTurn()
        {
            // выбрать игроков для текущего хода
            int i1 = _turn % Players.Count;
            int i2 = (i1 + 1) % Players.Count;
            Player p1 = Players[i1];
            Player p2 = Players[i2];

            // взять у игроков карты
            Card c1 = p1.PutCard();
            Card c2 = p2.PutCard();

            _gameInfo += $"{p1.Name} puts {c1.ToString()}\n";
            _gameInfo += $"{p2.Name} puts {c2.ToString()}\n";
            
            int compared = c1.CompareTo(c2);

            Card fromDeck = null;
            if (_deck.Count != 0)
            {
                fromDeck = _deck[_deck.Count - 1];
                _deck.RemoveAt(_deck.Count - 1);
            }

            // сравнить карты
            if (compared >= 0)
            {
                _gameInfo += $"{c1.ToString()} is bigger than {c2.ToString()}\n";
                _gameInfo += $"{p1.Name} takes two cards\n";
                p1.TakeCard(c2);
                if (fromDeck != null)
                    p1.TakeCard(fromDeck);
            }
            else
            {
                _gameInfo += $"{c2.ToString()} is bigger than {c1.ToString()}\n";
                _gameInfo += $"{p2.Name} takes two cards\n";
                p2.TakeCard(c1);
                if (fromDeck != null)
                    p2.TakeCard(fromDeck);
            }

            _gameInfo += $"{_deck.Count} cards left in deck\n";

            CheckPlayersHand(p2);
            CheckPlayersHand(p1);
                
            _turn++;
        }

        // Инициализировать карточную игру
        public void Start()
        {
            // проверить корректность количестве игроков
            if (Players.Count < 2 || Players.Count > 6)
                throw new Exception("Invalid number of players");

            // инициализировать колоду карт
            _deck = new List<Card>();
            for (int i = 0; i < 36; ++i)
            {
                _deck.Add(new Card {
                        Suit_ = (Card.Suit)((i / 9) % 4),
                        Type_ = (Card.Type)(i % 9)
                    }
                );
            }

            // перетасовать колоду
            ShuffleDeck();
            // выдать карты игрокам
            HandOutCards(_cardsInHand);

            // инициализировать игровой экран
            Screen.Initialize();
            Screen.DrawPlayers(Players);
            Screen.DrawPlayerHand(HPlayer);

            // основной цикл выполнения
            do
            {
                // выполнить один ход
                _gameInfo = "";
                TakeTurn();

                // отрисовать обновлённые данные на экране
                Screen.Initialize();
                Screen.DrawPlayers(Players);
                Screen.DrawPlayerHand(HPlayer);
                Screen.DrawGameInfo(_gameInfo);
                Screen.DrawStatusLine("Press any key to continue");
                ReadKey();

            } while (_winner == null);
        }
    }

    class Program
    {
        public static void Main()
        {
            // инициализировать игровой экран
            Screen.Initialize();

            // объект игрока (управляемого пользователем)
            Player player = new HumanPlayer { Name = "Player" };

            // объект игры
            Game game = new Game { 
                Players = new List<Player> {
                    player,
                    new Player { Name = "Valentin" }, 
                    new Player { Name = "Murasaki" }
                },
                HPlayer = player
            };

            // начать игру
            game.Start();

            // пауза программы и восстановление параметров
            ReadKey();
            Console.Clear();
            CursorVisible = true;
        }
    }
}

