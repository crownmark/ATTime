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
    [Route("odata/ATTime/DurationTypes")]
    public partial class DurationTypesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public DurationTypesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.DurationType> GetDurationTypes()
        {
            var items = this.context.DurationTypes.AsQueryable<CrownATTime.Server.Models.ATTime.DurationType>();
            this.OnDurationTypesRead(ref items);

            return items;
        }

        partial void OnDurationTypesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.DurationType> items);

        partial void OnDurationTypeGet(ref SingleResult<CrownATTime.Server.Models.ATTime.DurationType> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/DurationTypes(DurationTypeId={DurationTypeId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.DurationType> GetDurationType(int key)
        {
            var items = this.context.DurationTypes.Where(i => i.DurationTypeId == key);
            var result = SingleResult.Create(items);

            OnDurationTypeGet(ref result);

            return result;
        }
        partial void OnDurationTypeDeleted(CrownATTime.Server.Models.ATTime.DurationType item);
        partial void OnAfterDurationTypeDeleted(CrownATTime.Server.Models.ATTime.DurationType item);

        [HttpDelete("/odata/ATTime/DurationTypes(DurationTypeId={DurationTypeId})")]
        public IActionResult DeleteDurationType(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.DurationTypes
                    .Where(i => i.DurationTypeId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnDurationTypeDeleted(item);
                this.context.DurationTypes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterDurationTypeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDurationTypeUpdated(CrownATTime.Server.Models.ATTime.DurationType item);
        partial void OnAfterDurationTypeUpdated(CrownATTime.Server.Models.ATTime.DurationType item);

        [HttpPut("/odata/ATTime/DurationTypes(DurationTypeId={DurationTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutDurationType(int key, [FromBody]CrownATTime.Server.Models.ATTime.DurationType item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.DurationTypeId != key))
                {
                    return BadRequest();
                }
                this.OnDurationTypeUpdated(item);
                this.context.DurationTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.DurationTypes.Where(i => i.DurationTypeId == key);
                
                this.OnAfterDurationTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/DurationTypes(DurationTypeId={DurationTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchDurationType(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.DurationType> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.DurationTypes.Where(i => i.DurationTypeId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnDurationTypeUpdated(item);
                this.context.DurationTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.DurationTypes.Where(i => i.DurationTypeId == key);
                
                this.OnAfterDurationTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDurationTypeCreated(CrownATTime.Server.Models.ATTime.DurationType item);
        partial void OnAfterDurationTypeCreated(CrownATTime.Server.Models.ATTime.DurationType item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.DurationType item)
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

                this.OnDurationTypeCreated(item);
                this.context.DurationTypes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.DurationTypes.Where(i => i.DurationTypeId == item.DurationTypeId);

                

                this.OnAfterDurationTypeCreated(item);

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
