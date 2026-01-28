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
    [Route("odata/ATTime/AiPromptConfigurations")]
    public partial class AiPromptConfigurationsController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public AiPromptConfigurationsController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> GetAiPromptConfigurations()
        {
            var items = this.context.AiPromptConfigurations.AsQueryable<CrownATTime.Server.Models.ATTime.AiPromptConfiguration>();
            this.OnAiPromptConfigurationsRead(ref items);

            return items;
        }

        partial void OnAiPromptConfigurationsRead(ref IQueryable<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> items);

        partial void OnAiPromptConfigurationGet(ref SingleResult<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/AiPromptConfigurations(AiPromptConfigurationId={AiPromptConfigurationId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> GetAiPromptConfiguration(int key)
        {
            var items = this.context.AiPromptConfigurations.Where(i => i.AiPromptConfigurationId == key);
            var result = SingleResult.Create(items);

            OnAiPromptConfigurationGet(ref result);

            return result;
        }
        partial void OnAiPromptConfigurationDeleted(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);
        partial void OnAfterAiPromptConfigurationDeleted(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);

        [HttpDelete("/odata/ATTime/AiPromptConfigurations(AiPromptConfigurationId={AiPromptConfigurationId})")]
        public IActionResult DeleteAiPromptConfiguration(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.AiPromptConfigurations
                    .Where(i => i.AiPromptConfigurationId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnAiPromptConfigurationDeleted(item);
                this.context.AiPromptConfigurations.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAiPromptConfigurationDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAiPromptConfigurationUpdated(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);
        partial void OnAfterAiPromptConfigurationUpdated(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);

        [HttpPut("/odata/ATTime/AiPromptConfigurations(AiPromptConfigurationId={AiPromptConfigurationId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAiPromptConfiguration(int key, [FromBody]CrownATTime.Server.Models.ATTime.AiPromptConfiguration item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.AiPromptConfigurationId != key))
                {
                    return BadRequest();
                }
                this.OnAiPromptConfigurationUpdated(item);
                this.context.AiPromptConfigurations.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AiPromptConfigurations.Where(i => i.AiPromptConfigurationId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "TimeGuardSection");
                this.OnAfterAiPromptConfigurationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/AiPromptConfigurations(AiPromptConfigurationId={AiPromptConfigurationId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAiPromptConfiguration(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.AiPromptConfigurations.Where(i => i.AiPromptConfigurationId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnAiPromptConfigurationUpdated(item);
                this.context.AiPromptConfigurations.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AiPromptConfigurations.Where(i => i.AiPromptConfigurationId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "TimeGuardSection");
                this.OnAfterAiPromptConfigurationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAiPromptConfigurationCreated(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);
        partial void OnAfterAiPromptConfigurationCreated(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.AiPromptConfiguration item)
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

                this.OnAiPromptConfigurationCreated(item);
                this.context.AiPromptConfigurations.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AiPromptConfigurations.Where(i => i.AiPromptConfigurationId == item.AiPromptConfigurationId);

                Request.QueryString = Request.QueryString.Add("$expand", "TimeGuardSection");

                this.OnAfterAiPromptConfigurationCreated(item);

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
