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
    [Route("odata/ATTime/WorkflowSteps")]
    public partial class WorkflowStepsController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public WorkflowStepsController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowStep> GetWorkflowSteps()
        {
            var items = this.context.WorkflowSteps.AsQueryable<CrownATTime.Server.Models.ATTime.WorkflowStep>();
            this.OnWorkflowStepsRead(ref items);

            return items;
        }

        partial void OnWorkflowStepsRead(ref IQueryable<CrownATTime.Server.Models.ATTime.WorkflowStep> items);

        partial void OnWorkflowStepGet(ref SingleResult<CrownATTime.Server.Models.ATTime.WorkflowStep> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/WorkflowSteps(WorkflowStepId={WorkflowStepId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.WorkflowStep> GetWorkflowStep(int key)
        {
            var items = this.context.WorkflowSteps.Where(i => i.WorkflowStepId == key);
            var result = SingleResult.Create(items);

            OnWorkflowStepGet(ref result);

            return result;
        }
        partial void OnWorkflowStepDeleted(CrownATTime.Server.Models.ATTime.WorkflowStep item);
        partial void OnAfterWorkflowStepDeleted(CrownATTime.Server.Models.ATTime.WorkflowStep item);

        [HttpDelete("/odata/ATTime/WorkflowSteps(WorkflowStepId={WorkflowStepId})")]
        public IActionResult DeleteWorkflowStep(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.WorkflowSteps
                    .Where(i => i.WorkflowStepId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnWorkflowStepDeleted(item);
                this.context.WorkflowSteps.Remove(item);
                this.context.SaveChanges();
                this.OnAfterWorkflowStepDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWorkflowStepUpdated(CrownATTime.Server.Models.ATTime.WorkflowStep item);
        partial void OnAfterWorkflowStepUpdated(CrownATTime.Server.Models.ATTime.WorkflowStep item);

        [HttpPut("/odata/ATTime/WorkflowSteps(WorkflowStepId={WorkflowStepId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutWorkflowStep(int key, [FromBody]CrownATTime.Server.Models.ATTime.WorkflowStep item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.WorkflowStepId != key))
                {
                    return BadRequest();
                }
                this.OnWorkflowStepUpdated(item);
                this.context.WorkflowSteps.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowSteps.Where(i => i.WorkflowStepId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "EmailTemplate,NoteTemplate,TeamsMessageTemplate,TimeEntryTemplate,WorkflowRule,WorkflowStepType");
                this.OnAfterWorkflowStepUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/WorkflowSteps(WorkflowStepId={WorkflowStepId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchWorkflowStep(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.WorkflowStep> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.WorkflowSteps.Where(i => i.WorkflowStepId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnWorkflowStepUpdated(item);
                this.context.WorkflowSteps.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowSteps.Where(i => i.WorkflowStepId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "EmailTemplate,NoteTemplate,TeamsMessageTemplate,TimeEntryTemplate,WorkflowRule,WorkflowStepType");
                this.OnAfterWorkflowStepUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWorkflowStepCreated(CrownATTime.Server.Models.ATTime.WorkflowStep item);
        partial void OnAfterWorkflowStepCreated(CrownATTime.Server.Models.ATTime.WorkflowStep item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.WorkflowStep item)
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

                this.OnWorkflowStepCreated(item);
                this.context.WorkflowSteps.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowSteps.Where(i => i.WorkflowStepId == item.WorkflowStepId);

                Request.QueryString = Request.QueryString.Add("$expand", "EmailTemplate,NoteTemplate,TeamsMessageTemplate,TimeEntryTemplate,WorkflowRule,WorkflowStepType");

                this.OnAfterWorkflowStepCreated(item);

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
