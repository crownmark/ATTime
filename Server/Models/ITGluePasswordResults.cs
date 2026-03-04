namespace CrownATTime.Server.Models
{
    public class ITGluePasswordResults
    {

        public Datum[] data { get; set; }
        public Meta meta { get; set; }
        public Links links { get; set; }

        public class Meta
        {
            public int currentpage { get; set; }
            public object nextpage { get; set; }
            public object prevpage { get; set; }
            public int totalpages { get; set; }
            public int totalcount { get; set; }
            public Filters filters { get; set; }
        }

        public class Filters
        {
        }

        public class Links
        {
            public string self { get; set; }
            public string first { get; set; }
            public object prev { get; set; }
            public object next { get; set; }
            public string last { get; set; }
        }
        public class Datum
        {
            public string id { get; set; }
            public string type { get; set; }
            public Attributes attributes { get; set; }
        }

        public class Attributes
        {
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
        }


    }
}
