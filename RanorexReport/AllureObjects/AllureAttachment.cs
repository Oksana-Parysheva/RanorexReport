using Newtonsoft.Json;

namespace RanorexReport.AllureObjects
{
    public class AllureAttachment
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
