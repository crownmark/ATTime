namespace CrownATTime.Server.Models
{
    public class ContactDtoResult
    {


        public Item item { get; set; }


        public class Item
        {
            public int id { get; set; }
            public string additionalAddressInformation { get; set; }
            public string addressLine { get; set; }
            public string addressLine1 { get; set; }
            public string alternatePhone { get; set; }
            public object apiVendorID { get; set; }
            public object bulkEmailOptOutTime { get; set; }
            public string city { get; set; }
            public int companyID { get; set; }
            public object companyLocationID { get; set; }
            public int countryID { get; set; }
            public DateTime createDate { get; set; }
            public string emailAddress { get; set; }
            public object emailAddress2 { get; set; }
            public object emailAddress3 { get; set; }
            public string extension { get; set; }
            public string externalID { get; set; }
            public string facebookUrl { get; set; }
            public string faxNumber { get; set; }
            public string firstName { get; set; }
            public object impersonatorCreatorResourceID { get; set; }
            public int isActive { get; set; }
            public bool isOptedOutFromBulkEmail { get; set; }
            public DateTime lastActivityDate { get; set; }
            public DateTime lastModifiedDate { get; set; }
            public string lastName { get; set; }
            public string linkedInUrl { get; set; }
            public object middleInitial { get; set; }
            public string mobilePhone { get; set; }
            public object namePrefix { get; set; }
            public object nameSuffix { get; set; }
            public string note { get; set; }
            public bool receivesEmailNotifications { get; set; }
            public string phone { get; set; }
            public bool primaryContact { get; set; }
            public bool billingContact { get; set; }
            public string roomNumber { get; set; }
            public bool solicitationOptOut { get; set; }
            public object solicitationOptOutTime { get; set; }
            public string state { get; set; }
            public bool surveyOptOut { get; set; }
            public string title { get; set; }
            public string twitterUrl { get; set; }
            public string zipCode { get; set; }
            public Userdefinedfield[] userDefinedFields { get; set; }
        }

        public class Userdefinedfield
        {
            public string name { get; set; }
            public string value { get; set; }
        }


    }
}
