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

namespace CrownATTime.Server.Controllers.DistributorProducts
{
    [Route("odata/DistributorProducts/StreakwaveProductsWithEnrichments")]
    public partial class StreakwaveProductsWithEnrichmentsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public StreakwaveProductsWithEnrichmentsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment> GetStreakwaveProductsWithEnrichments()
        {
            var items = this.context.StreakwaveProductsWithEnrichments.AsQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>();
            this.OnStreakwaveProductsWithEnrichmentsRead(ref items);

            return items;
        }

        partial void OnStreakwaveProductsWithEnrichmentsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment> items);
    }
}
