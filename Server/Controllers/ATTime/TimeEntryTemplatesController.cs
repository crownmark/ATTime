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
    [Route("odata/ATTime/TimeEntryTemplates")]
    public partial class TimeEntryTemplatesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public TimeEntryTemplatesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> GetTimeEntryTemplates()
        {
            var items = this.context.TimeEntryTemplates.AsQueryable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>();
            this.OnTimeEntryTemplatesRead(ref items);

            return items;
        }

        partial void OnTimeEntryTemplatesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> items);

        partial void OnTimeEntryTemplateGet(ref SingleResult<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/TimeEntryTemplates(TimeEntryTemplateId={TimeEntryTemplateId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> GetTimeEntryTemplate(int key)
        {
            var items = this.context.TimeEntryTemplates.Where(i => i.TimeEntryTemplateId == key);
            var result = SingleResult.Create(items);

            OnTimeEntryTemplateGet(ref result);

            return result;
        }
        partial void OnTimeEntryTemplateDeleted(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);
        partial void OnAfterTimeEntryTemplateDeleted(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);

        [HttpDelete("/odata/ATTime/TimeEntryTemplates(TimeEntryTemplateId={TimeEntryTemplateId})")]
        public IActionResult DeleteTimeEntryTemplate(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TimeEntryTemplates
                    .Where(i => i.TimeEntryTemplateId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTimeEntryTemplateDeleted(item);
                this.context.TimeEntryTemplates.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTimeEntryTemplateDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTimeEntryTemplateUpdated(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);
        partial void OnAfterTimeEntryTemplateUpdated(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);

        [HttpPut("/odata/ATTime/TimeEntryTemplates(TimeEntryTemplateId={TimeEntryTemplateId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTimeEntryTemplate(int key, [FromBody]CrownATTime.Server.Models.ATTime.TimeEntryTemplate item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.TimeEntryTemplateId != key))
                {
                    return BadRequest();
                }
                this.OnTimeEntryTemplateUpdated(item);
                this.context.TimeEntryTemplates.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeEntryTemplates.Where(i => i.TimeEntryTemplateId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "BillingCodeCache,EmailTemplate");
                this.OnAfterTimeEntryTemplateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/TimeEntryTemplates(TimeEntryTemplateId={TimeEntryTemplateId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTimeEntryTemplate(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TimeEntryTemplates.Where(i => i.TimeEntryTemplateId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTimeEntryTemplateUpdated(item);
                this.context.TimeEntryTemplates.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeEntryTemplates.Where(i => i.TimeEntryTemplateId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "BillingCodeCache,EmailTemplate");
                this.OnAfterTimeEntryTemplateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTimeEntryTemplateCreated(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);
        partial void OnAfterTimeEntryTemplateCreated(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.TimeEntryTemplate item)
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

                this.OnTimeEntryTemplateCreated(item);
                this.context.TimeEntryTemplates.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeEntryTemplates.Where(i => i.TimeEntryTemplateId == item.TimeEntryTemplateId);

                Request.QueryString = Request.QueryString.Add("$expand", "BillingCodeCache,EmailTemplate");

                this.OnAfterTimeEntryTemplateCreated(item);

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
