using Microsoft.AspNetCore.Components;

namespace CrownATTime.Client.CustomComponents.DialogManager
{
    public class AppDialog
    {
        public int Id { get; set; }              // TicketId / TimeEntryId
        public string Type { get; set; } = "";   // "Ticket", "TimeEntry"

        public string Title { get; set; } = "";

        public RenderFragment? Content { get; set; }

        public bool IsMinimized { get; set; }
        public string Width { get; set; } = "98%";

        public int ZIndex { get; set; } = 1000;
    }
}
