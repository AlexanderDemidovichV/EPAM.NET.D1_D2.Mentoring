﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using Task.Infrastructure;

namespace Task
{
	[TestClass]
	public class SerializationSolutions
	{
		Northwind dbContext;

		[TestInitialize]
		public void Initialize()
		{
			dbContext = new Northwind();
		}

		[TestMethod]
		public void SerializationCallbacks()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Category>>(new NetDataContractSerializer(), true);
			var categories = dbContext.Categories.ToList();

			var c = categories.First();

			tester.SerializeAndDeserialize(categories);
		}

		[TestMethod]
		public void ISerializable()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

			var tester = new XmlDataContractSerializerTester<IEnumerable<Product>>(new NetDataContractSerializer(), true);
			var products = dbContext.Products.ToList();

			tester.SerializeAndDeserialize(products);
		}


		[TestMethod]
		public void ISerializationSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = false;

		    var selector = new SurrogateSelector();
		    selector.AddSurrogate(typeof(Order_Detail),
		        new StreamingContext(StreamingContextStates.All, dbContext),
		        new OrderDetailSurrogate());

		    var tester = new XmlDataContractSerializerTester<IEnumerable<Order_Detail>>(
		        new NetDataContractSerializer
		        {
		            SurrogateSelector = selector,
		            Context = new StreamingContext(StreamingContextStates.All, dbContext)
		        },
		        true);
            var orderDetails = dbContext.Order_Details.ToList();

			tester.SerializeAndDeserialize(orderDetails);
		}

		[TestMethod]
		public void IDataContractSurrogate()
		{
			dbContext.Configuration.ProxyCreationEnabled = true;
			dbContext.Configuration.LazyLoadingEnabled = true;
		    var settings = new DataContractSerializerSettings
		    {
		        DataContractSurrogate = new DataContractSurrogate()
            };
            var tester = new XmlDataContractSerializerTester<IEnumerable<Order>>(
                new DataContractSerializer(
                    typeof(IEnumerable<Order>), 
                    settings), 
                true);
			var orders = dbContext.Orders.ToList();

			tester.SerializeAndDeserialize(orders);
		}
	}
}
