namespace CrownATTime.Server.Models
{
    public class AttachmentCreateDto
    {

        public int id { get; set; }
        public DateTime attachDate { get; set; }
        public int? attachedByContactID { get; set; }
        public int? attachedByResourceID { get; set; }
        public string attachmentType { get; set; }
        public string contentType { get; set; }
        //public int creatorType { get; set; }
        //public int fileSize { get; set; }
        public string fullPath { get; set; }
        //public int? impersonatorCreatorResourceID { get; set; }
        //public int opportunityID { get; set; }
        //public int parentAttachmentID { get; set; }
        //public int parentID { get; set; }
        public int publish { get; set; }
        public int ticketID { get; set; }
        //public int ticketNoteID { get; set; }
        //public int timeEntryID { get; set; }
        public string title { get; set; }
        public string data { get; set; }

    }
}
