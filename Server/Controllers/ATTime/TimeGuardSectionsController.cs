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
    [Route("odata/ATTime/TimeGuardSections")]
    public partial class TimeGuardSectionsController : ODataController
    {
        private CrownATTime.Server.Data.ATTimeContext context;

        public TimeGuardSectionsController(CrownATTime.Server.Data.ATTimeContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.ATTime.TimeGuardSection> GetTimeGuardSections()
        {
            var items = this.context.TimeGuardSections.AsQueryable<CrownATTime.Server.Models.ATTime.TimeGuardSection>();
            this.OnTimeGuardSectionsRead(ref items);

            return items;
        }

        partial void OnTimeGuardSectionsRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeGuardSection> items);

        partial void OnTimeGuardSectionGet(ref SingleResult<CrownATTime.Server.Models.ATTime.TimeGuardSection> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ATTime/TimeGuardSections(TimeGuardSectionsId={TimeGuardSectionsId})")]
        public SingleResult<CrownATTime.Server.Models.ATTime.TimeGuardSection> GetTimeGuardSection(int key)
        {
            var items = this.context.TimeGuardSections.Where(i => i.TimeGuardSectionsId == key);
            var result = SingleResult.Create(items);

            OnTimeGuardSectionGet(ref result);

            return result;
        }
        partial void OnTimeGuardSectionDeleted(CrownATTime.Server.Models.ATTime.TimeGuardSection item);
        partial void OnAfterTimeGuardSectionDeleted(CrownATTime.Server.Models.ATTime.TimeGuardSection item);

        [HttpDelete("/odata/ATTime/TimeGuardSections(TimeGuardSectionsId={TimeGuardSectionsId})")]
        public IActionResult DeleteTimeGuardSection(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TimeGuardSections
                    .Where(i => i.TimeGuardSectionsId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTimeGuardSectionDeleted(item);
                this.context.TimeGuardSections.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTimeGuardSectionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTimeGuardSectionUpdated(CrownATTime.Server.Models.ATTime.TimeGuardSection item);
        partial void OnAfterTimeGuardSectionUpdated(CrownATTime.Server.Models.ATTime.TimeGuardSection item);

        [HttpPut("/odata/ATTime/TimeGuardSections(TimeGuardSectionsId={TimeGuardSectionsId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTimeGuardSection(int key, [FromBody]CrownATTime.Server.Models.ATTime.TimeGuardSection item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.TimeGuardSectionsId != key))
                {
                    return BadRequest();
                }
                this.OnTimeGuardSectionUpdated(item);
                this.context.TimeGuardSections.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeGuardSections.Where(i => i.TimeGuardSectionsId == key);
                
                this.OnAfterTimeGuardSectionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ATTime/TimeGuardSections(TimeGuardSectionsId={TimeGuardSectionsId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTimeGuardSection(int key, [FromBody]Delta<CrownATTime.Server.Models.ATTime.TimeGuardSection> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TimeGuardSections.Where(i => i.TimeGuardSectionsId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTimeGuardSectionUpdated(item);
                this.context.TimeGuardSections.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeGuardSections.Where(i => i.TimeGuardSectionsId == key);
                
                this.OnAfterTimeGuardSectionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTimeGuardSectionCreated(CrownATTime.Server.Models.ATTime.TimeGuardSection item);
        partial void OnAfterTimeGuardSectionCreated(CrownATTime.Server.Models.ATTime.TimeGuardSection item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CrownATTime.Server.Models.ATTime.TimeGuardSection item)
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

                this.OnTimeGuardSectionCreated(item);
                this.context.TimeGuardSections.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TimeGuardSections.Where(i => i.TimeGuardSectionsId == item.TimeGuardSectionsId);

                

                this.OnAfterTimeGuardSectionCreated(item);

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
