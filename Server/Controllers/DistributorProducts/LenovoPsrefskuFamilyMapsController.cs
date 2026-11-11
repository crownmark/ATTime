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
    [Route("odata/DistributorProducts/LenovoPsrefskuFamilyMaps")]
    public partial class LenovoPsrefskuFamilyMapsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public LenovoPsrefskuFamilyMapsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> GetLenovoPsrefskuFamilyMaps()
        {
            var items = this.context.LenovoPsrefskuFamilyMaps.AsQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>();
            this.OnLenovoPsrefskuFamilyMapsRead(ref items);

            return items;
        }

        partial void OnLenovoPsrefskuFamilyMapsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> items);

        partial void OnLenovoPsrefskuFamilyMapGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/LenovoPsrefskuFamilyMaps(VendorPartNumber={VendorPartNumber})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> GetLenovoPsrefskuFamilyMap(string key)
        {
            var items = this.context.LenovoPsrefskuFamilyMaps.Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnLenovoPsrefskuFamilyMapGet(ref result);

            return result;
        }
        partial void OnLenovoPsrefskuFamilyMapDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);
        partial void OnAfterLenovoPsrefskuFamilyMapDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);

        [HttpDelete("/odata/DistributorProducts/LenovoPsrefskuFamilyMaps(VendorPartNumber={VendorPartNumber})")]
        public IActionResult DeleteLenovoPsrefskuFamilyMap(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.LenovoPsrefskuFamilyMaps
                    .Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnLenovoPsrefskuFamilyMapDeleted(item);
                this.context.LenovoPsrefskuFamilyMaps.Remove(item);
                this.context.SaveChanges();
                this.OnAfterLenovoPsrefskuFamilyMapDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsrefskuFamilyMapUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);
        partial void OnAfterLenovoPsrefskuFamilyMapUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);

        [HttpPut("/odata/DistributorProducts/LenovoPsrefskuFamilyMaps(VendorPartNumber={VendorPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutLenovoPsrefskuFamilyMap(string key, [FromBody]CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.VendorPartNumber != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnLenovoPsrefskuFamilyMapUpdated(item);
                this.context.LenovoPsrefskuFamilyMaps.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefskuFamilyMaps.Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key));
                
                this.OnAfterLenovoPsrefskuFamilyMapUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/LenovoPsrefskuFamilyMaps(VendorPartNumber={VendorPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchLenovoPsrefskuFamilyMap(string key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.LenovoPsrefskuFamilyMaps.Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnLenovoPsrefskuFamilyMapUpdated(item);
                this.context.LenovoPsrefskuFamilyMaps.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefskuFamilyMaps.Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key));
                
                this.OnAfterLenovoPsrefskuFamilyMapUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsrefskuFamilyMapCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);
        partial void OnAfterLenovoPsrefskuFamilyMapCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item)
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

                this.OnLenovoPsrefskuFamilyMapCreated(item);
                this.context.LenovoPsrefskuFamilyMaps.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefskuFamilyMaps.Where(i => i.VendorPartNumber == item.VendorPartNumber);

                

                this.OnAfterLenovoPsrefskuFamilyMapCreated(item);

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
