using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using Task1.Entities;

namespace Task1.Queries
{
    public static class Task3
    {
        public static void AddNewEmployeeWithTerritories()
        {
            using (var db = new Northwind()) {
                var id = Convert.ToInt32(db.InsertWithIdentity(
                    new Employee { FirstName = "Sergei", LastName = "Yesenin" }));

                var territories = db.Territories.Select(x => x.Id).Take(2).ToArray();
                db.Insert(new EmployeeTerritory
                {
                    EmployeeId = id,
                    TerritoryId = territories[0]
                });
                db.Insert(new EmployeeTerritory
                {
                    EmployeeId = id,
                    TerritoryId = territories[1]
                });
            }
        }

        public static void MoveProductsToAnotherCategory()
        {
            using (var db = new Northwind()) {
                var category = db.Categories.First();
                var product = db.Products.First(
                    p => p.CategoryId != category.Id);

                product.CategoryId = category.Id;

                db.Update(product);
            }
        }

        public static void AddProducts()
        {
            using (var db = new Northwind()) {
                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "SomeProduct",
                        Category = new Category { Name = "SomeCategory" },
                        Supplier = new Supplier { CompanyName = "SomeSupplier" }
                    },
                    new Product
                    {
                        Name = "AnotherProduct",
                        Category = new Category { Name = "AnotherCategory" },
                        Supplier = new Supplier { CompanyName = "AnotherSupplier" }
                    }
                };

                foreach (var p in products) {
                    if (db.Categories.Any(c => c.Name == p.Category.Name)) {
                        p.CategoryId = db.Categories.
                            First(c => c.Name == p.Category.Name).Id;
                    } else {
                        p.CategoryId = Convert.ToInt32(
                            db.InsertWithIdentity(
                                new Category { Name = p.Category.Name }));
                    }

                    if (db.Suppliers.Any(s => s.CompanyName == p.Supplier.CompanyName)) {
                        p.SupplierId = db.Suppliers.
                            First(s => s.CompanyName == p.Supplier.CompanyName).Id;
                    } else {
                        p.SupplierId = Convert.ToInt32(
                            db.InsertWithIdentity(
                                new Supplier { CompanyName = p.Supplier.CompanyName }));
                    }

                    db.Insert(p);
                }
            }
        }

        public static void ReplaceProductWithAnalog()
        {
            using (var db = new Northwind())
            {
                var notShippedOrders = db.OrderDetails.ToList().Where(od =>
                    db.Orders.Where(o => o.ShippedDate == null).ToList().Any(o => o.Id == od.OrderId));

                foreach (var notShippedOrder in notShippedOrders){
                    var analog = db.Products.First(p => p.Id == FindAnalog(db, notShippedOrder));
                    db.OrderDetails.Where(
                        od => od.OrderId == notShippedOrder.OrderId && od.ProductId == notShippedOrder.ProductId).
                            Update(od => new OrderDetails {
                                ProductId = analog.Id,
                                UnitPrice = analog.UnitPrice
                            });
                }
            }
        }

        private static int FindAnalog(Northwind db, OrderDetails notShippedOrder)
        {
            var analogProduct = notShippedOrder.ProductId++;
            if (!db.Products.Any(p => p.Id == analogProduct)){
                analogProduct = notShippedOrder.ProductId--;
            }
            return analogProduct;
        }
    }
}
