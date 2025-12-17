using System.ComponentModel.DataAnnotations.Schema;

namespace CrownATTime.Server.Models
{
    public class NewNote
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int NoteType { get; set; }
        public bool Internal { get; set; } // 1 All Autotask Users, 2 Internal Project Team 4 internal and comanaged
        public int TicketId {  get; set; }
        public int CreatedBy { get; set; }
    }
}
