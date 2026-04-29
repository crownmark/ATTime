namespace CrownATTime.Server.Models
{
    public class ServiceCallCreateDto
    {
        public int id { get; set; }
        public int companyID { get; set; }
        public string description { get; set; }
        //public double duration { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public int isComplete { get; set; }
        
        public int status { get; set; }
        
    }
}
