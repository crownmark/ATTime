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
    [Route("odata/ATTime/FormCategories")]
    public partial class FormCategoriesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public FormCategoriesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.FormCategory> GetFormCategories()
        {
            var items = this.context.FormCategories.AsQueryable<CrownATTime.Server.Models.ATTime.FormCategory>();
            this.OnFormCategoriesRead(ref items);

            return items;
        }

        partial void OnFormCategoriesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.FormCategory> items);

        partial void OnFormCategoryGet(ref SingleResult<CrownATTime.Server.Models.ATTime.FormCategory> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/FormCategories(FormCategoryId={FormCategoryId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.FormCategory> GetFormCategory(int key)
        {
            var items = this.context.FormCategories.Where(i => i.FormCategoryId == key);
            var result = SingleResult.Create(items);

            OnFormCategoryGet(ref result);

            return result;
        }
        partial void OnFormCategoryDeleted(CrownATTime.Server.Models.ATTime.FormCategory item);
        partial void OnAfterFormCategoryDeleted(CrownATTime.Server.Models.ATTime.FormCategory item);

        [HttpDelete("/odata/ATTime/FormCategories(FormCategoryId={FormCategoryId})")]
        public IActionResult DeleteFormCategory(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.FormCategories
                    .Where(i => i.FormCategoryId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnFormCategoryDeleted(item);
                this.context.FormCategories.Remove(item);
                this.context.SaveChanges();
                this.OnAfterFormCategoryDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFormCategoryUpdated(CrownATTime.Server.Models.ATTime.FormCategory item);
        partial void OnAfterFormCategoryUpdated(CrownATTime.Server.Models.ATTime.FormCategory item);

        [HttpPut("/odata/ATTime/FormCategories(FormCategoryId={FormCategoryId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutFormCategory(int key, [FromBody]CrownATTime.Server.Models.ATTime.FormCategory item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.FormCategoryId != key))
                {
                    return BadRequest();
                }
                this.OnFormCategoryUpdated(item);
                this.context.FormCategories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FormCategories.Where(i => i.FormCategoryId == key);
                
                this.OnAfterFormCategoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/FormCategories(FormCategoryId={FormCategoryId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchFormCategory(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.FormCategory> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.FormCategories.Where(i => i.FormCategoryId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnFormCategoryUpdated(item);
                this.context.FormCategories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FormCategories.Where(i => i.FormCategoryId == key);
                
                this.OnAfterFormCategoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFormCategoryCreated(CrownATTime.Server.Models.ATTime.FormCategory item);
        partial void OnAfterFormCategoryCreated(CrownATTime.Server.Models.ATTime.FormCategory item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.FormCategory item)
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

                this.OnFormCategoryCreated(item);
                this.context.FormCategories.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FormCategories.Where(i => i.FormCategoryId == item.FormCategoryId);

                

                this.OnAfterFormCategoryCreated(item);

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
