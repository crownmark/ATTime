using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CrownATTime.Client.Pages
{
    public partial class TechDashboard
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        public class TicketSample
        {
            public int Id { get; set; }
            public string TicketNumber { get; set; }
            public string Title { get; set; }
            public string Account { get; set; }
            public string Contact { get; set; }
            public string Status { get; set; } // New, Waiting, Overdue, Scheduled
            public string Priority { get; set; } // High, Medium, Low
            public DateTime? DueDate { get; set; }
            public DateTime CreatedDate { get; set; }
            public string Queue { get; set; }
            public string AssignedTo { get; set; }
        }
        public List<TicketSample> Tickets = new()
        {
            new TicketSample
            {
                Id = 1,
                TicketNumber = "T20260412.0007",
                Title = "Outlook profile corruption on startup",
                Account = "Harmony Communities",
                Contact = "Robert Medina",
                Status = "Overdue",
                Priority = "High",
                DueDate = DateTime.Today.AddHours(9.5),
                CreatedDate = DateTime.Now.AddHours(-5),
                Queue = "Service Desk",
                AssignedTo = "Mark"
            },
            new TicketSample
            {
                Id = 2,
                TicketNumber = "T20260412.0010",
                Title = "Printer mapping issue in accounting office",
                Account = "Aaron Read & Associates",
                Contact = "Joelle",
                Status = "Scheduled",
                Priority = "Medium",
                DueDate = DateTime.Today.AddHours(11),
                CreatedDate = DateTime.Now.AddHours(-3),
                Queue = "Service Desk",
                AssignedTo = "Mark"
            },
            new TicketSample
            {
                Id = 3,
                TicketNumber = "T20260412.0014",
                Title = "New user onboarding for payroll clerk",
                Account = "Golden State Fire",
                Contact = "HR Team",
                Status = "Waiting",
                Priority = "Medium",
                DueDate = DateTime.Today.AddDays(1),
                CreatedDate = DateTime.Now.AddHours(-2),
                Queue = "Centralized Services",
                AssignedTo = "Team"
            },
            new TicketSample
            {
                Id = 4,
                TicketNumber = "T20260412.0019",
                Title = "VPN client not connecting from Maui office",
                Account = "ARA",
                Contact = "Aaron Read",
                Status = "New",
                Priority = "High",
                CreatedDate = DateTime.Now.AddMinutes(-15),
                Queue = "Service Desk",
                AssignedTo = null
            },
            new TicketSample
            {
                Id = 5,
                TicketNumber = "T20260411.0098",
                Title = "Server backup verification alert",
                Account = "CE Bro Frozen Foods",
                Contact = "System",
                Status = "Scheduled",
                Priority = "Low",
                DueDate = DateTime.Today.AddHours(14.5),
                CreatedDate = DateTime.Now.AddDays(-1),
                Queue = "Monitoring Alerts",
                AssignedTo = "Mark"
            },
            new TicketSample
            {
                Id = 6,
                TicketNumber = "T20260410.0082",
                Title = "Wi-Fi drops in conference room",
                Account = "San Joaquin County Bar Association",
                Contact = "Office Manager",
                Status = "Waiting",
                Priority = "Medium",
                DueDate = DateTime.Today.AddDays(2),
                CreatedDate = DateTime.Now.AddDays(-2),
                Queue = "Design Desk",
                AssignedTo = "Mark"
            }
        };
        public class TimeEntrySample
        {
            public int Id { get; set; }
            public string TicketNumber { get; set; }
            public string Title { get; set; }
            public DateTime StartTime { get; set; }
            public int DurationMinutes { get; set; }
            public bool IsRunning { get; set; }
            public string Account { get; set; }

        }
        public List<TimeEntrySample> TimeEntries = new()
        {
            new TimeEntrySample
            {
                Id = 1,
                TicketNumber = "T20260412.0019",
                Title = "VPN client not connecting",
                StartTime = DateTime.Now.AddMinutes(-18),
                DurationMinutes = 18,
                IsRunning = true,
                Account = "Aaron Read",

            },
            new TimeEntrySample
            {
                Id = 2,
                TicketNumber = "T20260412.0010",
                Title = "Printer mapping issue",
                StartTime = DateTime.Now.AddMinutes(-42),
                DurationMinutes = 42,
                IsRunning = false,
                Account = "San Joaquin County Bar Association",

            },
            new TimeEntrySample
            {
                Id = 3,
                TicketNumber = "T20260411.0098",
                Title = "Backup verification alert",
                StartTime = DateTime.Now.AddMinutes(-72),
                DurationMinutes = 72,
                IsRunning = false,
                Account = "Crown Enterprises",

            }
        };
        public class CalendarItemSample
        {
            public int Id { get; set; }
            public DateTime Start { get; set; }
            public int DurationMinutes { get; set; }
            public string Title { get; set; }
            public string Type { get; set; } // Meeting, Ticket, Internal
            public string TicketNumber { get; set; }
        }
        public List<CalendarItemSample> CalendarItems = new()
        {
            // TODAY
            new CalendarItemSample
            {
                Id = 1,
                Start = DateTime.Today.AddHours(9),
                DurationMinutes = 15,
                Title = "Daily Dispatch Review",
                Type = "Meeting"
            },
            new CalendarItemSample
            {
                Id = 2,
                Start = DateTime.Today.AddHours(11),
                DurationMinutes = 60,
                Title = "Printer Mapping Issue",
                Type = "Onsite Support",
                TicketNumber = "T20260412.0010"
            },
            new CalendarItemSample
            {
                Id = 3,
                Start = DateTime.Today.AddHours(14.5),
                DurationMinutes = 45,
                Title = "Backup Verification Alert Review",
                Type = "Remote Support",
                TicketNumber = "T20260411.0098"
            },

            // TOMORROW
            new CalendarItemSample
            {
                Id = 4,
                Start = DateTime.Today.AddDays(1).AddHours(8.5),
                DurationMinutes = 60,
                Title = "Firewall Rule Review",
                Type = "Meeting"
            },
            new CalendarItemSample
            {
                Id = 5,
                Start = DateTime.Today.AddDays(1).AddHours(10),
                DurationMinutes = 60,
                Title = "New User Setup Validation",
                Type = "Remote Support",
                TicketNumber = "T20260412.0014"
            },

            // DAY 3
            new CalendarItemSample
            {
                Id = 6,
                Start = DateTime.Today.AddDays(2).AddHours(9.5),
                DurationMinutes = 60,
                Title = "Wi-Fi Troubleshooting",
                Type = "Onsite Support",
                TicketNumber = "T20260410.0082"
            }
        };

        protected override async Task OnInitializedAsync()
        {
        }
    }
}