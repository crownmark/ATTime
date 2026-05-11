using CrownATTime.Server.Models;
using CrownATTime.Server.Models.ATTime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using static CrownATTime.Server.Models.ITGlueDocumentsResult;

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
        protected List<CalendarEvent> calendarEvents = new List<CalendarEvent>();
        protected IEnumerable<ResourceCache> resources = new List<ResourceCache>();

        protected int calendarEventsCount;

        protected bool calendarLoading = false;

        protected int sliderNumberOfDays { get; set; } = 3;
        protected bool showSlider { get; set; } = true;
        protected CalendarEvent calendarEvent { get; set; } = new CalendarEvent() { ServiceCallId = 999999999 };

        [Parameter]
        public int SelectedCalendarViewIndex { get; set; }

        [Parameter]
        public string SelectedResourceEmail { get; set; }

        [Parameter]
        public int TicketId { get; set; }

        protected int selectedMinutes { get; set; }
        protected string selectedActivity { get; set; }

        protected TicketDtoResult ticket { get; set; }

        protected IEnumerable<CrownATTime.Server.Models.ATTime.Duration> durations = new List<Duration>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                DialogService.OpenAsync("", ds =>
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
                                columnContent.AddContent(0, $"Loading...");
                            }));
                            rowContent.CloseComponent();
                        }));

                        dialogContent.CloseComponent();
                    };
                    return content;
                }, new DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });

                await GetResources();
                await GetLoggedInResource();

                if (TicketId > 0)
                {
                    ticket = await AutotaskService.GetTicket(TicketId);

                }
                DialogService.Close();
                await LoadCalendarData();

            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Unable to load calendar: {ex.Message}" });
            }
        }

        protected async Task GetLoggedInResource()
        {
            try
            {
                //TESTING
                //var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq 'philip@ce-technology.com'");
                //PRODUCTION
                var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq '{SelectedResourceEmail}'");
                resource = resourceResult.Value.FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

        }

        protected async Task GetResources()
        {
            try
            {

                var resourceResult = await ATTimeService.GetResourceCaches(filter: $"IsActive eq true");
                resources = resourceResult.Value.Where(x => x.IsActive == true && (x.LicenseType == 1 || x.LicenseType == 3) && !x.FirstName.Contains("Autotask") && !x.FirstName.Contains("Bassem")).OrderBy(x => x.FirstName).ToList();
            }
            catch (Exception ex)
            {

            }

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                calendarLoading = true;
                //await Task.Delay(300);

                var now = DateTime.Now; // 2:30 PM

                await JSRuntime.InvokeVoidAsync(
                    "scrollSchedulerToTime",
                    now.ToString("o") // ISO format
                );
            }
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
                DialogService.OpenAsync("", ds =>
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
                                columnContent.AddContent(0, $"Loading calendar for {resource.FullName}...");
                            }));
                            rowContent.CloseComponent();
                        }));

                        dialogContent.CloseComponent();
                    };
                    return content;
                }, new DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });

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

        //public async Task BusyDialog(string message)
        //{
        //    DialogService.OpenAsync("", ds =>
        //    {
        //        RenderFragment content = dialogContent =>
        //        {
        //            dialogContent.OpenComponent<RadzenRow>(0);
        //            dialogContent.AddComponentParameter(1, nameof(RadzenRow.ChildContent), (RenderFragment)(rowContent =>
        //            {
        //                rowContent.OpenComponent<RadzenColumn>(0);
        //                rowContent.AddComponentParameter(1, nameof(RadzenColumn.Size), 12);
        //                rowContent.AddComponentParameter(2, nameof(RadzenRow.ChildContent), (RenderFragment)(columnContent =>
        //                {
        //                    columnContent.AddContent(0, message);
        //                }));
        //                rowContent.CloseComponent();
        //            }));

        //            dialogContent.CloseComponent();
        //        };
        //        return content;
        //    }, new DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });
        //}

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

                        b.AddContent(4, "Updating Record.  Please wait...");

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
                    else if (draggedAppointment.ServiceCallId.HasValue && draggedAppointment.ServiceCallId != 999999999)
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
                    else if (draggedAppointment.ServiceCallId.HasValue && draggedAppointment.ServiceCallId == 999999999)
                    {
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
            try
            {
                if(TicketId > 0)
                {
                    var existingEvent = calendarEvents.Where(x => x.ServiceCallId == 999999999);
                    if (existingEvent.Any())
                    {
                        
                        var selectedEvent = existingEvent.First();
                        selectedEvent = calendarEvent;
                        selectedEvent.Start = args.Start;
                        selectedEvent.End = selectedEvent.Start.AddMinutes(selectedMinutes);
                    }
                    else
                    {
                        if(!string.IsNullOrEmpty(selectedActivity) && selectedMinutes > 0)
                        {
                            calendarEvent.TicketId = ticket.item.id;
                            calendarEvent.CompanyId = ticket.item.companyID;
                            calendarEvent.DurationMinutes = selectedMinutes;
                            calendarEvent.EventType = "";
                            calendarEvent.Start = args.Start;
                            calendarEvent.End = args.Start.AddMinutes(selectedMinutes);
                            calendarEvent.Status = selectedActivity == "Remote" ? 105 : selectedActivity == "Onsite" ? 106 : 1;
                            calendarEvent.ResourceId = resource.Id;
                            calendarEvent.Title = $"{ticket.item.ticketNumber} - {ticket.item.title}";
                            calendarEvents.Add(calendarEvent);
                        }
                        
                    }
                    await scheduler0.Reload();
                }
            }
            catch (Exception ex)
            {

            }
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
                    //BusyDialog($"Loading Ticket {args.Data.Title}...");
                    //DialogService.OpenAsync("", ds =>
                    //{
                    //    RenderFragment content = b =>
                    //    {
                    //        b.OpenElement(0, "div");
                    //        b.AddAttribute(1, "class", "row");

                    //        b.OpenElement(2, "div");
                    //        b.AddAttribute(3, "class", "col-md-12");

                    //        b.AddContent(4, $"Loading Ticket {args.Data.Title}...");

                    //        b.CloseElement();
                    //        b.CloseElement();
                    //    };
                    //    return content;
                    //}, new Radzen.DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });

                    if (resource.CalendarSlotClickEventActionId.Value == 1)
                    {
                        // Open Time Entry Dialog
                        
                        await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"{NavigationManager.BaseUri}TimeEntry/{args.Data.TicketId.Value}");
                        DialogService.Close();

                    }
                    else if (resource.CalendarSlotClickEventActionId.Value == 2)
                    {
                        // Open Ticket Details
                        var ticket = await AutotaskService.GetTicket(args.Data.TicketId.Value);
                        var primaryResource = await AutotaskService.GetResourceById(args.Data.ResourceId);
                        DialogService.Close();
                        await DialogService.OpenAsync<TicketDetails>("Ticket Details", new Dictionary<string, object>() { { "ResourceId", resource.Id }, { "Ticket", ticket }, { "PriorityName", ticket.item.priorityName }, { "StatusName", ticket.item.statusName }, { "PrimaryResource", $"{primaryResource.item.firstName} {primaryResource.item.lastName}" } }, new DialogOptions { Width = "1200px", CloseDialogOnOverlayClick = true });
                    }
                    else if (resource.CalendarSlotClickEventActionId.Value == 3)
                    {
                        // Open Autotask Ticket in new tab
                        await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"https://ww5.autotask.net/Autotask/AutotaskExtend/ExecuteCommand.aspx?Code=OpenTicketDetail&TicketID={args.Data.TicketId.Value}");
                        DialogService.Close();

                    }
                    else if (resource.CalendarSlotClickEventActionId.Value == 4)
                    {
                        // Open Autotask Ticket in new tab
                        //DialogService.OpenAsync("", ds =>
                        //{
                        //    RenderFragment content = b =>
                        //    {
                        //        b.OpenElement(0, "div");
                        //        b.AddAttribute(1, "class", "row");

                        //        b.OpenElement(2, "div");
                        //        b.AddAttribute(3, "class", "col-md-12");

                        //        b.AddContent(4, $"Loading Time Entry {args.Data.Title}...");

                        //        b.CloseElement();
                        //        b.CloseElement();
                        //    };
                        //    return content;
                        //}, new Radzen.DialogOptions() { ShowTitle = false, Style = "min-height:auto;min-width:auto;width:auto", CloseDialogOnEsc = false });
                        await JSRuntime.InvokeVoidAsync("open", TimeSpan.FromSeconds(1), $"timeentry/{args.Data.TicketId.Value.ToString()}");

                        DialogService.Close();

                    }
                    else
                    {
                        var ticket = await AutotaskService.GetTicket(args.Data.TicketId.Value);
                        var primaryResource = await AutotaskService.GetResourceById(args.Data.ResourceId);
                        DialogService.Close();
                        await DialogService.OpenAsync<TicketDetails>("Ticket Details", new Dictionary<string, object>() { { "ResourceId", resource.Id }, { "Ticket", ticket }, { "PriorityName", ticket.item.priorityName }, { "StatusName", ticket.item.statusName }, { "PrimaryResource", $"{primaryResource.item.firstName} {primaryResource.item.lastName}" } }, new DialogOptions { Width = "1200px", CloseDialogOnOverlayClick = true });
                    }
                        
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

        protected async System.Threading.Tasks.Task resourceSelectBarChange(string args)
        {
            SelectedResourceEmail = args;
            await GetLoggedInResource();
            await LoadCalendarData();
        }

        protected async System.Threading.Tasks.Task TemplateForm0Submit(Server.Models.CalendarEvent args)
        {
            DialogService.OpenAsync("", ds =>
            {
                RenderFragment content = b =>
                {
                    b.OpenElement(0, "div");
                    b.AddAttribute(1, "class", "row");

                    b.OpenElement(2, "div");
                    b.AddAttribute(3, "class", "col-md-12");

                    b.AddContent(4, "Creating Service Call.  Please wait...");

                    b.CloseElement();
                    b.CloseElement();
                };
                return content;
            }, new Radzen.DialogOptions() { ShowTitle = false, Style = "min-height:auto.;min-width:auto;width:auto", CloseDialogOnEsc = false });
            
            
            try
            {

                if (ticket.item.assignedResourceID == resource.Id || (!string.IsNullOrEmpty(ticket.item.secondaryResources) && ticket.item.secondaryResources.Contains($"{resource.FullName}")))
                {
                    calendarEvent.Description = selectedActivity == "Remote" ? $"Remote Service Call{Environment.NewLine}Notes:{Environment.NewLine}{calendarEvent.Description}" : selectedActivity == "Onsite" ? $"Onsite Service Call{Environment.NewLine}Notes:{Environment.NewLine}{calendarEvent.Description}" : $"{calendarEvent.Description}";

                    var newServiceCall = await AutotaskService.CreateServiceCall(new ServiceCallDto()
                    {
                        startDateTime = calendarEvent.Start,
                        endDateTime = calendarEvent.End,
                        companyID = calendarEvent.CompanyId,
                        description = calendarEvent.Description,
                        status = calendarEvent.Status.Value,
                        impersonatorCreatorResourceID = calendarEvent.ResourceId,
                    });
                    var newServiceCallTicket = await AutotaskService.CreateServiceCallTicket(new ServiceCallTicket()
                    {
                        serviceCallID = newServiceCall.itemId,
                        ticketID = calendarEvent.TicketId.Value
                    });
                    var newServiceCallTicketResource = await AutotaskService.CreateServiceCallTicketResource(new ServiceCallTicketResourceCreate()
                    {
                        resourceID = resource.Id,
                        serviceCallTicketID = newServiceCallTicket.itemId,
                    });
                    DialogService.Close();
                    DialogService.Close();
                }
                else
                {
                    //ADD USER AS A SECONDARY ON THE TICKET
                    try
                    {
                        var resourceServiceDeskRoles = await ATTimeService.GetServiceDeskRoleCaches(filter: $"ResourceId eq {resource.Id}");
                        var resourceServiceDeskRolesList = resourceServiceDeskRoles.Value.ToList();
                        var selectedRole = new ServiceDeskRoleCache();

                        if (resourceServiceDeskRolesList.Any())
                        {
                            if (resourceServiceDeskRolesList.Count() > 1)
                            {
                                selectedRole = resourceServiceDeskRolesList.FirstOrDefault(x => x.IsDefault == true) != null ?
                                    resourceServiceDeskRolesList.FirstOrDefault(x => x.IsDefault == true) : resourceServiceDeskRolesList.FirstOrDefault();
                            }
                            else
                            {
                                selectedRole = resourceServiceDeskRolesList.FirstOrDefault();
                            }
                            if (selectedRole != null)
                            {

                                //Add user as secondary
                                var secondaryResource = new TicketSecondaryResourcesCreate()
                                {
                                    resourceID = selectedRole.ResourceId,
                                    roleID = selectedRole.RoleId,
                                    ticketID = ticket.item.id
                                };

                                await AutotaskService.CreateSecondaryResource(secondaryResource);

                                calendarEvent.Description = selectedActivity == "Remote" ? $"Remote Service Call{Environment.NewLine}Notes:{Environment.NewLine}{calendarEvent.Description}" : selectedActivity == "Onsite" ? $"Onsite Service Call{Environment.NewLine}Notes:{Environment.NewLine}{calendarEvent.Description}" : $"{calendarEvent.Description}";

                                var newServiceCall = await AutotaskService.CreateServiceCall(new ServiceCallDto()
                                {
                                    startDateTime = calendarEvent.Start,
                                    endDateTime = calendarEvent.End,
                                    companyID = calendarEvent.CompanyId,
                                    description = calendarEvent.Description,
                                    status = calendarEvent.Status.Value,
                                    impersonatorCreatorResourceID = calendarEvent.ResourceId,
                                });
                                var newServiceCallTicket = await AutotaskService.CreateServiceCallTicket(new ServiceCallTicket()
                                {
                                    serviceCallID = newServiceCall.itemId,
                                    ticketID = calendarEvent.TicketId.Value
                                });
                                var newServiceCallTicketResource = await AutotaskService.CreateServiceCallTicketResource(new ServiceCallTicketResourceCreate()
                                {
                                    resourceID = resource.Id,
                                    serviceCallTicketID = newServiceCallTicket.itemId,
                                });
                                DialogService.Close();
                                DialogService.Close();
                            }
                            else
                            {
                                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Selected User MUST be added as a Secondary Resource on the Ticket" });

                            }
                        }
                        DialogService.Close();
                    }
                    catch (Exception ex)
                    {
                        NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Error Scheduling Secondary Resource: {ex.Message}" });

                        DialogService.Close();

                    }



                }

            }            
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"{ex.Message}" });
                DialogService.Close();
            }
        }

        protected async System.Threading.Tasks.Task ActivityTypeChange(int args)
        {
            selectedActivity = args == 1 ? "Remote" : args == 2 ? "Onsite" : "";
            selectedMinutes = 0;

            if (args == 1) 
            {
                durations = (await ATTimeService.GetDurations(filter: $"DurationTypeId eq {2}")).Value;

            }
            else if (args == 2)
            {
                durations = (await ATTimeService.GetDurations(filter: $"DurationTypeId eq {3}")).Value;

            }
            else if(args == 3)
            {
                durations = (await ATTimeService.GetDurations(filter: $"DurationTypeId eq {1}")).Value;

            }
            else
            {
                durations = new List<Duration>();
            }
            var existingEvent = calendarEvents.Where(x => x.ServiceCallId == 999999999);
            if (existingEvent.Any())
            {
                var selectedEvent = existingEvent.First();
                selectedEvent = calendarEvent;
                selectedEvent.ResourceId = resource.Id;
                selectedEvent.End = selectedEvent.Start.AddMinutes(selectedMinutes);
                selectedEvent.Status = selectedActivity == "Remote" ? 105 : selectedActivity == "Onsite" ? 106 : 1;

                await scheduler0.Reload();
            }
            StateHasChanged();
        }

        protected async System.Threading.Tasks.Task DurationChange(int args)
        {
            selectedMinutes = args;
            var existingEvent = calendarEvents.Where(x => x.ServiceCallId == 999999999);
            if (existingEvent.Any())
            {
                var selectedEvent = existingEvent.First();
                selectedEvent = calendarEvent;

                selectedEvent.ResourceId = resource.Id;
                selectedEvent.End = selectedEvent.Start.AddMinutes(selectedMinutes);
                selectedEvent.Status = selectedActivity == "Remote" ? 105 : selectedActivity == "Onsite" ? 106 : 1;
                await scheduler0.Reload();

            }
            StateHasChanged();

        }
    }
}