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
    [Route("odata/DistributorProducts/IngramWarehouseInventories")]
    public partial class IngramWarehouseInventoriesController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public IngramWarehouseInventoriesController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> GetIngramWarehouseInventories()
        {
            var items = this.context.IngramWarehouseInventories.AsQueryable<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>();
            this.OnIngramWarehouseInventoriesRead(ref items);

            return items;
        }

        partial void OnIngramWarehouseInventoriesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> items);

        partial void OnIngramWarehouseInventoryGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/IngramWarehouseInventories(IngramPartNumber={keyIngramPartNumber},WarehouseLocation={keyWarehouseLocation})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> GetIngramWarehouseInventory([FromODataUri] string keyIngramPartNumber, [FromODataUri] string keyWarehouseLocation)
        {
            var items = this.context.IngramWarehouseInventories.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(keyIngramPartNumber) && i.WarehouseLocation == Uri.UnescapeDataString(keyWarehouseLocation));
            var result = SingleResult.Create(items);

            OnIngramWarehouseInventoryGet(ref result);

            return result;
        }
        partial void OnIngramWarehouseInventoryDeleted(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);
        partial void OnAfterIngramWarehouseInventoryDeleted(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);

        [HttpDelete("/odata/DistributorProducts/IngramWarehouseInventories(IngramPartNumber={keyIngramPartNumber},WarehouseLocation={keyWarehouseLocation})")]
        public IActionResult DeleteIngramWarehouseInventory([FromODataUri] string keyIngramPartNumber, [FromODataUri] string keyWarehouseLocation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.IngramWarehouseInventories
                    .Where(i => i.IngramPartNumber == Uri.UnescapeDataString(keyIngramPartNumber) && i.WarehouseLocation == Uri.UnescapeDataString(keyWarehouseLocation))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnIngramWarehouseInventoryDeleted(item);
                this.context.IngramWarehouseInventories.Remove(item);
                this.context.SaveChanges();
                this.OnAfterIngramWarehouseInventoryDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramWarehouseInventoryUpdated(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);
        partial void OnAfterIngramWarehouseInventoryUpdated(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);

        [HttpPut("/odata/DistributorProducts/IngramWarehouseInventories(IngramPartNumber={keyIngramPartNumber},WarehouseLocation={keyWarehouseLocation})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutIngramWarehouseInventory([FromODataUri] string keyIngramPartNumber, [FromODataUri] string keyWarehouseLocation, [FromBody]CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.IngramPartNumber != Uri.UnescapeDataString(keyIngramPartNumber) && item.WarehouseLocation != Uri.UnescapeDataString(keyWarehouseLocation)))
                {
                    return BadRequest();
                }
                this.OnIngramWarehouseInventoryUpdated(item);
                this.context.IngramWarehouseInventories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramWarehouseInventories.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(keyIngramPartNumber) && i.WarehouseLocation == Uri.UnescapeDataString(keyWarehouseLocation));
                Request.QueryString = Request.QueryString.Add("$expand", "IngramProduct");
                this.OnAfterIngramWarehouseInventoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/IngramWarehouseInventories(IngramPartNumber={keyIngramPartNumber},WarehouseLocation={keyWarehouseLocation})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchIngramWarehouseInventory([FromODataUri] string keyIngramPartNumber, [FromODataUri] string keyWarehouseLocation, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.IngramWarehouseInventories.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(keyIngramPartNumber) && i.WarehouseLocation == Uri.UnescapeDataString(keyWarehouseLocation)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnIngramWarehouseInventoryUpdated(item);
                this.context.IngramWarehouseInventories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramWarehouseInventories.Where(i => i.IngramPartNumber == Uri.UnescapeDataString(keyIngramPartNumber) && i.WarehouseLocation == Uri.UnescapeDataString(keyWarehouseLocation));
                Request.QueryString = Request.QueryString.Add("$expand", "IngramProduct");
                this.OnAfterIngramWarehouseInventoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramWarehouseInventoryCreated(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);
        partial void OnAfterIngramWarehouseInventoryCreated(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item)
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

                this.OnIngramWarehouseInventoryCreated(item);
                this.context.IngramWarehouseInventories.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramWarehouseInventories.Where(i => i.IngramPartNumber == item.IngramPartNumber && i.WarehouseLocation == item.WarehouseLocation);

                Request.QueryString = Request.QueryString.Add("$expand", "IngramProduct");

                this.OnAfterIngramWarehouseInventoryCreated(item);

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
