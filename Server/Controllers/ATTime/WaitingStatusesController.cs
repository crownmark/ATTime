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
    [Route("odata/ATTime/WaitingStatuses")]
    public partial class WaitingStatusesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public WaitingStatusesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.WaitingStatus> GetWaitingStatuses()
        {
            var items = this.context.WaitingStatuses.AsQueryable<CrownATTime.Server.Models.ATTime.WaitingStatus>();
            this.OnWaitingStatusesRead(ref items);

            return items;
        }

        partial void OnWaitingStatusesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.WaitingStatus> items);

        partial void OnWaitingStatusGet(ref SingleResult<CrownATTime.Server.Models.ATTime.WaitingStatus> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/WaitingStatuses(WaitingStatusId={WaitingStatusId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.WaitingStatus> GetWaitingStatus(int key)
        {
            var items = this.context.WaitingStatuses.Where(i => i.WaitingStatusId == key);
            var result = SingleResult.Create(items);

            OnWaitingStatusGet(ref result);

            return result;
        }
        partial void OnWaitingStatusDeleted(CrownATTime.Server.Models.ATTime.WaitingStatus item);
        partial void OnAfterWaitingStatusDeleted(CrownATTime.Server.Models.ATTime.WaitingStatus item);

        [HttpDelete("/odata/ATTime/WaitingStatuses(WaitingStatusId={WaitingStatusId})")]
        public IActionResult DeleteWaitingStatus(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.WaitingStatuses
                    .Where(i => i.WaitingStatusId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnWaitingStatusDeleted(item);
                this.context.WaitingStatuses.Remove(item);
                this.context.SaveChanges();
                this.OnAfterWaitingStatusDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWaitingStatusUpdated(CrownATTime.Server.Models.ATTime.WaitingStatus item);
        partial void OnAfterWaitingStatusUpdated(CrownATTime.Server.Models.ATTime.WaitingStatus item);

        [HttpPut("/odata/ATTime/WaitingStatuses(WaitingStatusId={WaitingStatusId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutWaitingStatus(int key, [FromBody]CrownATTime.Server.Models.ATTime.WaitingStatus item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.WaitingStatusId != key))
                {
                    return BadRequest();
                }
                this.OnWaitingStatusUpdated(item);
                this.context.WaitingStatuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WaitingStatuses.Where(i => i.WaitingStatusId == key);
                
                this.OnAfterWaitingStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/WaitingStatuses(WaitingStatusId={WaitingStatusId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchWaitingStatus(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.WaitingStatus> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.WaitingStatuses.Where(i => i.WaitingStatusId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnWaitingStatusUpdated(item);
                this.context.WaitingStatuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WaitingStatuses.Where(i => i.WaitingStatusId == key);
                
                this.OnAfterWaitingStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnWaitingStatusCreated(CrownATTime.Server.Models.ATTime.WaitingStatus item);
        partial void OnAfterWaitingStatusCreated(CrownATTime.Server.Models.ATTime.WaitingStatus item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.WaitingStatus item)
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

                this.OnWaitingStatusCreated(item);
                this.context.WaitingStatuses.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.WaitingStatuses.Where(i => i.WaitingStatusId == item.WaitingStatusId);

                

                this.OnAfterWaitingStatusCreated(item);

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
