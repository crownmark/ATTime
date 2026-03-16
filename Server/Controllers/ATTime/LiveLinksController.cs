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
    [Route("odata/ATTime/LiveLinks")]
    public partial class LiveLinksController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public LiveLinksController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.LiveLink> GetLiveLinks()
        {
            var items = this.context.LiveLinks.AsQueryable<CrownATTime.Server.Models.ATTime.LiveLink>();
            this.OnLiveLinksRead(ref items);

            return items;
        }

        partial void OnLiveLinksRead(ref IQueryable<CrownATTime.Server.Models.ATTime.LiveLink> items);

        partial void OnLiveLinkGet(ref SingleResult<CrownATTime.Server.Models.ATTime.LiveLink> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/LiveLinks(LiveLinkId={LiveLinkId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.LiveLink> GetLiveLink(int key)
        {
            var items = this.context.LiveLinks.Where(i => i.LiveLinkId == key);
            var result = SingleResult.Create(items);

            OnLiveLinkGet(ref result);

            return result;
        }
        partial void OnLiveLinkDeleted(CrownATTime.Server.Models.ATTime.LiveLink item);
        partial void OnAfterLiveLinkDeleted(CrownATTime.Server.Models.ATTime.LiveLink item);

        [HttpDelete("/odata/ATTime/LiveLinks(LiveLinkId={LiveLinkId})")]
        public IActionResult DeleteLiveLink(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.LiveLinks
                    .Where(i => i.LiveLinkId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnLiveLinkDeleted(item);
                this.context.LiveLinks.Remove(item);
                this.context.SaveChanges();
                this.OnAfterLiveLinkDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLiveLinkUpdated(CrownATTime.Server.Models.ATTime.LiveLink item);
        partial void OnAfterLiveLinkUpdated(CrownATTime.Server.Models.ATTime.LiveLink item);

        [HttpPut("/odata/ATTime/LiveLinks(LiveLinkId={LiveLinkId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutLiveLink(int key, [FromBody]CrownATTime.Server.Models.ATTime.LiveLink item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.LiveLinkId != key))
                {
                    return BadRequest();
                }
                this.OnLiveLinkUpdated(item);
                this.context.LiveLinks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LiveLinks.Where(i => i.LiveLinkId == key);
                
                this.OnAfterLiveLinkUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/LiveLinks(LiveLinkId={LiveLinkId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchLiveLink(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.LiveLink> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.LiveLinks.Where(i => i.LiveLinkId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnLiveLinkUpdated(item);
                this.context.LiveLinks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LiveLinks.Where(i => i.LiveLinkId == key);
                
                this.OnAfterLiveLinkUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLiveLinkCreated(CrownATTime.Server.Models.ATTime.LiveLink item);
        partial void OnAfterLiveLinkCreated(CrownATTime.Server.Models.ATTime.LiveLink item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.LiveLink item)
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

                this.OnLiveLinkCreated(item);
                this.context.LiveLinks.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LiveLinks.Where(i => i.LiveLinkId == item.LiveLinkId);

                

                this.OnAfterLiveLinkCreated(item);

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
