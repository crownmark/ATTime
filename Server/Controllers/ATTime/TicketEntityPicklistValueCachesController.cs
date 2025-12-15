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
    [Route("odata/ATTime/TicketEntityPicklistValueCaches")]
    public partial class TicketEntityPicklistValueCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public TicketEntityPicklistValueCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> GetTicketEntityPicklistValueCaches()
        {
            var items = this.context.TicketEntityPicklistValueCaches.AsQueryable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>();
            this.OnTicketEntityPicklistValueCachesRead(ref items);

            return items;
        }

        partial void OnTicketEntityPicklistValueCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> items);

        partial void OnTicketEntityPicklistValueCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/TicketEntityPicklistValueCaches(TicketEntityPicklistValueId={TicketEntityPicklistValueId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> GetTicketEntityPicklistValueCache(int key)
        {
            var items = this.context.TicketEntityPicklistValueCaches.Where(i => i.TicketEntityPicklistValueId == key);
            var result = SingleResult.Create(items);

            OnTicketEntityPicklistValueCacheGet(ref result);

            return result;
        }
        partial void OnTicketEntityPicklistValueCacheDeleted(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);
        partial void OnAfterTicketEntityPicklistValueCacheDeleted(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);

        [HttpDelete("/odata/ATTime/TicketEntityPicklistValueCaches(TicketEntityPicklistValueId={TicketEntityPicklistValueId})")]
        public IActionResult DeleteTicketEntityPicklistValueCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TicketEntityPicklistValueCaches
                    .Where(i => i.TicketEntityPicklistValueId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTicketEntityPicklistValueCacheDeleted(item);
                this.context.TicketEntityPicklistValueCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTicketEntityPicklistValueCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTicketEntityPicklistValueCacheUpdated(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);
        partial void OnAfterTicketEntityPicklistValueCacheUpdated(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);

        [HttpPut("/odata/ATTime/TicketEntityPicklistValueCaches(TicketEntityPicklistValueId={TicketEntityPicklistValueId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTicketEntityPicklistValueCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.TicketEntityPicklistValueId != key))
                {
                    return BadRequest();
                }
                this.OnTicketEntityPicklistValueCacheUpdated(item);
                this.context.TicketEntityPicklistValueCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TicketEntityPicklistValueCaches.Where(i => i.TicketEntityPicklistValueId == key);
                
                this.OnAfterTicketEntityPicklistValueCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/TicketEntityPicklistValueCaches(TicketEntityPicklistValueId={TicketEntityPicklistValueId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTicketEntityPicklistValueCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TicketEntityPicklistValueCaches.Where(i => i.TicketEntityPicklistValueId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTicketEntityPicklistValueCacheUpdated(item);
                this.context.TicketEntityPicklistValueCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TicketEntityPicklistValueCaches.Where(i => i.TicketEntityPicklistValueId == key);
                
                this.OnAfterTicketEntityPicklistValueCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTicketEntityPicklistValueCacheCreated(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);
        partial void OnAfterTicketEntityPicklistValueCacheCreated(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item)
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

                this.OnTicketEntityPicklistValueCacheCreated(item);
                this.context.TicketEntityPicklistValueCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TicketEntityPicklistValueCaches.Where(i => i.TicketEntityPicklistValueId == item.TicketEntityPicklistValueId);

                

                this.OnAfterTicketEntityPicklistValueCacheCreated(item);

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
