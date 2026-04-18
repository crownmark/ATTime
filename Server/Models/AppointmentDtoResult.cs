
namespace CrownATTime.Server.Models
{
    public class AppointmentDtoResult
    {
        public long id { get; set; }
        public DateTime createDateTime { get; set; }
        public int creatorResourceID { get; set; }
        public string description { get; set; }
        public DateTime endDateTime { get; set; }   
        public int resourceID { get; set; }
        public DateTime startDateTime { get; set; }
        public string title { get; set; }
        public DateTime updateDateTime { get; set; }
    }
}
