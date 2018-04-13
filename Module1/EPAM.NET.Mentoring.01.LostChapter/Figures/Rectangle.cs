using System;

namespace EPAM.NET.Mentoring
{
    public class Rectangle: IFigure
    {
        public double A { get; }

        public double B { get; }

        public Rectangle(double a, double b)
        {
            if (a <= 0 || b <= 0)
                throw new ArgumentOutOfRangeException();

            A = a;
            B = b;
        }

        public double Area()
        {
            return A * B;
        }

        public double Perimeter()
        {
            return 2 * (A + B);
        }
    }
}
