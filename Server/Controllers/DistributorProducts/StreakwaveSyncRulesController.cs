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
    [Route("odata/DistributorProducts/StreakwaveSyncRules")]
    public partial class StreakwaveSyncRulesController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public StreakwaveSyncRulesController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> GetStreakwaveSyncRules()
        {
            var items = this.context.StreakwaveSyncRules.AsQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>();
            this.OnStreakwaveSyncRulesRead(ref items);

            return items;
        }

        partial void OnStreakwaveSyncRulesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> items);

        partial void OnStreakwaveSyncRuleGet(ref SingleResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DistributorProducts/StreakwaveSyncRules(RuleId={RuleId})")]
        public SingleResult<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> GetStreakwaveSyncRule(int key)
        {
            var items = this.context.StreakwaveSyncRules.Where(i => i.RuleId == key);
            var result = SingleResult.Create(items);

            OnStreakwaveSyncRuleGet(ref result);

            return result;
        }
        partial void OnStreakwaveSyncRuleDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);
        partial void OnAfterStreakwaveSyncRuleDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);

        [HttpDelete("/odata/DistributorProducts/StreakwaveSyncRules(RuleId={RuleId})")]
        public IActionResult DeleteStreakwaveSyncRule(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.StreakwaveSyncRules
                    .Where(i => i.RuleId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnStreakwaveSyncRuleDeleted(item);
                this.context.StreakwaveSyncRules.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStreakwaveSyncRuleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStreakwaveSyncRuleUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);
        partial void OnAfterStreakwaveSyncRuleUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);

        [HttpPut("/odata/DistributorProducts/StreakwaveSyncRules(RuleId={RuleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStreakwaveSyncRule(int key, [FromBody]CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item)
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
                this.OnStreakwaveSyncRuleUpdated(item);
                this.context.StreakwaveSyncRules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveSyncRules.Where(i => i.RuleId == key);
                
                this.OnAfterStreakwaveSyncRuleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DistributorProducts/StreakwaveSyncRules(RuleId={RuleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStreakwaveSyncRule(int key, [FromBody]Delta<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.StreakwaveSyncRules.Where(i => i.RuleId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnStreakwaveSyncRuleUpdated(item);
                this.context.StreakwaveSyncRules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveSyncRules.Where(i => i.RuleId == key);
                
                this.OnAfterStreakwaveSyncRuleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStreakwaveSyncRuleCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);
        partial void OnAfterStreakwaveSyncRuleCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item)
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

                this.OnStreakwaveSyncRuleCreated(item);
                this.context.StreakwaveSyncRules.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StreakwaveSyncRules.Where(i => i.RuleId == item.RuleId);

                

                this.OnAfterStreakwaveSyncRuleCreated(item);

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
