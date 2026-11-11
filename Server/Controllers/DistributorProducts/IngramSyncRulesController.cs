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
    [Route("odata/DistributorProducts/IngramSyncRules")]
    public partial class IngramSyncRulesController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public IngramSyncRulesController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> GetIngramSyncRules()
        {
            var items = this.context.IngramSyncRules.AsQueryable<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>();
            this.OnIngramSyncRulesRead(ref items);

            return items;
        }

        partial void OnIngramSyncRulesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> items);

        partial void OnIngramSyncRuleGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/IngramSyncRules(RuleId={RuleId})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> GetIngramSyncRule(int key)
        {
            var items = this.context.IngramSyncRules.Where(i => i.RuleId == key);
            var result = SingleResult.Create(items);

            OnIngramSyncRuleGet(ref result);

            return result;
        }
        partial void OnIngramSyncRuleDeleted(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);
        partial void OnAfterIngramSyncRuleDeleted(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);

        [HttpDelete("/odata/DistributorProducts/IngramSyncRules(RuleId={RuleId})")]
        public IActionResult DeleteIngramSyncRule(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.IngramSyncRules
                    .Where(i => i.RuleId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnIngramSyncRuleDeleted(item);
                this.context.IngramSyncRules.Remove(item);
                this.context.SaveChanges();
                this.OnAfterIngramSyncRuleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramSyncRuleUpdated(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);
        partial void OnAfterIngramSyncRuleUpdated(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);

        [HttpPut("/odata/DistributorProducts/IngramSyncRules(RuleId={RuleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutIngramSyncRule(int key, [FromBody]CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.RuleId != key))
                {
                    return BadRequest();
                }
                this.OnIngramSyncRuleUpdated(item);
                this.context.IngramSyncRules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramSyncRules.Where(i => i.RuleId == key);
                
                this.OnAfterIngramSyncRuleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/IngramSyncRules(RuleId={RuleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchIngramSyncRule(int key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.IngramSyncRules.Where(i => i.RuleId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnIngramSyncRuleUpdated(item);
                this.context.IngramSyncRules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramSyncRules.Where(i => i.RuleId == key);
                
                this.OnAfterIngramSyncRuleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnIngramSyncRuleCreated(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);
        partial void OnAfterIngramSyncRuleCreated(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item)
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

                this.OnIngramSyncRuleCreated(item);
                this.context.IngramSyncRules.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.IngramSyncRules.Where(i => i.RuleId == item.RuleId);

                

                this.OnAfterIngramSyncRuleCreated(item);

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
