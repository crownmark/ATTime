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
    [Route("odata/ATTime/EmailTemplates")]
    public partial class EmailTemplatesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public EmailTemplatesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.EmailTemplate> GetEmailTemplates()
        {
            var items = this.context.EmailTemplates.AsQueryable<CrownATTime.Server.Models.ATTime.EmailTemplate>();
            this.OnEmailTemplatesRead(ref items);

            return items;
        }

        partial void OnEmailTemplatesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.EmailTemplate> items);

        partial void OnEmailTemplateGet(ref SingleResult<CrownATTime.Server.Models.ATTime.EmailTemplate> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/EmailTemplates(EmailTemplateId={EmailTemplateId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.EmailTemplate> GetEmailTemplate(int key)
        {
            var items = this.context.EmailTemplates.Where(i => i.EmailTemplateId == key);
            var result = SingleResult.Create(items);

            OnEmailTemplateGet(ref result);

            return result;
        }
        partial void OnEmailTemplateDeleted(CrownATTime.Server.Models.ATTime.EmailTemplate item);
        partial void OnAfterEmailTemplateDeleted(CrownATTime.Server.Models.ATTime.EmailTemplate item);

        [HttpDelete("/odata/ATTime/EmailTemplates(EmailTemplateId={EmailTemplateId})")]
        public IActionResult DeleteEmailTemplate(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.EmailTemplates
                    .Where(i => i.EmailTemplateId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnEmailTemplateDeleted(item);
                this.context.EmailTemplates.Remove(item);
                this.context.SaveChanges();
                this.OnAfterEmailTemplateDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnEmailTemplateUpdated(CrownATTime.Server.Models.ATTime.EmailTemplate item);
        partial void OnAfterEmailTemplateUpdated(CrownATTime.Server.Models.ATTime.EmailTemplate item);

        [HttpPut("/odata/ATTime/EmailTemplates(EmailTemplateId={EmailTemplateId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutEmailTemplate(int key, [FromBody]CrownATTime.Server.Models.ATTime.EmailTemplate item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.EmailTemplateId != key))
                {
                    return BadRequest();
                }
                this.OnEmailTemplateUpdated(item);
                this.context.EmailTemplates.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.EmailTemplates.Where(i => i.EmailTemplateId == key);
                
                this.OnAfterEmailTemplateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/EmailTemplates(EmailTemplateId={EmailTemplateId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchEmailTemplate(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.EmailTemplate> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.EmailTemplates.Where(i => i.EmailTemplateId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnEmailTemplateUpdated(item);
                this.context.EmailTemplates.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.EmailTemplates.Where(i => i.EmailTemplateId == key);
                
                this.OnAfterEmailTemplateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnEmailTemplateCreated(CrownATTime.Server.Models.ATTime.EmailTemplate item);
        partial void OnAfterEmailTemplateCreated(CrownATTime.Server.Models.ATTime.EmailTemplate item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.EmailTemplate item)
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

                this.OnEmailTemplateCreated(item);
                this.context.EmailTemplates.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.EmailTemplates.Where(i => i.EmailTemplateId == item.EmailTemplateId);

                

                this.OnAfterEmailTemplateCreated(item);

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
