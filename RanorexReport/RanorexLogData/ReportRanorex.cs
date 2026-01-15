using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    [XmlRoot("report", Namespace = "")]
    public partial class ReportRanorex
    {
        [XmlElement("activity")]
        public ReportActivity Activity { get; set; }

        [XmlAttribute("progress")]
        public string Progress { get; set; }

        public string PathToPdfFile { get; set; }

        public string PathToLogDataFile { get; set; }
    }
}
