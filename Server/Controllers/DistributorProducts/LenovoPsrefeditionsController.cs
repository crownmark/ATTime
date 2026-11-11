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
    [Route("odata/DistributorProducts/LenovoPsrefeditions")]
    public partial class LenovoPsrefeditionsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public LenovoPsrefeditionsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> GetLenovoPsrefeditions()
        {
            var items = this.context.LenovoPsrefeditions.AsQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>();
            this.OnLenovoPsrefeditionsRead(ref items);

            return items;
        }

        partial void OnLenovoPsrefeditionsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> items);

        partial void OnLenovoPsrefeditionGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/LenovoPsrefeditions(EditionId={EditionId})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> GetLenovoPsrefedition(long key)
        {
            var items = this.context.LenovoPsrefeditions.Where(i => i.EditionId == key);
            var result = SingleResult.Create(items);

            OnLenovoPsrefeditionGet(ref result);

            return result;
        }
        partial void OnLenovoPsrefeditionDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);
        partial void OnAfterLenovoPsrefeditionDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);

        [HttpDelete("/odata/DistributorProducts/LenovoPsrefeditions(EditionId={EditionId})")]
        public IActionResult DeleteLenovoPsrefedition(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.LenovoPsrefeditions
                    .Where(i => i.EditionId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnLenovoPsrefeditionDeleted(item);
                this.context.LenovoPsrefeditions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterLenovoPsrefeditionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsrefeditionUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);
        partial void OnAfterLenovoPsrefeditionUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);

        [HttpPut("/odata/DistributorProducts/LenovoPsrefeditions(EditionId={EditionId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutLenovoPsrefedition(long key, [FromBody]CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.EditionId != key))
                {
                    return BadRequest();
                }
                this.OnLenovoPsrefeditionUpdated(item);
                this.context.LenovoPsrefeditions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefeditions.Where(i => i.EditionId == key);
                
                this.OnAfterLenovoPsrefeditionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/LenovoPsrefeditions(EditionId={EditionId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchLenovoPsrefedition(long key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.LenovoPsrefeditions.Where(i => i.EditionId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnLenovoPsrefeditionUpdated(item);
                this.context.LenovoPsrefeditions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefeditions.Where(i => i.EditionId == key);
                
                this.OnAfterLenovoPsrefeditionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLenovoPsrefeditionCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);
        partial void OnAfterLenovoPsrefeditionCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item)
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

                this.OnLenovoPsrefeditionCreated(item);
                this.context.LenovoPsrefeditions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LenovoPsrefeditions.Where(i => i.EditionId == item.EditionId);

                

                this.OnAfterLenovoPsrefeditionCreated(item);

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
