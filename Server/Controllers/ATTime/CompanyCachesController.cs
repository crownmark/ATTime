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
    [Route("odata/ATTime/CompanyCaches")]
    public partial class CompanyCachesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public CompanyCachesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.CompanyCache> GetCompanyCaches()
        {
            var items = this.context.CompanyCaches.AsQueryable<CrownATTime.Server.Models.ATTime.CompanyCache>();
            this.OnCompanyCachesRead(ref items);

            return items;
        }

        partial void OnCompanyCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.CompanyCache> items);

        partial void OnCompanyCacheGet(ref SingleResult<CrownATTime.Server.Models.ATTime.CompanyCache> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/CompanyCaches(Id={Id})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.CompanyCache> GetCompanyCache(int key)
        {
            var items = this.context.CompanyCaches.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnCompanyCacheGet(ref result);

            return result;
        }
        partial void OnCompanyCacheDeleted(CrownATTime.Server.Models.ATTime.CompanyCache item);
        partial void OnAfterCompanyCacheDeleted(CrownATTime.Server.Models.ATTime.CompanyCache item);

        [HttpDelete("/odata/ATTime/CompanyCaches(Id={Id})")]
        public IActionResult DeleteCompanyCache(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.CompanyCaches
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCompanyCacheDeleted(item);
                this.context.CompanyCaches.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCompanyCacheDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCompanyCacheUpdated(CrownATTime.Server.Models.ATTime.CompanyCache item);
        partial void OnAfterCompanyCacheUpdated(CrownATTime.Server.Models.ATTime.CompanyCache item);

        [HttpPut("/odata/ATTime/CompanyCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCompanyCache(int key, [FromBody]CrownATTime.Server.Models.ATTime.CompanyCache item)
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
                this.OnCompanyCacheUpdated(item);
                this.context.CompanyCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompanyCaches.Where(i => i.Id == key);
                
                this.OnAfterCompanyCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/CompanyCaches(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCompanyCache(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.CompanyCache> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.CompanyCaches.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCompanyCacheUpdated(item);
                this.context.CompanyCaches.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompanyCaches.Where(i => i.Id == key);
                
                this.OnAfterCompanyCacheUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCompanyCacheCreated(CrownATTime.Server.Models.ATTime.CompanyCache item);
        partial void OnAfterCompanyCacheCreated(CrownATTime.Server.Models.ATTime.CompanyCache item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.CompanyCache item)
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

                this.OnCompanyCacheCreated(item);
                this.context.CompanyCaches.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CompanyCaches.Where(i => i.Id == item.Id);

                

                this.OnAfterCompanyCacheCreated(item);

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
