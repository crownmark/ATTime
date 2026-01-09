namespace CrownATTime.Server.Models
{
    public class RoleDto
    {
        public int id { get; set; }
        public string description { get; set; }
        public decimal hourlyFactor { get; set; }
        public decimal hourlyRate { get; set; }
        public bool isActive { get; set; }
        public bool? isExcludedFromNewContracts { get; set; }
        public bool? isSystemRole { get; set; }
        public string name { get; set; }
        public int? quoteItemDefaultTaxCategoryId { get; set; }
        public int? roleType { get; set; }
    }
}
