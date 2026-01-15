using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    public partial class ItemMetainfo
    {
        [XmlText()]
        public string Value { get; set; }

        [XmlAttribute("stacktrace")]
        public string Stacktrace { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("itempath")]
        public string Itempath { get; set; }

        [XmlAttribute("fullname")]
        public string Fullname { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("timeout")]
        public ulong Timeout { get; set; }

        [XmlAttribute("codefile")]
        public string Codefile { get; set; }

        [XmlAttribute("codeline")]
        public ushort Codeline { get; set; }

        [XmlAttribute("itemindex")]
        public byte Itemindex { get; set; }

        [XmlAttribute("loglvl")]
        public string Loglvl { get; set; }

        [XmlAttribute("failedStep")]
        public string FailedStep { get; set; }

        [XmlAttribute("bestPathPrefix")]
        public string BestPathPrefix { get; set; }
    }
}
