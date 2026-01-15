using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace RanorexReport.RanorexLogData
{
    public partial class ActivityParams
    {
        [XmlIgnore()]
        private Collection<ActivityParam> _param = new();

        [XmlElement("param")]
        public Collection<ActivityParam> Param
        {
            get
            {
                return _param;
            }
            private set
            {
                _param = value;
            }
        }

        public ActivityParams()
        {
            _param = new Collection<ActivityParam>();
        }
    }
}
