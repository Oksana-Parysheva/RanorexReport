using Newtonsoft.Json;

namespace RanorexReport.AllureObjects
{
    public class StatusDetails
    {
        [JsonProperty("known")]
        public bool Known { get; set; } = false;

        [JsonProperty("muted")]
        public bool Muted { get; set; } = false;

        [JsonProperty("flaky")]
        public bool Flaky { get; set; } = false;

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("trace")]
        public string Trace { get; set; }
    }
}
