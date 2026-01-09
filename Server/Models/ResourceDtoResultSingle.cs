namespace CrownATTime.Server.Models
{
    public class ResourceDtoResultSingle
    {
        public Item item { get; set; }
        public class Item
        {
            public int id { get; set; }
            //public string accountingReferenceID { get; set; }
            //public string dateFormat { get; set; }
            //public int? defaultServiceDeskRoleID { get; set; }
            public string email { get; set; }
            public string email2 { get; set; }
            public string email3 { get; set; }
            public string emailTypeCode { get; set; }
            public string emailTypeCode2 { get; set; }
            public string emailTypeCode3 { get; set; }
            public string firstName { get; set; }
            //public string gender { get; set; }
            //public int greeting { get; set; }
            //public DateTime hireDate { get; set; }
            //public string homePhone { get; set; }
            //public string initials { get; set; }
            //public decimal internalCost { get; set; }
            public bool isActive { get; set; }
            public string lastName { get; set; }
            public int licenseType { get; set; }
            //public int? locationID { get; set; }
            public string middleName { get; set; }
            public string mobilePhone { get; set; }
            public string numberFormat { get; set; }
            public string officeExtension { get; set; }
            public string officePhone { get; set; }
            //public string payrollIdentifier { get; set; }
            //public int payrollType { get; set; }
            public string resourceType { get; set; }
            //public int? suffix { get; set; }
            //public int? surveyResourceRating { get; set; }
            public string timeFormat { get; set; }
            public string title { get; set; }
            public string travelAvailabilityPct { get; set; }
            public string userName { get; set; }
            //public int userType { get; set; }
        }

    }
}
