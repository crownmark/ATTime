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
    [Route("odata/DistributorProducts/IngramProductEnrichments")]
    public partial class IngramProductEnrichmentsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public IngramProductEnrichmentsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> GetIngramProductEnrichments()
        {
            var items = this.context.IngramProductEnrichments.AsQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>();
            this.OnIngramProductEnrichmentsRead(ref items);

            return items;
        }

        partial void OnIngramProductEnrichmentsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> items);

        partial void OnIngramProductEnrichmentGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/IngramProductEnrichments(IngramPartNumber={IngramPartNumber})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> GetIngramProductEnrichment(string key)
        {
            var items = this.context.IngramProductEnrichments.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnIngramProductEnrichmentGet(ref result);

            return result;
        }
        partial void OnIngramProductEnrichmentDeleted(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);
        partial void OnAfterIngramProductEnrichmentDeleted(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);

        [HttpDelete("/odata/DistributorProducts/IngramProductEnrichments(IngramPartNumber={IngramPartNumber})")]
        public IActionResult DeleteIngramProductEnrichment(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.IngramProductEnrichments
                    .Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnIngramProductEnrichmentDeleted(item);
                this.context.IngramProductEnrichments.Remove(item);
                this.context.SaveChanges();
                this.OnAfterIngramProductEnrichmentDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramProductEnrichmentUpdated(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);
        partial void OnAfterIngramProductEnrichmentUpdated(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);

        [HttpPut("/odata/DistributorProducts/IngramProductEnrichments(IngramPartNumber={IngramPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutIngramProductEnrichment(string key, [FromBody]CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.IngramPartNumber != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnIngramProductEnrichmentUpdated(item);
                this.context.IngramProductEnrichments.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramProductEnrichments.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "IngramProduct");
                this.OnAfterIngramProductEnrichmentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/IngramProductEnrichments(IngramPartNumber={IngramPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchIngramProductEnrichment(string key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.IngramProductEnrichments.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnIngramProductEnrichmentUpdated(item);
                this.context.IngramProductEnrichments.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramProductEnrichments.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "IngramProduct");
                this.OnAfterIngramProductEnrichmentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramProductEnrichmentCreated(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);
        partial void OnAfterIngramProductEnrichmentCreated(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item)
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

                this.OnIngramProductEnrichmentCreated(item);
                this.context.IngramProductEnrichments.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramProductEnrichments.Where(i => i.IngramPartNumber == item.IngramPartNumber);

                Request.QueryString = Request.QueryString.Add("$expand", "IngramProduct");

                this.OnAfterIngramProductEnrichmentCreated(item);

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
