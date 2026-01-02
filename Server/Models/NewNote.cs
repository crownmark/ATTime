using System.ComponentModel.DataAnnotations.Schema;

namespace CrownATTime.Server.Models
{
    public class NewNote
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int NoteType { get; set; }
        public int NotePublish { get; set; }
        public int? TicketStatus { get; set; }
        public int TicketId {  get; set; }
        public int NoteTemplateId { get; set; }
        public int? EmailTemplateId {  get; set; }
    }
}
