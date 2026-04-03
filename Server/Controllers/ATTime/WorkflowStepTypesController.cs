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
    [Route("odata/ATTime/WorkflowStepTypes")]
    public partial class WorkflowStepTypesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public WorkflowStepTypesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowStepType> GetWorkflowStepTypes()
        {
            var items = this.context.WorkflowStepTypes.AsQueryable<CrownATTime.Server.Models.ATTime.WorkflowStepType>();
            this.OnWorkflowStepTypesRead(ref items);

            return items;
        }

        partial void OnWorkflowStepTypesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.WorkflowStepType> items);

        partial void OnWorkflowStepTypeGet(ref SingleResult<CrownATTime.Server.Models.ATTime.WorkflowStepType> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/WorkflowStepTypes(WorkflowStepTypeId={WorkflowStepTypeId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.WorkflowStepType> GetWorkflowStepType(int key)
        {
            var items = this.context.WorkflowStepTypes.Where(i => i.WorkflowStepTypeId == key);
            var result = SingleResult.Create(items);

            OnWorkflowStepTypeGet(ref result);

            return result;
        }
        partial void OnWorkflowStepTypeDeleted(CrownATTime.Server.Models.ATTime.WorkflowStepType item);
        partial void OnAfterWorkflowStepTypeDeleted(CrownATTime.Server.Models.ATTime.WorkflowStepType item);

        [HttpDelete("/odata/ATTime/WorkflowStepTypes(WorkflowStepTypeId={WorkflowStepTypeId})")]
        public IActionResult DeleteWorkflowStepType(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.WorkflowStepTypes
                    .Where(i => i.WorkflowStepTypeId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnWorkflowStepTypeDeleted(item);
                this.context.WorkflowStepTypes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterWorkflowStepTypeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWorkflowStepTypeUpdated(CrownATTime.Server.Models.ATTime.WorkflowStepType item);
        partial void OnAfterWorkflowStepTypeUpdated(CrownATTime.Server.Models.ATTime.WorkflowStepType item);

        [HttpPut("/odata/ATTime/WorkflowStepTypes(WorkflowStepTypeId={WorkflowStepTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutWorkflowStepType(int key, [FromBody]CrownATTime.Server.Models.ATTime.WorkflowStepType item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.WorkflowStepTypeId != key))
                {
                    return BadRequest();
                }
                this.OnWorkflowStepTypeUpdated(item);
                this.context.WorkflowStepTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowStepTypes.Where(i => i.WorkflowStepTypeId == key);
                
                this.OnAfterWorkflowStepTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/WorkflowStepTypes(WorkflowStepTypeId={WorkflowStepTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchWorkflowStepType(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.WorkflowStepType> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.WorkflowStepTypes.Where(i => i.WorkflowStepTypeId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnWorkflowStepTypeUpdated(item);
                this.context.WorkflowStepTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowStepTypes.Where(i => i.WorkflowStepTypeId == key);
                
                this.OnAfterWorkflowStepTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWorkflowStepTypeCreated(CrownATTime.Server.Models.ATTime.WorkflowStepType item);
        partial void OnAfterWorkflowStepTypeCreated(CrownATTime.Server.Models.ATTime.WorkflowStepType item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.WorkflowStepType item)
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

                this.OnWorkflowStepTypeCreated(item);
                this.context.WorkflowStepTypes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowStepTypes.Where(i => i.WorkflowStepTypeId == item.WorkflowStepTypeId);

                

                this.OnAfterWorkflowStepTypeCreated(item);

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
