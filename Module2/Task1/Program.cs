using System;
using System.Linq;
using Task2;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter new string: ");
            string input = Console.ReadLine();

            try {
                int[] numbers = GetNumbers(input, int.Parse);

                Output(numbers);

                numbers = GetNumbers(input, new CustomInt32Parser().Parse);

                Output(numbers);
            } catch (FormatApplicationException ex) {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        private static int[] GetNumbers(string inputString, Func<string,int> parse)
        {
            try {
                int[] numbers = inputString.Trim().Split(' ').Select(parse).ToArray();

                return numbers;
            } catch (ArgumentNullException ex) {
                throw new FormatApplicationException("Something went wrong... sorry(", ex);
            } catch (OverflowException ex) {
                throw new FormatApplicationException("Something went wrong... sorry(", ex);
            } catch (FormatException ex) {
                throw new FormatApplicationException("Something went wrong... sorry(", ex);
            } catch (Int32ParserException ex) {
                throw new FormatApplicationException("Something went wrong... sorry(", ex);
            }

        }

        private static void Output(int[] integers)
        {
            int firstValue = integers.First();
            int middleValue = integers[integers.Length / 2];
            int lastValue = integers.Last();

            double arithmeticMean = integers.Average();

            Console.WriteLine($"The first value:  {firstValue}; The middle value: {middleValue}; The last one: {lastValue}; The arithmetic mean: {arithmeticMean}");
        }
    }
}
