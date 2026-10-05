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

namespace CrownATTime.Server.Controllers.ATTime
{
    [Route("odata/ATTime/FavoriteBillingCodes")]
    public partial class FavoriteBillingCodesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public FavoriteBillingCodesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.FavoriteBillingCode> GetFavoriteBillingCodes()
        {
            var items = this.context.FavoriteBillingCodes.AsQueryable<CrownATTime.Server.Models.ATTime.FavoriteBillingCode>();
            this.OnFavoriteBillingCodesRead(ref items);

            return items;
        }

        partial void OnFavoriteBillingCodesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.FavoriteBillingCode> items);

        partial void OnFavoriteBillingCodeGet(ref SingleResult<CrownATTime.Server.Models.ATTime.FavoriteBillingCode> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/FavoriteBillingCodes(FavoriteBillingCodeId={FavoriteBillingCodeId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.FavoriteBillingCode> GetFavoriteBillingCode(int key)
        {
            var items = this.context.FavoriteBillingCodes.Where(i => i.FavoriteBillingCodeId == key);
            var result = SingleResult.Create(items);

            OnFavoriteBillingCodeGet(ref result);

            return result;
        }
        partial void OnFavoriteBillingCodeDeleted(CrownATTime.Server.Models.ATTime.FavoriteBillingCode item);
        partial void OnAfterFavoriteBillingCodeDeleted(CrownATTime.Server.Models.ATTime.FavoriteBillingCode item);

        [HttpDelete("/odata/ATTime/FavoriteBillingCodes(FavoriteBillingCodeId={FavoriteBillingCodeId})")]
        public IActionResult DeleteFavoriteBillingCode(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.FavoriteBillingCodes
                    .Where(i => i.FavoriteBillingCodeId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnFavoriteBillingCodeDeleted(item);
                this.context.FavoriteBillingCodes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterFavoriteBillingCodeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFavoriteBillingCodeUpdated(CrownATTime.Server.Models.ATTime.FavoriteBillingCode item);
        partial void OnAfterFavoriteBillingCodeUpdated(CrownATTime.Server.Models.ATTime.FavoriteBillingCode item);

        [HttpPut("/odata/ATTime/FavoriteBillingCodes(FavoriteBillingCodeId={FavoriteBillingCodeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutFavoriteBillingCode(int key, [FromBody]CrownATTime.Server.Models.ATTime.FavoriteBillingCode item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.FavoriteBillingCodeId != key))
                {
                    return BadRequest();
                }
                this.OnFavoriteBillingCodeUpdated(item);
                this.context.FavoriteBillingCodes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FavoriteBillingCodes.Where(i => i.FavoriteBillingCodeId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "BillingCodeCache,ResourceCache");
                this.OnAfterFavoriteBillingCodeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/FavoriteBillingCodes(FavoriteBillingCodeId={FavoriteBillingCodeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchFavoriteBillingCode(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.FavoriteBillingCode> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.FavoriteBillingCodes.Where(i => i.FavoriteBillingCodeId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnFavoriteBillingCodeUpdated(item);
                this.context.FavoriteBillingCodes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FavoriteBillingCodes.Where(i => i.FavoriteBillingCodeId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "BillingCodeCache,ResourceCache");
                this.OnAfterFavoriteBillingCodeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFavoriteBillingCodeCreated(CrownATTime.Server.Models.ATTime.FavoriteBillingCode item);
        partial void OnAfterFavoriteBillingCodeCreated(CrownATTime.Server.Models.ATTime.FavoriteBillingCode item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.FavoriteBillingCode item)
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

                this.OnFavoriteBillingCodeCreated(item);
                this.context.FavoriteBillingCodes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FavoriteBillingCodes.Where(i => i.FavoriteBillingCodeId == item.FavoriteBillingCodeId);

                Request.QueryString = Request.QueryString.Add("$expand", "BillingCodeCache,ResourceCache");

                this.OnAfterFavoriteBillingCodeCreated(item);

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
