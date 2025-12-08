using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CrownATTime.Server.Controllers.ATTime
{
    [Route("odata/ATTime/TimeEntries")]
    public partial class TimeEntriesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public TimeEntriesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntry> GetTimeEntries()
        {
            var items = this.context.TimeEntries.AsQueryable<CrownATTime.Server.Models.ATTime.TimeEntry>();
            this.OnTimeEntriesRead(ref items);

            return items;
        }

        partial void OnTimeEntriesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeEntry> items);

        partial void OnTimeEntryGet(ref SingleResult<CrownATTime.Server.Models.ATTime.TimeEntry> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/TimeEntries(TimeEntryId={TimeEntryId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.TimeEntry> GetTimeEntry(int key)
        {
            var items = this.context.TimeEntries.Where(i => i.TimeEntryId == key);
            var result = SingleResult.Create(items);

            OnTimeEntryGet(ref result);

            return result;
        }
        partial void OnTimeEntryDeleted(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryDeleted(CrownATTime.Server.Models.ATTime.TimeEntry item);

        [HttpDelete("/odata/ATTime/TimeEntries(TimeEntryId={TimeEntryId})")]
        public IActionResult DeleteTimeEntry(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TimeEntries
                    .Where(i => i.TimeEntryId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTimeEntryDeleted(item);
                this.context.TimeEntries.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTimeEntryDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTimeEntryUpdated(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryUpdated(CrownATTime.Server.Models.ATTime.TimeEntry item);

        [HttpPut("/odata/ATTime/TimeEntries(TimeEntryId={TimeEntryId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTimeEntry(int key, [FromBody]CrownATTime.Server.Models.ATTime.TimeEntry item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.TimeEntryId != key))
                {
                    return BadRequest();
                }
                this.OnTimeEntryUpdated(item);
                this.context.TimeEntries.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeEntries.Where(i => i.TimeEntryId == key);
                
                this.OnAfterTimeEntryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/TimeEntries(TimeEntryId={TimeEntryId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTimeEntry(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.TimeEntry> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TimeEntries.Where(i => i.TimeEntryId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTimeEntryUpdated(item);
                this.context.TimeEntries.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeEntries.Where(i => i.TimeEntryId == key);
                
                this.OnAfterTimeEntryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTimeEntryCreated(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryCreated(CrownATTime.Server.Models.ATTime.TimeEntry item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.TimeEntry item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnTimeEntryCreated(item);
                this.context.TimeEntries.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeEntries.Where(i => i.TimeEntryId == item.TimeEntryId);

                

                this.OnAfterTimeEntryCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
