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
    [Route("odata/ATTime/ResourceCaches")]
    public partial class ResourceCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public ResourceCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.ResourceCache> GetResourceCaches()
        {
            var items = this.context.ResourceCaches.AsQueryable<CrownATTime.Server.Models.ATTime.ResourceCache>();
            this.OnResourceCachesRead(ref items);

            return items;
        }

        partial void OnResourceCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.ResourceCache> items);

        partial void OnResourceCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.ResourceCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/ResourceCaches(Id={Id})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.ResourceCache> GetResourceCache(int key)
        {
            var items = this.context.ResourceCaches.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnResourceCacheGet(ref result);

            return result;
        }
        partial void OnResourceCacheDeleted(CrownATTime.Server.Models.ATTime.ResourceCache item);
        partial void OnAfterResourceCacheDeleted(CrownATTime.Server.Models.ATTime.ResourceCache item);

        [HttpDelete("/odata/ATTime/ResourceCaches(Id={Id})")]
        public IActionResult DeleteResourceCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.ResourceCaches
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnResourceCacheDeleted(item);
                this.context.ResourceCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterResourceCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnResourceCacheUpdated(CrownATTime.Server.Models.ATTime.ResourceCache item);
        partial void OnAfterResourceCacheUpdated(CrownATTime.Server.Models.ATTime.ResourceCache item);

        [HttpPut("/odata/ATTime/ResourceCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutResourceCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.ResourceCache item)
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
                this.OnResourceCacheUpdated(item);
                this.context.ResourceCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ResourceCaches.Where(i => i.Id == key);
                
                this.OnAfterResourceCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/ResourceCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchResourceCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.ResourceCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.ResourceCaches.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnResourceCacheUpdated(item);
                this.context.ResourceCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ResourceCaches.Where(i => i.Id == key);
                
                this.OnAfterResourceCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnResourceCacheCreated(CrownATTime.Server.Models.ATTime.ResourceCache item);
        partial void OnAfterResourceCacheCreated(CrownATTime.Server.Models.ATTime.ResourceCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.ResourceCache item)
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

                this.OnResourceCacheCreated(item);
                this.context.ResourceCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ResourceCaches.Where(i => i.Id == item.Id);

                

                this.OnAfterResourceCacheCreated(item);

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
