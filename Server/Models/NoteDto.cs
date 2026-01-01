namespace CrownATTime.Server.Models
{
    public class NoteDto
    {

        public int id { get; set; }
        public DateTime createDateTime { get; set; }
        public int? createdByContactID { get; set; }
        public int? creatorResourceID { get; set; }
        public string description { get; set; }
        public int? impersonatorCreatorResourceID { get; set; }
        public int? impersonatorUpdaterResourceID { get; set; }
        public DateTime lastActivityDate { get; set; }
        public int noteType { get; set; }
        public int publish { get; set; }
        public int ticketID { get; set; }
        public string title { get; set; }
    }
}
