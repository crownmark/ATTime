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
    [Route("odata/ATTime/BillingCodeCaches")]
    public partial class BillingCodeCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public BillingCodeCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.BillingCodeCache> GetBillingCodeCaches()
        {
            var items = this.context.BillingCodeCaches.AsQueryable<CrownATTime.Server.Models.ATTime.BillingCodeCache>();
            this.OnBillingCodeCachesRead(ref items);

            return items;
        }

        partial void OnBillingCodeCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.BillingCodeCache> items);

        partial void OnBillingCodeCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.BillingCodeCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/BillingCodeCaches(Id={Id})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.BillingCodeCache> GetBillingCodeCache(int key)
        {
            var items = this.context.BillingCodeCaches.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnBillingCodeCacheGet(ref result);

            return result;
        }
        partial void OnBillingCodeCacheDeleted(CrownATTime.Server.Models.ATTime.BillingCodeCache item);
        partial void OnAfterBillingCodeCacheDeleted(CrownATTime.Server.Models.ATTime.BillingCodeCache item);

        [HttpDelete("/odata/ATTime/BillingCodeCaches(Id={Id})")]
        public IActionResult DeleteBillingCodeCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.BillingCodeCaches
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnBillingCodeCacheDeleted(item);
                this.context.BillingCodeCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterBillingCodeCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBillingCodeCacheUpdated(CrownATTime.Server.Models.ATTime.BillingCodeCache item);
        partial void OnAfterBillingCodeCacheUpdated(CrownATTime.Server.Models.ATTime.BillingCodeCache item);

        [HttpPut("/odata/ATTime/BillingCodeCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutBillingCodeCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.BillingCodeCache item)
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
                this.OnBillingCodeCacheUpdated(item);
                this.context.BillingCodeCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.BillingCodeCaches.Where(i => i.Id == key);
                
                this.OnAfterBillingCodeCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/BillingCodeCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchBillingCodeCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.BillingCodeCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.BillingCodeCaches.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnBillingCodeCacheUpdated(item);
                this.context.BillingCodeCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.BillingCodeCaches.Where(i => i.Id == key);
                
                this.OnAfterBillingCodeCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBillingCodeCacheCreated(CrownATTime.Server.Models.ATTime.BillingCodeCache item);
        partial void OnAfterBillingCodeCacheCreated(CrownATTime.Server.Models.ATTime.BillingCodeCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.BillingCodeCache item)
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

                this.OnBillingCodeCacheCreated(item);
                this.context.BillingCodeCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.BillingCodeCaches.Where(i => i.Id == item.Id);

                

                this.OnAfterBillingCodeCacheCreated(item);

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
