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
    [Route("odata/ATTime/RoleCaches")]
    public partial class RoleCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public RoleCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.RoleCache> GetRoleCaches()
        {
            var items = this.context.RoleCaches.AsQueryable<CrownATTime.Server.Models.ATTime.RoleCache>();
            this.OnRoleCachesRead(ref items);

            return items;
        }

        partial void OnRoleCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.RoleCache> items);

        partial void OnRoleCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.RoleCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/RoleCaches(Id={Id})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.RoleCache> GetRoleCache(int key)
        {
            var items = this.context.RoleCaches.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnRoleCacheGet(ref result);

            return result;
        }
        partial void OnRoleCacheDeleted(CrownATTime.Server.Models.ATTime.RoleCache item);
        partial void OnAfterRoleCacheDeleted(CrownATTime.Server.Models.ATTime.RoleCache item);

        [HttpDelete("/odata/ATTime/RoleCaches(Id={Id})")]
        public IActionResult DeleteRoleCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.RoleCaches
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnRoleCacheDeleted(item);
                this.context.RoleCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterRoleCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnRoleCacheUpdated(CrownATTime.Server.Models.ATTime.RoleCache item);
        partial void OnAfterRoleCacheUpdated(CrownATTime.Server.Models.ATTime.RoleCache item);

        [HttpPut("/odata/ATTime/RoleCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutRoleCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.RoleCache item)
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
                this.OnRoleCacheUpdated(item);
                this.context.RoleCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.RoleCaches.Where(i => i.Id == key);
                
                this.OnAfterRoleCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/RoleCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchRoleCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.RoleCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.RoleCaches.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnRoleCacheUpdated(item);
                this.context.RoleCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.RoleCaches.Where(i => i.Id == key);
                
                this.OnAfterRoleCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnRoleCacheCreated(CrownATTime.Server.Models.ATTime.RoleCache item);
        partial void OnAfterRoleCacheCreated(CrownATTime.Server.Models.ATTime.RoleCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.RoleCache item)
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

                this.OnRoleCacheCreated(item);
                this.context.RoleCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.RoleCaches.Where(i => i.Id == item.Id);

                

                this.OnAfterRoleCacheCreated(item);

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
