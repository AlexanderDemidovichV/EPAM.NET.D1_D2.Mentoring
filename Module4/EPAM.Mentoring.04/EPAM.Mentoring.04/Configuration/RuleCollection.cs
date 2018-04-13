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
    }
    public class Rule : ConfigurationElement
    {
        [RegexStringValidator (@"A(\d{6})_B(\d{6})_v(\d{1,3}\.\d{1,2})_(.*?)\.xml")]
        [ConfigurationProperty("FilePattern", IsRequired = true, IsKey = true)]
        public string FilePattern => (string)base["FilePattern"];

        [ConfigurationProperty("DestinationFolder", IsRequired = true)]
        [RegexStringValidator(@"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$")]
        public bool DestinationFolder => (bool)base["DestinationFolder"];

        [ConfigurationProperty("ActionToTakeWhenInputFileNameIsChanged", IsRequired = true)]
        public ActionToTakeWhenInputFileNameIsChanged ActionToTakeWhenInputFileNameIsChanged =>
            (ActionToTakeWhenInputFileNameIsChanged)Enum.Parse(
                typeof(ActionToTakeWhenInputFileNameIsChanged),
                base["ActionToTakeWhenInputFileNameIsChanged"].ToString());
    }
}