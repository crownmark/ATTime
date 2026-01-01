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

        [HttpGet("/export/ATTime/billingcodecaches/csv")]
        [HttpGet("/export/ATTime/billingcodecaches/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBillingCodeCachesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBillingCodeCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/billingcodecaches/excel")]
        [HttpGet("/export/ATTime/billingcodecaches/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBillingCodeCachesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBillingCodeCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/contractcaches/csv")]
        [HttpGet("/export/ATTime/contractcaches/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportContractCachesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetContractCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/contractcaches/excel")]
        [HttpGet("/export/ATTime/contractcaches/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportContractCachesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetContractCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/resourcecaches/csv")]
        [HttpGet("/export/ATTime/resourcecaches/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportResourceCachesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetResourceCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/resourcecaches/excel")]
        [HttpGet("/export/ATTime/resourcecaches/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportResourceCachesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetResourceCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/rolecaches/csv")]
        [HttpGet("/export/ATTime/rolecaches/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportRoleCachesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetRoleCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/rolecaches/excel")]
        [HttpGet("/export/ATTime/rolecaches/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportRoleCachesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetRoleCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/servicedeskrolecaches/csv")]
        [HttpGet("/export/ATTime/servicedeskrolecaches/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportServiceDeskRoleCachesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetServiceDeskRoleCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/servicedeskrolecaches/excel")]
        [HttpGet("/export/ATTime/servicedeskrolecaches/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportServiceDeskRoleCachesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetServiceDeskRoleCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/ticketentitypicklistvaluecaches/csv")]
        [HttpGet("/export/ATTime/ticketentitypicklistvaluecaches/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTicketEntityPicklistValueCachesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTicketEntityPicklistValueCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/ticketentitypicklistvaluecaches/excel")]
        [HttpGet("/export/ATTime/ticketentitypicklistvaluecaches/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTicketEntityPicklistValueCachesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTicketEntityPicklistValueCaches(), Request.Query, false), fileName);
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

        [HttpGet("/export/ATTime/emailtemplates/csv")]
        [HttpGet("/export/ATTime/emailtemplates/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmailTemplatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEmailTemplates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/emailtemplates/excel")]
        [HttpGet("/export/ATTime/emailtemplates/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmailTemplatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEmailTemplates(), Request.Query, false), fileName);
        }
    }
}
