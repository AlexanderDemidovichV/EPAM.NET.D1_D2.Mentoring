using BasicSerialization;
using BasicSerialization.Model;
using NUnit.Framework;

namespace Basic.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Serialize()
        {
            var serializer = new XmlBookSerializer();
            Catalog res = serializer.Deserialize("c:\\Users\\Aliaksandr_Dzemidovi\\source\\repos\\Ser\\books.xml");

            Assert.AreEqual("2016-02-05", res.DateString);
            Assert.True(res.Books.Exists(book => book.Author == "Löwy, Juval"));
        }

        [Test]
        public void Deserialize()
        {
            var serializer = new XmlBookSerializer();
            Catalog res = serializer.Deserialize("c:\\Users\\Aliaksandr_Dzemidovi\\source\\repos\\Ser\\books.xml");

            
            serializer.Serialize("c:\\Users\\Aliaksandr_Dzemidovi\\source\\repos\\Ser\\DeserialitionResult.xml", res);
            var deserializationCatalog = serializer.Deserialize("c:\\Users\\Aliaksandr_Dzemidovi\\source\\repos\\Ser\\DeserialitionResult.xml");

            Assert.AreEqual(deserializationCatalog.Date, res.Date);
            Assert.AreEqual(res.Books.Exists(book => book.Author == "Vanya)"), 
                deserializationCatalog.Books.Exists(book => book.Author == "Vanya)"));
        }
    }
}
