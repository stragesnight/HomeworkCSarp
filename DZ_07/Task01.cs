/*
 *  -= Домашняя Работа №7, Задание №1 =-
 *      Ученик: Шелест Александр
 *
 *  Разработать абстрактный класс "ГеометрическаяФигура" с методами
 *  "ПлощадьФигуры" и "ПериметрФигуры".
 *  Разработать классы-наследники: Треугольник, Квадрат, Ромб, Прямоугольник,
 *  Параллелограмм, Трапеция, Круг, Эллипс. 
 *  Реализовать конструкторы, которые однозначно определяют объекты данных классов.
 *
 *  Реализовать класс "СоставнаяФигура", который может состоять из 
 *  любого количества "ГеометрическихФигур". Для данного класса определить метод
 *  нахождения площади фигуры.
 */

using System;
using System.Collections.Generic;

namespace DZ_07
{
    // Геометрическая фигура
    public abstract class Figure
    {
        // Получить периметр фигуры
        public abstract double Perimeter();
        // Получить площадь фигуры
        public abstract double Area();

        // Получить строку фигуры
        public override string ToString()
        {
            return $"Периметр P = {Perimeter().ToString("N2")} см,\n"
            + $"Площадь S = {Area().ToString("N2")} см^2";
        }
    }

    // Треугольник
    class Triangle : Figure
    {
        private double _a;
        private double _b;
        private double _c;

        public double A
        {
            get { return _a; }
            set { if (value > 0) _a = value; }
        }

        public double B
        {
            get { return _b; }
            set { if (value > 0) _b = value; }
        }

        public double C
        {
            get { return _c; }
            set { if (value > 0) _c = value; }
        }

        public Triangle(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;

        }

        public override double Perimeter() => A + B + C;

        public override double Area()
        {
            double p = Perimeter() / 2d;
            // формула Герона
            return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
        }

        public override string ToString()
        {
            return $"Треугольник (A = {A}, B = {B}, C = {C}):\n"
                + base.ToString();
        }
    }

    // Квадрат
    class Square : Figure
    {
        private double _a;

        public double A
        {
            get { return _a; }
            set { if (value > 0) _a = value; }
        }

        public Square(double a)
        {
            A = a;
        }

        public override double Perimeter() => A * 4;
        public override double Area() => A * A;

        public override string ToString()
        {
            return $"Квадрат (A = {A}):\n" + base.ToString();
        }
    }

    // Ромб
    class Rhombus : Figure
    {
        private double _a;
        private double _h;

        public double A
        {
            get { return _a; }
            set { if (value > 0) _a = value; }
        }

        public double H
        {
            get { return _h; }
            set { if (value > 0) _h = value; }
        }

        public Rhombus(double a, double h)
        {
            A = a;
            H = h;
        }

        public override double Perimeter() => A * 4;
        public override double Area() => A * H;

        public override string ToString()
        {
            return $"Ромб (A = {A}, h = {H}):\n" + base.ToString();
        }
    }

    // Прямоугольник
    class Rectange : Figure
    {
        private double _a;
        private double _b;

        public double A
        {
            get { return _a; }
            set { if (value > 0) _a = value; }
        }

        public double B
        {
            get { return _b; }
            set { if (value > 0) _b = value; }
        }

        public Rectange(double a, double b)
        {
            A = a;
            B = b;
        }

        public override double Perimeter() => (A * 2) + (B * 2);
        public override double Area() => A * B;

        public override string ToString()
        {
            return $"Прямоугольник (A = {A}, B = {B}):\n" + base.ToString();
        }
    }

    // Параллелограмм
    class Parallelogram : Figure
    {
        private double _a;
        private double _b;
        private double _alpha;

        public double A
        {
            get { return _a; }
            set { if (value > 0) _a = value; }
        }

        public double B
        {
            get { return _b; }
            set { if (value > 0) _b = value; }
        }

        public double Alpha
        {
            get { return _alpha; }
            set { if ((value > 0) && (value < 180)) _alpha = value; }
        }

        public Parallelogram(double a, double b, double alpha)
        {
            A = a;
            B = b;
            Alpha = alpha;
        }

        public override double Perimeter() => (A * 2) + (B * 2);
        public override double Area() => A * B * Math.Sin(Alpha);

        public override string ToString()
        {
            return $"Параллелограмм (A = {A}, B = {B}, alpha = {Alpha}*):\n"
                + base.ToString();
        }
    }

    // Трапеция
    class Trapezium : Figure
    {
        private double _a;
        private double _b;
        private double _c;
        private double _d;

        public double A
        {
            get { return _a; }
            set { if (value > 0) _a = value; }
        }

        public double B
        {
            get { return _b; }
            set { if (value > 0) _b = value; }
        }

        public double C
        {
            get { return _c; }
            set { if (value > 0) _c = value; }
        }

        public double D
        {
            get { return _d; }
            set { if (value > 0) _d = value; }
        }

        public Trapezium(double a, double b, double c, double d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public override double Perimeter() => A + B + C + D;
        public override double Area()
        {
            double tmp = (((B - A) * (B - A)) + (C * C) - (D * D)) / (2 * (B - A));
            return Math.Sqrt(((A + B) / 2d) * Math.Sqrt(Math.Abs((C * C) - (tmp * tmp))));
        }

        public override string ToString()
        {
            return $"Трапеция (A = {A}, B = {B}, C = {C}, D = {D}):\n"
                + base.ToString();
        }
    }

    // Круг
    class Circle : Figure
    {
        private double _r;

        public double R
        {
            get { return _r; }
            set { if (value > 0) _r = value; }
        }

        public Circle(double r)
        {
            R = r;
        }

        public override double Perimeter() => Math.PI * (R * 2);
        public override double Area() => Math.PI * (R * R);

        public override string ToString()
        {
            return $"Круг (R = {R}):\n" + base.ToString();
        }
    }

    // Эллипс
    class Ellipse : Figure
    {
        private double _rShort;
        private double _rLong;

        public double RShort
        {
            get { return _rShort; }
            set { if (value > 0) _rShort = value; }
        }

        public double RLong
        {
            get { return _rLong; }
            set { if (value > 0) _rLong = value; }
        }

        public Ellipse(double rShort, double rLong)
        {
            RShort = rShort;
            RLong = rLong;
        }

        public override double Perimeter()
        {
            return 2 * Math.PI * Math.Sqrt(((RLong * RLong) + (RShort * RShort)) / 2d);
        }
        public override double Area() => Math.PI * ((RLong * 2) * (RShort * 2));

        public override string ToString()
        {
            return $"Эллипс (r = {RShort}, R = {RLong}):\n" + base.ToString();
        }
    }

    // Составная фигура
    class CompositeFigure : Figure
    {
        List<Figure> _figures;

        public CompositeFigure(List<Figure> figures)
        {
            _figures = new List<Figure>(figures);
        }

        public override double Perimeter()
        {
            double res = 0;

            foreach(Figure f in _figures)
                res += f.Perimeter();

            return res;
        }
        
        public override double Area()
        {
            double res = 0;

            foreach(Figure f in _figures)
                res += f.Area();

            return res;
        }

        public override string ToString()
        {
            return $"Составная фигура (всего фигур: {_figures.Count}):\n"
                + base.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // список геометрических фигур
            List<Figure> figures = new List<Figure> {
                new Triangle(3, 4, 5),
                new Square(6),
                new Rhombus(6, 7),
                new Parallelogram(4, 5, 40),
                new Trapezium(5, 6, 2, 4),
                new Circle(4),
                new Ellipse(4, 3)
            };

            Console.WriteLine("-= Домашняя Работа №7, Задание №1 =-");
            Console.WriteLine("    Ученик: Шелест Александр\n");
            Console.WriteLine("Разработать абстрактный класс \"ГеометрическаяФигура\" с методами");
            Console.WriteLine("\"ПлощадьФигуры\" и \"ПериметрФигуры\".");
            Console.WriteLine("Разработать классы-наследники: Треугольник, Квадрат, Ромб, Прямоугольник,");
            Console.WriteLine("Параллелограмм, Трапеция, Круг, Эллипс.");
            Console.WriteLine("Реализовать конструкторы, которые однозначно определяют объекты данных классов.\n");
            Console.WriteLine("Реализовать класс \"СоставнаяФигура\", который может состоять из ");
            Console.WriteLine("любого количества \"ГеометрическихФигур\". Для данного класса определить метод");
            Console.WriteLine("нахождения площади фигуры.\n");

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();

            Console.WriteLine("Список геометрических фигур:");
            // вывод каждой фигуры на экран
            foreach(Figure f in figures)
            {
                Console.WriteLine(f);
                Console.WriteLine("");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.WriteLine("Создание составной фигуры из всех вышеописанных...");

            // составная фигура
            CompositeFigure comp = new CompositeFigure(figures);
            // вывод составной фигуры на экран
            Console.WriteLine(comp);

            Console.WriteLine("\nНажмите любую клавишу для выхода из программы...");
            Console.ReadKey();
        }
    }
}
