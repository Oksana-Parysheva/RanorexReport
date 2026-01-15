using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    public partial class ActivityVarbinding
    {
        [XmlText()]
        public string Value { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string ValueItem { get; set; }
    }
}
