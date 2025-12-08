namespace CrownATTime.Server.Models
{
    public class SericeDeskRoleDto
    {

        public int id { get; set; }
        public bool isActive { get; set; }
        public bool isDefault { get; set; }
        public int resourceID { get; set; }
        public int roleID { get; set; }
        public Soapparentpropertyid soapParentPropertyId { get; set; }
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
