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

namespace CrownATTime.Server.Controllers.DistributorProducts
{
    [Route("odata/DistributorProducts/LenovoPsreffamilies")]
    public partial class LenovoPsreffamiliesController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public LenovoPsreffamiliesController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> GetLenovoPsreffamilies()
        {
            var items = this.context.LenovoPsreffamilies.AsQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>();
            this.OnLenovoPsreffamiliesRead(ref items);

            return items;
        }

        partial void OnLenovoPsreffamiliesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> items);

        partial void OnLenovoPsreffamilyGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/LenovoPsreffamilies(FamilyId={FamilyId})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> GetLenovoPsreffamily(long key)
        {
            var items = this.context.LenovoPsreffamilies.Where(i => i.FamilyId == key);
            var result = SingleResult.Create(items);

            OnLenovoPsreffamilyGet(ref result);

            return result;
        }
        partial void OnLenovoPsreffamilyDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);
        partial void OnAfterLenovoPsreffamilyDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);

        [HttpDelete("/odata/DistributorProducts/LenovoPsreffamilies(FamilyId={FamilyId})")]
        public IActionResult DeleteLenovoPsreffamily(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.LenovoPsreffamilies
                    .Where(i => i.FamilyId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnLenovoPsreffamilyDeleted(item);
                this.context.LenovoPsreffamilies.Remove(item);
                this.context.SaveChanges();
                this.OnAfterLenovoPsreffamilyDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsreffamilyUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);
        partial void OnAfterLenovoPsreffamilyUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);

        [HttpPut("/odata/DistributorProducts/LenovoPsreffamilies(FamilyId={FamilyId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutLenovoPsreffamily(long key, [FromBody]CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.FamilyId != key))
                {
                    return BadRequest();
                }
                this.OnLenovoPsreffamilyUpdated(item);
                this.context.LenovoPsreffamilies.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsreffamilies.Where(i => i.FamilyId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsrefedition");
                this.OnAfterLenovoPsreffamilyUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/LenovoPsreffamilies(FamilyId={FamilyId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchLenovoPsreffamily(long key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.LenovoPsreffamilies.Where(i => i.FamilyId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnLenovoPsreffamilyUpdated(item);
                this.context.LenovoPsreffamilies.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsreffamilies.Where(i => i.FamilyId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsrefedition");
                this.OnAfterLenovoPsreffamilyUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsreffamilyCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);
        partial void OnAfterLenovoPsreffamilyCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item)
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

                this.OnLenovoPsreffamilyCreated(item);
                this.context.LenovoPsreffamilies.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsreffamilies.Where(i => i.FamilyId == item.FamilyId);

                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsrefedition");

                this.OnAfterLenovoPsreffamilyCreated(item);

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
