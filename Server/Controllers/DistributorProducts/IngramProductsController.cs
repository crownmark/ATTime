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
    [Route("odata/DistributorProducts/IngramProducts")]
    public partial class IngramProductsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public IngramProductsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.IngramProduct> GetIngramProducts()
        {
            var items = this.context.IngramProducts.AsQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProduct>();
            this.OnIngramProductsRead(ref items);

            return items;
        }

        partial void OnIngramProductsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProduct> items);

        partial void OnIngramProductGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramProduct> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/IngramProducts(IngramPartNumber={IngramPartNumber})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramProduct> GetIngramProduct(string key)
        {
            var items = this.context.IngramProducts.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnIngramProductGet(ref result);

            return result;
        }
        partial void OnIngramProductDeleted(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);
        partial void OnAfterIngramProductDeleted(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);

        [HttpDelete("/odata/DistributorProducts/IngramProducts(IngramPartNumber={IngramPartNumber})")]
        public IActionResult DeleteIngramProduct(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.IngramProducts
                    .Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnIngramProductDeleted(item);
                this.context.IngramProducts.Remove(item);
                this.context.SaveChanges();
                this.OnAfterIngramProductDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramProductUpdated(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);
        partial void OnAfterIngramProductUpdated(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);

        [HttpPut("/odata/DistributorProducts/IngramProducts(IngramPartNumber={IngramPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutIngramProduct(string key, [FromBody]CrownATTime.Server.Models.DistributorProducts.IngramProduct item)
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
                this.OnIngramProductUpdated(item);
                this.context.IngramProducts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramProducts.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key));
                
                this.OnAfterIngramProductUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/IngramProducts(IngramPartNumber={IngramPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchIngramProduct(string key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.IngramProduct> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.IngramProducts.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnIngramProductUpdated(item);
                this.context.IngramProducts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramProducts.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(key));
                
                this.OnAfterIngramProductUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramProductCreated(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);
        partial void OnAfterIngramProductCreated(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.IngramProduct item)
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

                this.OnIngramProductCreated(item);
                this.context.IngramProducts.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramProducts.Where(i => i.IngramPartNumber == item.IngramPartNumber);

                

                this.OnAfterIngramProductCreated(item);

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
