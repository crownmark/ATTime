using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using CrownATTime.Server.Data;

namespace CrownATTime.Server.Controllers
{
    public partial class ExportATTimeController : ExportController
    {
        private readonly ATTimeContext context;
        private readonly ATTimeService service;

        public ExportATTimeController(ATTimeContext context, ATTimeService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ATTime/timeentries/csv")]
        [HttpGet("/export/ATTime/timeentries/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTimeEntriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTimeEntries(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/timeentries/excel")]
        [HttpGet("/export/ATTime/timeentries/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTimeEntriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTimeEntries(), Request.Query, false), fileName);
        }
    }
}
