using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrownATTime.Client.Pages
{
    public partial class MyCalendar
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
        [Inject]
        protected AutotaskService AutotaskService { get; set; }

        [Inject]
        public ATTimeService ATTimeService { get; set; }

        protected RadzenScheduler<CalendarEvent> scheduler0;
        protected ResourceCache resource { get; set; }
        protected IEnumerable<CalendarEvent> calendarEvents = new List<CalendarEvent>();

        protected int calendarEventsCount;

        protected bool calendarLoading = false;

        protected int sliderNumberOfDays { get; set; } = 3;
        protected bool showSlider { get; set; } = true;

        [Parameter]
        public int SelectedCalendarViewIndex { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq '{Security.User.Email}'");
                resource = resourceResult.Value.FirstOrDefault();
                SelectedCalendarViewIndex = 1;
                await LoadCalendarData();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load calendar: {ex.Message}" });
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
        }


        protected async System.Threading.Tasks.Task ReloadCalendarData(Radzen.LoadDataArgs args)
        {
            try
            {
                //calendarLoading = true;
                await LoadCalendarData();
                //calendarLoading = false;

            }
            catch (Exception ex)
            {
                //calendarLoading = false;

            }
        }
        protected async Task LoadCalendarData()
        {
            try
            {
                BusyDialog($"Loading calendar for {resource.FullName}...");

                // calendarLoading = true;
                calendarEvents = await AutotaskService.GetCalendarEventsForResource(resource.Id);
                calendarEventsCount = calendarEvents.Count();
                // calendarLoading = false;
                DialogService.Close();

            }
            catch (Exception ex)
            {
                // calendarLoading = false;
                DialogService.Close();

            }
        }

        async Task BusyDialog(string message)
        {
            await DialogService.OpenAsync("", ds =>
            {
                RenderFragment content = dialogContent =>
                {
                    dialogContent.OpenComponent<RadzenRow>(0);
                    dialogContent.AddComponentParameter(1, nameof(RadzenRow.ChildContent), (RenderFragment)(rowContent =>
                    {
                        rowContent.OpenComponent<RadzenColumn>(0);
                        rowContent.AddComponentParameter(1, nameof(RadzenColumn.Size), 12);
                        rowContent.AddComponentParameter(2, nameof(RadzenRow.ChildContent), (RenderFragment)(columnContent =>
                        {
                            columnContent.AddContent(0, message);
                        }));
                        rowContent.CloseComponent();
                    }));

                    dialogContent.CloseComponent();
                };
                return content;
            }, new DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });
        }

        protected async System.Threading.Tasks.Task Scheduler0AppointmentMove(Radzen.SchedulerAppointmentMoveEventArgs args)
        {
            try
            {
                DialogService.OpenAsync("", ds =>
                {
                    RenderFragment content = b =>
                    {
                        b.OpenElement(0, "div");
                        b.AddAttribute(1, "class", "row");

                        b.OpenElement(2, "div");
                        b.AddAttribute(3, "class", "col-md-12");

                        b.AddContent(4, "Updating Task.  Please wait...");

                        b.CloseElement();
                        b.CloseElement();
                    };
                    return content;
                }, new Radzen.DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });
                var draggedAppointment = calendarEvents.FirstOrDefault(x => x == args.Appointment.Data);

                if (draggedAppointment != null)
                {
                    
                    if(draggedAppointment.CompanyToDoId.HasValue)
                    {
                        
                        await AutotaskService.UpdateCompanyTodo(new CompanyToDoCreate()
                        {
                            id = draggedAppointment.CompanyToDoId.Value,
                            startDateTime = draggedAppointment.Start + args.TimeSpan,
                            endDateTime = draggedAppointment.End + args.TimeSpan,
                            completedDate = draggedAppointment.IsComplete == true ? DateTime.UtcNow : (DateTime?)null,
                            ticketID = draggedAppointment.TicketId,
                            assignedToResourceID = draggedAppointment.ResourceId,
                            companyID = draggedAppointment.CompanyId,
                            actionType = draggedAppointment.ActionType.Value
                        });
                        draggedAppointment.Start = draggedAppointment.Start + args.TimeSpan;
                        draggedAppointment.End = draggedAppointment.End + args.TimeSpan;
                    }
                    else if (draggedAppointment.ServiceCallId.HasValue)
                    {
                        await AutotaskService.UpdateServiceCall(new ServiceCallCreateDto()
                        {
                            id = draggedAppointment.ServiceCallId.Value,
                            startDateTime = draggedAppointment.Start + args.TimeSpan,
                            endDateTime = draggedAppointment.End + args.TimeSpan,
                            isComplete = draggedAppointment.IsComplete == true ? 1 : 0,
                            companyID = draggedAppointment.CompanyId,
                            description = draggedAppointment.Description,
                            status = draggedAppointment.Status.Value
                        });
                        draggedAppointment.Start = draggedAppointment.Start + args.TimeSpan;
                        draggedAppointment.End = draggedAppointment.End + args.TimeSpan;
                    }
                    else
                    {

                    }

                    //await scheduler0.Reload();
                }
                DialogService.Close();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}" });
                DialogService.Close();


            }
        }

        protected void Scheduler0SlotRender(Radzen.SchedulerSlotRenderEventArgs args)
        {
            //args.Attributes.Add("ondragover", "event.preventDefault();event.target.classList.add('my-class')");
            //args.Attributes.Add("ondragover", "event.target.classList.add('my-class')");
            //args.Attributes.Add("ondragleave", "event.target.classList.remove('my-class')");
            //args.Attributes.Add("ondrop", EventCallback.Factory.Create<DragEventArgs>(this, () =>
            //{
            //    JSRuntime.InvokeVoidAsync("eval", $"document.querySelector('.my-class').classList.remove('my-class')");
            //}));
            // Highlight working hours (9-18)
            if ((args.View.Text == "Multi-Day" || args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour > 7 && args.Start.Hour < 17)
            {
                args.Attributes["style"] = "background: var(--rz-scheduler-event-color);";
            }
        }

        protected async System.Threading.Tasks.Task Scheduler0SlotSelect(Radzen.SchedulerSlotSelectEventArgs args)
        {
        }



        protected async System.Threading.Tasks.Task Scheduler0ContextMenu(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            //ContextMenuService.Open(args,
            //    new List<ContextMenuItem> {
            //        new ContextMenuItem(){ Text = "Cell Menu - Edit", Value = 1, Icon = "edit" },
            //        new ContextMenuItem(){ Text = "Cell Menu - Delete", Value = 2, Icon = "delete" },
            //        new ContextMenuItem(){ Text = "Cell Menu - Copy", Value = 3, Icon = "content_copy" },
            //    },
            //    (e) => {
            //        //console.Log($"Cell context menu item clicked. Value={e.Value}, Column: {args.Column.Property}, EmployeeID: {args.Data.EmployeeID}");
            //    }
            //);
        }

        protected async System.Threading.Tasks.Task Scheduler0AppointmentMouseEnter(Radzen.Blazor.SchedulerAppointmentMouseEventArgs<Server.Models.CalendarEvent> args)
        {
            TooltipService.Open(args.Element, $"Title: {args.Data.Title}\nStart: {args.Data.Start}\nEnd: {args.Data.End}");
        }

        protected async System.Threading.Tasks.Task Scheduler0AppointmentMouseLeave(Radzen.Blazor.SchedulerAppointmentMouseEventArgs<Server.Models.CalendarEvent> args)
        {
            TooltipService.Close();
        }

        protected async System.Threading.Tasks.Task Scheduler0AppointmentSelect(Radzen.SchedulerAppointmentSelectEventArgs<Server.Models.CalendarEvent> args)
        {
            //ContextMenuService.Open(new MouseEventArgs(),
            //    new List<ContextMenuItem> {
            //        new ContextMenuItem(){ Text = "Cell Menu - Edit", Value = 1, Icon = "edit" },
            //        new ContextMenuItem(){ Text = "Cell Menu - Delete", Value = 2, Icon = "delete" },
            //        new ContextMenuItem(){ Text = "Cell Menu - Copy", Value = 3, Icon = "content_copy" },
            //    },
            //    (e) => {
            //        //console.Log($"Cell context menu item clicked. Value={e.Value}, Column: {args.Column.Property}, EmployeeID: {args.Data.EmployeeID}");
            //    }
            //);
            try
            {
                if (args.Data.TicketId.HasValue)
                {
                    BusyDialog($"Loading Ticket {args.Data.Title}...");

                    var ticket = await AutotaskService.GetTicket(args.Data.TicketId.Value);
                    var primaryResource = await AutotaskService.GetResourceById(args.Data.ResourceId);
                    DialogService.Close();
                    await DialogService.OpenAsync<TicketDetails>("Ticket Details", new Dictionary<string, object>() { { "ResourceId", resource.Id }, { "Ticket", ticket }, { "PriorityName", ticket.item.priorityName }, { "StatusName", ticket.item.statusName }, { "PrimaryResource", $"{primaryResource.item.firstName} {primaryResource.item.lastName}" } }, new DialogOptions { Width = "1200px", CloseDialogOnOverlayClick = true });
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected async System.Threading.Tasks.Task Scheduler0MoreSelect(Radzen.SchedulerMoreSelectEventArgs args)
        {
        }

        void NumberOfDaysChange()
        {
            StateHasChanged();
        }

        protected async System.Threading.Tasks.Task Scheduler0DaySelect(Radzen.SchedulerDaySelectEventArgs args)
        {
        }

        protected void Scheduler0AppointmentRender(Radzen.SchedulerAppointmentRenderEventArgs<Server.Models.CalendarEvent> args)
        {
            if (args.Data.EventType == "Flexible")
            {
                args.Attributes["style"] = "background: var(--rz-primary-dark);";
            }
            //(calendarEvent.ActionType.HasValue && calendarEvent.ActionType.Value == 29683373)
            else if ((args.Data.ActionType.HasValue && args.Data.ActionType.Value == 29683373) || (args.Data.Status.HasValue && args.Data.Status.Value == 105))
            {
                args.Attributes["style"] = "background: var(--rz-warning-dark);";

            }
            else if ((args.Data.ActionType.HasValue && args.Data.ActionType.Value == 29683374) || (args.Data.Status.HasValue && args.Data.Status.Value == 106))
            {
                args.Attributes["style"] = "background: var(--rz-warning-dark);";

            }           
            else if (args.Data.EventType == "Fixed")
            {
                args.Attributes["style"] = "background: var(--rz-warning-dark);";

            }
            else if (args.Data.EventType == "Service Call")
            {
                args.Attributes["style"] = "background: var(--rz-warning-dark);";

            }
            else
            {
                args.Attributes["style"] = "background: var(--rz-secondary-dark);";

            }
        }

        protected async System.Threading.Tasks.Task RefreshCalendarDataButton0Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await LoadCalendarData();
        }
    }
}