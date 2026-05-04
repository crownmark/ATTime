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
    [Route("odata/ATTime/Durations")]
    public partial class DurationsController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public DurationsController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.Duration> GetDurations()
        {
            var items = this.context.Durations.AsQueryable<CrownATTime.Server.Models.ATTime.Duration>();
            this.OnDurationsRead(ref items);

            return items;
        }

        partial void OnDurationsRead(ref IQueryable<CrownATTime.Server.Models.ATTime.Duration> items);

        partial void OnDurationGet(ref SingleResult<CrownATTime.Server.Models.ATTime.Duration> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/Durations(DurationId={DurationId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.Duration> GetDuration(int key)
        {
            var items = this.context.Durations.Where(i => i.DurationId == key);
            var result = SingleResult.Create(items);

            OnDurationGet(ref result);

            return result;
        }
        partial void OnDurationDeleted(CrownATTime.Server.Models.ATTime.Duration item);
        partial void OnAfterDurationDeleted(CrownATTime.Server.Models.ATTime.Duration item);

        [HttpDelete("/odata/ATTime/Durations(DurationId={DurationId})")]
        public IActionResult DeleteDuration(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Durations
                    .Where(i => i.DurationId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnDurationDeleted(item);
                this.context.Durations.Remove(item);
                this.context.SaveChanges();
                this.OnAfterDurationDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDurationUpdated(CrownATTime.Server.Models.ATTime.Duration item);
        partial void OnAfterDurationUpdated(CrownATTime.Server.Models.ATTime.Duration item);

        [HttpPut("/odata/ATTime/Durations(DurationId={DurationId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutDuration(int key, [FromBody]CrownATTime.Server.Models.ATTime.Duration item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.DurationId != key))
                {
                    return BadRequest();
                }
                this.OnDurationUpdated(item);
                this.context.Durations.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Durations.Where(i => i.DurationId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "DurationType");
                this.OnAfterDurationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/Durations(DurationId={DurationId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchDuration(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.Duration> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Durations.Where(i => i.DurationId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnDurationUpdated(item);
                this.context.Durations.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Durations.Where(i => i.DurationId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "DurationType");
                this.OnAfterDurationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDurationCreated(CrownATTime.Server.Models.ATTime.Duration item);
        partial void OnAfterDurationCreated(CrownATTime.Server.Models.ATTime.Duration item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.Duration item)
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

                this.OnDurationCreated(item);
                this.context.Durations.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Durations.Where(i => i.DurationId == item.DurationId);

                Request.QueryString = Request.QueryString.Add("$expand", "DurationType");

                this.OnAfterDurationCreated(item);

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
