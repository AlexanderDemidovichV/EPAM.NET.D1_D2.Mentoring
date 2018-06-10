using System.IO;
using System.Xml.Serialization;
using BasicSerialization.Model;

namespace BasicSerialization
{
    public class XmlBookSerializer
    {
        public void Serialize(string filePath, Catalog element)
        {
            var serializer = new XmlSerializer(typeof(Catalog));
            using (FileStream fs = new FileStream(filePath, FileMode.Create)) {
                serializer.Serialize(fs, element);
            }
        }

        public Catalog Deserialize(string filePath)
        {
            var serializer = new XmlSerializer(typeof(Catalog));
            using (FileStream fs = new FileStream(filePath, FileMode.Open)) {
                return (Catalog)serializer.Deserialize(fs);
            }
        }
    }
}
