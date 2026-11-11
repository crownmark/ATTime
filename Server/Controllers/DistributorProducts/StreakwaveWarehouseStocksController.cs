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
    [Route("odata/DistributorProducts/StreakwaveWarehouseStocks")]
    public partial class StreakwaveWarehouseStocksController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public StreakwaveWarehouseStocksController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> GetStreakwaveWarehouseStocks()
        {
            var items = this.context.StreakwaveWarehouseStocks.AsQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>();
            this.OnStreakwaveWarehouseStocksRead(ref items);

            return items;
        }

        partial void OnStreakwaveWarehouseStocksRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> items);

        partial void OnStreakwaveWarehouseStockGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/StreakwaveWarehouseStocks(Sku={keySku},WarehouseCode={keyWarehouseCode})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> GetStreakwaveWarehouseStock([FromODataUri] string keySku, [FromODataUri] string keyWarehouseCode)
        {
            var items = this.context.StreakwaveWarehouseStocks.Where(i => i.Sku == Uri.UnescapeDataString(keySku) && i.WarehouseCode == Uri.UnescapeDataString(keyWarehouseCode));
            var result = SingleResult.Create(items);

            OnStreakwaveWarehouseStockGet(ref result);

            return result;
        }
        partial void OnStreakwaveWarehouseStockDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);
        partial void OnAfterStreakwaveWarehouseStockDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);

        [HttpDelete("/odata/DistributorProducts/StreakwaveWarehouseStocks(Sku={keySku},WarehouseCode={keyWarehouseCode})")]
        public IActionResult DeleteStreakwaveWarehouseStock([FromODataUri] string keySku, [FromODataUri] string keyWarehouseCode)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.StreakwaveWarehouseStocks
                    .Where(i => i.Sku == Uri.UnescapeDataString(keySku) && i.WarehouseCode == Uri.UnescapeDataString(keyWarehouseCode))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnStreakwaveWarehouseStockDeleted(item);
                this.context.StreakwaveWarehouseStocks.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStreakwaveWarehouseStockDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStreakwaveWarehouseStockUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);
        partial void OnAfterStreakwaveWarehouseStockUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);

        [HttpPut("/odata/DistributorProducts/StreakwaveWarehouseStocks(Sku={keySku},WarehouseCode={keyWarehouseCode})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStreakwaveWarehouseStock([FromODataUri] string keySku, [FromODataUri] string keyWarehouseCode, [FromBody]CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Sku != Uri.UnescapeDataString(keySku) && item.WarehouseCode != Uri.UnescapeDataString(keyWarehouseCode)))
                {
                    return BadRequest();
                }
                this.OnStreakwaveWarehouseStockUpdated(item);
                this.context.StreakwaveWarehouseStocks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveWarehouseStocks.Where(i => i.Sku == Uri.UnescapeDataString(keySku) && i.WarehouseCode == Uri.UnescapeDataString(keyWarehouseCode));
                Request.QueryString = Request.QueryString.Add("$expand", "StreakwaveProduct");
                this.OnAfterStreakwaveWarehouseStockUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/StreakwaveWarehouseStocks(Sku={keySku},WarehouseCode={keyWarehouseCode})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStreakwaveWarehouseStock([FromODataUri] string keySku, [FromODataUri] string keyWarehouseCode, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.StreakwaveWarehouseStocks.Where(i => i.Sku == Uri.UnescapeDataString(keySku) && i.WarehouseCode == Uri.UnescapeDataString(keyWarehouseCode)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnStreakwaveWarehouseStockUpdated(item);
                this.context.StreakwaveWarehouseStocks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveWarehouseStocks.Where(i => i.Sku == Uri.UnescapeDataString(keySku) && i.WarehouseCode == Uri.UnescapeDataString(keyWarehouseCode));
                Request.QueryString = Request.QueryString.Add("$expand", "StreakwaveProduct");
                this.OnAfterStreakwaveWarehouseStockUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStreakwaveWarehouseStockCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);
        partial void OnAfterStreakwaveWarehouseStockCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item)
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

                this.OnStreakwaveWarehouseStockCreated(item);
                this.context.StreakwaveWarehouseStocks.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveWarehouseStocks.Where(i => i.Sku == item.Sku && i.WarehouseCode == item.WarehouseCode);

                Request.QueryString = Request.QueryString.Add("$expand", "StreakwaveProduct");

                this.OnAfterStreakwaveWarehouseStockCreated(item);

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
