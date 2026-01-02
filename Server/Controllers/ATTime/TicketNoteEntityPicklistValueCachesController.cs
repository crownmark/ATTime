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
    [Route("odata/ATTime/TicketNoteEntityPicklistValueCaches")]
    public partial class TicketNoteEntityPicklistValueCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public TicketNoteEntityPicklistValueCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> GetTicketNoteEntityPicklistValueCaches()
        {
            var items = this.context.TicketNoteEntityPicklistValueCaches.AsQueryable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>();
            this.OnTicketNoteEntityPicklistValueCachesRead(ref items);

            return items;
        }

        partial void OnTicketNoteEntityPicklistValueCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> items);

        partial void OnTicketNoteEntityPicklistValueCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/TicketNoteEntityPicklistValueCaches(TicketNoteEntityPicklistValueId={TicketNoteEntityPicklistValueId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> GetTicketNoteEntityPicklistValueCache(int key)
        {
            var items = this.context.TicketNoteEntityPicklistValueCaches.Where(i => i.TicketNoteEntityPicklistValueId == key);
            var result = SingleResult.Create(items);

            OnTicketNoteEntityPicklistValueCacheGet(ref result);

            return result;
        }
        partial void OnTicketNoteEntityPicklistValueCacheDeleted(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);
        partial void OnAfterTicketNoteEntityPicklistValueCacheDeleted(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);

        [HttpDelete("/odata/ATTime/TicketNoteEntityPicklistValueCaches(TicketNoteEntityPicklistValueId={TicketNoteEntityPicklistValueId})")]
        public IActionResult DeleteTicketNoteEntityPicklistValueCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TicketNoteEntityPicklistValueCaches
                    .Where(i => i.TicketNoteEntityPicklistValueId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTicketNoteEntityPicklistValueCacheDeleted(item);
                this.context.TicketNoteEntityPicklistValueCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTicketNoteEntityPicklistValueCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTicketNoteEntityPicklistValueCacheUpdated(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);
        partial void OnAfterTicketNoteEntityPicklistValueCacheUpdated(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);

        [HttpPut("/odata/ATTime/TicketNoteEntityPicklistValueCaches(TicketNoteEntityPicklistValueId={TicketNoteEntityPicklistValueId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTicketNoteEntityPicklistValueCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.TicketNoteEntityPicklistValueId != key))
                {
                    return BadRequest();
                }
                this.OnTicketNoteEntityPicklistValueCacheUpdated(item);
                this.context.TicketNoteEntityPicklistValueCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TicketNoteEntityPicklistValueCaches.Where(i => i.TicketNoteEntityPicklistValueId == key);
                
                this.OnAfterTicketNoteEntityPicklistValueCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/TicketNoteEntityPicklistValueCaches(TicketNoteEntityPicklistValueId={TicketNoteEntityPicklistValueId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTicketNoteEntityPicklistValueCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TicketNoteEntityPicklistValueCaches.Where(i => i.TicketNoteEntityPicklistValueId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTicketNoteEntityPicklistValueCacheUpdated(item);
                this.context.TicketNoteEntityPicklistValueCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TicketNoteEntityPicklistValueCaches.Where(i => i.TicketNoteEntityPicklistValueId == key);
                
                this.OnAfterTicketNoteEntityPicklistValueCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTicketNoteEntityPicklistValueCacheCreated(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);
        partial void OnAfterTicketNoteEntityPicklistValueCacheCreated(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item)
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

                this.OnTicketNoteEntityPicklistValueCacheCreated(item);
                this.context.TicketNoteEntityPicklistValueCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TicketNoteEntityPicklistValueCaches.Where(i => i.TicketNoteEntityPicklistValueId == item.TicketNoteEntityPicklistValueId);

                

                this.OnAfterTicketNoteEntityPicklistValueCacheCreated(item);

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
