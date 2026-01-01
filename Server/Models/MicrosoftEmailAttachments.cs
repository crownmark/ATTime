namespace CrownATTime.Server.Models
{
    public class MicrosoftEmailAttachments
    {
        public Value[] value { get; set; }

        public class Value
        {
            public string odatatype { get; set; }
            public string contentType { get; set; }
            public string contentLocation { get; set; }
            public string contentBytes { get; set; }
            public string contentId { get; set; }
            public string lastModifiedDateTime { get; set; }
            public string id { get; set; }
            public bool isInline { get; set; }
            public string name { get; set; }
            public int size { get; set; }
        }
    }
}
