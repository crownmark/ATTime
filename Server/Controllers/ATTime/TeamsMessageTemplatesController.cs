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
    [Route("odata/ATTime/TeamsMessageTemplates")]
    public partial class TeamsMessageTemplatesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public TeamsMessageTemplatesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> GetTeamsMessageTemplates()
        {
            var items = this.context.TeamsMessageTemplates.AsQueryable<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate>();
            this.OnTeamsMessageTemplatesRead(ref items);

            return items;
        }

        partial void OnTeamsMessageTemplatesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> items);

        partial void OnTeamsMessageTemplateGet(ref SingleResult<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/TeamsMessageTemplates(TeamsMessageTemplateId={TeamsMessageTemplateId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> GetTeamsMessageTemplate(int key)
        {
            var items = this.context.TeamsMessageTemplates.Where(i => i.TeamsMessageTemplateId == key);
            var result = SingleResult.Create(items);

            OnTeamsMessageTemplateGet(ref result);

            return result;
        }
        partial void OnTeamsMessageTemplateDeleted(CrownATTime.Server.Models.ATTime.TeamsMessageTemplate item);
        partial void OnAfterTeamsMessageTemplateDeleted(CrownATTime.Server.Models.ATTime.TeamsMessageTemplate item);

        [HttpDelete("/odata/ATTime/TeamsMessageTemplates(TeamsMessageTemplateId={TeamsMessageTemplateId})")]
        public IActionResult DeleteTeamsMessageTemplate(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TeamsMessageTemplates
                    .Where(i => i.TeamsMessageTemplateId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTeamsMessageTemplateDeleted(item);
                this.context.TeamsMessageTemplates.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTeamsMessageTemplateDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTeamsMessageTemplateUpdated(CrownATTime.Server.Models.ATTime.TeamsMessageTemplate item);
        partial void OnAfterTeamsMessageTemplateUpdated(CrownATTime.Server.Models.ATTime.TeamsMessageTemplate item);

        [HttpPut("/odata/ATTime/TeamsMessageTemplates(TeamsMessageTemplateId={TeamsMessageTemplateId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTeamsMessageTemplate(int key, [FromBody]CrownATTime.Server.Models.ATTime.TeamsMessageTemplate item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.TeamsMessageTemplateId != key))
                {
                    return BadRequest();
                }
                this.OnTeamsMessageTemplateUpdated(item);
                this.context.TeamsMessageTemplates.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TeamsMessageTemplates.Where(i => i.TeamsMessageTemplateId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "TeamsMessageType");
                this.OnAfterTeamsMessageTemplateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/TeamsMessageTemplates(TeamsMessageTemplateId={TeamsMessageTemplateId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTeamsMessageTemplate(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TeamsMessageTemplates.Where(i => i.TeamsMessageTemplateId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTeamsMessageTemplateUpdated(item);
                this.context.TeamsMessageTemplates.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TeamsMessageTemplates.Where(i => i.TeamsMessageTemplateId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "TeamsMessageType");
                this.OnAfterTeamsMessageTemplateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTeamsMessageTemplateCreated(CrownATTime.Server.Models.ATTime.TeamsMessageTemplate item);
        partial void OnAfterTeamsMessageTemplateCreated(CrownATTime.Server.Models.ATTime.TeamsMessageTemplate item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.TeamsMessageTemplate item)
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

                this.OnTeamsMessageTemplateCreated(item);
                this.context.TeamsMessageTemplates.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TeamsMessageTemplates.Where(i => i.TeamsMessageTemplateId == item.TeamsMessageTemplateId);

                Request.QueryString = Request.QueryString.Add("$expand", "TeamsMessageType");

                this.OnAfterTeamsMessageTemplateCreated(item);

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
