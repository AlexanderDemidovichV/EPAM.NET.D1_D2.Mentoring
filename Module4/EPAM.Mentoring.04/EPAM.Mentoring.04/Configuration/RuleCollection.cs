using System;
using System.Configuration;

namespace EPAM.Mentoring
{
    [ConfigurationCollection(typeof(Rule), AddItemName = "Rule")]
    public class RuleCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new Rule();

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            return ((Rule)element).FilePattern;
        }

        private class Rule : ConfigurationElement
        {
            [ConfigurationProperty("FilePattern", IsRequired = true, IsKey = true)]
            public string FilePattern => (string)base["FilePattern"];

            [ConfigurationProperty("DestinationFolder", IsRequired = true)]
            public bool DestinationFolder => (bool)base["DestinationFolder"];

            [ConfigurationProperty("ActionToTakeWhenInputFileNameIsChanged", IsRequired = true)]
            public ActionToTakeWhenInputFileNameIsChanged ActionToTakeWhenInputFileNameIsChanged =>
                (ActionToTakeWhenInputFileNameIsChanged)Enum.Parse(
                    typeof(ActionToTakeWhenInputFileNameIsChanged), 
                    base["ActionToTakeWhenInputFileNameIsChanged"].ToString());
        }
    }
}