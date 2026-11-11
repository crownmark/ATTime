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
    [Route("odata/DistributorProducts/LenovoPsrefimageData")]
    public partial class LenovoPsrefimageDataController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public LenovoPsrefimageDataController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> GetLenovoPsrefimageData()
        {
            var items = this.context.LenovoPsrefimageData.AsQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>();
            this.OnLenovoPsrefimageDataRead(ref items);

            return items;
        }

        partial void OnLenovoPsrefimageDataRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> items);

        partial void OnLenovoPsrefimageDatumGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/LenovoPsrefimageData(ImageId={ImageId})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> GetLenovoPsrefimageDatum(long key)
        {
            var items = this.context.LenovoPsrefimageData.Where(i => i.ImageId == key);
            var result = SingleResult.Create(items);

            OnLenovoPsrefimageDatumGet(ref result);

            return result;
        }
        partial void OnLenovoPsrefimageDatumDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);
        partial void OnAfterLenovoPsrefimageDatumDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);

        [HttpDelete("/odata/DistributorProducts/LenovoPsrefimageData(ImageId={ImageId})")]
        public IActionResult DeleteLenovoPsrefimageDatum(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.LenovoPsrefimageData
                    .Where(i => i.ImageId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnLenovoPsrefimageDatumDeleted(item);
                this.context.LenovoPsrefimageData.Remove(item);
                this.context.SaveChanges();
                this.OnAfterLenovoPsrefimageDatumDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsrefimageDatumUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);
        partial void OnAfterLenovoPsrefimageDatumUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);

        [HttpPut("/odata/DistributorProducts/LenovoPsrefimageData(ImageId={ImageId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutLenovoPsrefimageDatum(long key, [FromBody]CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item)
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
                this.OnLenovoPsrefimageDatumUpdated(item);
                this.context.LenovoPsrefimageData.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefimageData.Where(i => i.ImageId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsreffamilyImage");
                this.OnAfterLenovoPsrefimageDatumUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/LenovoPsrefimageData(ImageId={ImageId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchLenovoPsrefimageDatum(long key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.LenovoPsrefimageData.Where(i => i.ImageId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnLenovoPsrefimageDatumUpdated(item);
                this.context.LenovoPsrefimageData.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefimageData.Where(i => i.ImageId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsreffamilyImage");
                this.OnAfterLenovoPsrefimageDatumUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsrefimageDatumCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);
        partial void OnAfterLenovoPsrefimageDatumCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item)
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

                this.OnLenovoPsrefimageDatumCreated(item);
                this.context.LenovoPsrefimageData.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefimageData.Where(i => i.ImageId == item.ImageId);

                Request.QueryString = Request.QueryString.Add("$expand", "LenovoPsreffamilyImage");

                this.OnAfterLenovoPsrefimageDatumCreated(item);

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
