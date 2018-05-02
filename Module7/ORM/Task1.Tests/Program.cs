using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Queries;

namespace Task1.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Task2:");
            Console.WriteLine("ProductListWithCategoryAndSuppliers:");
            Task2.ProductListWithCategoryAndSuppliers();

            Console.WriteLine();
            Console.WriteLine("EmployeeListWithRegion:");
            Task2.EmployeeListWithRegion();

            Console.WriteLine();
            Console.WriteLine("EmployeeCountForRegion:");
            Task2.EmployeeCountForRegion();

            Console.WriteLine();
            Console.WriteLine("EmployeeListWithOrders:");
            Task2.EmployeeListWithOrders();

            Console.WriteLine();
            Console.WriteLine("Task3:");
            Console.WriteLine("AddNewEmployeeWithTerritories");
            Task3.AddNewEmployeeWithTerritories();

            Console.WriteLine();
            Console.WriteLine("MoveProductsToAnotherCategory");
            Task3.MoveProductsToAnotherCategory();

            Console.WriteLine();
            Console.WriteLine("AddProducts");
            Task3.AddProducts();

            Console.WriteLine();
            Console.WriteLine("ReplaceProductWithAnalog");
            Task3.ReplaceProductWithAnalog();
        }
    }
}
