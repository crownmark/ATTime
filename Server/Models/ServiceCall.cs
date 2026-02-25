namespace CrownATTime.Server.Models
{
    public class ServiceCall
    {
        public int id { get; set; }
        //public object cancelationNoticeHours { get; set; }
        //public object canceledByResourceID { get; set; }
        //public object canceledDateTime { get; set; }
        public int companyID { get; set; }
        public int? companyLocationID { get; set; }
        public DateTime createDateTime { get; set; }
        public int creatorResourceID { get; set; }
        public string description { get; set; }
        public double duration { get; set; }
        public DateTime endDateTime { get; set; }
        public int? impersonatorCreatorResourceID { get; set; }
        public int isComplete { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public DateTime startDateTime { get; set; }
        public int status { get; set; }

    }
}
