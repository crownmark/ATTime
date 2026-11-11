using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using CrownATTime.Server.Data;

namespace CrownATTime.Server
{
    public partial class DistributorProductsService
    {
        DistributorProductsContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly DistributorProductsContext context;
        private readonly NavigationManager navigationManager;

        public DistributorProductsService(DistributorProductsContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportIngramLenovoFamilyImagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramlenovofamilyimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramlenovofamilyimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramLenovoFamilyImagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramlenovofamilyimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramlenovofamilyimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramLenovoFamilyImagesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>> GetIngramLenovoFamilyImages(Query query = null)
        {
            var items = Context.IngramLenovoFamilyImages.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramLenovoFamilyImagesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIngramLenovoFamilyImageGet(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);
        partial void OnGetIngramLenovoFamilyImageByMachineType(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> GetIngramLenovoFamilyImageByMachineType(string machinetype)
        {
            var items = Context.IngramLenovoFamilyImages
                              .AsNoTracking()
                              .Where(i => i.MachineType == machinetype);

 
            OnGetIngramLenovoFamilyImageByMachineType(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnIngramLenovoFamilyImageGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnIngramLenovoFamilyImageCreated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);
        partial void OnAfterIngramLenovoFamilyImageCreated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> CreateIngramLenovoFamilyImage(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage ingramlenovofamilyimage)
        {
            OnIngramLenovoFamilyImageCreated(ingramlenovofamilyimage);

            var existingItem = Context.IngramLenovoFamilyImages
                              .Where(i => i.MachineType == ingramlenovofamilyimage.MachineType)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.IngramLenovoFamilyImages.Add(ingramlenovofamilyimage);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ingramlenovofamilyimage).State = EntityState.Detached;
                throw;
            }

            OnAfterIngramLenovoFamilyImageCreated(ingramlenovofamilyimage);

            return ingramlenovofamilyimage;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> CancelIngramLenovoFamilyImageChanges(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnIngramLenovoFamilyImageUpdated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);
        partial void OnAfterIngramLenovoFamilyImageUpdated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> UpdateIngramLenovoFamilyImage(string machinetype, CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage ingramlenovofamilyimage)
        {
            OnIngramLenovoFamilyImageUpdated(ingramlenovofamilyimage);

            var itemToUpdate = Context.IngramLenovoFamilyImages
                              .Where(i => i.MachineType == ingramlenovofamilyimage.MachineType)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(ingramlenovofamilyimage).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterIngramLenovoFamilyImageUpdated(ingramlenovofamilyimage);

            return ingramlenovofamilyimage;
        }

        partial void OnIngramLenovoFamilyImageDeleted(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);
        partial void OnAfterIngramLenovoFamilyImageDeleted(CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage> DeleteIngramLenovoFamilyImage(string machinetype)
        {
            var itemToDelete = Context.IngramLenovoFamilyImages
                              .Where(i => i.MachineType == machinetype)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnIngramLenovoFamilyImageDeleted(itemToDelete);

            Reset();

            Context.IngramLenovoFamilyImages.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterIngramLenovoFamilyImageDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportIngramLenovoScrapeSourcesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramlenovoscrapesources/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramlenovoscrapesources/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramLenovoScrapeSourcesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramlenovoscrapesources/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramlenovoscrapesources/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramLenovoScrapeSourcesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>> GetIngramLenovoScrapeSources(Query query = null)
        {
            var items = Context.IngramLenovoScrapeSources.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramLenovoScrapeSourcesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIngramLenovoScrapeSourceGet(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);
        partial void OnGetIngramLenovoScrapeSourceByVendorPartNumber(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> GetIngramLenovoScrapeSourceByVendorPartNumber(string vendorpartnumber)
        {
            var items = Context.IngramLenovoScrapeSources
                              .AsNoTracking()
                              .Where(i => i.VendorPartNumber == vendorpartnumber);

 
            OnGetIngramLenovoScrapeSourceByVendorPartNumber(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnIngramLenovoScrapeSourceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnIngramLenovoScrapeSourceCreated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);
        partial void OnAfterIngramLenovoScrapeSourceCreated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> CreateIngramLenovoScrapeSource(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource ingramlenovoscrapesource)
        {
            OnIngramLenovoScrapeSourceCreated(ingramlenovoscrapesource);

            var existingItem = Context.IngramLenovoScrapeSources
                              .Where(i => i.VendorPartNumber == ingramlenovoscrapesource.VendorPartNumber)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.IngramLenovoScrapeSources.Add(ingramlenovoscrapesource);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ingramlenovoscrapesource).State = EntityState.Detached;
                throw;
            }

            OnAfterIngramLenovoScrapeSourceCreated(ingramlenovoscrapesource);

            return ingramlenovoscrapesource;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> CancelIngramLenovoScrapeSourceChanges(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnIngramLenovoScrapeSourceUpdated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);
        partial void OnAfterIngramLenovoScrapeSourceUpdated(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> UpdateIngramLenovoScrapeSource(string vendorpartnumber, CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource ingramlenovoscrapesource)
        {
            OnIngramLenovoScrapeSourceUpdated(ingramlenovoscrapesource);

            var itemToUpdate = Context.IngramLenovoScrapeSources
                              .Where(i => i.VendorPartNumber == ingramlenovoscrapesource.VendorPartNumber)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(ingramlenovoscrapesource).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterIngramLenovoScrapeSourceUpdated(ingramlenovoscrapesource);

            return ingramlenovoscrapesource;
        }

        partial void OnIngramLenovoScrapeSourceDeleted(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);
        partial void OnAfterIngramLenovoScrapeSourceDeleted(CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource> DeleteIngramLenovoScrapeSource(string vendorpartnumber)
        {
            var itemToDelete = Context.IngramLenovoScrapeSources
                              .Where(i => i.VendorPartNumber == vendorpartnumber)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnIngramLenovoScrapeSourceDeleted(itemToDelete);

            Reset();

            Context.IngramLenovoScrapeSources.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterIngramLenovoScrapeSourceDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportIngramManufacturerContentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingrammanufacturercontents/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingrammanufacturercontents/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramManufacturerContentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingrammanufacturercontents/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingrammanufacturercontents/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramManufacturerContentsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>> GetIngramManufacturerContents(Query query = null)
        {
            var items = Context.IngramManufacturerContents.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramManufacturerContentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIngramManufacturerContentGet(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);
        partial void OnGetIngramManufacturerContentByVendorNameAndVendorPartNumber(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> GetIngramManufacturerContentByVendorNameAndVendorPartNumber(string vendorname, string vendorpartnumber)
        {
            var items = Context.IngramManufacturerContents
                              .AsNoTracking()
                              .Where(i => i.VendorName == vendorname && i.VendorPartNumber == vendorpartnumber);

 
            OnGetIngramManufacturerContentByVendorNameAndVendorPartNumber(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnIngramManufacturerContentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnIngramManufacturerContentCreated(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);
        partial void OnAfterIngramManufacturerContentCreated(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> CreateIngramManufacturerContent(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent ingrammanufacturercontent)
        {
            OnIngramManufacturerContentCreated(ingrammanufacturercontent);

            var existingItem = Context.IngramManufacturerContents
                              .Where(i => i.VendorName == ingrammanufacturercontent.VendorName && i.VendorPartNumber == ingrammanufacturercontent.VendorPartNumber)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.IngramManufacturerContents.Add(ingrammanufacturercontent);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ingrammanufacturercontent).State = EntityState.Detached;
                throw;
            }

            OnAfterIngramManufacturerContentCreated(ingrammanufacturercontent);

            return ingrammanufacturercontent;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> CancelIngramManufacturerContentChanges(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnIngramManufacturerContentUpdated(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);
        partial void OnAfterIngramManufacturerContentUpdated(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> UpdateIngramManufacturerContent(string vendorname, string vendorpartnumber, CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent ingrammanufacturercontent)
        {
            OnIngramManufacturerContentUpdated(ingrammanufacturercontent);

            var itemToUpdate = Context.IngramManufacturerContents
                              .Where(i => i.VendorName == ingrammanufacturercontent.VendorName && i.VendorPartNumber == ingrammanufacturercontent.VendorPartNumber)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(ingrammanufacturercontent).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterIngramManufacturerContentUpdated(ingrammanufacturercontent);

            return ingrammanufacturercontent;
        }

        partial void OnIngramManufacturerContentDeleted(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);
        partial void OnAfterIngramManufacturerContentDeleted(CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent> DeleteIngramManufacturerContent(string vendorname, string vendorpartnumber)
        {
            var itemToDelete = Context.IngramManufacturerContents
                              .Where(i => i.VendorName == vendorname && i.VendorPartNumber == vendorpartnumber)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnIngramManufacturerContentDeleted(itemToDelete);

            Reset();

            Context.IngramManufacturerContents.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterIngramManufacturerContentDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportIngramProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproducts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproducts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproducts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproducts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramProductsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProduct> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProduct>> GetIngramProducts(Query query = null)
        {
            var items = Context.IngramProducts.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramProductsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIngramProductGet(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);
        partial void OnGetIngramProductByIngramPartNumber(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProduct> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProduct> GetIngramProductByIngramPartNumber(string ingrampartnumber)
        {
            var items = Context.IngramProducts
                              .AsNoTracking()
                              .Where(i => i.IngramPartNumber == ingrampartnumber);

 
            OnGetIngramProductByIngramPartNumber(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnIngramProductGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnIngramProductCreated(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);
        partial void OnAfterIngramProductCreated(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProduct> CreateIngramProduct(CrownATTime.Server.Models.DistributorProducts.IngramProduct ingramproduct)
        {
            OnIngramProductCreated(ingramproduct);

            var existingItem = Context.IngramProducts
                              .Where(i => i.IngramPartNumber == ingramproduct.IngramPartNumber)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.IngramProducts.Add(ingramproduct);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ingramproduct).State = EntityState.Detached;
                throw;
            }

            OnAfterIngramProductCreated(ingramproduct);

            return ingramproduct;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProduct> CancelIngramProductChanges(CrownATTime.Server.Models.DistributorProducts.IngramProduct item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnIngramProductUpdated(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);
        partial void OnAfterIngramProductUpdated(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProduct> UpdateIngramProduct(string ingrampartnumber, CrownATTime.Server.Models.DistributorProducts.IngramProduct ingramproduct)
        {
            OnIngramProductUpdated(ingramproduct);

            var itemToUpdate = Context.IngramProducts
                              .Where(i => i.IngramPartNumber == ingramproduct.IngramPartNumber)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(ingramproduct).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterIngramProductUpdated(ingramproduct);

            return ingramproduct;
        }

        partial void OnIngramProductDeleted(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);
        partial void OnAfterIngramProductDeleted(CrownATTime.Server.Models.DistributorProducts.IngramProduct item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProduct> DeleteIngramProduct(string ingrampartnumber)
        {
            var itemToDelete = Context.IngramProducts
                              .Where(i => i.IngramPartNumber == ingrampartnumber)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnIngramProductDeleted(itemToDelete);

            Reset();

            Context.IngramProducts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterIngramProductDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportIngramProductEnrichmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramProductEnrichmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramProductEnrichmentsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>> GetIngramProductEnrichments(Query query = null)
        {
            var items = Context.IngramProductEnrichments.AsQueryable();

            items = items.Include(i => i.IngramProduct);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramProductEnrichmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIngramProductEnrichmentGet(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);
        partial void OnGetIngramProductEnrichmentByIngramPartNumber(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> GetIngramProductEnrichmentByIngramPartNumber(string ingrampartnumber)
        {
            var items = Context.IngramProductEnrichments
                              .AsNoTracking()
                              .Where(i => i.IngramPartNumber == ingrampartnumber);

            items = items.Include(i => i.IngramProduct);
 
            OnGetIngramProductEnrichmentByIngramPartNumber(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnIngramProductEnrichmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnIngramProductEnrichmentCreated(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);
        partial void OnAfterIngramProductEnrichmentCreated(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> CreateIngramProductEnrichment(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment ingramproductenrichment)
        {
            OnIngramProductEnrichmentCreated(ingramproductenrichment);

            var existingItem = Context.IngramProductEnrichments
                              .Where(i => i.IngramPartNumber == ingramproductenrichment.IngramPartNumber)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.IngramProductEnrichments.Add(ingramproductenrichment);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ingramproductenrichment).State = EntityState.Detached;
                throw;
            }

            OnAfterIngramProductEnrichmentCreated(ingramproductenrichment);

            return ingramproductenrichment;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> CancelIngramProductEnrichmentChanges(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnIngramProductEnrichmentUpdated(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);
        partial void OnAfterIngramProductEnrichmentUpdated(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> UpdateIngramProductEnrichment(string ingrampartnumber, CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment ingramproductenrichment)
        {
            OnIngramProductEnrichmentUpdated(ingramproductenrichment);

            var itemToUpdate = Context.IngramProductEnrichments
                              .Where(i => i.IngramPartNumber == ingramproductenrichment.IngramPartNumber)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            ingramproductenrichment.IngramProduct = null;

            Context.Attach(ingramproductenrichment).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterIngramProductEnrichmentUpdated(ingramproductenrichment);

            return ingramproductenrichment;
        }

        partial void OnIngramProductEnrichmentDeleted(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);
        partial void OnAfterIngramProductEnrichmentDeleted(CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment> DeleteIngramProductEnrichment(string ingrampartnumber)
        {
            var itemToDelete = Context.IngramProductEnrichments
                              .Where(i => i.IngramPartNumber == ingrampartnumber)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnIngramProductEnrichmentDeleted(itemToDelete);

            Reset();

            Context.IngramProductEnrichments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterIngramProductEnrichmentDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportIngramSyncRulesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramsyncrules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramsyncrules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramSyncRulesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramsyncrules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramsyncrules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramSyncRulesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>> GetIngramSyncRules(Query query = null)
        {
            var items = Context.IngramSyncRules.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramSyncRulesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIngramSyncRuleGet(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);
        partial void OnGetIngramSyncRuleByRuleId(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> GetIngramSyncRuleByRuleId(int ruleid)
        {
            var items = Context.IngramSyncRules
                              .AsNoTracking()
                              .Where(i => i.RuleId == ruleid);

 
            OnGetIngramSyncRuleByRuleId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnIngramSyncRuleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnIngramSyncRuleCreated(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);
        partial void OnAfterIngramSyncRuleCreated(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> CreateIngramSyncRule(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule ingramsyncrule)
        {
            OnIngramSyncRuleCreated(ingramsyncrule);

            var existingItem = Context.IngramSyncRules
                              .Where(i => i.RuleId == ingramsyncrule.RuleId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.IngramSyncRules.Add(ingramsyncrule);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ingramsyncrule).State = EntityState.Detached;
                throw;
            }

            OnAfterIngramSyncRuleCreated(ingramsyncrule);

            return ingramsyncrule;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> CancelIngramSyncRuleChanges(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnIngramSyncRuleUpdated(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);
        partial void OnAfterIngramSyncRuleUpdated(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> UpdateIngramSyncRule(int ruleid, CrownATTime.Server.Models.DistributorProducts.IngramSyncRule ingramsyncrule)
        {
            OnIngramSyncRuleUpdated(ingramsyncrule);

            var itemToUpdate = Context.IngramSyncRules
                              .Where(i => i.RuleId == ingramsyncrule.RuleId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(ingramsyncrule).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterIngramSyncRuleUpdated(ingramsyncrule);

            return ingramsyncrule;
        }

        partial void OnIngramSyncRuleDeleted(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);
        partial void OnAfterIngramSyncRuleDeleted(CrownATTime.Server.Models.DistributorProducts.IngramSyncRule item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule> DeleteIngramSyncRule(int ruleid)
        {
            var itemToDelete = Context.IngramSyncRules
                              .Where(i => i.RuleId == ruleid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnIngramSyncRuleDeleted(itemToDelete);

            Reset();

            Context.IngramSyncRules.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterIngramSyncRuleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportIngramWarehouseInventoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramwarehouseinventories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramwarehouseinventories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramWarehouseInventoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramwarehouseinventories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramwarehouseinventories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramWarehouseInventoriesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>> GetIngramWarehouseInventories(Query query = null)
        {
            var items = Context.IngramWarehouseInventories.AsQueryable();

            items = items.Include(i => i.IngramProduct);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramWarehouseInventoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnIngramWarehouseInventoryGet(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);
        partial void OnGetIngramWarehouseInventoryByIngramPartNumberAndWarehouseLocation(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> GetIngramWarehouseInventoryByIngramPartNumberAndWarehouseLocation(string ingrampartnumber, string warehouselocation)
        {
            var items = Context.IngramWarehouseInventories
                              .AsNoTracking()
                              .Where(i => i.IngramPartNumber == ingrampartnumber && i.WarehouseLocation == warehouselocation);

            items = items.Include(i => i.IngramProduct);
 
            OnGetIngramWarehouseInventoryByIngramPartNumberAndWarehouseLocation(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnIngramWarehouseInventoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnIngramWarehouseInventoryCreated(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);
        partial void OnAfterIngramWarehouseInventoryCreated(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> CreateIngramWarehouseInventory(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory ingramwarehouseinventory)
        {
            OnIngramWarehouseInventoryCreated(ingramwarehouseinventory);

            var existingItem = Context.IngramWarehouseInventories
                              .Where(i => i.IngramPartNumber == ingramwarehouseinventory.IngramPartNumber && i.WarehouseLocation == ingramwarehouseinventory.WarehouseLocation)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.IngramWarehouseInventories.Add(ingramwarehouseinventory);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ingramwarehouseinventory).State = EntityState.Detached;
                throw;
            }

            OnAfterIngramWarehouseInventoryCreated(ingramwarehouseinventory);

            return ingramwarehouseinventory;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> CancelIngramWarehouseInventoryChanges(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnIngramWarehouseInventoryUpdated(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);
        partial void OnAfterIngramWarehouseInventoryUpdated(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> UpdateIngramWarehouseInventory(string ingrampartnumber, string warehouselocation, CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory ingramwarehouseinventory)
        {
            OnIngramWarehouseInventoryUpdated(ingramwarehouseinventory);

            var itemToUpdate = Context.IngramWarehouseInventories
                              .Where(i => i.IngramPartNumber == ingramwarehouseinventory.IngramPartNumber && i.WarehouseLocation == ingramwarehouseinventory.WarehouseLocation)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            ingramwarehouseinventory.IngramProduct = null;

            Context.Attach(ingramwarehouseinventory).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterIngramWarehouseInventoryUpdated(ingramwarehouseinventory);

            return ingramwarehouseinventory;
        }

        partial void OnIngramWarehouseInventoryDeleted(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);
        partial void OnAfterIngramWarehouseInventoryDeleted(CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory> DeleteIngramWarehouseInventory(string ingrampartnumber, string warehouselocation)
        {
            var itemToDelete = Context.IngramWarehouseInventories
                              .Where(i => i.IngramPartNumber == ingrampartnumber && i.WarehouseLocation == warehouselocation)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnIngramWarehouseInventoryDeleted(itemToDelete);

            Reset();

            Context.IngramWarehouseInventories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterIngramWarehouseInventoryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLenovoPsrefeditionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefeditions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefeditions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsrefeditionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefeditions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefeditions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsrefeditionsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>> GetLenovoPsrefeditions(Query query = null)
        {
            var items = Context.LenovoPsrefeditions.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsrefeditionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLenovoPsrefeditionGet(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);
        partial void OnGetLenovoPsrefeditionByEditionId(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> GetLenovoPsrefeditionByEditionId(long editionid)
        {
            var items = Context.LenovoPsrefeditions
                              .AsNoTracking()
                              .Where(i => i.EditionId == editionid);

 
            OnGetLenovoPsrefeditionByEditionId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLenovoPsrefeditionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLenovoPsrefeditionCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);
        partial void OnAfterLenovoPsrefeditionCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> CreateLenovoPsrefedition(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition lenovopsrefedition)
        {
            OnLenovoPsrefeditionCreated(lenovopsrefedition);

            var existingItem = Context.LenovoPsrefeditions
                              .Where(i => i.EditionId == lenovopsrefedition.EditionId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.LenovoPsrefeditions.Add(lenovopsrefedition);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(lenovopsrefedition).State = EntityState.Detached;
                throw;
            }

            OnAfterLenovoPsrefeditionCreated(lenovopsrefedition);

            return lenovopsrefedition;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> CancelLenovoPsrefeditionChanges(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLenovoPsrefeditionUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);
        partial void OnAfterLenovoPsrefeditionUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> UpdateLenovoPsrefedition(long editionid, CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition lenovopsrefedition)
        {
            OnLenovoPsrefeditionUpdated(lenovopsrefedition);

            var itemToUpdate = Context.LenovoPsrefeditions
                              .Where(i => i.EditionId == lenovopsrefedition.EditionId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(lenovopsrefedition).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLenovoPsrefeditionUpdated(lenovopsrefedition);

            return lenovopsrefedition;
        }

        partial void OnLenovoPsrefeditionDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);
        partial void OnAfterLenovoPsrefeditionDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition> DeleteLenovoPsrefedition(long editionid)
        {
            var itemToDelete = Context.LenovoPsrefeditions
                              .Where(i => i.EditionId == editionid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLenovoPsrefeditionDeleted(itemToDelete);

            Reset();

            Context.LenovoPsrefeditions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLenovoPsrefeditionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLenovoPsreffamiliesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsreffamilies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsreffamilies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsreffamiliesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsreffamilies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsreffamilies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsreffamiliesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>> GetLenovoPsreffamilies(Query query = null)
        {
            var items = Context.LenovoPsreffamilies.AsQueryable();

            items = items.Include(i => i.LenovoPsrefedition);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsreffamiliesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLenovoPsreffamilyGet(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);
        partial void OnGetLenovoPsreffamilyByFamilyId(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> GetLenovoPsreffamilyByFamilyId(long familyid)
        {
            var items = Context.LenovoPsreffamilies
                              .AsNoTracking()
                              .Where(i => i.FamilyId == familyid);

            items = items.Include(i => i.LenovoPsrefedition);
 
            OnGetLenovoPsreffamilyByFamilyId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLenovoPsreffamilyGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLenovoPsreffamilyCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);
        partial void OnAfterLenovoPsreffamilyCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> CreateLenovoPsreffamily(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily lenovopsreffamily)
        {
            OnLenovoPsreffamilyCreated(lenovopsreffamily);

            var existingItem = Context.LenovoPsreffamilies
                              .Where(i => i.FamilyId == lenovopsreffamily.FamilyId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.LenovoPsreffamilies.Add(lenovopsreffamily);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(lenovopsreffamily).State = EntityState.Detached;
                throw;
            }

            OnAfterLenovoPsreffamilyCreated(lenovopsreffamily);

            return lenovopsreffamily;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> CancelLenovoPsreffamilyChanges(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLenovoPsreffamilyUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);
        partial void OnAfterLenovoPsreffamilyUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> UpdateLenovoPsreffamily(long familyid, CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily lenovopsreffamily)
        {
            OnLenovoPsreffamilyUpdated(lenovopsreffamily);

            var itemToUpdate = Context.LenovoPsreffamilies
                              .Where(i => i.FamilyId == lenovopsreffamily.FamilyId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            lenovopsreffamily.LenovoPsrefedition = null;

            Context.Attach(lenovopsreffamily).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLenovoPsreffamilyUpdated(lenovopsreffamily);

            return lenovopsreffamily;
        }

        partial void OnLenovoPsreffamilyDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);
        partial void OnAfterLenovoPsreffamilyDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily> DeleteLenovoPsreffamily(long familyid)
        {
            var itemToDelete = Context.LenovoPsreffamilies
                              .Where(i => i.FamilyId == familyid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLenovoPsreffamilyDeleted(itemToDelete);

            Reset();

            Context.LenovoPsreffamilies.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLenovoPsreffamilyDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLenovoPsreffamilyImagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsreffamilyimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsreffamilyimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsreffamilyImagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsreffamilyimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsreffamilyimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsreffamilyImagesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>> GetLenovoPsreffamilyImages(Query query = null)
        {
            var items = Context.LenovoPsreffamilyImages.AsQueryable();

            items = items.Include(i => i.LenovoPsreffamily);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsreffamilyImagesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLenovoPsreffamilyImageGet(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);
        partial void OnGetLenovoPsreffamilyImageByImageId(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> GetLenovoPsreffamilyImageByImageId(long imageid)
        {
            var items = Context.LenovoPsreffamilyImages
                              .AsNoTracking()
                              .Where(i => i.ImageId == imageid);

            items = items.Include(i => i.LenovoPsreffamily);
 
            OnGetLenovoPsreffamilyImageByImageId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLenovoPsreffamilyImageGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLenovoPsreffamilyImageCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);
        partial void OnAfterLenovoPsreffamilyImageCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> CreateLenovoPsreffamilyImage(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage lenovopsreffamilyimage)
        {
            OnLenovoPsreffamilyImageCreated(lenovopsreffamilyimage);

            var existingItem = Context.LenovoPsreffamilyImages
                              .Where(i => i.ImageId == lenovopsreffamilyimage.ImageId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.LenovoPsreffamilyImages.Add(lenovopsreffamilyimage);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(lenovopsreffamilyimage).State = EntityState.Detached;
                throw;
            }

            OnAfterLenovoPsreffamilyImageCreated(lenovopsreffamilyimage);

            return lenovopsreffamilyimage;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> CancelLenovoPsreffamilyImageChanges(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLenovoPsreffamilyImageUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);
        partial void OnAfterLenovoPsreffamilyImageUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> UpdateLenovoPsreffamilyImage(long imageid, CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage lenovopsreffamilyimage)
        {
            OnLenovoPsreffamilyImageUpdated(lenovopsreffamilyimage);

            var itemToUpdate = Context.LenovoPsreffamilyImages
                              .Where(i => i.ImageId == lenovopsreffamilyimage.ImageId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            lenovopsreffamilyimage.LenovoPsreffamily = null;

            Context.Attach(lenovopsreffamilyimage).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLenovoPsreffamilyImageUpdated(lenovopsreffamilyimage);

            return lenovopsreffamilyimage;
        }

        partial void OnLenovoPsreffamilyImageDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);
        partial void OnAfterLenovoPsreffamilyImageDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage> DeleteLenovoPsreffamilyImage(long imageid)
        {
            var itemToDelete = Context.LenovoPsreffamilyImages
                              .Where(i => i.ImageId == imageid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLenovoPsreffamilyImageDeleted(itemToDelete);

            Reset();

            Context.LenovoPsreffamilyImages.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLenovoPsreffamilyImageDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLenovoPsrefgalleryBeforeV27SToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefgallerybeforev27s/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefgallerybeforev27s/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsrefgalleryBeforeV27SToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefgallerybeforev27s/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefgallerybeforev27s/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsrefgalleryBeforeV27SRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27>> GetLenovoPsrefgalleryBeforeV27S(Query query = null)
        {
            var items = Context.LenovoPsrefgalleryBeforeV27S.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsrefgalleryBeforeV27SRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportLenovoPsrefimageDataToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefimagedata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefimagedata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsrefimageDataToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefimagedata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefimagedata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsrefimageDataRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>> GetLenovoPsrefimageData(Query query = null)
        {
            var items = Context.LenovoPsrefimageData.AsQueryable();

            items = items.Include(i => i.LenovoPsreffamilyImage);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsrefimageDataRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLenovoPsrefimageDatumGet(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);
        partial void OnGetLenovoPsrefimageDatumByImageId(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> GetLenovoPsrefimageDatumByImageId(long imageid)
        {
            var items = Context.LenovoPsrefimageData
                              .AsNoTracking()
                              .Where(i => i.ImageId == imageid);

            items = items.Include(i => i.LenovoPsreffamilyImage);
 
            OnGetLenovoPsrefimageDatumByImageId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLenovoPsrefimageDatumGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLenovoPsrefimageDatumCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);
        partial void OnAfterLenovoPsrefimageDatumCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> CreateLenovoPsrefimageDatum(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum lenovopsrefimagedatum)
        {
            OnLenovoPsrefimageDatumCreated(lenovopsrefimagedatum);

            var existingItem = Context.LenovoPsrefimageData
                              .Where(i => i.ImageId == lenovopsrefimagedatum.ImageId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.LenovoPsrefimageData.Add(lenovopsrefimagedatum);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(lenovopsrefimagedatum).State = EntityState.Detached;
                throw;
            }

            OnAfterLenovoPsrefimageDatumCreated(lenovopsrefimagedatum);

            return lenovopsrefimagedatum;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> CancelLenovoPsrefimageDatumChanges(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLenovoPsrefimageDatumUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);
        partial void OnAfterLenovoPsrefimageDatumUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> UpdateLenovoPsrefimageDatum(long imageid, CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum lenovopsrefimagedatum)
        {
            OnLenovoPsrefimageDatumUpdated(lenovopsrefimagedatum);

            var itemToUpdate = Context.LenovoPsrefimageData
                              .Where(i => i.ImageId == lenovopsrefimagedatum.ImageId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            lenovopsrefimagedatum.LenovoPsreffamilyImage = null;

            Context.Attach(lenovopsrefimagedatum).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLenovoPsrefimageDatumUpdated(lenovopsrefimagedatum);

            return lenovopsrefimagedatum;
        }

        partial void OnLenovoPsrefimageDatumDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);
        partial void OnAfterLenovoPsrefimageDatumDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum> DeleteLenovoPsrefimageDatum(long imageid)
        {
            var itemToDelete = Context.LenovoPsrefimageData
                              .Where(i => i.ImageId == imageid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLenovoPsrefimageDatumDeleted(itemToDelete);

            Reset();

            Context.LenovoPsrefimageData.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLenovoPsrefimageDatumDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLenovoPsrefskuFamilyMapsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefskufamilymaps/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefskufamilymaps/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsrefskuFamilyMapsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefskufamilymaps/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefskufamilymaps/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsrefskuFamilyMapsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>> GetLenovoPsrefskuFamilyMaps(Query query = null)
        {
            var items = Context.LenovoPsrefskuFamilyMaps.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsrefskuFamilyMapsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLenovoPsrefskuFamilyMapGet(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);
        partial void OnGetLenovoPsrefskuFamilyMapByVendorPartNumber(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> GetLenovoPsrefskuFamilyMapByVendorPartNumber(string vendorpartnumber)
        {
            var items = Context.LenovoPsrefskuFamilyMaps
                              .AsNoTracking()
                              .Where(i => i.VendorPartNumber == vendorpartnumber);

 
            OnGetLenovoPsrefskuFamilyMapByVendorPartNumber(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLenovoPsrefskuFamilyMapGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLenovoPsrefskuFamilyMapCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);
        partial void OnAfterLenovoPsrefskuFamilyMapCreated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> CreateLenovoPsrefskuFamilyMap(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap lenovopsrefskufamilymap)
        {
            OnLenovoPsrefskuFamilyMapCreated(lenovopsrefskufamilymap);

            var existingItem = Context.LenovoPsrefskuFamilyMaps
                              .Where(i => i.VendorPartNumber == lenovopsrefskufamilymap.VendorPartNumber)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.LenovoPsrefskuFamilyMaps.Add(lenovopsrefskufamilymap);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(lenovopsrefskufamilymap).State = EntityState.Detached;
                throw;
            }

            OnAfterLenovoPsrefskuFamilyMapCreated(lenovopsrefskufamilymap);

            return lenovopsrefskufamilymap;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> CancelLenovoPsrefskuFamilyMapChanges(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLenovoPsrefskuFamilyMapUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);
        partial void OnAfterLenovoPsrefskuFamilyMapUpdated(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> UpdateLenovoPsrefskuFamilyMap(string vendorpartnumber, CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap lenovopsrefskufamilymap)
        {
            OnLenovoPsrefskuFamilyMapUpdated(lenovopsrefskufamilymap);

            var itemToUpdate = Context.LenovoPsrefskuFamilyMaps
                              .Where(i => i.VendorPartNumber == lenovopsrefskufamilymap.VendorPartNumber)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(lenovopsrefskufamilymap).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLenovoPsrefskuFamilyMapUpdated(lenovopsrefskufamilymap);

            return lenovopsrefskufamilymap;
        }

        partial void OnLenovoPsrefskuFamilyMapDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);
        partial void OnAfterLenovoPsrefskuFamilyMapDeleted(CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap> DeleteLenovoPsrefskuFamilyMap(string vendorpartnumber)
        {
            var itemToDelete = Context.LenovoPsrefskuFamilyMaps
                              .Where(i => i.VendorPartNumber == vendorpartnumber)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLenovoPsrefskuFamilyMapDeleted(itemToDelete);

            Reset();

            Context.LenovoPsrefskuFamilyMaps.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLenovoPsrefskuFamilyMapDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStreakwaveProductsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproducts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproducts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStreakwaveProductsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproducts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproducts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStreakwaveProductsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>> GetStreakwaveProducts(Query query = null)
        {
            var items = Context.StreakwaveProducts.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStreakwaveProductsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStreakwaveProductGet(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);
        partial void OnGetStreakwaveProductBySku(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> GetStreakwaveProductBySku(string sku)
        {
            var items = Context.StreakwaveProducts
                              .AsNoTracking()
                              .Where(i => i.Sku == sku);

 
            OnGetStreakwaveProductBySku(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStreakwaveProductGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStreakwaveProductCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);
        partial void OnAfterStreakwaveProductCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> CreateStreakwaveProduct(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct streakwaveproduct)
        {
            OnStreakwaveProductCreated(streakwaveproduct);

            var existingItem = Context.StreakwaveProducts
                              .Where(i => i.Sku == streakwaveproduct.Sku)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StreakwaveProducts.Add(streakwaveproduct);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(streakwaveproduct).State = EntityState.Detached;
                throw;
            }

            OnAfterStreakwaveProductCreated(streakwaveproduct);

            return streakwaveproduct;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> CancelStreakwaveProductChanges(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStreakwaveProductUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);
        partial void OnAfterStreakwaveProductUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> UpdateStreakwaveProduct(string sku, CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct streakwaveproduct)
        {
            OnStreakwaveProductUpdated(streakwaveproduct);

            var itemToUpdate = Context.StreakwaveProducts
                              .Where(i => i.Sku == streakwaveproduct.Sku)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(streakwaveproduct).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStreakwaveProductUpdated(streakwaveproduct);

            return streakwaveproduct;
        }

        partial void OnStreakwaveProductDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);
        partial void OnAfterStreakwaveProductDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct> DeleteStreakwaveProduct(string sku)
        {
            var itemToDelete = Context.StreakwaveProducts
                              .Where(i => i.Sku == sku)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStreakwaveProductDeleted(itemToDelete);

            Reset();

            Context.StreakwaveProducts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStreakwaveProductDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStreakwaveProductEnrichmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStreakwaveProductEnrichmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStreakwaveProductEnrichmentsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>> GetStreakwaveProductEnrichments(Query query = null)
        {
            var items = Context.StreakwaveProductEnrichments.AsQueryable();

            items = items.Include(i => i.StreakwaveProduct);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStreakwaveProductEnrichmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStreakwaveProductEnrichmentGet(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);
        partial void OnGetStreakwaveProductEnrichmentBySku(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> GetStreakwaveProductEnrichmentBySku(string sku)
        {
            var items = Context.StreakwaveProductEnrichments
                              .AsNoTracking()
                              .Where(i => i.Sku == sku);

            items = items.Include(i => i.StreakwaveProduct);
 
            OnGetStreakwaveProductEnrichmentBySku(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStreakwaveProductEnrichmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStreakwaveProductEnrichmentCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);
        partial void OnAfterStreakwaveProductEnrichmentCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> CreateStreakwaveProductEnrichment(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment streakwaveproductenrichment)
        {
            OnStreakwaveProductEnrichmentCreated(streakwaveproductenrichment);

            var existingItem = Context.StreakwaveProductEnrichments
                              .Where(i => i.Sku == streakwaveproductenrichment.Sku)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StreakwaveProductEnrichments.Add(streakwaveproductenrichment);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(streakwaveproductenrichment).State = EntityState.Detached;
                throw;
            }

            OnAfterStreakwaveProductEnrichmentCreated(streakwaveproductenrichment);

            return streakwaveproductenrichment;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> CancelStreakwaveProductEnrichmentChanges(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStreakwaveProductEnrichmentUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);
        partial void OnAfterStreakwaveProductEnrichmentUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> UpdateStreakwaveProductEnrichment(string sku, CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment streakwaveproductenrichment)
        {
            OnStreakwaveProductEnrichmentUpdated(streakwaveproductenrichment);

            var itemToUpdate = Context.StreakwaveProductEnrichments
                              .Where(i => i.Sku == streakwaveproductenrichment.Sku)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            streakwaveproductenrichment.StreakwaveProduct = null;

            Context.Attach(streakwaveproductenrichment).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStreakwaveProductEnrichmentUpdated(streakwaveproductenrichment);

            return streakwaveproductenrichment;
        }

        partial void OnStreakwaveProductEnrichmentDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);
        partial void OnAfterStreakwaveProductEnrichmentDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment> DeleteStreakwaveProductEnrichment(string sku)
        {
            var itemToDelete = Context.StreakwaveProductEnrichments
                              .Where(i => i.Sku == sku)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStreakwaveProductEnrichmentDeleted(itemToDelete);

            Reset();

            Context.StreakwaveProductEnrichments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStreakwaveProductEnrichmentDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStreakwaveSyncRulesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwavesyncrules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwavesyncrules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStreakwaveSyncRulesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwavesyncrules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwavesyncrules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStreakwaveSyncRulesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>> GetStreakwaveSyncRules(Query query = null)
        {
            var items = Context.StreakwaveSyncRules.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStreakwaveSyncRulesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStreakwaveSyncRuleGet(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);
        partial void OnGetStreakwaveSyncRuleByRuleId(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> GetStreakwaveSyncRuleByRuleId(int ruleid)
        {
            var items = Context.StreakwaveSyncRules
                              .AsNoTracking()
                              .Where(i => i.RuleId == ruleid);

 
            OnGetStreakwaveSyncRuleByRuleId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStreakwaveSyncRuleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStreakwaveSyncRuleCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);
        partial void OnAfterStreakwaveSyncRuleCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> CreateStreakwaveSyncRule(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule streakwavesyncrule)
        {
            OnStreakwaveSyncRuleCreated(streakwavesyncrule);

            var existingItem = Context.StreakwaveSyncRules
                              .Where(i => i.RuleId == streakwavesyncrule.RuleId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StreakwaveSyncRules.Add(streakwavesyncrule);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(streakwavesyncrule).State = EntityState.Detached;
                throw;
            }

            OnAfterStreakwaveSyncRuleCreated(streakwavesyncrule);

            return streakwavesyncrule;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> CancelStreakwaveSyncRuleChanges(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStreakwaveSyncRuleUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);
        partial void OnAfterStreakwaveSyncRuleUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> UpdateStreakwaveSyncRule(int ruleid, CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule streakwavesyncrule)
        {
            OnStreakwaveSyncRuleUpdated(streakwavesyncrule);

            var itemToUpdate = Context.StreakwaveSyncRules
                              .Where(i => i.RuleId == streakwavesyncrule.RuleId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(streakwavesyncrule).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStreakwaveSyncRuleUpdated(streakwavesyncrule);

            return streakwavesyncrule;
        }

        partial void OnStreakwaveSyncRuleDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);
        partial void OnAfterStreakwaveSyncRuleDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule> DeleteStreakwaveSyncRule(int ruleid)
        {
            var itemToDelete = Context.StreakwaveSyncRules
                              .Where(i => i.RuleId == ruleid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStreakwaveSyncRuleDeleted(itemToDelete);

            Reset();

            Context.StreakwaveSyncRules.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStreakwaveSyncRuleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStreakwaveWarehouseStocksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwavewarehousestocks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwavewarehousestocks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStreakwaveWarehouseStocksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwavewarehousestocks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwavewarehousestocks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStreakwaveWarehouseStocksRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>> GetStreakwaveWarehouseStocks(Query query = null)
        {
            var items = Context.StreakwaveWarehouseStocks.AsQueryable();

            items = items.Include(i => i.StreakwaveProduct);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStreakwaveWarehouseStocksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStreakwaveWarehouseStockGet(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);
        partial void OnGetStreakwaveWarehouseStockBySkuAndWarehouseCode(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> items);


        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> GetStreakwaveWarehouseStockBySkuAndWarehouseCode(string sku, string warehousecode)
        {
            var items = Context.StreakwaveWarehouseStocks
                              .AsNoTracking()
                              .Where(i => i.Sku == sku && i.WarehouseCode == warehousecode);

            items = items.Include(i => i.StreakwaveProduct);
 
            OnGetStreakwaveWarehouseStockBySkuAndWarehouseCode(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStreakwaveWarehouseStockGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStreakwaveWarehouseStockCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);
        partial void OnAfterStreakwaveWarehouseStockCreated(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> CreateStreakwaveWarehouseStock(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock streakwavewarehousestock)
        {
            OnStreakwaveWarehouseStockCreated(streakwavewarehousestock);

            var existingItem = Context.StreakwaveWarehouseStocks
                              .Where(i => i.Sku == streakwavewarehousestock.Sku && i.WarehouseCode == streakwavewarehousestock.WarehouseCode)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StreakwaveWarehouseStocks.Add(streakwavewarehousestock);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(streakwavewarehousestock).State = EntityState.Detached;
                throw;
            }

            OnAfterStreakwaveWarehouseStockCreated(streakwavewarehousestock);

            return streakwavewarehousestock;
        }

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> CancelStreakwaveWarehouseStockChanges(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStreakwaveWarehouseStockUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);
        partial void OnAfterStreakwaveWarehouseStockUpdated(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> UpdateStreakwaveWarehouseStock(string sku, string warehousecode, CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock streakwavewarehousestock)
        {
            OnStreakwaveWarehouseStockUpdated(streakwavewarehousestock);

            var itemToUpdate = Context.StreakwaveWarehouseStocks
                              .Where(i => i.Sku == streakwavewarehousestock.Sku && i.WarehouseCode == streakwavewarehousestock.WarehouseCode)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            streakwavewarehousestock.StreakwaveProduct = null;

            Context.Attach(streakwavewarehousestock).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStreakwaveWarehouseStockUpdated(streakwavewarehousestock);

            return streakwavewarehousestock;
        }

        partial void OnStreakwaveWarehouseStockDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);
        partial void OnAfterStreakwaveWarehouseStockDeleted(CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock item);

        public async Task<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock> DeleteStreakwaveWarehouseStock(string sku, string warehousecode)
        {
            var itemToDelete = Context.StreakwaveWarehouseStocks
                              .Where(i => i.Sku == sku && i.WarehouseCode == warehousecode)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStreakwaveWarehouseStockDeleted(itemToDelete);

            Reset();

            Context.StreakwaveWarehouseStocks.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStreakwaveWarehouseStockDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportIngramProductsForQuotingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductsforquotings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductsforquotings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramProductsForQuotingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductsforquotings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductsforquotings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramProductsForQuotingsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>> GetIngramProductsForQuotings(Query query = null)
        {
            var items = Context.IngramProductsForQuotings.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramProductsForQuotingsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportIngramProductsWithDetailsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductswithdetails/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductswithdetails/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportIngramProductsWithDetailsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/ingramproductswithdetails/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/ingramproductswithdetails/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnIngramProductsWithDetailsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>> GetIngramProductsWithDetails(Query query = null)
        {
            var items = Context.IngramProductsWithDetails.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnIngramProductsWithDetailsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportLenovoPsrefapprovedSkuImagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefapprovedskuimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefapprovedskuimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsrefapprovedSkuImagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefapprovedskuimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefapprovedskuimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsrefapprovedSkuImagesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefapprovedSkuImage> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefapprovedSkuImage>> GetLenovoPsrefapprovedSkuImages(Query query = null)
        {
            var items = Context.LenovoPsrefapprovedSkuImages.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsrefapprovedSkuImagesRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportLenovoPsrefcurrentFamiliesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefcurrentfamilies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefcurrentfamilies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsrefcurrentFamiliesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefcurrentfamilies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefcurrentfamilies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsrefcurrentFamiliesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefcurrentFamily> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefcurrentFamily>> GetLenovoPsrefcurrentFamilies(Query query = null)
        {
            var items = Context.LenovoPsrefcurrentFamilies.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsrefcurrentFamiliesRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportLenovoPsrefimageStorageSummariesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefimagestoragesummaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefimagestoragesummaries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsrefimageStorageSummariesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefimagestoragesummaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefimagestoragesummaries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsrefimageStorageSummariesRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageStorageSummary> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageStorageSummary>> GetLenovoPsrefimageStorageSummaries(Query query = null)
        {
            var items = Context.LenovoPsrefimageStorageSummaries.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsrefimageStorageSummariesRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportLenovoPsrefunmatchedSkusToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefunmatchedskus/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefunmatchedskus/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLenovoPsrefunmatchedSkusToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/lenovopsrefunmatchedskus/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/lenovopsrefunmatchedskus/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLenovoPsrefunmatchedSkusRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefunmatchedSku> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefunmatchedSku>> GetLenovoPsrefunmatchedSkus(Query query = null)
        {
            var items = Context.LenovoPsrefunmatchedSkus.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLenovoPsrefunmatchedSkusRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportStreakwaveProductsForQuotingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductsforquotings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductsforquotings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStreakwaveProductsForQuotingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductsforquotings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductsforquotings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStreakwaveProductsForQuotingsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>> GetStreakwaveProductsForQuotings(Query query = null)
        {
            var items = Context.StreakwaveProductsForQuotings.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStreakwaveProductsForQuotingsRead(ref items);

            return await Task.FromResult(items);
        }

        public async Task ExportStreakwaveProductsWithEnrichmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductswithenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductswithenrichments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStreakwaveProductsWithEnrichmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/distributorproducts/streakwaveproductswithenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/distributorproducts/streakwaveproductswithenrichments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStreakwaveProductsWithEnrichmentsRead(ref IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment> items);

        public async Task<IQueryable<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>> GetStreakwaveProductsWithEnrichments(Query query = null)
        {
            var items = Context.StreakwaveProductsWithEnrichments.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStreakwaveProductsWithEnrichmentsRead(ref items);

            return await Task.FromResult(items);
        }
    }
}