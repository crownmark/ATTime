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
    [Route("odata/ATTime/TeamsMessageTypes")]
    public partial class TeamsMessageTypesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public TeamsMessageTypesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.TeamsMessageType> GetTeamsMessageTypes()
        {
            var items = this.context.TeamsMessageTypes.AsQueryable<CrownATTime.Server.Models.ATTime.TeamsMessageType>();
            this.OnTeamsMessageTypesRead(ref items);

            return items;
        }

        partial void OnTeamsMessageTypesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TeamsMessageType> items);

        partial void OnTeamsMessageTypeGet(ref SingleResult<CrownATTime.Server.Models.ATTime.TeamsMessageType> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/TeamsMessageTypes(TeamsMessageTypeId={TeamsMessageTypeId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.TeamsMessageType> GetTeamsMessageType(int key)
        {
            var items = this.context.TeamsMessageTypes.Where(i => i.TeamsMessageTypeId == key);
            var result = SingleResult.Create(items);

            OnTeamsMessageTypeGet(ref result);

            return result;
        }
        partial void OnTeamsMessageTypeDeleted(CrownATTime.Server.Models.ATTime.TeamsMessageType item);
        partial void OnAfterTeamsMessageTypeDeleted(CrownATTime.Server.Models.ATTime.TeamsMessageType item);

        [HttpDelete("/odata/ATTime/TeamsMessageTypes(TeamsMessageTypeId={TeamsMessageTypeId})")]
        public IActionResult DeleteTeamsMessageType(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TeamsMessageTypes
                    .Where(i => i.TeamsMessageTypeId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTeamsMessageTypeDeleted(item);
                this.context.TeamsMessageTypes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTeamsMessageTypeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTeamsMessageTypeUpdated(CrownATTime.Server.Models.ATTime.TeamsMessageType item);
        partial void OnAfterTeamsMessageTypeUpdated(CrownATTime.Server.Models.ATTime.TeamsMessageType item);

        [HttpPut("/odata/ATTime/TeamsMessageTypes(TeamsMessageTypeId={TeamsMessageTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTeamsMessageType(int key, [FromBody]CrownATTime.Server.Models.ATTime.TeamsMessageType item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.TeamsMessageTypeId != key))
                {
                    return BadRequest();
                }
                this.OnTeamsMessageTypeUpdated(item);
                this.context.TeamsMessageTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TeamsMessageTypes.Where(i => i.TeamsMessageTypeId == key);
                
                this.OnAfterTeamsMessageTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/TeamsMessageTypes(TeamsMessageTypeId={TeamsMessageTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTeamsMessageType(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.TeamsMessageType> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TeamsMessageTypes.Where(i => i.TeamsMessageTypeId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTeamsMessageTypeUpdated(item);
                this.context.TeamsMessageTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TeamsMessageTypes.Where(i => i.TeamsMessageTypeId == key);
                
                this.OnAfterTeamsMessageTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTeamsMessageTypeCreated(CrownATTime.Server.Models.ATTime.TeamsMessageType item);
        partial void OnAfterTeamsMessageTypeCreated(CrownATTime.Server.Models.ATTime.TeamsMessageType item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.TeamsMessageType item)
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

                this.OnTeamsMessageTypeCreated(item);
                this.context.TeamsMessageTypes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TeamsMessageTypes.Where(i => i.TeamsMessageTypeId == item.TeamsMessageTypeId);

                

                this.OnAfterTeamsMessageTypeCreated(item);

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
