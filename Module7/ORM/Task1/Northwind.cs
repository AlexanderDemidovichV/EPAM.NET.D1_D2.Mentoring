using LinqToDB;
using LinqToDB.Data;
using Task1.Entities;

namespace Task1
{
    public class Northwind : DataConnection
    {
        public Northwind() : base("Northwind")
        {
        }

        public ITable<Category> Categories => GetTable<Category>();

        public ITable<Customer> Customers => GetTable<Customer>();

        public ITable<Employee> Employees => GetTable<Employee>();

        public ITable<EmployeeTerritory> EmployeeTerritories
        {
            get
            {
                return GetTable<EmployeeTerritory>()
                    .LoadWith(et => et.Employee)
                    .LoadWith(et => et.Territory)
                    .LoadWith(et => et.Territory.Region);
            }
        }

        public ITable<Order> Orders { get { return GetTable<Order>().LoadWith(o => o.Employee).LoadWith(o => o.Shipper); } }

        public ITable<OrderDetails> OrderDetails => GetTable<OrderDetails>();

        public ITable<Product> Products => GetTable<Product>();

        public ITable<Region> Regions => GetTable<Region>();

        public ITable<Shipper> Shippers => GetTable<Shipper>();

        public ITable<Supplier> Suppliers => GetTable<Supplier>();

        public ITable<Territory> Territories => GetTable<Territory>();
    }
}