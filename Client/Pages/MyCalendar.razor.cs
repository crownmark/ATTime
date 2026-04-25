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
        protected ResourceCache resource { get; set; }
        protected IEnumerable<CalendarEvent> calendarEvents = new List<CalendarEvent>();

        protected int calendarEventsCount;

        protected bool calendarLoading = false;


        protected override async Task OnInitializedAsync()
        {
            try
            {
                var resourceResult = await ATTimeService.GetResourceCaches(filter: $"Email eq '{Security.User.Email}'");
                resource = resourceResult.Value.FirstOrDefault();
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
                    draggedAppointment.Start = draggedAppointment.Start + args.TimeSpan;
                    draggedAppointment.End = draggedAppointment.End + args.TimeSpan;

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
        }

        protected async System.Threading.Tasks.Task Scheduler0SlotSelect(Radzen.SchedulerSlotSelectEventArgs args)
        {
        }
    }
}