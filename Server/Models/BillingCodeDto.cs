namespace CrownATTime.Server.Models
{
    // Models/BillingCodeDto.cs
    using System.Text.Json.Serialization;

    public class BillingCodeDto
    {
        public int id { get; set; }
        public int? afterHoursWorkType { get; set; }
        public int billingCodeType { get; set; }
        public int? department { get; set; }
        public string description { get; set; }
        public string externalNumber { get; set; }
        public int? generalLedgerAccount { get; set; }
        public bool isActive { get; set; }
        public bool isExcludedFromNewContracts { get; set; }
        public decimal? markupRate { get; set; }
        public string name { get; set; }
        public int? taxCategoryID { get; set; }
        public decimal unitCost { get; set; }
        public decimal unitPrice { get; set; }
        public int useType { get; set; }
    }

}
