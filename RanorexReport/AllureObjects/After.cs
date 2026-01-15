using Newtonsoft.Json;

namespace RanorexReport.AllureObjects
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class After
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
        public List<object> Steps { get; set; }

        [JsonProperty("attachments")]
        public List<AllureAttachment> Attachments { get; set; }

        [JsonProperty("parameters")]
        public List<object> Parameters { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("stop")]
        public long Stop { get; set; }
    }


}
