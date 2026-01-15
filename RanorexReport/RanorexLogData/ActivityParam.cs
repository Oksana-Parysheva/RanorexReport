using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    public partial class ActivityParam
    {
        [XmlText()]
        public string Value { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
