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
    [Route("odata/DistributorProducts/LenovoPsrefgalleryBeforeV27S")]
    public partial class LenovoPsrefgalleryBeforeV27SController : ODataController
    {
        private CrownATTime.Server.Data.DistributorProductsContext context;

        public LenovoPsrefgalleryBeforeV27SController(CrownATTime.Server.Data.DistributorProductsContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27> GetLenovoPsrefgalleryBeforeV27S()
        {
            var items = this.context.LenovoPsrefgalleryBeforeV27S.AsQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27>();
            this.OnLenovoPsrefgalleryBeforeV27SRead(ref items);

            return items;
        }

        partial void OnLenovoPsrefgalleryBeforeV27SRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27> items);
    }
}
