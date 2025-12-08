using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using CrownATTime.Server.Data;

namespace CrownATTime.Server
{
    public partial class ATTimeService
    {
        ATTimeContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ATTimeContext context;
        private readonly NavigationManager navigationManager;

        public ATTimeService(ATTimeContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportTimeEntriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTimeEntriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTimeEntriesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeEntry> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.TimeEntry>> GetTimeEntries(Query query = null)
        {
            var items = Context.TimeEntries.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTimeEntriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTimeEntryGet(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnGetTimeEntryByTimeEntryId(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeEntry> items);


        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> GetTimeEntryByTimeEntryId(int timeentryid)
        {
            var items = Context.TimeEntries
                              .AsNoTracking()
                              .Where(i => i.TimeEntryId == timeentryid);

 
            OnGetTimeEntryByTimeEntryId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTimeEntryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTimeEntryCreated(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryCreated(CrownATTime.Server.Models.ATTime.TimeEntry item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> CreateTimeEntry(CrownATTime.Server.Models.ATTime.TimeEntry timeentry)
        {
            OnTimeEntryCreated(timeentry);

            var existingItem = Context.TimeEntries
                              .Where(i => i.TimeEntryId == timeentry.TimeEntryId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TimeEntries.Add(timeentry);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(timeentry).State = EntityState.Detached;
                throw;
            }

            OnAfterTimeEntryCreated(timeentry);

            return timeentry;
        }

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> CancelTimeEntryChanges(CrownATTime.Server.Models.ATTime.TimeEntry item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTimeEntryUpdated(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryUpdated(CrownATTime.Server.Models.ATTime.TimeEntry item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> UpdateTimeEntry(int timeentryid, CrownATTime.Server.Models.ATTime.TimeEntry timeentry)
        {
            OnTimeEntryUpdated(timeentry);

            var itemToUpdate = Context.TimeEntries
                              .Where(i => i.TimeEntryId == timeentry.TimeEntryId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(timeentry).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTimeEntryUpdated(timeentry);

            return timeentry;
        }

        partial void OnTimeEntryDeleted(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryDeleted(CrownATTime.Server.Models.ATTime.TimeEntry item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> DeleteTimeEntry(int timeentryid)
        {
            var itemToDelete = Context.TimeEntries
                              .Where(i => i.TimeEntryId == timeentryid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTimeEntryDeleted(itemToDelete);

            Reset();

            Context.TimeEntries.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTimeEntryDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}