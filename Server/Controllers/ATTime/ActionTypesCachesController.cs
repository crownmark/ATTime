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
    [Route("odata/ATTime/ActionTypesCaches")]
    public partial class ActionTypesCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public ActionTypesCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.ActionTypesCache> GetActionTypesCaches()
        {
            var items = this.context.ActionTypesCaches.AsQueryable<CrownATTime.Server.Models.ATTime.ActionTypesCache>();
            this.OnActionTypesCachesRead(ref items);

            return items;
        }

        partial void OnActionTypesCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.ActionTypesCache> items);

        partial void OnActionTypesCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.ActionTypesCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/ActionTypesCaches(Id={Id})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.ActionTypesCache> GetActionTypesCache(int key)
        {
            var items = this.context.ActionTypesCaches.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnActionTypesCacheGet(ref result);

            return result;
        }
        partial void OnActionTypesCacheDeleted(CrownATTime.Server.Models.ATTime.ActionTypesCache item);
        partial void OnAfterActionTypesCacheDeleted(CrownATTime.Server.Models.ATTime.ActionTypesCache item);

        [HttpDelete("/odata/ATTime/ActionTypesCaches(Id={Id})")]
        public IActionResult DeleteActionTypesCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.ActionTypesCaches
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnActionTypesCacheDeleted(item);
                this.context.ActionTypesCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterActionTypesCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnActionTypesCacheUpdated(CrownATTime.Server.Models.ATTime.ActionTypesCache item);
        partial void OnAfterActionTypesCacheUpdated(CrownATTime.Server.Models.ATTime.ActionTypesCache item);

        [HttpPut("/odata/ATTime/ActionTypesCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutActionTypesCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.ActionTypesCache item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Id != key))
                {
                    return BadRequest();
                }
                this.OnActionTypesCacheUpdated(item);
                this.context.ActionTypesCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ActionTypesCaches.Where(i => i.Id == key);
                
                this.OnAfterActionTypesCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/ActionTypesCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchActionTypesCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.ActionTypesCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.ActionTypesCaches.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnActionTypesCacheUpdated(item);
                this.context.ActionTypesCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ActionTypesCaches.Where(i => i.Id == key);
                
                this.OnAfterActionTypesCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnActionTypesCacheCreated(CrownATTime.Server.Models.ATTime.ActionTypesCache item);
        partial void OnAfterActionTypesCacheCreated(CrownATTime.Server.Models.ATTime.ActionTypesCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.ActionTypesCache item)
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

                this.OnActionTypesCacheCreated(item);
                this.context.ActionTypesCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ActionTypesCaches.Where(i => i.Id == item.Id);

                

                this.OnAfterActionTypesCacheCreated(item);

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
