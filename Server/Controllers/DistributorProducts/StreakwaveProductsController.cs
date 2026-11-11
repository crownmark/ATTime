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
    [Route("odata/DistributorProducts/StreakwaveProducts")]
    public partial class StreakwaveProductsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public StreakwaveProductsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> GetStreakwaveProducts()
        {
            var items = this.context.StreakwaveProducts.AsQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>();
            this.OnStreakwaveProductsRead(ref items);

            return items;
        }

        partial void OnStreakwaveProductsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> items);

        partial void OnStreakwaveProductGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/StreakwaveProducts(Sku={Sku})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> GetStreakwaveProduct(string key)
        {
            var items = this.context.StreakwaveProducts.Where(i => i.Sku == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnStreakwaveProductGet(ref result);

            return result;
        }
        partial void OnStreakwaveProductDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);
        partial void OnAfterStreakwaveProductDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);

        [HttpDelete("/odata/DistributorProducts/StreakwaveProducts(Sku={Sku})")]
        public IActionResult DeleteStreakwaveProduct(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.StreakwaveProducts
                    .Where(i => i.Sku == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnStreakwaveProductDeleted(item);
                this.context.StreakwaveProducts.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStreakwaveProductDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStreakwaveProductUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);
        partial void OnAfterStreakwaveProductUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);

        [HttpPut("/odata/DistributorProducts/StreakwaveProducts(Sku={Sku})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStreakwaveProduct(string key, [FromBody]CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item)
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
                this.OnStreakwaveProductUpdated(item);
                this.context.StreakwaveProducts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveProducts.Where(i => i.Sku == Uri.UnescapeDataString(key));
                
                this.OnAfterStreakwaveProductUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/StreakwaveProducts(Sku={Sku})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStreakwaveProduct(string key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.StreakwaveProducts.Where(i => i.Sku == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnStreakwaveProductUpdated(item);
                this.context.StreakwaveProducts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveProducts.Where(i => i.Sku == Uri.UnescapeDataString(key));
                
                this.OnAfterStreakwaveProductUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStreakwaveProductCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);
        partial void OnAfterStreakwaveProductCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item)
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

                this.OnStreakwaveProductCreated(item);
                this.context.StreakwaveProducts.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveProducts.Where(i => i.Sku == item.Sku);

                

                this.OnAfterStreakwaveProductCreated(item);

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
