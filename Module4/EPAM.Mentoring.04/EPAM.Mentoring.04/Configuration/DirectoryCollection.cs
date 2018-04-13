using System;
using System.Configuration;

namespace EPAM.Mentoring
{
    [ConfigurationCollection(typeof(Directory), AddItemName = "Directory")]
    public class DirectoryCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new Directory();

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            return ((Directory)element).Path;
        }
    }

    public class Directory : ConfigurationElement
    {
        [ConfigurationProperty("Path", IsRequired = true, IsKey = true)]
        [RegexStringValidator(@"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$")]
        public string Path => (string)base["Path"];
    }
}