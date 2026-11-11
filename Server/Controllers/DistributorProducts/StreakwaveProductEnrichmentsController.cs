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
    [Route("odata/DistributorProducts/StreakwaveProductEnrichments")]
    public partial class StreakwaveProductEnrichmentsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public StreakwaveProductEnrichmentsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> GetStreakwaveProductEnrichments()
        {
            var items = this.context.StreakwaveProductEnrichments.AsQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>();
            this.OnStreakwaveProductEnrichmentsRead(ref items);

            return items;
        }

        partial void OnStreakwaveProductEnrichmentsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> items);

        partial void OnStreakwaveProductEnrichmentGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/StreakwaveProductEnrichments(Sku={Sku})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> GetStreakwaveProductEnrichment(string key)
        {
            var items = this.context.StreakwaveProductEnrichments.Where(i => i.Sku == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnStreakwaveProductEnrichmentGet(ref result);

            return result;
        }
        partial void OnStreakwaveProductEnrichmentDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);
        partial void OnAfterStreakwaveProductEnrichmentDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);

        [HttpDelete("/odata/DistributorProducts/StreakwaveProductEnrichments(Sku={Sku})")]
        public IActionResult DeleteStreakwaveProductEnrichment(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.StreakwaveProductEnrichments
                    .Where(i => i.Sku == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnStreakwaveProductEnrichmentDeleted(item);
                this.context.StreakwaveProductEnrichments.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStreakwaveProductEnrichmentDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStreakwaveProductEnrichmentUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);
        partial void OnAfterStreakwaveProductEnrichmentUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);

        [HttpPut("/odata/DistributorProducts/StreakwaveProductEnrichments(Sku={Sku})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStreakwaveProductEnrichment(string key, [FromBody]CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Sku != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnStreakwaveProductEnrichmentUpdated(item);
                this.context.StreakwaveProductEnrichments.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveProductEnrichments.Where(i => i.Sku == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "StreakwaveProduct");
                this.OnAfterStreakwaveProductEnrichmentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/StreakwaveProductEnrichments(Sku={Sku})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStreakwaveProductEnrichment(string key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.StreakwaveProductEnrichments.Where(i => i.Sku == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnStreakwaveProductEnrichmentUpdated(item);
                this.context.StreakwaveProductEnrichments.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveProductEnrichments.Where(i => i.Sku == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "StreakwaveProduct");
                this.OnAfterStreakwaveProductEnrichmentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStreakwaveProductEnrichmentCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);
        partial void OnAfterStreakwaveProductEnrichmentCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item)
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

                this.OnStreakwaveProductEnrichmentCreated(item);
                this.context.StreakwaveProductEnrichments.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveProductEnrichments.Where(i => i.Sku == item.Sku);

                Request.QueryString = Request.QueryString.Add("$expand", "StreakwaveProduct");

                this.OnAfterStreakwaveProductEnrichmentCreated(item);

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
