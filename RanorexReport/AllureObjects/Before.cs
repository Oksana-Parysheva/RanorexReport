using Newtonsoft.Json;

namespace RanorexReport.AllureObjects
{
    public class Before
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusDetails")]
        public StatusDetails StatusDetails { get; set; }

        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; } = new List<Step>();

        [JsonProperty("attachments")]
        public List<AllureAttachment> Attachments { get; set; } = new List<AllureAttachment>();

        [JsonProperty("parameters")]
        public List<AllureParameter> Parameters { get; set; } = new List<AllureParameter>();

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("stop")]
        public long Stop { get; set; }
    }


}
