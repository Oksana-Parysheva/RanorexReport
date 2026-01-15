using Newtonsoft.Json;

namespace RanorexReport.AllureObjects
{
    public class AllureTestResult
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("historyId")]
        public string HistoryId { get; set; }

        [JsonProperty("testCaseId")]
        public string TestCaseId { get; set; }

        [JsonProperty("titlePath")]
        public List<string> TitlePath { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("testCaseName")]
        public string TestCaseName;

        [JsonProperty("labels")]
        public List<AllureLabel> Labels { get; set; } = new List<AllureLabel>();

        [JsonProperty("links")]
        public List<string> Links { get; set; } = new List<string>();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusDetails")]
        public StatusDetails StatusDetails { get; set; }

        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("descriptionHtml")]
        public string DescriptionHtml { get; set; }

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
