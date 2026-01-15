using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    public partial class ActivityItem
    {
        [XmlElement("message")]
        public ItemMessage Message { get; set; }

        [XmlElement("metainfo")]
        public ItemMetainfo Metainfo { get; set; }

        [XmlAttribute("timeRelativeToTestSuiteStartTime")]
        public string TimeRelativeToTestSuiteStartTime { get; set; }

        [XmlAttribute("timeRelativeToTestModuleStartTime")]
        public string TimeRelativeToTestModuleStartTime { get; set; }

        [XmlAttribute("timeWallClock", DataType = "time")]
        public DateTime TimeWallClock { get; set; }

        [XmlAttribute("time")]
        public string Time { get; set; }

        [XmlAttribute("level")]
        public string Level { get; set; }

        [XmlAttribute("category")]
        public string Category { get; set; }

        [XmlAttribute("errimg")]
        public string Errimg { get; set; }

        [XmlAttribute("errthumb")]
        public string Errthumb { get; set; }
    }
}
