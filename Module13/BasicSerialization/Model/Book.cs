using System;
using System.Xml.Serialization;

namespace BasicSerialization.Model
{
    public class Book
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("isbn")]
        public string Isbn { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("genre")]
        public Genre Genre { get; set; }

        [XmlElement("publisher")]
        public string Publisher { get; set; }

        [XmlIgnore]
        public DateTime PublishDate { get; set; }

        [XmlElement("publish_date")]
        public string PublishDateString
        {
            get => PublishDate.ToString("yyyy-MM-dd");
            set => PublishDate = DateTime.Parse(value);
        }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlIgnore]
        public string RegistrationDate { get; set; }

        [XmlElement("registration_date")]
        public string RegistrationDateString
        {
            get => PublishDate.ToString("yyyy-MM-dd");
            set => PublishDate = DateTime.Parse(value);
        }
    }
}
