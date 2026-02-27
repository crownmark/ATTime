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
    [Route("odata/ATTime/AllowedTicketStatuses")]
    public partial class AllowedTicketStatusesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public AllowedTicketStatusesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.AllowedTicketStatus> GetAllowedTicketStatuses()
        {
            var items = this.context.AllowedTicketStatuses.AsQueryable<CrownATTime.Server.Models.ATTime.AllowedTicketStatus>();
            this.OnAllowedTicketStatusesRead(ref items);

            return items;
        }

        partial void OnAllowedTicketStatusesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.AllowedTicketStatus> items);

        partial void OnAllowedTicketStatusGet(ref SingleResult<CrownATTime.Server.Models.ATTime.AllowedTicketStatus> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/AllowedTicketStatuses(AllowedTicketStatusId={AllowedTicketStatusId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.AllowedTicketStatus> GetAllowedTicketStatus(int key)
        {
            var items = this.context.AllowedTicketStatuses.Where(i => i.AllowedTicketStatusId == key);
            var result = SingleResult.Create(items);

            OnAllowedTicketStatusGet(ref result);

            return result;
        }
        partial void OnAllowedTicketStatusDeleted(CrownATTime.Server.Models.ATTime.AllowedTicketStatus item);
        partial void OnAfterAllowedTicketStatusDeleted(CrownATTime.Server.Models.ATTime.AllowedTicketStatus item);

        [HttpDelete("/odata/ATTime/AllowedTicketStatuses(AllowedTicketStatusId={AllowedTicketStatusId})")]
        public IActionResult DeleteAllowedTicketStatus(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.AllowedTicketStatuses
                    .Where(i => i.AllowedTicketStatusId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnAllowedTicketStatusDeleted(item);
                this.context.AllowedTicketStatuses.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAllowedTicketStatusDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAllowedTicketStatusUpdated(CrownATTime.Server.Models.ATTime.AllowedTicketStatus item);
        partial void OnAfterAllowedTicketStatusUpdated(CrownATTime.Server.Models.ATTime.AllowedTicketStatus item);

        [HttpPut("/odata/ATTime/AllowedTicketStatuses(AllowedTicketStatusId={AllowedTicketStatusId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAllowedTicketStatus(int key, [FromBody]CrownATTime.Server.Models.ATTime.AllowedTicketStatus item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.AllowedTicketStatusId != key))
                {
                    return BadRequest();
                }
                this.OnAllowedTicketStatusUpdated(item);
                this.context.AllowedTicketStatuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AllowedTicketStatuses.Where(i => i.AllowedTicketStatusId == key);
                
                this.OnAfterAllowedTicketStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/AllowedTicketStatuses(AllowedTicketStatusId={AllowedTicketStatusId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAllowedTicketStatus(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.AllowedTicketStatus> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.AllowedTicketStatuses.Where(i => i.AllowedTicketStatusId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnAllowedTicketStatusUpdated(item);
                this.context.AllowedTicketStatuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AllowedTicketStatuses.Where(i => i.AllowedTicketStatusId == key);
                
                this.OnAfterAllowedTicketStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAllowedTicketStatusCreated(CrownATTime.Server.Models.ATTime.AllowedTicketStatus item);
        partial void OnAfterAllowedTicketStatusCreated(CrownATTime.Server.Models.ATTime.AllowedTicketStatus item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.AllowedTicketStatus item)
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

                this.OnAllowedTicketStatusCreated(item);
                this.context.AllowedTicketStatuses.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AllowedTicketStatuses.Where(i => i.AllowedTicketStatusId == item.AllowedTicketStatusId);

                

                this.OnAfterAllowedTicketStatusCreated(item);

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
