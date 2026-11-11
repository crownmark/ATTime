using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using CrownATTime.Server.Data;

namespace CrownATTime.Server.Controllers
{
    public partial class ExportDistributorProductsController : ExportController
    {
        private readonly DistributorProductsContext context;
        private readonly DistributorProductsService service;

        public ExportDistributorProductsController(DistributorProductsContext context, DistributorProductsService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/DistributorProducts/ingramlenovofamilyimages/csv")]
        [HttpGet("/export/DistributorProducts/ingramlenovofamilyimages/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramLenovoFamilyImagesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramLenovoFamilyImages(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramlenovofamilyimages/excel")]
        [HttpGet("/export/DistributorProducts/ingramlenovofamilyimages/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramLenovoFamilyImagesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramLenovoFamilyImages(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramlenovoscrapesources/csv")]
        [HttpGet("/export/DistributorProducts/ingramlenovoscrapesources/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramLenovoScrapeSourcesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramLenovoScrapeSources(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramlenovoscrapesources/excel")]
        [HttpGet("/export/DistributorProducts/ingramlenovoscrapesources/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramLenovoScrapeSourcesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramLenovoScrapeSources(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingrammanufacturercontents/csv")]
        [HttpGet("/export/DistributorProducts/ingrammanufacturercontents/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramManufacturerContentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramManufacturerContents(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingrammanufacturercontents/excel")]
        [HttpGet("/export/DistributorProducts/ingrammanufacturercontents/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramManufacturerContentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramManufacturerContents(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramproducts/csv")]
        [HttpGet("/export/DistributorProducts/ingramproducts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramProductsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramProducts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramproducts/excel")]
        [HttpGet("/export/DistributorProducts/ingramproducts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramProductsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramProducts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramproductenrichments/csv")]
        [HttpGet("/export/DistributorProducts/ingramproductenrichments/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramProductEnrichmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramProductEnrichments(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramproductenrichments/excel")]
        [HttpGet("/export/DistributorProducts/ingramproductenrichments/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramProductEnrichmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramProductEnrichments(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramsyncrules/csv")]
        [HttpGet("/export/DistributorProducts/ingramsyncrules/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramSyncRulesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramSyncRules(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramsyncrules/excel")]
        [HttpGet("/export/DistributorProducts/ingramsyncrules/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramSyncRulesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramSyncRules(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramwarehouseinventories/csv")]
        [HttpGet("/export/DistributorProducts/ingramwarehouseinventories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramWarehouseInventoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramWarehouseInventories(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramwarehouseinventories/excel")]
        [HttpGet("/export/DistributorProducts/ingramwarehouseinventories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramWarehouseInventoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramWarehouseInventories(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefeditions/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsrefeditions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefeditionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsrefeditions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefeditions/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsrefeditions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefeditionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsrefeditions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsreffamilies/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsreffamilies/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsreffamiliesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsreffamilies(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsreffamilies/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsreffamilies/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsreffamiliesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsreffamilies(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsreffamilyimages/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsreffamilyimages/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsreffamilyImagesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsreffamilyImages(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsreffamilyimages/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsreffamilyimages/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsreffamilyImagesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsreffamilyImages(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefgallerybeforev27s/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsrefgallerybeforev27s/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefgalleryBeforeV27SToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsrefgalleryBeforeV27S(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefgallerybeforev27s/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsrefgallerybeforev27s/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefgalleryBeforeV27SToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsrefgalleryBeforeV27S(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefimagedata/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsrefimagedata/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefimageDataToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsrefimageData(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefimagedata/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsrefimagedata/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefimageDataToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsrefimageData(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefskufamilymaps/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsrefskufamilymaps/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefskuFamilyMapsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsrefskuFamilyMaps(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefskufamilymaps/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsrefskufamilymaps/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefskuFamilyMapsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsrefskuFamilyMaps(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwaveproducts/csv")]
        [HttpGet("/export/DistributorProducts/streakwaveproducts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveProductsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStreakwaveProducts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwaveproducts/excel")]
        [HttpGet("/export/DistributorProducts/streakwaveproducts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveProductsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStreakwaveProducts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwaveproductenrichments/csv")]
        [HttpGet("/export/DistributorProducts/streakwaveproductenrichments/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveProductEnrichmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStreakwaveProductEnrichments(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwaveproductenrichments/excel")]
        [HttpGet("/export/DistributorProducts/streakwaveproductenrichments/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveProductEnrichmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStreakwaveProductEnrichments(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwavesyncrules/csv")]
        [HttpGet("/export/DistributorProducts/streakwavesyncrules/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveSyncRulesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStreakwaveSyncRules(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwavesyncrules/excel")]
        [HttpGet("/export/DistributorProducts/streakwavesyncrules/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveSyncRulesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStreakwaveSyncRules(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwavewarehousestocks/csv")]
        [HttpGet("/export/DistributorProducts/streakwavewarehousestocks/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveWarehouseStocksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStreakwaveWarehouseStocks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwavewarehousestocks/excel")]
        [HttpGet("/export/DistributorProducts/streakwavewarehousestocks/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveWarehouseStocksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStreakwaveWarehouseStocks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramproductsforquotings/csv")]
        [HttpGet("/export/DistributorProducts/ingramproductsforquotings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramProductsForQuotingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramProductsForQuotings(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramproductsforquotings/excel")]
        [HttpGet("/export/DistributorProducts/ingramproductsforquotings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramProductsForQuotingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramProductsForQuotings(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramproductswithdetails/csv")]
        [HttpGet("/export/DistributorProducts/ingramproductswithdetails/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramProductsWithDetailsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetIngramProductsWithDetails(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/ingramproductswithdetails/excel")]
        [HttpGet("/export/DistributorProducts/ingramproductswithdetails/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportIngramProductsWithDetailsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetIngramProductsWithDetails(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefapprovedskuimages/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsrefapprovedskuimages/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefapprovedSkuImagesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsrefapprovedSkuImages(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefapprovedskuimages/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsrefapprovedskuimages/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefapprovedSkuImagesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsrefapprovedSkuImages(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefcurrentfamilies/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsrefcurrentfamilies/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefcurrentFamiliesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsrefcurrentFamilies(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefcurrentfamilies/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsrefcurrentfamilies/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefcurrentFamiliesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsrefcurrentFamilies(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefimagestoragesummaries/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsrefimagestoragesummaries/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefimageStorageSummariesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsrefimageStorageSummaries(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefimagestoragesummaries/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsrefimagestoragesummaries/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefimageStorageSummariesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsrefimageStorageSummaries(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefunmatchedskus/csv")]
        [HttpGet("/export/DistributorProducts/lenovopsrefunmatchedskus/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefunmatchedSkusToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLenovoPsrefunmatchedSkus(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/lenovopsrefunmatchedskus/excel")]
        [HttpGet("/export/DistributorProducts/lenovopsrefunmatchedskus/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLenovoPsrefunmatchedSkusToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLenovoPsrefunmatchedSkus(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwaveproductsforquotings/csv")]
        [HttpGet("/export/DistributorProducts/streakwaveproductsforquotings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveProductsForQuotingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStreakwaveProductsForQuotings(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwaveproductsforquotings/excel")]
        [HttpGet("/export/DistributorProducts/streakwaveproductsforquotings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveProductsForQuotingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStreakwaveProductsForQuotings(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwaveproductswithenrichments/csv")]
        [HttpGet("/export/DistributorProducts/streakwaveproductswithenrichments/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveProductsWithEnrichmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStreakwaveProductsWithEnrichments(), Request.Query, true), fileName);
        }

        [HttpGet("/export/DistributorProducts/streakwaveproductswithenrichments/excel")]
        [HttpGet("/export/DistributorProducts/streakwaveproductswithenrichments/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStreakwaveProductsWithEnrichmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStreakwaveProductsWithEnrichments(), Request.Query, true), fileName);
        }
    }
}
