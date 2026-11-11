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
    [Route("odata/DistributorProducts/IngramManufacturerContents")]
    public partial class IngramManufacturerContentsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public IngramManufacturerContentsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> GetIngramManufacturerContents()
        {
            var items = this.context.IngramManufacturerContents.AsQueryable<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>();
            this.OnIngramManufacturerContentsRead(ref items);

            return items;
        }

        partial void OnIngramManufacturerContentsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> items);

        partial void OnIngramManufacturerContentGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/IngramManufacturerContents(VendorName={keyVendorName},VendorPartNumber={keyVendorPartNumber})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> GetIngramManufacturerContent([FromODataUri] string keyVendorName, [FromODataUri] string keyVendorPartNumber)
        {
            var items = this.context.IngramManufacturerContents.Where(i => i.VendorName == Uri.UnescapeDataString(keyVendorName) && i.VendorPartNumber == Uri.UnescapeDataString(keyVendorPartNumber));
            var result = SingleResult.Create(items);

            OnIngramManufacturerContentGet(ref result);

            return result;
        }
        partial void OnIngramManufacturerContentDeleted(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);
        partial void OnAfterIngramManufacturerContentDeleted(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);

        [HttpDelete("/odata/DistributorProducts/IngramManufacturerContents(VendorName={keyVendorName},VendorPartNumber={keyVendorPartNumber})")]
        public IActionResult DeleteIngramManufacturerContent([FromODataUri] string keyVendorName, [FromODataUri] string keyVendorPartNumber)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.IngramManufacturerContents
                    .Where(i => i.VendorName == Uri.UnescapeDataString(keyVendorName) && i.VendorPartNumber == Uri.UnescapeDataString(keyVendorPartNumber))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnIngramManufacturerContentDeleted(item);
                this.context.IngramManufacturerContents.Remove(item);
                this.context.SaveChanges();
                this.OnAfterIngramManufacturerContentDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramManufacturerContentUpdated(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);
        partial void OnAfterIngramManufacturerContentUpdated(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);

        [HttpPut("/odata/DistributorProducts/IngramManufacturerContents(VendorName={keyVendorName},VendorPartNumber={keyVendorPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutIngramManufacturerContent([FromODataUri] string keyVendorName, [FromODataUri] string keyVendorPartNumber, [FromBody]CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.VendorName != Uri.UnescapeDataString(keyVendorName) && item.VendorPartNumber != Uri.UnescapeDataString(keyVendorPartNumber)))
                {
                    return BadRequest();
                }
                this.OnIngramManufacturerContentUpdated(item);
                this.context.IngramManufacturerContents.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramManufacturerContents.Where(i => i.VendorName == Uri.UnescapeDataString(keyVendorName) && i.VendorPartNumber == Uri.UnescapeDataString(keyVendorPartNumber));
                
                this.OnAfterIngramManufacturerContentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/IngramManufacturerContents(VendorName={keyVendorName},VendorPartNumber={keyVendorPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchIngramManufacturerContent([FromODataUri] string keyVendorName, [FromODataUri] string keyVendorPartNumber, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.IngramManufacturerContents.Where(i => i.VendorName == Uri.UnescapeDataString(keyVendorName) && i.VendorPartNumber == Uri.UnescapeDataString(keyVendorPartNumber)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnIngramManufacturerContentUpdated(item);
                this.context.IngramManufacturerContents.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramManufacturerContents.Where(i => i.VendorName == Uri.UnescapeDataString(keyVendorName) && i.VendorPartNumber == Uri.UnescapeDataString(keyVendorPartNumber));
                
                this.OnAfterIngramManufacturerContentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramManufacturerContentCreated(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);
        partial void OnAfterIngramManufacturerContentCreated(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item)
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

                this.OnIngramManufacturerContentCreated(item);
                this.context.IngramManufacturerContents.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramManufacturerContents.Where(i => i.VendorName == item.VendorName && i.VendorPartNumber == item.VendorPartNumber);

                

                this.OnAfterIngramManufacturerContentCreated(item);

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
