using Newtonsoft.Json;

namespace RanorexReport.AllureObjects
{
    public class AllureParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("excluded")]
        public bool Excluded { get; set; } = false;

        [JsonProperty("mode")]
        public string Mode { get; set; }
    }
}
