namespace EPAM.Mentoring.Models
{
    public class RuleModel
    {
        public string FilePattern { get; set; }

        public string DestinationFolder { get; set; }

        public ActionToTakeWhenInputFileNameIsChanged 
            ActionToTakeWhenInputFileNameIsChanged { get; set; }

        public int Counter { get; set; }
    }
}
