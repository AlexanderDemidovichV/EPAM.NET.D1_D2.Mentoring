// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Where - Task 1")]
		[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
		public void Linq1()
		{
			int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			var lowNums =
				from num in numbers
				where num < 5
				select num;

			Console.WriteLine("Numbers < 5:");
			foreach (var x in lowNums)
			{
				Console.WriteLine(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 2")]
		[Description("This sample return return all presented in market products")]

		public void Linq2()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

	    [Category("EPAM.Mentoring")]
        [Title("Task 1")]
	    [Description("This sample return Customers with total turnover more than custom value.")]

	    public void Linq01()
	    {
	        IEnumerable<string> GetCustomersWithTotalTurnoverMoreThanX(
	            int x, IEnumerable<Customer> customers) =>
	            customers.
	                Where(c => c.Orders.Sum(o => o.Total) > x).
	                Select(c => c.CompanyName);

	        var productsWithTotalTurnoverMoreThan3000 =
	            GetCustomersWithTotalTurnoverMoreThanX(3000, dataSource.Customers);
	        var productsWithTotalTurnoverMoreThan5000 =
	            GetCustomersWithTotalTurnoverMoreThanX(5000, dataSource.Customers);

	        ObjectDumper.Write("Products With Total Turnover More Than 3000:");
	        Output(productsWithTotalTurnoverMoreThan3000);
	        ObjectDumper.Write("Products With Total Turnover More Than 5000:");
	        Output(productsWithTotalTurnoverMoreThan5000);
	    }

	    [Category("EPAM.Mentoring")]
        [Title("Task 2")]
        [Description("This sample return customers with corresponding suppliers")]

        public void Linq02()
        {

            var customers = dataSource.Customers.
                Select(c => new{
                    c.CompanyName,
                    c.Country,
                    c.City,
                    SupplierGroup = dataSource.Suppliers.
                        Where(supplier => supplier.Country == c.Country
                                         && supplier.City == c.City)
                });

            Output(customers);
        }

	    [Category("EPAM.Mentoring")]
        [Title("Task 3")]
	    [Description("This sample return customers with total sum of orders more than some X")]

	    public void Linq03()
	    {
	        IEnumerable<Customer> GetCustomersWithtotalSumOfOrdersMoreThanX(
	            decimal x, IEnumerable<Customer> customers) =>
	            customers.Where(c => 
                    c.Orders.Sum(o => o.Total) >= x);

            var result = GetCustomersWithtotalSumOfOrdersMoreThanX(1402.96M, dataSource.Customers);

            Output(result);
	    }

        [Category("EPAM.Mentoring")]
        [Title("Task 4")]
        [Description("This sample return customers with date of very first order.")]

        public void Linq04()
        {
            var result = dataSource.Customers.
                Select(customer => new {
                    customer.CompanyName,
                    DateOfFounding =
                        customer.Orders.Any() ?
                        customer.Orders.Min(order => order.OrderDate) :
                        default(DateTime)
                }).
                Select(c => new {
                    c.CompanyName,
                    YearOfFounding = c.DateOfFounding.Year,
                    MonthOfFounding = c.DateOfFounding.Month
                });

            Output(result);
        }

        [Category("EPAM.Mentoring")]
        [Title("Task 5")]
        [Description("This sample return customers with date of very first order " +
                     "ordered by year, month, turnover, name.")]

        public void Linq05()
        {
            var customers = dataSource.Customers.
                Select(customer => new {
                    customer,
                    DateOfFounding =
                        customer.Orders.Any() ?
                        customer.Orders.Min(order => order.OrderDate) :
                        default(DateTime)
                }).
                Select(c => new {
                    c.customer.CompanyName,
                    YearOfFounding = c.DateOfFounding.Year,
                    MonthOfFounding = c.DateOfFounding.Month,
                    TotalTurnover =
                        c.customer.Orders.Sum(order => order.Total)

                }).
                OrderBy(c => c.YearOfFounding).
                ThenBy(c => c.MonthOfFounding).
                ThenByDescending(c => c.TotalTurnover).
                ThenBy(c => c.CompanyName);


            Output(customers);
        }

        [Category("EPAM.Mentoring")]
        [Title("Task 6")]
        [Description("This sample return customers who have postal code" +
                     "or have not region " +
                     "or code for phone.")]

        public void Linq06()
        {

            var regex = new Regex(@"^(\()");

            var customers = dataSource.Customers.
                Where(c =>
                    !string.IsNullOrEmpty(c.PostalCode) ||
                    string.IsNullOrEmpty(c.Region) ||
                    regex.IsMatch(c.Phone)
            );

            Output(customers);
        }

        [Category("EPAM.Mentoring")]
        [Title("Task 7")]
        [Description("This sample return products grouped by " +
                     "category, units in stock and sorted by price")]

        public void Linq07()
        {
            var products = dataSource.Products.
                GroupBy(p => p.Category).
                Select(categoryGrouping => new {
                    Name = categoryGrouping.Key,
                    Count = categoryGrouping.Count(),
                    Products =
                        categoryGrouping.
                        GroupBy(p => p.UnitsInStock).
                        Select(unitInStockGrouping => new {
                            Name = unitInStockGrouping.Key,
                            Count = unitInStockGrouping.Count(),
                            Products = unitInStockGrouping.
                                OrderBy(p => p.UnitPrice)
                        })
                });

            Output(products);
        }

        [Category("EPAM.Mentoring")]
        [Title("Task 8")]
        [Description("This sample return products grouped in " +
                     "the following categories:" +
                     "\"cheap\", \"medium\", \"expensive\"")]

        public void Linq08()
        {
            var max = dataSource.Products.Max(p => p.UnitPrice);

            var ranges = new[]
            {
                new { Start = 0M , End = 40M},
                new { Start = 40M , End = 200M},
                new { Start = 200M , End = max}
            };

            var products = ranges
                .Select(range => new {
                    Range = range,
                    Values = dataSource.Products.
                        Where(product =>
                            product.UnitPrice > range.Start
                            && product.UnitPrice <= range.End)
                }).
                Select(range => new {
                    range.Range.Start,
                    range.Range.End,
                    Count = range.Values.Count(),
                    range.Values
                });

            Output(products);
        }

        [Category("EPAM.Mentoring")]
        [Title("Task 9")]
        [Description("This sample return average total for cities and " +
                     "average amount of orders for customers")]

        public void Linq09()
        {
            var cities = dataSource.Customers.
                GroupBy(customer => customer.City).
                Select(cityGrouping => new {
                    cityGrouping.Key,
                    AverageTotal = cityGrouping.
                        Select(customer => customer.Orders).
                        SelectMany(orders => orders).
                        Select(order => order.Total).Average(),
                    AverageOrdersAmount = cityGrouping.
                        Select(customer => customer.Orders.Length).Average()

                });

            Output(cities);
        }

        [Category("EPAM.Mentoring")]
        [Title("Task 10")]
        [Description("This sample return statistics that mean annual customers activity at yearly, monthly or yearly and monthly")]

        public void Linq10()
        {
            var groupbyYear = dataSource.Customers.
                SelectMany(c => c.Orders).
                GroupBy(o => o.OrderDate.Year);

            var groupbyMonth = dataSource.Customers.
                SelectMany(c => c.Orders).
                GroupBy(o => o.OrderDate.Month);

            var groupbyMonthAndYear = dataSource.Customers.
                SelectMany(c => c.Orders).
                GroupBy(o => new {
                    o.OrderDate.Year,
                    o.OrderDate.Month
                });

            ObjectDumper.Write("group by Year:");
            Output(groupbyYear);
            ObjectDumper.Write("group by Month:");
            Output(groupbyMonth);
            ObjectDumper.Write("group by Month and Year:");
            Output(groupbyMonthAndYear);
        }

        private void Output(IEnumerable result)
	    {
	        if (result == null) {
	            ObjectDumper.Write("Empty!");
	            return;
	        }

	        foreach (var value in result) {
	            ObjectDumper.Write(value);
	        }
	    }
    }
}
