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
    [Route("odata/ATTime/ClickEventActions")]
    public partial class ClickEventActionsController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public ClickEventActionsController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.ClickEventAction> GetClickEventActions()
        {
            var items = this.context.ClickEventActions.AsQueryable<CrownATTime.Server.Models.ATTime.ClickEventAction>();
            this.OnClickEventActionsRead(ref items);

            return items;
        }

        partial void OnClickEventActionsRead(ref IQueryable<CrownATTime.Server.Models.ATTime.ClickEventAction> items);

        partial void OnClickEventActionGet(ref SingleResult<CrownATTime.Server.Models.ATTime.ClickEventAction> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/ClickEventActions(ClickEventActionId={ClickEventActionId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.ClickEventAction> GetClickEventAction(int key)
        {
            var items = this.context.ClickEventActions.Where(i => i.ClickEventActionId == key);
            var result = SingleResult.Create(items);

            OnClickEventActionGet(ref result);

            return result;
        }
        partial void OnClickEventActionDeleted(CrownATTime.Server.Models.ATTime.ClickEventAction item);
        partial void OnAfterClickEventActionDeleted(CrownATTime.Server.Models.ATTime.ClickEventAction item);

        [HttpDelete("/odata/ATTime/ClickEventActions(ClickEventActionId={ClickEventActionId})")]
        public IActionResult DeleteClickEventAction(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.ClickEventActions
                    .Where(i => i.ClickEventActionId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnClickEventActionDeleted(item);
                this.context.ClickEventActions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterClickEventActionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnClickEventActionUpdated(CrownATTime.Server.Models.ATTime.ClickEventAction item);
        partial void OnAfterClickEventActionUpdated(CrownATTime.Server.Models.ATTime.ClickEventAction item);

        [HttpPut("/odata/ATTime/ClickEventActions(ClickEventActionId={ClickEventActionId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutClickEventAction(int key, [FromBody]CrownATTime.Server.Models.ATTime.ClickEventAction item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ClickEventActionId != key))
                {
                    return BadRequest();
                }
                this.OnClickEventActionUpdated(item);
                this.context.ClickEventActions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClickEventActions.Where(i => i.ClickEventActionId == key);
                
                this.OnAfterClickEventActionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/ClickEventActions(ClickEventActionId={ClickEventActionId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchClickEventAction(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.ClickEventAction> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.ClickEventActions.Where(i => i.ClickEventActionId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnClickEventActionUpdated(item);
                this.context.ClickEventActions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClickEventActions.Where(i => i.ClickEventActionId == key);
                
                this.OnAfterClickEventActionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnClickEventActionCreated(CrownATTime.Server.Models.ATTime.ClickEventAction item);
        partial void OnAfterClickEventActionCreated(CrownATTime.Server.Models.ATTime.ClickEventAction item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.ClickEventAction item)
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

                this.OnClickEventActionCreated(item);
                this.context.ClickEventActions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ClickEventActions.Where(i => i.ClickEventActionId == item.ClickEventActionId);

                

                this.OnAfterClickEventActionCreated(item);

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
