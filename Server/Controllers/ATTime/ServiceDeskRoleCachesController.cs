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
    [Route("odata/ATTime/ServiceDeskRoleCaches")]
    public partial class ServiceDeskRoleCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public ServiceDeskRoleCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> GetServiceDeskRoleCaches()
        {
            var items = this.context.ServiceDeskRoleCaches.AsQueryable<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>();
            this.OnServiceDeskRoleCachesRead(ref items);

            return items;
        }

        partial void OnServiceDeskRoleCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> items);

        partial void OnServiceDeskRoleCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/ServiceDeskRoleCaches(Id={Id})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> GetServiceDeskRoleCache(int key)
        {
            var items = this.context.ServiceDeskRoleCaches.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnServiceDeskRoleCacheGet(ref result);

            return result;
        }
        partial void OnServiceDeskRoleCacheDeleted(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);
        partial void OnAfterServiceDeskRoleCacheDeleted(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);

        [HttpDelete("/odata/ATTime/ServiceDeskRoleCaches(Id={Id})")]
        public IActionResult DeleteServiceDeskRoleCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.ServiceDeskRoleCaches
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnServiceDeskRoleCacheDeleted(item);
                this.context.ServiceDeskRoleCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterServiceDeskRoleCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnServiceDeskRoleCacheUpdated(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);
        partial void OnAfterServiceDeskRoleCacheUpdated(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);

        [HttpPut("/odata/ATTime/ServiceDeskRoleCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutServiceDeskRoleCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item)
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
                this.OnServiceDeskRoleCacheUpdated(item);
                this.context.ServiceDeskRoleCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ServiceDeskRoleCaches.Where(i => i.Id == key);
                
                this.OnAfterServiceDeskRoleCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/ServiceDeskRoleCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchServiceDeskRoleCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.ServiceDeskRoleCaches.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnServiceDeskRoleCacheUpdated(item);
                this.context.ServiceDeskRoleCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ServiceDeskRoleCaches.Where(i => i.Id == key);
                
                this.OnAfterServiceDeskRoleCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnServiceDeskRoleCacheCreated(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);
        partial void OnAfterServiceDeskRoleCacheCreated(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item)
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

                this.OnServiceDeskRoleCacheCreated(item);
                this.context.ServiceDeskRoleCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ServiceDeskRoleCaches.Where(i => i.Id == item.Id);

                

                this.OnAfterServiceDeskRoleCacheCreated(item);

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
