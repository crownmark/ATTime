namespace CrownATTime.Server.Models
{
    public class CompanyTodoDtoResult
    {
        public int id { get; set; }
        public int actionType { get; set; }
        public string activityDescription { get; set; }
        public int assignedToResourceID { get; set; }
        public int companyID { get; set; }
        public DateTime? completedDate { get; set; }
        public int? contactID { get; set; }
        public int? contractID { get; set; }
        public DateTime createDateTime { get; set; }
        public int creatorResourceID { get; set; }
        public DateTime endDateTime { get; set; }
        public int? impersonatorCreatorResourceID { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public int? opportunityID { get; set; }
        public DateTime startDateTime { get; set; }
        public int? ticketID { get; set; }
    }
}
