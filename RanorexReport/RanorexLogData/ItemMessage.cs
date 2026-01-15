using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    public partial class ItemMessage
    {
        [XmlElement("br")]
        public object Br { get; set; }

        [XmlText()]
        public string[] Text { get; set; }
    }
}
