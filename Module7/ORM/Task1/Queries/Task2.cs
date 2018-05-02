using System;
using System.Linq;
using LinqToDB;

namespace Task1.Queries
{
    public static class Task2
    {
        public static void ProductListWithCategoryAndSuppliers()
        {
            using (var db = new Northwind()){
                foreach (var product in db.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier)){
                    Console.WriteLine("Product Name - " + product.Name + ", Category - " + product.Category.Name + ", Supplier - " + product.Supplier.CompanyName);
                }
            }
        }

        public static void EmployeeListWithRegion()
        {
            using (var db = new Northwind()) {
                foreach (var et in db.EmployeeTerritories.Select(et => new {
                        et.Employee.FirstName,
                        et.Employee.LastName,
                        Region = et.Territory.Region.Description
                    }).Distinct()) {
                    Console.WriteLine("First Name: " + et.FirstName + ", Last Name: " + et.LastName + ", Region - " + et.Region);
                }
            }
        }

        public static void EmployeeCountForRegion()
        {
            using (var db = new Northwind()) {
                foreach (var et in db.EmployeeTerritories.Select(et => new {
                    EmployeeName = et.Employee.LastName + ", " + et.Employee.LastName,
                    Region = et.Territory.Region.Description
                }).Distinct().GroupBy(et => et.Region)){
                    Console.WriteLine("Region - " + et.Key + ", Count - " + et.Count());
                }
            }
        }

        public static void EmployeeListWithOrders()
        {
            using (var db = new Northwind()) {
                foreach (var o in db.Orders.Select(o => new {
                    EmployeeName = o.Employee.LastName + ", " + o.Employee.LastName,
                    ShipperName = o.Shipper.Name
                }).Distinct().GroupBy(o => o.EmployeeName))
                {
                    foreach (var order in o){
                        Console.WriteLine("EmployeeName - " + o.Key + ", ShipperName - " + order.ShipperName);
                    }
                }
            }
        }
    }
}
