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

        [HttpGet("/export/ATTime/companycaches/csv")]
        [HttpGet("/export/ATTime/companycaches/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompanyCachesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCompanyCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/companycaches/excel")]
        [HttpGet("/export/ATTime/companycaches/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompanyCachesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCompanyCaches(), Request.Query, false), fileName);
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

        [HttpGet("/export/ATTime/notetemplates/csv")]
        [HttpGet("/export/ATTime/notetemplates/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportNoteTemplatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetNoteTemplates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/notetemplates/excel")]
        [HttpGet("/export/ATTime/notetemplates/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportNoteTemplatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetNoteTemplates(), Request.Query, false), fileName);
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

        [HttpGet("/export/ATTime/ticketnoteentitypicklistvaluecaches/csv")]
        [HttpGet("/export/ATTime/ticketnoteentitypicklistvaluecaches/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTicketNoteEntityPicklistValueCachesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTicketNoteEntityPicklistValueCaches(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/ticketnoteentitypicklistvaluecaches/excel")]
        [HttpGet("/export/ATTime/ticketnoteentitypicklistvaluecaches/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTicketNoteEntityPicklistValueCachesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTicketNoteEntityPicklistValueCaches(), Request.Query, false), fileName);
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

        [HttpGet("/export/ATTime/timeentrytemplates/csv")]
        [HttpGet("/export/ATTime/timeentrytemplates/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTimeEntryTemplatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTimeEntryTemplates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ATTime/timeentrytemplates/excel")]
        [HttpGet("/export/ATTime/timeentrytemplates/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTimeEntryTemplatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTimeEntryTemplates(), Request.Query, false), fileName);
        }
    }
}
