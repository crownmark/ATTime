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
    [Route("odata/ATTime/CalendarNotificationEventTypes")]
    public partial class CalendarNotificationEventTypesController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public CalendarNotificationEventTypesController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.CalendarNotificationEventType> GetCalendarNotificationEventTypes()
        {
            var items = this.context.CalendarNotificationEventTypes.AsQueryable<CrownATTime.Server.Models.ATTime.CalendarNotificationEventType>();
            this.OnCalendarNotificationEventTypesRead(ref items);

            return items;
        }

        partial void OnCalendarNotificationEventTypesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.CalendarNotificationEventType> items);

        partial void OnCalendarNotificationEventTypeGet(ref SingleResult<CrownATTime.Server.Models.ATTime.CalendarNotificationEventType> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/CalendarNotificationEventTypes(CalendarNotificationEventTypeId={CalendarNotificationEventTypeId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.CalendarNotificationEventType> GetCalendarNotificationEventType(int key)
        {
            var items = this.context.CalendarNotificationEventTypes.Where(i => i.CalendarNotificationEventTypeId == key);
            var result = SingleResult.Create(items);

            OnCalendarNotificationEventTypeGet(ref result);

            return result;
        }
        partial void OnCalendarNotificationEventTypeDeleted(CrownATTime.Server.Models.ATTime.CalendarNotificationEventType item);
        partial void OnAfterCalendarNotificationEventTypeDeleted(CrownATTime.Server.Models.ATTime.CalendarNotificationEventType item);

        [HttpDelete("/odata/ATTime/CalendarNotificationEventTypes(CalendarNotificationEventTypeId={CalendarNotificationEventTypeId})")]
        public IActionResult DeleteCalendarNotificationEventType(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.CalendarNotificationEventTypes
                    .Where(i => i.CalendarNotificationEventTypeId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCalendarNotificationEventTypeDeleted(item);
                this.context.CalendarNotificationEventTypes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCalendarNotificationEventTypeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCalendarNotificationEventTypeUpdated(CrownATTime.Server.Models.ATTime.CalendarNotificationEventType item);
        partial void OnAfterCalendarNotificationEventTypeUpdated(CrownATTime.Server.Models.ATTime.CalendarNotificationEventType item);

        [HttpPut("/odata/ATTime/CalendarNotificationEventTypes(CalendarNotificationEventTypeId={CalendarNotificationEventTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCalendarNotificationEventType(int key, [FromBody]CrownATTime.Server.Models.ATTime.CalendarNotificationEventType item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.CalendarNotificationEventTypeId != key))
                {
                    return BadRequest();
                }
                this.OnCalendarNotificationEventTypeUpdated(item);
                this.context.CalendarNotificationEventTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CalendarNotificationEventTypes.Where(i => i.CalendarNotificationEventTypeId == key);
                
                this.OnAfterCalendarNotificationEventTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/CalendarNotificationEventTypes(CalendarNotificationEventTypeId={CalendarNotificationEventTypeId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCalendarNotificationEventType(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.CalendarNotificationEventType> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.CalendarNotificationEventTypes.Where(i => i.CalendarNotificationEventTypeId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCalendarNotificationEventTypeUpdated(item);
                this.context.CalendarNotificationEventTypes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CalendarNotificationEventTypes.Where(i => i.CalendarNotificationEventTypeId == key);
                
                this.OnAfterCalendarNotificationEventTypeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCalendarNotificationEventTypeCreated(CrownATTime.Server.Models.ATTime.CalendarNotificationEventType item);
        partial void OnAfterCalendarNotificationEventTypeCreated(CrownATTime.Server.Models.ATTime.CalendarNotificationEventType item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.CalendarNotificationEventType item)
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

                this.OnCalendarNotificationEventTypeCreated(item);
                this.context.CalendarNotificationEventTypes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CalendarNotificationEventTypes.Where(i => i.CalendarNotificationEventTypeId == item.CalendarNotificationEventTypeId);

                

                this.OnAfterCalendarNotificationEventTypeCreated(item);

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
