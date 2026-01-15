using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    public partial class RanorexDataRow
    {
        [XmlElement("field")]
        public List<DataField> Fields { get; set; }
    }
}
