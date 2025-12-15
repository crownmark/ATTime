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
    [Route("odata/ATTime/ContractCaches")]
    public partial class ContractCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public ContractCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.ContractCache> GetContractCaches()
        {
            var items = this.context.ContractCaches.AsQueryable<CrownATTime.Server.Models.ATTime.ContractCache>();
            this.OnContractCachesRead(ref items);

            return items;
        }

        partial void OnContractCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.ContractCache> items);

        partial void OnContractCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.ContractCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/ContractCaches(Id={Id})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.ContractCache> GetContractCache(int key)
        {
            var items = this.context.ContractCaches.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnContractCacheGet(ref result);

            return result;
        }
        partial void OnContractCacheDeleted(CrownATTime.Server.Models.ATTime.ContractCache item);
        partial void OnAfterContractCacheDeleted(CrownATTime.Server.Models.ATTime.ContractCache item);

        [HttpDelete("/odata/ATTime/ContractCaches(Id={Id})")]
        public IActionResult DeleteContractCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.ContractCaches
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnContractCacheDeleted(item);
                this.context.ContractCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterContractCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnContractCacheUpdated(CrownATTime.Server.Models.ATTime.ContractCache item);
        partial void OnAfterContractCacheUpdated(CrownATTime.Server.Models.ATTime.ContractCache item);

        [HttpPut("/odata/ATTime/ContractCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutContractCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.ContractCache item)
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
                this.OnContractCacheUpdated(item);
                this.context.ContractCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ContractCaches.Where(i => i.Id == key);
                
                this.OnAfterContractCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/ContractCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchContractCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.ContractCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.ContractCaches.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnContractCacheUpdated(item);
                this.context.ContractCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ContractCaches.Where(i => i.Id == key);
                
                this.OnAfterContractCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnContractCacheCreated(CrownATTime.Server.Models.ATTime.ContractCache item);
        partial void OnAfterContractCacheCreated(CrownATTime.Server.Models.ATTime.ContractCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.ContractCache item)
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

                this.OnContractCacheCreated(item);
                this.context.ContractCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ContractCaches.Where(i => i.Id == item.Id);

                

                this.OnAfterContractCacheCreated(item);

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
