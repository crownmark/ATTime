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
    [Route("odata/DistributorProducts/LenovoPsreffamilyImages")]
    public partial class LenovoPsreffamilyImagesController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public LenovoPsreffamilyImagesController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> GetLenovoPsreffamilyImages()
        {
            var items = this.context.LenovoPsreffamilyImages.AsQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>();
            this.OnLenovoPsreffamilyImagesRead(ref items);

            return items;
        }

        partial void OnLenovoPsreffamilyImagesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> items);

        partial void OnLenovoPsreffamilyImageGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/LenovoPsreffamilyImages(ImageId={ImageId})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> GetLenovoPsreffamilyImage(long key)
        {
            var items = this.context.LenovoPsreffamilyImages.Where(i => i.ImageId == key);
            var result = SingleResult.Create(items);

            OnLenovoPsreffamilyImageGet(ref result);

            return result;
        }
        partial void OnLenovoPsreffamilyImageDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);
        partial void OnAfterLenovoPsreffamilyImageDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);

        [HttpDelete("/odata/DistributorProducts/LenovoPsreffamilyImages(ImageId={ImageId})")]
        public IActionResult DeleteLenovoPsreffamilyImage(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.LenovoPsreffamilyImages
                    .Where(i => i.ImageId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnLenovoPsreffamilyImageDeleted(item);
                this.context.LenovoPsreffamilyImages.Remove(item);
                this.context.SaveChanges();
                this.OnAfterLenovoPsreffamilyImageDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsreffamilyImageUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);
        partial void OnAfterLenovoPsreffamilyImageUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);

        [HttpPut("/odata/DistributorProducts/LenovoPsreffamilyImages(ImageId={ImageId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutLenovoPsreffamilyImage(long key, [FromBody]CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ImageId != key))
                {
                    return BadRequest();
                }
                this.OnLenovoPsreffamilyImageUpdated(item);
                this.context.LenovoPsreffamilyImages.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsreffamilyImages.Where(i => i.ImageId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsreffamily");
                this.OnAfterLenovoPsreffamilyImageUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/LenovoPsreffamilyImages(ImageId={ImageId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchLenovoPsreffamilyImage(long key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.LenovoPsreffamilyImages.Where(i => i.ImageId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnLenovoPsreffamilyImageUpdated(item);
                this.context.LenovoPsreffamilyImages.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsreffamilyImages.Where(i => i.ImageId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsreffamily");
                this.OnAfterLenovoPsreffamilyImageUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsreffamilyImageCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);
        partial void OnAfterLenovoPsreffamilyImageCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item)
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

                this.OnLenovoPsreffamilyImageCreated(item);
                this.context.LenovoPsreffamilyImages.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsreffamilyImages.Where(i => i.ImageId == item.ImageId);

                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsreffamily");

                this.OnAfterLenovoPsreffamilyImageCreated(item);

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
