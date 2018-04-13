using System;

namespace EPAM.NET.Mentoring
{
    public class Triangle: IFigure
    {
        public double A { get; }

        public double B { get; }

        public double C { get; }

        public Triangle(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0)
                throw new ArgumentOutOfRangeException();

            if (!isTriangle(a, b, c))
                throw new ArgumentOutOfRangeException();

            A = a;
            B = b;
            C = c;
        }

        public double Area()
        {
            double semiPerimeter = (A + B + C) / 2;
            return Math.Sqrt(semiPerimeter * (semiPerimeter - A) * (semiPerimeter - B) * (semiPerimeter - C));
        }

        public double Perimeter()
        {
            return A + B + C;
        }

        private bool isTriangle(double a, double b, double c)
        {
            return a < b + c && (b < a + c) && (c < a + b);
        }
    }
}
