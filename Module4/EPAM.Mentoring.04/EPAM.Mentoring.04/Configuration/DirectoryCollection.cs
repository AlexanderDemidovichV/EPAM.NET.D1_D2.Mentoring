using System;
using System.Configuration;

namespace EPAM.Mentoring
{
    [ConfigurationCollection(typeof(DirectoryElement), AddItemName = "Directory")]
    public class DirectoryCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new DirectoryElement();

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            return ((DirectoryElement)element).Path;
        }

        private class DirectoryElement : ConfigurationElement
        {
            [ConfigurationProperty("Path", IsRequired = true, IsKey = true)]
            public string Path => (string)base["Path"];
        }
    }
}