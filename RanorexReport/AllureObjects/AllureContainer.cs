using Newtonsoft.Json;

namespace RanorexReport.AllureObjects
{
    public class AllureContainer
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("children")]
        public List<string> Children { get; set; } = new List<string>();

        [JsonProperty("befores")]
        public List<Before> Befores { get; set; } = new List<Before>();

        [JsonProperty("afters")]
        public List<After> Afters { get; set; } = new List<After>();

        [JsonProperty("links")]
        public List<string> Links { get; set; } = new List<string>();

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("stop")]
        public long Stop { get; set; }
    }
}
