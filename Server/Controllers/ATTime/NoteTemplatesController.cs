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
    [Route("odata/ATTime/NoteTemplates")]
    public partial class NoteTemplatesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public NoteTemplatesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.NoteTemplate> GetNoteTemplates()
        {
            var items = this.context.NoteTemplates.AsQueryable<CrownATTime.Server.Models.ATTime.NoteTemplate>();
            this.OnNoteTemplatesRead(ref items);

            return items;
        }

        partial void OnNoteTemplatesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.NoteTemplate> items);

        partial void OnNoteTemplateGet(ref SingleResult<CrownATTime.Server.Models.ATTime.NoteTemplate> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/NoteTemplates(NoteTemplateId={NoteTemplateId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.NoteTemplate> GetNoteTemplate(int key)
        {
            var items = this.context.NoteTemplates.Where(i => i.NoteTemplateId == key);
            var result = SingleResult.Create(items);

            OnNoteTemplateGet(ref result);

            return result;
        }
        partial void OnNoteTemplateDeleted(CrownATTime.Server.Models.ATTime.NoteTemplate item);
        partial void OnAfterNoteTemplateDeleted(CrownATTime.Server.Models.ATTime.NoteTemplate item);

        [HttpDelete("/odata/ATTime/NoteTemplates(NoteTemplateId={NoteTemplateId})")]
        public IActionResult DeleteNoteTemplate(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.NoteTemplates
                    .Where(i => i.NoteTemplateId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnNoteTemplateDeleted(item);
                this.context.NoteTemplates.Remove(item);
                this.context.SaveChanges();
                this.OnAfterNoteTemplateDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnNoteTemplateUpdated(CrownATTime.Server.Models.ATTime.NoteTemplate item);
        partial void OnAfterNoteTemplateUpdated(CrownATTime.Server.Models.ATTime.NoteTemplate item);

        [HttpPut("/odata/ATTime/NoteTemplates(NoteTemplateId={NoteTemplateId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutNoteTemplate(int key, [FromBody]CrownATTime.Server.Models.ATTime.NoteTemplate item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.NoteTemplateId != key))
                {
                    return BadRequest();
                }
                this.OnNoteTemplateUpdated(item);
                this.context.NoteTemplates.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.NoteTemplates.Where(i => i.NoteTemplateId == key);
                
                this.OnAfterNoteTemplateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/NoteTemplates(NoteTemplateId={NoteTemplateId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchNoteTemplate(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.NoteTemplate> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.NoteTemplates.Where(i => i.NoteTemplateId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnNoteTemplateUpdated(item);
                this.context.NoteTemplates.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.NoteTemplates.Where(i => i.NoteTemplateId == key);
                
                this.OnAfterNoteTemplateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnNoteTemplateCreated(CrownATTime.Server.Models.ATTime.NoteTemplate item);
        partial void OnAfterNoteTemplateCreated(CrownATTime.Server.Models.ATTime.NoteTemplate item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.NoteTemplate item)
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

                this.OnNoteTemplateCreated(item);
                this.context.NoteTemplates.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.NoteTemplates.Where(i => i.NoteTemplateId == item.NoteTemplateId);

                

                this.OnAfterNoteTemplateCreated(item);

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
