using System;
using System.Linq;

namespace Task2.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Task();
        }

        static void Task()
        {
            using (var context = new Northwind())
            {
                foreach (var category in context.Categories) {
                    foreach (var product in category.Products) {
                        var orderDetails = context.Order_Details.Where(o => o.ProductID == product.ProductID);
                        foreach (var od in orderDetails) {
                            var productOrders = context.Orders.Where(o => o.OrderID == od.OrderID);

                            foreach (var categoryOrder in productOrders) {
                                Console.WriteLine(od.Product.Category.CategoryName);
                                Console.WriteLine("Product Name - " + od.Product.ProductName);
                                Console.WriteLine("Company Name - " + categoryOrder.Customer.CompanyName);
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
        }
    }
}
