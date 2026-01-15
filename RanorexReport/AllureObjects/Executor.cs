using Newtonsoft.Json;

namespace RanorexReport.AllureObjects
{
    public class Executor
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("reportName")]
        public string ReportName { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("buildOrder")]
        public int BuildOrder { get; set; }

        [JsonProperty("buildName")]
        public string BuildName { get; set; }

        [JsonProperty("buildUrl")]
        public string BuildUrl { get; set; }

        [JsonProperty("reportUrl")]
        public string ReportUrl { get; set; }
    }
}
