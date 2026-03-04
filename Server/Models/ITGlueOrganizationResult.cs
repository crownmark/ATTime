namespace CrownATTime.Server.Models
{
    public class ITGlueOrganizationResult
    {
        public class Rootobject
        {
            public Datum[] data { get; set; }
            public Meta meta { get; set; }
            public Links links { get; set; }
        }

        public class Meta
        {
            public int currentpage { get; set; }
            public int nextpage { get; set; }
            public object prevpage { get; set; }
            public int totalpages { get; set; }
            public int totalcount { get; set; }
            public Filters filters { get; set; }
        }

        public class Filters
        {
            public Id id { get; set; }
            public Name name { get; set; }
            public OrganizationTypeId organizationtypeid { get; set; }
            public OrganizationStatusId organizationstatusid { get; set; }
            public CreatedAt createdat { get; set; }
            public UpdatedAt updatedat { get; set; }
            public MyGlueAccountId myglueaccountid { get; set; }
        }

        public class Id
        {
            public object[] permittedvalues { get; set; }
        }

        public class Name
        {
            public object[] permittedvalues { get; set; }
        }

        public class OrganizationTypeId
        {
            public PermittedValues[] permittedvalues { get; set; }
        }

        public class PermittedValues
        {
            public int value { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public string name { get; set; }
        }

        public class OrganizationStatusId
        {
            public PermittedValues1[] permittedvalues { get; set; }
        }

        public class PermittedValues1
        {
            public int value { get; set; }
            public Data1 data { get; set; }
        }

        public class Data1
        {
            public string name { get; set; }
        }

        public class CreatedAt
        {
            public object[] permittedvalues { get; set; }
        }

        public class UpdatedAt
        {
            public object[] permittedvalues { get; set; }
        }

        public class MyGlueAccountId
        {
            public object[] permittedvalues { get; set; }
        }

        public class Links
        {
            public string self { get; set; }
            public string first { get; set; }
            public object prev { get; set; }
            public string next { get; set; }
            public string last { get; set; }
        }

        public class Datum
        {
            public string id { get; set; }
            public string type { get; set; }
            public Attributes attributes { get; set; }
            public Relationships relationships { get; set; }
        }

        public class Attributes
        {
            public string psaintegration { get; set; }
            public bool syncactive { get; set; }
            public string name { get; set; }
            public object alert { get; set; }
            public object description { get; set; }
            public int? organizationtypeid { get; set; }
            public string organizationtypename { get; set; }
            public int? organizationstatusid { get; set; }
            public string organizationstatusname { get; set; }
            public object myglueaccountid { get; set; }
            public bool primary { get; set; }
            public object quicknotes { get; set; }
            public object quicknotevaultid { get; set; }
            public string shortname { get; set; }
            public DateTime createdat { get; set; }
            public DateTime updatedat { get; set; }
            public int?[] ancestorids { get; set; }
            public int? parentid { get; set; }
            public object myglueaccountstatus { get; set; }
            public string logo { get; set; }
            public bool haspersonalpasswords { get; set; }
        }

        public class Relationships
        {
            public AdaptersResources adaptersresources { get; set; }
            public RmmCompanies rmmcompanies { get; set; }
        }

        public class AdaptersResources
        {
            public Datum1[] data { get; set; }
        }

        public class Datum1
        {
            public string id { get; set; }
            public string type { get; set; }
        }

        public class RmmCompanies
        {
            public Datum2[] data { get; set; }
        }

        public class Datum2
        {
            public string id { get; set; }
            public string type { get; set; }
        }
    }
}
