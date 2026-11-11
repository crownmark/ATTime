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
    [Route("odata/DistributorProducts/IngramLenovoFamilyImages")]
    public partial class IngramLenovoFamilyImagesController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public IngramLenovoFamilyImagesController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> GetIngramLenovoFamilyImages()
        {
            var items = this.context.IngramLenovoFamilyImages.AsQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>();
            this.OnIngramLenovoFamilyImagesRead(ref items);

            return items;
        }

        partial void OnIngramLenovoFamilyImagesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> items);

        partial void OnIngramLenovoFamilyImageGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/IngramLenovoFamilyImages(MachineType={MachineType})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> GetIngramLenovoFamilyImage(string key)
        {
            var items = this.context.IngramLenovoFamilyImages.Where(i => i.MachineType == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnIngramLenovoFamilyImageGet(ref result);

            return result;
        }
        partial void OnIngramLenovoFamilyImageDeleted(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);
        partial void OnAfterIngramLenovoFamilyImageDeleted(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);

        [HttpDelete("/odata/DistributorProducts/IngramLenovoFamilyImages(MachineType={MachineType})")]
        public IActionResult DeleteIngramLenovoFamilyImage(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.IngramLenovoFamilyImages
                    .Where(i => i.MachineType == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnIngramLenovoFamilyImageDeleted(item);
                this.context.IngramLenovoFamilyImages.Remove(item);
                this.context.SaveChanges();
                this.OnAfterIngramLenovoFamilyImageDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramLenovoFamilyImageUpdated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);
        partial void OnAfterIngramLenovoFamilyImageUpdated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);

        [HttpPut("/odata/DistributorProducts/IngramLenovoFamilyImages(MachineType={MachineType})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutIngramLenovoFamilyImage(string key, [FromBody]CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.MachineType != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnIngramLenovoFamilyImageUpdated(item);
                this.context.IngramLenovoFamilyImages.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramLenovoFamilyImages.Where(i => i.MachineType == Uri.UnescapeDataString(key));
                
                this.OnAfterIngramLenovoFamilyImageUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/IngramLenovoFamilyImages(MachineType={MachineType})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchIngramLenovoFamilyImage(string key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.IngramLenovoFamilyImages.Where(i => i.MachineType == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnIngramLenovoFamilyImageUpdated(item);
                this.context.IngramLenovoFamilyImages.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramLenovoFamilyImages.Where(i => i.MachineType == Uri.UnescapeDataString(key));
                
                this.OnAfterIngramLenovoFamilyImageUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramLenovoFamilyImageCreated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);
        partial void OnAfterIngramLenovoFamilyImageCreated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item)
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

                this.OnIngramLenovoFamilyImageCreated(item);
                this.context.IngramLenovoFamilyImages.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramLenovoFamilyImages.Where(i => i.MachineType == item.MachineType);

                

                this.OnAfterIngramLenovoFamilyImageCreated(item);

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
