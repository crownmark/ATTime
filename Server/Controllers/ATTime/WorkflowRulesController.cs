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
    [Route("odata/ATTime/WorkflowRules")]
    public partial class WorkflowRulesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public WorkflowRulesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowRule> GetWorkflowRules()
        {
            var items = this.context.WorkflowRules.AsQueryable<CrownATTime.Server.Models.ATTime.WorkflowRule>();
            this.OnWorkflowRulesRead(ref items);

            return items;
        }

        partial void OnWorkflowRulesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.WorkflowRule> items);

        partial void OnWorkflowRuleGet(ref SingleResult<CrownATTime.Server.Models.ATTime.WorkflowRule> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/WorkflowRules(WorkflowRuleId={WorkflowRuleId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.WorkflowRule> GetWorkflowRule(int key)
        {
            var items = this.context.WorkflowRules.Where(i => i.WorkflowRuleId == key);
            var result = SingleResult.Create(items);

            OnWorkflowRuleGet(ref result);

            return result;
        }
        partial void OnWorkflowRuleDeleted(CrownATTime.Server.Models.ATTime.WorkflowRule item);
        partial void OnAfterWorkflowRuleDeleted(CrownATTime.Server.Models.ATTime.WorkflowRule item);

        [HttpDelete("/odata/ATTime/WorkflowRules(WorkflowRuleId={WorkflowRuleId})")]
        public IActionResult DeleteWorkflowRule(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.WorkflowRules
                    .Where(i => i.WorkflowRuleId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnWorkflowRuleDeleted(item);
                this.context.WorkflowRules.Remove(item);
                this.context.SaveChanges();
                this.OnAfterWorkflowRuleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWorkflowRuleUpdated(CrownATTime.Server.Models.ATTime.WorkflowRule item);
        partial void OnAfterWorkflowRuleUpdated(CrownATTime.Server.Models.ATTime.WorkflowRule item);

        [HttpPut("/odata/ATTime/WorkflowRules(WorkflowRuleId={WorkflowRuleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutWorkflowRule(int key, [FromBody]CrownATTime.Server.Models.ATTime.WorkflowRule item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.WorkflowRuleId != key))
                {
                    return BadRequest();
                }
                this.OnWorkflowRuleUpdated(item);
                this.context.WorkflowRules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowRules.Where(i => i.WorkflowRuleId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "CompanyCache,WorkflowTriggerType");
                this.OnAfterWorkflowRuleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/WorkflowRules(WorkflowRuleId={WorkflowRuleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchWorkflowRule(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.WorkflowRule> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.WorkflowRules.Where(i => i.WorkflowRuleId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnWorkflowRuleUpdated(item);
                this.context.WorkflowRules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowRules.Where(i => i.WorkflowRuleId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "CompanyCache,WorkflowTriggerType");
                this.OnAfterWorkflowRuleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWorkflowRuleCreated(CrownATTime.Server.Models.ATTime.WorkflowRule item);
        partial void OnAfterWorkflowRuleCreated(CrownATTime.Server.Models.ATTime.WorkflowRule item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.WorkflowRule item)
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

                this.OnWorkflowRuleCreated(item);
                this.context.WorkflowRules.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowRules.Where(i => i.WorkflowRuleId == item.WorkflowRuleId);

                Request.QueryString = Request.QueryString.Add("$expand", "CompanyCache,WorkflowTriggerType");

                this.OnAfterWorkflowRuleCreated(item);

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
