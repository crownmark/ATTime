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
    [Route("odata/DistributorProducts/IngramProductsForQuotings")]
    public partial class IngramProductsForQuotingsController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public IngramProductsForQuotingsController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting> GetIngramProductsForQuotings()
        {
            var items = this.context.IngramProductsForQuotings.AsQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>();
            this.OnIngramProductsForQuotingsRead(ref items);

            return items;
        }

        partial void OnIngramProductsForQuotingsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting> items);
    }
}
