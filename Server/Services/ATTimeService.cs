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
    public partial class ATTimeService
    {
        ATTimeContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ATTimeContext context;
        private readonly NavigationManager navigationManager;

        public ATTimeService(ATTimeContext context, NavigationManager navigationManager)
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


        public async Task ExportAiPromptConfigurationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/aipromptconfigurations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/aipromptconfigurations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAiPromptConfigurationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/aipromptconfigurations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/aipromptconfigurations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAiPromptConfigurationsRead(ref IQueryable<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.AiPromptConfiguration>> GetAiPromptConfigurations(Query query = null)
        {
            var items = Context.AiPromptConfigurations.AsQueryable();

            items = items.Include(i => i.TimeGuardSection);

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

            OnAiPromptConfigurationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAiPromptConfigurationGet(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);
        partial void OnGetAiPromptConfigurationByAiPromptConfigurationId(ref IQueryable<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> items);


        public async Task<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> GetAiPromptConfigurationByAiPromptConfigurationId(int aipromptconfigurationid)
        {
            var items = Context.AiPromptConfigurations
                              .AsNoTracking()
                              .Where(i => i.AiPromptConfigurationId == aipromptconfigurationid);

            items = items.Include(i => i.TimeGuardSection);
 
            OnGetAiPromptConfigurationByAiPromptConfigurationId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAiPromptConfigurationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAiPromptConfigurationCreated(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);
        partial void OnAfterAiPromptConfigurationCreated(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);

        public async Task<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> CreateAiPromptConfiguration(CrownATTime.Server.Models.ATTime.AiPromptConfiguration aipromptconfiguration)
        {
            OnAiPromptConfigurationCreated(aipromptconfiguration);

            var existingItem = Context.AiPromptConfigurations
                              .Where(i => i.AiPromptConfigurationId == aipromptconfiguration.AiPromptConfigurationId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AiPromptConfigurations.Add(aipromptconfiguration);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(aipromptconfiguration).State = EntityState.Detached;
                throw;
            }

            OnAfterAiPromptConfigurationCreated(aipromptconfiguration);

            return aipromptconfiguration;
        }

        public async Task<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> CancelAiPromptConfigurationChanges(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAiPromptConfigurationUpdated(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);
        partial void OnAfterAiPromptConfigurationUpdated(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);

        public async Task<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> UpdateAiPromptConfiguration(int aipromptconfigurationid, CrownATTime.Server.Models.ATTime.AiPromptConfiguration aipromptconfiguration)
        {
            OnAiPromptConfigurationUpdated(aipromptconfiguration);

            var itemToUpdate = Context.AiPromptConfigurations
                              .Where(i => i.AiPromptConfigurationId == aipromptconfiguration.AiPromptConfigurationId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            aipromptconfiguration.TimeGuardSection = null;

            Context.Attach(aipromptconfiguration).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAiPromptConfigurationUpdated(aipromptconfiguration);

            return aipromptconfiguration;
        }

        partial void OnAiPromptConfigurationDeleted(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);
        partial void OnAfterAiPromptConfigurationDeleted(CrownATTime.Server.Models.ATTime.AiPromptConfiguration item);

        public async Task<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> DeleteAiPromptConfiguration(int aipromptconfigurationid)
        {
            var itemToDelete = Context.AiPromptConfigurations
                              .Where(i => i.AiPromptConfigurationId == aipromptconfigurationid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAiPromptConfigurationDeleted(itemToDelete);

            Reset();

            Context.AiPromptConfigurations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAiPromptConfigurationDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBillingCodeCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/billingcodecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/billingcodecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBillingCodeCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/billingcodecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/billingcodecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBillingCodeCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.BillingCodeCache> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.BillingCodeCache>> GetBillingCodeCaches(Query query = null)
        {
            var items = Context.BillingCodeCaches.AsQueryable();


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

            OnBillingCodeCachesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBillingCodeCacheGet(CrownATTime.Server.Models.ATTime.BillingCodeCache item);
        partial void OnGetBillingCodeCacheById(ref IQueryable<CrownATTime.Server.Models.ATTime.BillingCodeCache> items);


        public async Task<CrownATTime.Server.Models.ATTime.BillingCodeCache> GetBillingCodeCacheById(int id)
        {
            var items = Context.BillingCodeCaches
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetBillingCodeCacheById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBillingCodeCacheGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBillingCodeCacheCreated(CrownATTime.Server.Models.ATTime.BillingCodeCache item);
        partial void OnAfterBillingCodeCacheCreated(CrownATTime.Server.Models.ATTime.BillingCodeCache item);

        public async Task<CrownATTime.Server.Models.ATTime.BillingCodeCache> CreateBillingCodeCache(CrownATTime.Server.Models.ATTime.BillingCodeCache billingcodecache)
        {
            OnBillingCodeCacheCreated(billingcodecache);

            var existingItem = Context.BillingCodeCaches
                              .Where(i => i.Id == billingcodecache.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.BillingCodeCaches.Add(billingcodecache);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(billingcodecache).State = EntityState.Detached;
                throw;
            }

            OnAfterBillingCodeCacheCreated(billingcodecache);

            return billingcodecache;
        }

        public async Task<CrownATTime.Server.Models.ATTime.BillingCodeCache> CancelBillingCodeCacheChanges(CrownATTime.Server.Models.ATTime.BillingCodeCache item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBillingCodeCacheUpdated(CrownATTime.Server.Models.ATTime.BillingCodeCache item);
        partial void OnAfterBillingCodeCacheUpdated(CrownATTime.Server.Models.ATTime.BillingCodeCache item);

        public async Task<CrownATTime.Server.Models.ATTime.BillingCodeCache> UpdateBillingCodeCache(int id, CrownATTime.Server.Models.ATTime.BillingCodeCache billingcodecache)
        {
            OnBillingCodeCacheUpdated(billingcodecache);

            var itemToUpdate = Context.BillingCodeCaches
                              .Where(i => i.Id == billingcodecache.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(billingcodecache).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBillingCodeCacheUpdated(billingcodecache);

            return billingcodecache;
        }

        partial void OnBillingCodeCacheDeleted(CrownATTime.Server.Models.ATTime.BillingCodeCache item);
        partial void OnAfterBillingCodeCacheDeleted(CrownATTime.Server.Models.ATTime.BillingCodeCache item);

        public async Task<CrownATTime.Server.Models.ATTime.BillingCodeCache> DeleteBillingCodeCache(int id)
        {
            var itemToDelete = Context.BillingCodeCaches
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBillingCodeCacheDeleted(itemToDelete);

            Reset();

            Context.BillingCodeCaches.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBillingCodeCacheDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCompanyCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/companycaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/companycaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCompanyCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/companycaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/companycaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCompanyCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.CompanyCache> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.CompanyCache>> GetCompanyCaches(Query query = null)
        {
            var items = Context.CompanyCaches.AsQueryable();


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

            OnCompanyCachesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompanyCacheGet(CrownATTime.Server.Models.ATTime.CompanyCache item);
        partial void OnGetCompanyCacheById(ref IQueryable<CrownATTime.Server.Models.ATTime.CompanyCache> items);


        public async Task<CrownATTime.Server.Models.ATTime.CompanyCache> GetCompanyCacheById(int id)
        {
            var items = Context.CompanyCaches
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetCompanyCacheById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCompanyCacheGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCompanyCacheCreated(CrownATTime.Server.Models.ATTime.CompanyCache item);
        partial void OnAfterCompanyCacheCreated(CrownATTime.Server.Models.ATTime.CompanyCache item);

        public async Task<CrownATTime.Server.Models.ATTime.CompanyCache> CreateCompanyCache(CrownATTime.Server.Models.ATTime.CompanyCache companycache)
        {
            OnCompanyCacheCreated(companycache);

            var existingItem = Context.CompanyCaches
                              .Where(i => i.Id == companycache.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CompanyCaches.Add(companycache);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(companycache).State = EntityState.Detached;
                throw;
            }

            OnAfterCompanyCacheCreated(companycache);

            return companycache;
        }

        public async Task<CrownATTime.Server.Models.ATTime.CompanyCache> CancelCompanyCacheChanges(CrownATTime.Server.Models.ATTime.CompanyCache item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCompanyCacheUpdated(CrownATTime.Server.Models.ATTime.CompanyCache item);
        partial void OnAfterCompanyCacheUpdated(CrownATTime.Server.Models.ATTime.CompanyCache item);

        public async Task<CrownATTime.Server.Models.ATTime.CompanyCache> UpdateCompanyCache(int id, CrownATTime.Server.Models.ATTime.CompanyCache companycache)
        {
            OnCompanyCacheUpdated(companycache);

            var itemToUpdate = Context.CompanyCaches
                              .Where(i => i.Id == companycache.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(companycache).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCompanyCacheUpdated(companycache);

            return companycache;
        }

        partial void OnCompanyCacheDeleted(CrownATTime.Server.Models.ATTime.CompanyCache item);
        partial void OnAfterCompanyCacheDeleted(CrownATTime.Server.Models.ATTime.CompanyCache item);

        public async Task<CrownATTime.Server.Models.ATTime.CompanyCache> DeleteCompanyCache(int id)
        {
            var itemToDelete = Context.CompanyCaches
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCompanyCacheDeleted(itemToDelete);

            Reset();

            Context.CompanyCaches.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCompanyCacheDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportContractCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/contractcaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/contractcaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportContractCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/contractcaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/contractcaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnContractCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.ContractCache> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.ContractCache>> GetContractCaches(Query query = null)
        {
            var items = Context.ContractCaches.AsQueryable();


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

            OnContractCachesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnContractCacheGet(CrownATTime.Server.Models.ATTime.ContractCache item);
        partial void OnGetContractCacheById(ref IQueryable<CrownATTime.Server.Models.ATTime.ContractCache> items);


        public async Task<CrownATTime.Server.Models.ATTime.ContractCache> GetContractCacheById(int id)
        {
            var items = Context.ContractCaches
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetContractCacheById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnContractCacheGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnContractCacheCreated(CrownATTime.Server.Models.ATTime.ContractCache item);
        partial void OnAfterContractCacheCreated(CrownATTime.Server.Models.ATTime.ContractCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ContractCache> CreateContractCache(CrownATTime.Server.Models.ATTime.ContractCache contractcache)
        {
            OnContractCacheCreated(contractcache);

            var existingItem = Context.ContractCaches
                              .Where(i => i.Id == contractcache.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ContractCaches.Add(contractcache);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(contractcache).State = EntityState.Detached;
                throw;
            }

            OnAfterContractCacheCreated(contractcache);

            return contractcache;
        }

        public async Task<CrownATTime.Server.Models.ATTime.ContractCache> CancelContractCacheChanges(CrownATTime.Server.Models.ATTime.ContractCache item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnContractCacheUpdated(CrownATTime.Server.Models.ATTime.ContractCache item);
        partial void OnAfterContractCacheUpdated(CrownATTime.Server.Models.ATTime.ContractCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ContractCache> UpdateContractCache(int id, CrownATTime.Server.Models.ATTime.ContractCache contractcache)
        {
            OnContractCacheUpdated(contractcache);

            var itemToUpdate = Context.ContractCaches
                              .Where(i => i.Id == contractcache.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(contractcache).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterContractCacheUpdated(contractcache);

            return contractcache;
        }

        partial void OnContractCacheDeleted(CrownATTime.Server.Models.ATTime.ContractCache item);
        partial void OnAfterContractCacheDeleted(CrownATTime.Server.Models.ATTime.ContractCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ContractCache> DeleteContractCache(int id)
        {
            var itemToDelete = Context.ContractCaches
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnContractCacheDeleted(itemToDelete);

            Reset();

            Context.ContractCaches.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterContractCacheDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportEmailTemplatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/emailtemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/emailtemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEmailTemplatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/emailtemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/emailtemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEmailTemplatesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.EmailTemplate> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.EmailTemplate>> GetEmailTemplates(Query query = null)
        {
            var items = Context.EmailTemplates.AsQueryable();


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

            OnEmailTemplatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmailTemplateGet(CrownATTime.Server.Models.ATTime.EmailTemplate item);
        partial void OnGetEmailTemplateByEmailTemplateId(ref IQueryable<CrownATTime.Server.Models.ATTime.EmailTemplate> items);


        public async Task<CrownATTime.Server.Models.ATTime.EmailTemplate> GetEmailTemplateByEmailTemplateId(int emailtemplateid)
        {
            var items = Context.EmailTemplates
                              .AsNoTracking()
                              .Where(i => i.EmailTemplateId == emailtemplateid);

 
            OnGetEmailTemplateByEmailTemplateId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnEmailTemplateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEmailTemplateCreated(CrownATTime.Server.Models.ATTime.EmailTemplate item);
        partial void OnAfterEmailTemplateCreated(CrownATTime.Server.Models.ATTime.EmailTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.EmailTemplate> CreateEmailTemplate(CrownATTime.Server.Models.ATTime.EmailTemplate emailtemplate)
        {
            OnEmailTemplateCreated(emailtemplate);

            var existingItem = Context.EmailTemplates
                              .Where(i => i.EmailTemplateId == emailtemplate.EmailTemplateId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.EmailTemplates.Add(emailtemplate);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(emailtemplate).State = EntityState.Detached;
                throw;
            }

            OnAfterEmailTemplateCreated(emailtemplate);

            return emailtemplate;
        }

        public async Task<CrownATTime.Server.Models.ATTime.EmailTemplate> CancelEmailTemplateChanges(CrownATTime.Server.Models.ATTime.EmailTemplate item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEmailTemplateUpdated(CrownATTime.Server.Models.ATTime.EmailTemplate item);
        partial void OnAfterEmailTemplateUpdated(CrownATTime.Server.Models.ATTime.EmailTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.EmailTemplate> UpdateEmailTemplate(int emailtemplateid, CrownATTime.Server.Models.ATTime.EmailTemplate emailtemplate)
        {
            OnEmailTemplateUpdated(emailtemplate);

            var itemToUpdate = Context.EmailTemplates
                              .Where(i => i.EmailTemplateId == emailtemplate.EmailTemplateId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(emailtemplate).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEmailTemplateUpdated(emailtemplate);

            return emailtemplate;
        }

        partial void OnEmailTemplateDeleted(CrownATTime.Server.Models.ATTime.EmailTemplate item);
        partial void OnAfterEmailTemplateDeleted(CrownATTime.Server.Models.ATTime.EmailTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.EmailTemplate> DeleteEmailTemplate(int emailtemplateid)
        {
            var itemToDelete = Context.EmailTemplates
                              .Where(i => i.EmailTemplateId == emailtemplateid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEmailTemplateDeleted(itemToDelete);

            Reset();

            Context.EmailTemplates.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEmailTemplateDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportNoteTemplatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/notetemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/notetemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportNoteTemplatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/notetemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/notetemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnNoteTemplatesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.NoteTemplate> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.NoteTemplate>> GetNoteTemplates(Query query = null)
        {
            var items = Context.NoteTemplates.AsQueryable();


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

            OnNoteTemplatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnNoteTemplateGet(CrownATTime.Server.Models.ATTime.NoteTemplate item);
        partial void OnGetNoteTemplateByNoteTemplateId(ref IQueryable<CrownATTime.Server.Models.ATTime.NoteTemplate> items);


        public async Task<CrownATTime.Server.Models.ATTime.NoteTemplate> GetNoteTemplateByNoteTemplateId(int notetemplateid)
        {
            var items = Context.NoteTemplates
                              .AsNoTracking()
                              .Where(i => i.NoteTemplateId == notetemplateid);

 
            OnGetNoteTemplateByNoteTemplateId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnNoteTemplateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnNoteTemplateCreated(CrownATTime.Server.Models.ATTime.NoteTemplate item);
        partial void OnAfterNoteTemplateCreated(CrownATTime.Server.Models.ATTime.NoteTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.NoteTemplate> CreateNoteTemplate(CrownATTime.Server.Models.ATTime.NoteTemplate notetemplate)
        {
            OnNoteTemplateCreated(notetemplate);

            var existingItem = Context.NoteTemplates
                              .Where(i => i.NoteTemplateId == notetemplate.NoteTemplateId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.NoteTemplates.Add(notetemplate);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(notetemplate).State = EntityState.Detached;
                throw;
            }

            OnAfterNoteTemplateCreated(notetemplate);

            return notetemplate;
        }

        public async Task<CrownATTime.Server.Models.ATTime.NoteTemplate> CancelNoteTemplateChanges(CrownATTime.Server.Models.ATTime.NoteTemplate item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnNoteTemplateUpdated(CrownATTime.Server.Models.ATTime.NoteTemplate item);
        partial void OnAfterNoteTemplateUpdated(CrownATTime.Server.Models.ATTime.NoteTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.NoteTemplate> UpdateNoteTemplate(int notetemplateid, CrownATTime.Server.Models.ATTime.NoteTemplate notetemplate)
        {
            OnNoteTemplateUpdated(notetemplate);

            var itemToUpdate = Context.NoteTemplates
                              .Where(i => i.NoteTemplateId == notetemplate.NoteTemplateId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(notetemplate).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterNoteTemplateUpdated(notetemplate);

            return notetemplate;
        }

        partial void OnNoteTemplateDeleted(CrownATTime.Server.Models.ATTime.NoteTemplate item);
        partial void OnAfterNoteTemplateDeleted(CrownATTime.Server.Models.ATTime.NoteTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.NoteTemplate> DeleteNoteTemplate(int notetemplateid)
        {
            var itemToDelete = Context.NoteTemplates
                              .Where(i => i.NoteTemplateId == notetemplateid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnNoteTemplateDeleted(itemToDelete);

            Reset();

            Context.NoteTemplates.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterNoteTemplateDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportResourceCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/resourcecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/resourcecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportResourceCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/resourcecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/resourcecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnResourceCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.ResourceCache> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.ResourceCache>> GetResourceCaches(Query query = null)
        {
            var items = Context.ResourceCaches.AsQueryable();


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

            OnResourceCachesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnResourceCacheGet(CrownATTime.Server.Models.ATTime.ResourceCache item);
        partial void OnGetResourceCacheById(ref IQueryable<CrownATTime.Server.Models.ATTime.ResourceCache> items);


        public async Task<CrownATTime.Server.Models.ATTime.ResourceCache> GetResourceCacheById(int id)
        {
            var items = Context.ResourceCaches
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetResourceCacheById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnResourceCacheGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnResourceCacheCreated(CrownATTime.Server.Models.ATTime.ResourceCache item);
        partial void OnAfterResourceCacheCreated(CrownATTime.Server.Models.ATTime.ResourceCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ResourceCache> CreateResourceCache(CrownATTime.Server.Models.ATTime.ResourceCache resourcecache)
        {
            OnResourceCacheCreated(resourcecache);

            var existingItem = Context.ResourceCaches
                              .Where(i => i.Id == resourcecache.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ResourceCaches.Add(resourcecache);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(resourcecache).State = EntityState.Detached;
                throw;
            }

            OnAfterResourceCacheCreated(resourcecache);

            return resourcecache;
        }

        public async Task<CrownATTime.Server.Models.ATTime.ResourceCache> CancelResourceCacheChanges(CrownATTime.Server.Models.ATTime.ResourceCache item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnResourceCacheUpdated(CrownATTime.Server.Models.ATTime.ResourceCache item);
        partial void OnAfterResourceCacheUpdated(CrownATTime.Server.Models.ATTime.ResourceCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ResourceCache> UpdateResourceCache(int id, CrownATTime.Server.Models.ATTime.ResourceCache resourcecache)
        {
            OnResourceCacheUpdated(resourcecache);

            var itemToUpdate = Context.ResourceCaches
                              .Where(i => i.Id == resourcecache.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(resourcecache).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterResourceCacheUpdated(resourcecache);

            return resourcecache;
        }

        partial void OnResourceCacheDeleted(CrownATTime.Server.Models.ATTime.ResourceCache item);
        partial void OnAfterResourceCacheDeleted(CrownATTime.Server.Models.ATTime.ResourceCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ResourceCache> DeleteResourceCache(int id)
        {
            var itemToDelete = Context.ResourceCaches
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnResourceCacheDeleted(itemToDelete);

            Reset();

            Context.ResourceCaches.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterResourceCacheDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportRoleCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/rolecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/rolecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportRoleCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/rolecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/rolecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnRoleCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.RoleCache> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.RoleCache>> GetRoleCaches(Query query = null)
        {
            var items = Context.RoleCaches.AsQueryable();


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

            OnRoleCachesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnRoleCacheGet(CrownATTime.Server.Models.ATTime.RoleCache item);
        partial void OnGetRoleCacheById(ref IQueryable<CrownATTime.Server.Models.ATTime.RoleCache> items);


        public async Task<CrownATTime.Server.Models.ATTime.RoleCache> GetRoleCacheById(int id)
        {
            var items = Context.RoleCaches
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetRoleCacheById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnRoleCacheGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnRoleCacheCreated(CrownATTime.Server.Models.ATTime.RoleCache item);
        partial void OnAfterRoleCacheCreated(CrownATTime.Server.Models.ATTime.RoleCache item);

        public async Task<CrownATTime.Server.Models.ATTime.RoleCache> CreateRoleCache(CrownATTime.Server.Models.ATTime.RoleCache rolecache)
        {
            OnRoleCacheCreated(rolecache);

            var existingItem = Context.RoleCaches
                              .Where(i => i.Id == rolecache.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.RoleCaches.Add(rolecache);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(rolecache).State = EntityState.Detached;
                throw;
            }

            OnAfterRoleCacheCreated(rolecache);

            return rolecache;
        }

        public async Task<CrownATTime.Server.Models.ATTime.RoleCache> CancelRoleCacheChanges(CrownATTime.Server.Models.ATTime.RoleCache item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnRoleCacheUpdated(CrownATTime.Server.Models.ATTime.RoleCache item);
        partial void OnAfterRoleCacheUpdated(CrownATTime.Server.Models.ATTime.RoleCache item);

        public async Task<CrownATTime.Server.Models.ATTime.RoleCache> UpdateRoleCache(int id, CrownATTime.Server.Models.ATTime.RoleCache rolecache)
        {
            OnRoleCacheUpdated(rolecache);

            var itemToUpdate = Context.RoleCaches
                              .Where(i => i.Id == rolecache.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(rolecache).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterRoleCacheUpdated(rolecache);

            return rolecache;
        }

        partial void OnRoleCacheDeleted(CrownATTime.Server.Models.ATTime.RoleCache item);
        partial void OnAfterRoleCacheDeleted(CrownATTime.Server.Models.ATTime.RoleCache item);

        public async Task<CrownATTime.Server.Models.ATTime.RoleCache> DeleteRoleCache(int id)
        {
            var itemToDelete = Context.RoleCaches
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnRoleCacheDeleted(itemToDelete);

            Reset();

            Context.RoleCaches.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterRoleCacheDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportServiceDeskRoleCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/servicedeskrolecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/servicedeskrolecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportServiceDeskRoleCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/servicedeskrolecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/servicedeskrolecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnServiceDeskRoleCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>> GetServiceDeskRoleCaches(Query query = null)
        {
            var items = Context.ServiceDeskRoleCaches.AsQueryable();


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

            OnServiceDeskRoleCachesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnServiceDeskRoleCacheGet(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);
        partial void OnGetServiceDeskRoleCacheById(ref IQueryable<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> items);


        public async Task<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> GetServiceDeskRoleCacheById(int id)
        {
            var items = Context.ServiceDeskRoleCaches
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetServiceDeskRoleCacheById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnServiceDeskRoleCacheGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnServiceDeskRoleCacheCreated(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);
        partial void OnAfterServiceDeskRoleCacheCreated(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> CreateServiceDeskRoleCache(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache servicedeskrolecache)
        {
            OnServiceDeskRoleCacheCreated(servicedeskrolecache);

            var existingItem = Context.ServiceDeskRoleCaches
                              .Where(i => i.Id == servicedeskrolecache.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ServiceDeskRoleCaches.Add(servicedeskrolecache);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(servicedeskrolecache).State = EntityState.Detached;
                throw;
            }

            OnAfterServiceDeskRoleCacheCreated(servicedeskrolecache);

            return servicedeskrolecache;
        }

        public async Task<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> CancelServiceDeskRoleCacheChanges(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnServiceDeskRoleCacheUpdated(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);
        partial void OnAfterServiceDeskRoleCacheUpdated(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> UpdateServiceDeskRoleCache(int id, CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache servicedeskrolecache)
        {
            OnServiceDeskRoleCacheUpdated(servicedeskrolecache);

            var itemToUpdate = Context.ServiceDeskRoleCaches
                              .Where(i => i.Id == servicedeskrolecache.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(servicedeskrolecache).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterServiceDeskRoleCacheUpdated(servicedeskrolecache);

            return servicedeskrolecache;
        }

        partial void OnServiceDeskRoleCacheDeleted(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);
        partial void OnAfterServiceDeskRoleCacheDeleted(CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache item);

        public async Task<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> DeleteServiceDeskRoleCache(int id)
        {
            var itemToDelete = Context.ServiceDeskRoleCaches
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnServiceDeskRoleCacheDeleted(itemToDelete);

            Reset();

            Context.ServiceDeskRoleCaches.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterServiceDeskRoleCacheDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTicketEntityPicklistValueCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/ticketentitypicklistvaluecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/ticketentitypicklistvaluecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTicketEntityPicklistValueCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/ticketentitypicklistvaluecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/ticketentitypicklistvaluecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTicketEntityPicklistValueCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>> GetTicketEntityPicklistValueCaches(Query query = null)
        {
            var items = Context.TicketEntityPicklistValueCaches.AsQueryable();


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

            OnTicketEntityPicklistValueCachesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTicketEntityPicklistValueCacheGet(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);
        partial void OnGetTicketEntityPicklistValueCacheByTicketEntityPicklistValueId(ref IQueryable<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> items);


        public async Task<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> GetTicketEntityPicklistValueCacheByTicketEntityPicklistValueId(int ticketentitypicklistvalueid)
        {
            var items = Context.TicketEntityPicklistValueCaches
                              .AsNoTracking()
                              .Where(i => i.TicketEntityPicklistValueId == ticketentitypicklistvalueid);

 
            OnGetTicketEntityPicklistValueCacheByTicketEntityPicklistValueId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTicketEntityPicklistValueCacheGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTicketEntityPicklistValueCacheCreated(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);
        partial void OnAfterTicketEntityPicklistValueCacheCreated(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);

        public async Task<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> CreateTicketEntityPicklistValueCache(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache ticketentitypicklistvaluecache)
        {
            OnTicketEntityPicklistValueCacheCreated(ticketentitypicklistvaluecache);

            var existingItem = Context.TicketEntityPicklistValueCaches
                              .Where(i => i.TicketEntityPicklistValueId == ticketentitypicklistvaluecache.TicketEntityPicklistValueId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TicketEntityPicklistValueCaches.Add(ticketentitypicklistvaluecache);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ticketentitypicklistvaluecache).State = EntityState.Detached;
                throw;
            }

            OnAfterTicketEntityPicklistValueCacheCreated(ticketentitypicklistvaluecache);

            return ticketentitypicklistvaluecache;
        }

        public async Task<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> CancelTicketEntityPicklistValueCacheChanges(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTicketEntityPicklistValueCacheUpdated(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);
        partial void OnAfterTicketEntityPicklistValueCacheUpdated(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);

        public async Task<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> UpdateTicketEntityPicklistValueCache(int ticketentitypicklistvalueid, CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache ticketentitypicklistvaluecache)
        {
            OnTicketEntityPicklistValueCacheUpdated(ticketentitypicklistvaluecache);

            var itemToUpdate = Context.TicketEntityPicklistValueCaches
                              .Where(i => i.TicketEntityPicklistValueId == ticketentitypicklistvaluecache.TicketEntityPicklistValueId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(ticketentitypicklistvaluecache).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTicketEntityPicklistValueCacheUpdated(ticketentitypicklistvaluecache);

            return ticketentitypicklistvaluecache;
        }

        partial void OnTicketEntityPicklistValueCacheDeleted(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);
        partial void OnAfterTicketEntityPicklistValueCacheDeleted(CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache item);

        public async Task<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> DeleteTicketEntityPicklistValueCache(int ticketentitypicklistvalueid)
        {
            var itemToDelete = Context.TicketEntityPicklistValueCaches
                              .Where(i => i.TicketEntityPicklistValueId == ticketentitypicklistvalueid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTicketEntityPicklistValueCacheDeleted(itemToDelete);

            Reset();

            Context.TicketEntityPicklistValueCaches.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTicketEntityPicklistValueCacheDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTicketNoteEntityPicklistValueCachesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/ticketnoteentitypicklistvaluecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/ticketnoteentitypicklistvaluecaches/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTicketNoteEntityPicklistValueCachesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/ticketnoteentitypicklistvaluecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/ticketnoteentitypicklistvaluecaches/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTicketNoteEntityPicklistValueCachesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>> GetTicketNoteEntityPicklistValueCaches(Query query = null)
        {
            var items = Context.TicketNoteEntityPicklistValueCaches.AsQueryable();


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

            OnTicketNoteEntityPicklistValueCachesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTicketNoteEntityPicklistValueCacheGet(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);
        partial void OnGetTicketNoteEntityPicklistValueCacheByTicketNoteEntityPicklistValueId(ref IQueryable<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> items);


        public async Task<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> GetTicketNoteEntityPicklistValueCacheByTicketNoteEntityPicklistValueId(int ticketnoteentitypicklistvalueid)
        {
            var items = Context.TicketNoteEntityPicklistValueCaches
                              .AsNoTracking()
                              .Where(i => i.TicketNoteEntityPicklistValueId == ticketnoteentitypicklistvalueid);

 
            OnGetTicketNoteEntityPicklistValueCacheByTicketNoteEntityPicklistValueId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTicketNoteEntityPicklistValueCacheGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTicketNoteEntityPicklistValueCacheCreated(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);
        partial void OnAfterTicketNoteEntityPicklistValueCacheCreated(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);

        public async Task<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> CreateTicketNoteEntityPicklistValueCache(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache ticketnoteentitypicklistvaluecache)
        {
            OnTicketNoteEntityPicklistValueCacheCreated(ticketnoteentitypicklistvaluecache);

            var existingItem = Context.TicketNoteEntityPicklistValueCaches
                              .Where(i => i.TicketNoteEntityPicklistValueId == ticketnoteentitypicklistvaluecache.TicketNoteEntityPicklistValueId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TicketNoteEntityPicklistValueCaches.Add(ticketnoteentitypicklistvaluecache);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(ticketnoteentitypicklistvaluecache).State = EntityState.Detached;
                throw;
            }

            OnAfterTicketNoteEntityPicklistValueCacheCreated(ticketnoteentitypicklistvaluecache);

            return ticketnoteentitypicklistvaluecache;
        }

        public async Task<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> CancelTicketNoteEntityPicklistValueCacheChanges(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTicketNoteEntityPicklistValueCacheUpdated(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);
        partial void OnAfterTicketNoteEntityPicklistValueCacheUpdated(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);

        public async Task<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> UpdateTicketNoteEntityPicklistValueCache(int ticketnoteentitypicklistvalueid, CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache ticketnoteentitypicklistvaluecache)
        {
            OnTicketNoteEntityPicklistValueCacheUpdated(ticketnoteentitypicklistvaluecache);

            var itemToUpdate = Context.TicketNoteEntityPicklistValueCaches
                              .Where(i => i.TicketNoteEntityPicklistValueId == ticketnoteentitypicklistvaluecache.TicketNoteEntityPicklistValueId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(ticketnoteentitypicklistvaluecache).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTicketNoteEntityPicklistValueCacheUpdated(ticketnoteentitypicklistvaluecache);

            return ticketnoteentitypicklistvaluecache;
        }

        partial void OnTicketNoteEntityPicklistValueCacheDeleted(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);
        partial void OnAfterTicketNoteEntityPicklistValueCacheDeleted(CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache item);

        public async Task<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> DeleteTicketNoteEntityPicklistValueCache(int ticketnoteentitypicklistvalueid)
        {
            var itemToDelete = Context.TicketNoteEntityPicklistValueCaches
                              .Where(i => i.TicketNoteEntityPicklistValueId == ticketnoteentitypicklistvalueid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTicketNoteEntityPicklistValueCacheDeleted(itemToDelete);

            Reset();

            Context.TicketNoteEntityPicklistValueCaches.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTicketNoteEntityPicklistValueCacheDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTimeEntriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentries/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTimeEntriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentries/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTimeEntriesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeEntry> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.TimeEntry>> GetTimeEntries(Query query = null)
        {
            var items = Context.TimeEntries.AsQueryable();


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

            OnTimeEntriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTimeEntryGet(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnGetTimeEntryByTimeEntryId(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeEntry> items);


        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> GetTimeEntryByTimeEntryId(int timeentryid)
        {
            var items = Context.TimeEntries
                              .AsNoTracking()
                              .Where(i => i.TimeEntryId == timeentryid);

 
            OnGetTimeEntryByTimeEntryId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTimeEntryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTimeEntryCreated(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryCreated(CrownATTime.Server.Models.ATTime.TimeEntry item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> CreateTimeEntry(CrownATTime.Server.Models.ATTime.TimeEntry timeentry)
        {
            OnTimeEntryCreated(timeentry);

            var existingItem = Context.TimeEntries
                              .Where(i => i.TimeEntryId == timeentry.TimeEntryId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TimeEntries.Add(timeentry);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(timeentry).State = EntityState.Detached;
                throw;
            }

            OnAfterTimeEntryCreated(timeentry);

            return timeentry;
        }

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> CancelTimeEntryChanges(CrownATTime.Server.Models.ATTime.TimeEntry item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTimeEntryUpdated(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryUpdated(CrownATTime.Server.Models.ATTime.TimeEntry item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> UpdateTimeEntry(int timeentryid, CrownATTime.Server.Models.ATTime.TimeEntry timeentry)
        {
            OnTimeEntryUpdated(timeentry);

            var itemToUpdate = Context.TimeEntries
                              .Where(i => i.TimeEntryId == timeentry.TimeEntryId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(timeentry).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTimeEntryUpdated(timeentry);

            return timeentry;
        }

        partial void OnTimeEntryDeleted(CrownATTime.Server.Models.ATTime.TimeEntry item);
        partial void OnAfterTimeEntryDeleted(CrownATTime.Server.Models.ATTime.TimeEntry item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntry> DeleteTimeEntry(int timeentryid)
        {
            var itemToDelete = Context.TimeEntries
                              .Where(i => i.TimeEntryId == timeentryid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTimeEntryDeleted(itemToDelete);

            Reset();

            Context.TimeEntries.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTimeEntryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTimeEntryTemplatesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentrytemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentrytemplates/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTimeEntryTemplatesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeentrytemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeentrytemplates/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTimeEntryTemplatesRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>> GetTimeEntryTemplates(Query query = null)
        {
            var items = Context.TimeEntryTemplates.AsQueryable();

            items = items.Include(i => i.BillingCodeCache);

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

            OnTimeEntryTemplatesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTimeEntryTemplateGet(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);
        partial void OnGetTimeEntryTemplateByTimeEntryTemplateId(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> items);


        public async Task<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> GetTimeEntryTemplateByTimeEntryTemplateId(int timeentrytemplateid)
        {
            var items = Context.TimeEntryTemplates
                              .AsNoTracking()
                              .Where(i => i.TimeEntryTemplateId == timeentrytemplateid);

            items = items.Include(i => i.BillingCodeCache);
 
            OnGetTimeEntryTemplateByTimeEntryTemplateId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTimeEntryTemplateGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTimeEntryTemplateCreated(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);
        partial void OnAfterTimeEntryTemplateCreated(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> CreateTimeEntryTemplate(CrownATTime.Server.Models.ATTime.TimeEntryTemplate timeentrytemplate)
        {
            OnTimeEntryTemplateCreated(timeentrytemplate);

            var existingItem = Context.TimeEntryTemplates
                              .Where(i => i.TimeEntryTemplateId == timeentrytemplate.TimeEntryTemplateId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TimeEntryTemplates.Add(timeentrytemplate);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(timeentrytemplate).State = EntityState.Detached;
                throw;
            }

            OnAfterTimeEntryTemplateCreated(timeentrytemplate);

            return timeentrytemplate;
        }

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> CancelTimeEntryTemplateChanges(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTimeEntryTemplateUpdated(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);
        partial void OnAfterTimeEntryTemplateUpdated(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> UpdateTimeEntryTemplate(int timeentrytemplateid, CrownATTime.Server.Models.ATTime.TimeEntryTemplate timeentrytemplate)
        {
            OnTimeEntryTemplateUpdated(timeentrytemplate);

            var itemToUpdate = Context.TimeEntryTemplates
                              .Where(i => i.TimeEntryTemplateId == timeentrytemplate.TimeEntryTemplateId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();
            timeentrytemplate.BillingCodeCache = null;

            Context.Attach(timeentrytemplate).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTimeEntryTemplateUpdated(timeentrytemplate);

            return timeentrytemplate;
        }

        partial void OnTimeEntryTemplateDeleted(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);
        partial void OnAfterTimeEntryTemplateDeleted(CrownATTime.Server.Models.ATTime.TimeEntryTemplate item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> DeleteTimeEntryTemplate(int timeentrytemplateid)
        {
            var itemToDelete = Context.TimeEntryTemplates
                              .Where(i => i.TimeEntryTemplateId == timeentrytemplateid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTimeEntryTemplateDeleted(itemToDelete);

            Reset();

            Context.TimeEntryTemplates.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTimeEntryTemplateDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTimeGuardSectionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeguardsections/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeguardsections/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTimeGuardSectionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/attime/timeguardsections/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/attime/timeguardsections/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTimeGuardSectionsRead(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeGuardSection> items);

        public async Task<IQueryable<CrownATTime.Server.Models.ATTime.TimeGuardSection>> GetTimeGuardSections(Query query = null)
        {
            var items = Context.TimeGuardSections.AsQueryable();


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

            OnTimeGuardSectionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTimeGuardSectionGet(CrownATTime.Server.Models.ATTime.TimeGuardSection item);
        partial void OnGetTimeGuardSectionByTimeGuardSectionsId(ref IQueryable<CrownATTime.Server.Models.ATTime.TimeGuardSection> items);


        public async Task<CrownATTime.Server.Models.ATTime.TimeGuardSection> GetTimeGuardSectionByTimeGuardSectionsId(int timeguardsectionsid)
        {
            var items = Context.TimeGuardSections
                              .AsNoTracking()
                              .Where(i => i.TimeGuardSectionsId == timeguardsectionsid);

 
            OnGetTimeGuardSectionByTimeGuardSectionsId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTimeGuardSectionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTimeGuardSectionCreated(CrownATTime.Server.Models.ATTime.TimeGuardSection item);
        partial void OnAfterTimeGuardSectionCreated(CrownATTime.Server.Models.ATTime.TimeGuardSection item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeGuardSection> CreateTimeGuardSection(CrownATTime.Server.Models.ATTime.TimeGuardSection timeguardsection)
        {
            OnTimeGuardSectionCreated(timeguardsection);

            var existingItem = Context.TimeGuardSections
                              .Where(i => i.TimeGuardSectionsId == timeguardsection.TimeGuardSectionsId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TimeGuardSections.Add(timeguardsection);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(timeguardsection).State = EntityState.Detached;
                throw;
            }

            OnAfterTimeGuardSectionCreated(timeguardsection);

            return timeguardsection;
        }

        public async Task<CrownATTime.Server.Models.ATTime.TimeGuardSection> CancelTimeGuardSectionChanges(CrownATTime.Server.Models.ATTime.TimeGuardSection item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTimeGuardSectionUpdated(CrownATTime.Server.Models.ATTime.TimeGuardSection item);
        partial void OnAfterTimeGuardSectionUpdated(CrownATTime.Server.Models.ATTime.TimeGuardSection item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeGuardSection> UpdateTimeGuardSection(int timeguardsectionsid, CrownATTime.Server.Models.ATTime.TimeGuardSection timeguardsection)
        {
            OnTimeGuardSectionUpdated(timeguardsection);

            var itemToUpdate = Context.TimeGuardSections
                              .Where(i => i.TimeGuardSectionsId == timeguardsection.TimeGuardSectionsId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(timeguardsection).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTimeGuardSectionUpdated(timeguardsection);

            return timeguardsection;
        }

        partial void OnTimeGuardSectionDeleted(CrownATTime.Server.Models.ATTime.TimeGuardSection item);
        partial void OnAfterTimeGuardSectionDeleted(CrownATTime.Server.Models.ATTime.TimeGuardSection item);

        public async Task<CrownATTime.Server.Models.ATTime.TimeGuardSection> DeleteTimeGuardSection(int timeguardsectionsid)
        {
            var itemToDelete = Context.TimeGuardSections
                              .Where(i => i.TimeGuardSectionsId == timeguardsectionsid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTimeGuardSectionDeleted(itemToDelete);

            Reset();

            Context.TimeGuardSections.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTimeGuardSectionDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}