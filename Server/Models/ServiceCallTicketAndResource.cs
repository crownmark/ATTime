namespace CrownATTime.Server.Models
{
    public class ServiceCallTicketAndResource
    {
        public int serviceCallTicketID { get; set; }
        public int serviceCallID { get; set; }
        public int ticketID { get; set; }

        public int serviceCallTicketResourceID { get; set; }
        public int resourceID { get; set; }
    }
}
