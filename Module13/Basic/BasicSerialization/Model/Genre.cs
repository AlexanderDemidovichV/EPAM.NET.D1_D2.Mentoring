using System.Xml.Serialization;

namespace BasicSerialization.Model
{
    public enum Genre
    {
        Computer,
        Fantasy,
        Romance,
        Horror,
        [XmlEnum("Science Fiction")]
        ScienceFiction
    }
}
