using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    public partial class DataField
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}
