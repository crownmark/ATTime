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
    [Route("odata/DistributorProducts/IngramLenovoScrapeSources")]
    public partial class IngramLenovoScrapeSourcesController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public IngramLenovoScrapeSourcesController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> GetIngramLenovoScrapeSources()
        {
            var items = this.context.IngramLenovoScrapeSources.AsQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>();
            this.OnIngramLenovoScrapeSourcesRead(ref items);

            return items;
        }

        partial void OnIngramLenovoScrapeSourcesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> items);

        partial void OnIngramLenovoScrapeSourceGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/IngramLenovoScrapeSources(VendorPartNumber={VendorPartNumber})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> GetIngramLenovoScrapeSource(string key)
        {
            var items = this.context.IngramLenovoScrapeSources.Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnIngramLenovoScrapeSourceGet(ref result);

            return result;
        }
        partial void OnIngramLenovoScrapeSourceDeleted(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);
        partial void OnAfterIngramLenovoScrapeSourceDeleted(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);

        [HttpDelete("/odata/DistributorProducts/IngramLenovoScrapeSources(VendorPartNumber={VendorPartNumber})")]
        public IActionResult DeleteIngramLenovoScrapeSource(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.IngramLenovoScrapeSources
                    .Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnIngramLenovoScrapeSourceDeleted(item);
                this.context.IngramLenovoScrapeSources.Remove(item);
                this.context.SaveChanges();
                this.OnAfterIngramLenovoScrapeSourceDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramLenovoScrapeSourceUpdated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);
        partial void OnAfterIngramLenovoScrapeSourceUpdated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);

        [HttpPut("/odata/DistributorProducts/IngramLenovoScrapeSources(VendorPartNumber={VendorPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutIngramLenovoScrapeSource(string key, [FromBody]CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item)
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
                this.OnIngramLenovoScrapeSourceUpdated(item);
                this.context.IngramLenovoScrapeSources.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramLenovoScrapeSources.Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key));
                
                this.OnAfterIngramLenovoScrapeSourceUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/IngramLenovoScrapeSources(VendorPartNumber={VendorPartNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchIngramLenovoScrapeSource(string key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.IngramLenovoScrapeSources.Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnIngramLenovoScrapeSourceUpdated(item);
                this.context.IngramLenovoScrapeSources.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramLenovoScrapeSources.Where(i => i.VendorPartNumber == Uri.UnescapeDataString(key));
                
                this.OnAfterIngramLenovoScrapeSourceUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramLenovoScrapeSourceCreated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);
        partial void OnAfterIngramLenovoScrapeSourceCreated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item)
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

                this.OnIngramLenovoScrapeSourceCreated(item);
                this.context.IngramLenovoScrapeSources.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramLenovoScrapeSources.Where(i => i.VendorPartNumber == item.VendorPartNumber);

                

                this.OnAfterIngramLenovoScrapeSourceCreated(item);

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
