using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace RanorexReport.RanorexLogData
{
    public partial class ReportActivity
    {
        [XmlElement("detail")]
        public string Detail { get; set; }

        [XmlElement("activity")]
        public Collection<ReportActivity> Activity { get; set; } = new();

        [XmlIgnore]
        public ReportActivity Parent { get; set; }

        [XmlAttribute("result")]
        public string Result { get; set; }

        [XmlAttribute("duration")]
        public string Duration { get; set; }

        [XmlAttribute("durationms")]
        public uint Durationms { get; set; }

        [XmlAttribute("iteration-exectype")]
        public string IterationEexecType { get; set; }

        [XmlAttribute("dataiterationcount")]
        public int DataIterationCount { get; set; }

        [XmlAttribute("headertext")]
        public string Headertext { get; set; }

        [XmlAttribute("runconfigname")]
        public string Runconfigname { get; set; }

        [XmlAttribute("runlabel")]
        public string Runlabel { get; set; }

        [XmlAttribute("reportdescription")]
        public string Reportdescription { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("rid")]
        public string Rid { get; set; }

        [XmlAttribute("testsuitename")]
        public string Testsuitename { get; set; }

        [XmlAttribute("totalerrorcount")]
        public int Totalerrorcount { get; set; }

        [XmlAttribute("totalwarningcount")]
        public int Totalwarningcount { get; set; }

        [XmlAttribute("totalsuccesstestcasecount")]
        public int Totalsuccesstestcasecount { get; set; }

        [XmlAttribute("totalfailedtestcasecount")]
        public int Totalfailedtestcasecount { get; set; }

        [XmlAttribute("totalblockedtestcasecount")]
        public int Totalblockedtestcasecount { get; set; }

        [XmlAttribute("testcontainerid")]
        public string Testcontainerid { get; set; }

        [XmlAttribute("testcontainername")]
        public string Testcontainername { get; set; }

        [XmlAttribute("childsuccesscount")]
        public int Childsuccesscount { get; set; }

        [XmlAttribute("childfailedcount")]
        public int Childfailedcount { get; set; }

        [XmlAttribute("childblockedcount")]
        public int Childblockedcount { get; set; }

        [XmlAttribute("totalsuccesscount")]
        public ushort Totalsuccesscount { get; set; }

        [XmlAttribute("totalfailedcount")]
        public int Totalfailedcount { get; set; }

        [XmlAttribute("totalblockedcount")]
        public int Totalblockedcount { get; set; }

        [XmlAttribute("totalmaintenancemodecount")]
        public int Totalmaintenancemodecount { get; set; }

        [XmlAttribute("user")]
        public string User { get; set; }

        [XmlAttribute("rxversion")]
        public string Rxversion { get; set; }

        [XmlAttribute("host")]
        public string Host { get; set; }

        [XmlAttribute("osversion")]
        public string Osversion { get; set; }

        [XmlAttribute("runtimeversion")]
        public string Runtimeversion { get; set; }

        [XmlAttribute("procarch")]
        public string Procarch { get; set; }

        [XmlAttribute("oslanguage")]
        public string Oslanguage { get; set; }

        [XmlAttribute("dtnlanguage")]
        public string Dtnlanguage { get; set; }

        [XmlAttribute("screenresolution")]
        public string Screenresolution { get; set; }

        [XmlAttribute("timestamp")]
        public string Timestamp { get; set; }

        [XmlAttribute("timestampiso", DataType = "dateTime")]
        public DateTime Timestampiso { get; set; }

        [XmlAttribute("endtime")]
        public string Endtime { get; set; }

        [XmlAttribute("timeoutfactor")]
        public byte Timeoutfactor { get; set; }

        [XmlAttribute("runid")]
        public string Runid { get; set; }

        [XmlAttribute("sutversion")]
        public string Sutversion { get; set; }

        [XmlAttribute("modulename")]
        public string Modulename { get; set; }

        [XmlAttribute("displayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("moduleid")]
        public string Moduleid { get; set; }

        [XmlAttribute("moduletype")]
        public string Moduletype { get; set; }

        [XmlAttribute("iteration")]
        public string Iteration { get; set; }

        [XmlElement("datarow")]
        public RanorexDataRow DataRow { get; set; }

        [XmlAttribute("activity-exectype")]
        public string ActivityExectype { get; set; }

        [XmlAttribute("testentry-activity-type")]
        public string TestentryActivityType { get; set; }

        [XmlAttribute("videofile")]
        public string Videofile { get; set; }

        [XmlIgnore()]
        private Collection<ActivityItem> _item = new();

        [XmlElement("item")]
        public Collection<ActivityItem> Item
        {
            get
            {
                return _item;
            }
            private set
            {
                _item = value;
            }
        }

        [XmlElement("errormessage")]
        public ItemMessage Errormessage { get; set; }

        [XmlIgnore()]
        private Collection<ActivityVarbinding> _varbindings = new ();

        [XmlArray("varbindings")]
        [XmlArrayItem("varbinding", Namespace = "")]
        public Collection<ActivityVarbinding> Varbindings
        {
            get
            {
                return _varbindings;
            }
            private set
            {
                _varbindings = value;
            }
        }

        [XmlIgnore()]
        private Collection<ActivityParam> _params = new();

        [XmlArray("params")]
        [XmlArrayItem("param", Namespace = "")]
        public Collection<ActivityParam> Params
        {
            get
            {
                return _params;
            }
            private set
            {
                _params = value;
            }
        }
    }
}
