using System;

namespace DZ_04
{
    // Зажигалка
    // Класс, иммитирующий поведение зажигалки
    class Lighter
    {
        // приватные поля

        private double _price;          // цена
        private float _resourceLeft;    // остача ресурса
        private string _manufacturer;   // производитель
        private bool _isUsingGas;       // использует ли зажигалка газ
        private uint _useCount;         // количество использований
        private uint _wastedUseTries;   // количество неудачных попыток использовать
        private int _avgLightUpCount;   // соотношение удачных попыток к неудачным

        // статические поля

        private static double _revenue = 0;             // общий доход компании
        private static uint _activeLighterCount = 0;    // количество активных зажигалок


        // Конструктор с параметрами
        public Lighter(double price, float resource, string manufacturer, bool isUsingGas)
        {
            _price = price;
            _resourceLeft = resource;
            _manufacturer = manufacturer;
            _isUsingGas = isUsingGas;
            _useCount = 0;
            _wastedUseTries = 0;
            _avgLightUpCount = 0;

            _revenue += price;
            _activeLighterCount++;
        }

        // Попытаться зажечь зажигалку
        private void TryLightUp(ref string obj)
        {
            uint n = 0;
            Random r = new Random();

            do
            {
                n++;
                Console.WriteLine($"{n}-ая попытка поджечь '{obj}'...");

            } while ((r.Next() % 5) != 0);

            _wastedUseTries += n;
        }

        // Зажечь зажигалку
        public void LightUp(ref string obj)
        {
            // проверка, есть ли в зажигалке речурс
            if (_resourceLeft <= 0)
            {
                Console.WriteLine("Невозможно использовать зажигалку! У неё закончился ресурс");
                return;
            }

            // попытаться зажечь зажигалку
            TryLightUp(ref obj);

            // обновить состояние объекта
            Console.WriteLine($"Успешно! '{obj}' теперь подожжён");
            obj += " (подожжён)";

            // обновить значения полей
            _useCount++;
            _resourceLeft -= 1.2f;
            _avgLightUpCount = (int)((float)_wastedUseTries / (float)_useCount);

            // оповестить пользователя, если у зажигалки закончился ресурс
            if (_resourceLeft <= 0)
            {
                Console.WriteLine("У зажигалки закончился ресурс.");
                _activeLighterCount--;
            }
        }

        // Получить количество оставшегося ресурса
        public float CheckResourceLeft() => _resourceLeft;

        // Превратить объект класса в строку
        public override string ToString()
        {
            return $"Зажигалка '{_manufacturer}':\n"
                + $"\tКол-во использований: {_useCount}\n"
                + $"\tОстача ресурса: {_resourceLeft}\n"
                + $"\tСреднее кол-во попыток зажечь: {_avgLightUpCount}\n"
                + $"\tИспользует газ?: {(_isUsingGas ? "да" : "нет")}\n"
                + $"\tБыла продана за ${_price}";
        }

        // Получить общий доход компании
        public static double GetTotalRevenue() => _revenue;
        // Получить количество активных зажигалок
        public static uint GetTotalActiveLighterCount() => _activeLighterCount;
    }
}

