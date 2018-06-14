using System.Data.Entity.Infrastructure;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.Infrastructure
{
    public class OrderDetailSurrogate : ISerializationSurrogate
    {
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var data = (Order_Detail)obj;
            data.OrderID = info.GetInt32("OrderID");
            data.ProductID = info.GetInt32("ProductID");
            data.UnitPrice = info.GetDecimal("UnitPrice");
            data.Quantity = info.GetInt16("Quantity");
            data.Discount = (float)info.GetValue("Discount", typeof(float));
            data.Order = (Order)info.GetValue("Order", typeof(Order));
            data.Product = (Product)info.GetValue("Product", typeof(Product));
            return data;
        }

        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var data = (Order_Detail)obj;
            var dbContext = (context.Context as IObjectContextAdapter).ObjectContext;
            dbContext.LoadProperty(data, x => x.Order);
            dbContext.LoadProperty(data, x => x.Product);
            info.AddValue("OrderID", data.OrderID);
            info.AddValue("ProductID", data.ProductID);
            info.AddValue("UnitPrice", data.UnitPrice);
            info.AddValue("Quantity", data.Quantity);
            info.AddValue("Discount", data.Discount);
            info.AddValue("Order", data.Order);
            info.AddValue("Product", data.Product);
        }
    }
}
