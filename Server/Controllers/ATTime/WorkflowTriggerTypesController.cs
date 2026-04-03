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
    [Route("odata/ATTime/WorkflowTriggerTypes")]
    public partial class WorkflowTriggerTypesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public WorkflowTriggerTypesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.WorkflowTriggerType> GetWorkflowTriggerTypes()
        {
            var items = this.context.WorkflowTriggerTypes.AsQueryable<CrownATTime.Server.Models.ATTime.WorkflowTriggerType>();
            this.OnWorkflowTriggerTypesRead(ref items);

            return items;
        }

        partial void OnWorkflowTriggerTypesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.WorkflowTriggerType> items);

        partial void OnWorkflowTriggerTypeGet(ref SingleResult<CrownATTime.Server.Models.ATTime.WorkflowTriggerType> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/WorkflowTriggerTypes(WorkflowTriggerTypeId={WorkflowTriggerTypeId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.WorkflowTriggerType> GetWorkflowTriggerType(int key)
        {
            var items = this.context.WorkflowTriggerTypes.Where(i => i.WorkflowTriggerTypeId == key);
            var result = SingleResult.Create(items);

            OnWorkflowTriggerTypeGet(ref result);

            return result;
        }
        partial void OnWorkflowTriggerTypeDeleted(CrownATTime.Server.Models.ATTime.WorkflowTriggerType item);
        partial void OnAfterWorkflowTriggerTypeDeleted(CrownATTime.Server.Models.ATTime.WorkflowTriggerType item);

        [HttpDelete("/odata/ATTime/WorkflowTriggerTypes(WorkflowTriggerTypeId={WorkflowTriggerTypeId})")]
        public IActionResult DeleteWorkflowTriggerType(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.WorkflowTriggerTypes
                    .Where(i => i.WorkflowTriggerTypeId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnWorkflowTriggerTypeDeleted(item);
                this.context.WorkflowTriggerTypes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterWorkflowTriggerTypeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWorkflowTriggerTypeUpdated(CrownATTime.Server.Models.ATTime.WorkflowTriggerType item);
        partial void OnAfterWorkflowTriggerTypeUpdated(CrownATTime.Server.Models.ATTime.WorkflowTriggerType item);

        [HttpPut("/odata/ATTime/WorkflowTriggerTypes(WorkflowTriggerTypeId={WorkflowTriggerTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutWorkflowTriggerType(int key, [FromBody]CrownATTime.Server.Models.ATTime.WorkflowTriggerType item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.WorkflowTriggerTypeId != key))
                {
                    return BadRequest();
                }
                this.OnWorkflowTriggerTypeUpdated(item);
                this.context.WorkflowTriggerTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowTriggerTypes.Where(i => i.WorkflowTriggerTypeId == key);
                
                this.OnAfterWorkflowTriggerTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/WorkflowTriggerTypes(WorkflowTriggerTypeId={WorkflowTriggerTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchWorkflowTriggerType(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.WorkflowTriggerType> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.WorkflowTriggerTypes.Where(i => i.WorkflowTriggerTypeId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnWorkflowTriggerTypeUpdated(item);
                this.context.WorkflowTriggerTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowTriggerTypes.Where(i => i.WorkflowTriggerTypeId == key);
                
                this.OnAfterWorkflowTriggerTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWorkflowTriggerTypeCreated(CrownATTime.Server.Models.ATTime.WorkflowTriggerType item);
        partial void OnAfterWorkflowTriggerTypeCreated(CrownATTime.Server.Models.ATTime.WorkflowTriggerType item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.WorkflowTriggerType item)
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

                this.OnWorkflowTriggerTypeCreated(item);
                this.context.WorkflowTriggerTypes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WorkflowTriggerTypes.Where(i => i.WorkflowTriggerTypeId == item.WorkflowTriggerTypeId);

                

                this.OnAfterWorkflowTriggerTypeCreated(item);

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
