namespace CrownATTime.Server.Models
{
    public class ITGluePasswordAttributeResults
    {
        public string id { get; set; }
        public string type { get; set; }

        public int organizationid { get; set; }
        public string organizationname { get; set; }
        public string resourceurl { get; set; }
        public bool restricted { get; set; }
        public bool myglue { get; set; }
        public string name { get; set; }
        public object autofillselectors { get; set; }
        public string username { get; set; }
        public string url { get; set; }
        public string notes { get; set; }
        public DateTime passwordupdatedat { get; set; }
        public int? updatedby { get; set; }
        public int? resourceid { get; set; }
        public string resourcetype { get; set; }
        public string cachedresourcetypename { get; set; }
        public string cachedresourcename { get; set; }
        public int? passwordcategoryid { get; set; }
        public string passwordcategoryname { get; set; }
        public DateTime createdat { get; set; }
        public DateTime updatedat { get; set; }
        public string password { get; set; }
    }
}
