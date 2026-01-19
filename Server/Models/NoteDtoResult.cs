namespace CrownATTime.Server.Models
{
    public class NoteDtoResult
    {

        
        public class Item
        {
            public int id { get; set; }
            public DateTime createDateTime { get; set; }
            public int? createdByContactID { get; set; }
            public int? creatorResourceID { get; set; }
            public string description { get; set; }
            public int? impersonatorCreatorResourceID { get; set; }
            public int? impersonatorUpdaterResourceID { get; set; }
            public DateTime lastActivityDate { get; set; }
            public int noteType { get; set; }
            public int publish { get; set; }
            public int ticketID { get; set; }
            public string title { get; set; }
            public Soapparentpropertyid soapParentPropertyId { get; set; }
        }

        public class Soapparentpropertyid
        {
            public string type { get; set; }
            public string nodeType { get; set; }
            public Parameter[] parameters { get; set; }
            public string name { get; set; }
            public Body body { get; set; }
            public string returnType { get; set; }
            public bool tailCall { get; set; }
            public bool canReduce { get; set; }
        }

        public class Body
        {
            public string nodeType { get; set; }
            public string type { get; set; }
            public bool canReduce { get; set; }
        }

        public class Parameter
        {
            public string type { get; set; }
            public string nodeType { get; set; }
            public string name { get; set; }
            public bool isByRef { get; set; }
            public bool canReduce { get; set; }
        }

    }
}
