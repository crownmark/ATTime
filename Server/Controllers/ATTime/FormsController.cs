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
    [Route("odata/ATTime/Forms")]
    public partial class FormsController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public FormsController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.Form> GetForms()
        {
            var items = this.context.Forms.AsQueryable<CrownATTime.Server.Models.ATTime.Form>();
            this.OnFormsRead(ref items);

            return items;
        }

        partial void OnFormsRead(ref IQueryable<CrownATTime.Server.Models.ATTime.Form> items);

        partial void OnFormGet(ref SingleResult<CrownATTime.Server.Models.ATTime.Form> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/Forms(FormId={FormId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.Form> GetForm(int key)
        {
            var items = this.context.Forms.Where(i => i.FormId == key);
            var result = SingleResult.Create(items);

            OnFormGet(ref result);

            return result;
        }
        partial void OnFormDeleted(CrownATTime.Server.Models.ATTime.Form item);
        partial void OnAfterFormDeleted(CrownATTime.Server.Models.ATTime.Form item);

        [HttpDelete("/odata/ATTime/Forms(FormId={FormId})")]
        public IActionResult DeleteForm(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Forms
                    .Where(i => i.FormId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnFormDeleted(item);
                this.context.Forms.Remove(item);
                this.context.SaveChanges();
                this.OnAfterFormDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFormUpdated(CrownATTime.Server.Models.ATTime.Form item);
        partial void OnAfterFormUpdated(CrownATTime.Server.Models.ATTime.Form item);

        [HttpPut("/odata/ATTime/Forms(FormId={FormId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutForm(int key, [FromBody]CrownATTime.Server.Models.ATTime.Form item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.FormId != key))
                {
                    return BadRequest();
                }
                this.OnFormUpdated(item);
                this.context.Forms.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Forms.Where(i => i.FormId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "FormCategory");
                this.OnAfterFormUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/Forms(FormId={FormId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchForm(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.Form> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Forms.Where(i => i.FormId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnFormUpdated(item);
                this.context.Forms.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Forms.Where(i => i.FormId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "FormCategory");
                this.OnAfterFormUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFormCreated(CrownATTime.Server.Models.ATTime.Form item);
        partial void OnAfterFormCreated(CrownATTime.Server.Models.ATTime.Form item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.Form item)
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

                this.OnFormCreated(item);
                this.context.Forms.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Forms.Where(i => i.FormId == item.FormId);

                Request.QueryString = Request.QueryString.Add("$expand", "FormCategory");

                this.OnAfterFormCreated(item);

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
