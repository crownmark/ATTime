using CrownATTime.Client;
using CrownATTime.Client.CustomComponents.DialogManager;
using CrownATTime.Server.Components;
using CrownATTime.Server.Models;
using CrownATTime.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OData.ModelBuilder;
using Radzen;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "CrownATTimeTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddHttpClient();
// Needed for IHttpContextAccessor injection
builder.Services.AddHttpContextAccessor();
//builder.Services.AddSingleton<CrownATTime.Server.Services.NotificationService>();
//builder.Services.AddHostedService(sp => sp.GetRequiredService<CrownATTime.Server.Services.NotificationService>());
builder.Services.AddHostedService<CrownATTime.Server.Services.NotificationService>();
builder.Services.AddHostedService<CrownATTime.Server.Services.AutotaskSyncService>();
builder.Services.AddScoped<DialogManager>();
builder.Services.AddScoped<CrownATTime.Server.ATTimeService>();
builder.Services.Configure<ThreeCxOptions>(opt =>
{
    opt.TokenUrl = "https://crownenterprises.ca.3cx.us/connect/token";
    opt.ClientId = "n8nmark";
    // IMPORTANT: don’t hardcode secrets in source control.
    // Use environment variable or user-secrets:
    opt.ClientSecret = builder.Configuration["pbmXE8qX3FWd7Dfh8p8f8fuf3cAXTa80"] ?? "";
//opt.Scope = builder.Configuration["THREE_CX_SCOPE"]; // optional
});
builder.Services.AddHttpClient<ThreeCxApiClient>();
builder.Services.AddDbContext<CrownATTime.Server.Data.ATTimeContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(builder.Configuration.GetConnectionString("ATTimeConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderATTime = new ODataConventionModelBuilder();
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.ActionTypesCache>("ActionTypesCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.AiPromptConfiguration>("AiPromptConfigurations");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.AllowedTicketStatus>("AllowedTicketStatuses");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.BillingCodeCache>("BillingCodeCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.CalendarNotificationEventType>("CalendarNotificationEventTypes");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.ClickEventAction>("ClickEventActions");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.CompanyCache>("CompanyCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.ContractCache>("ContractCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.Duration>("Durations");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.DurationType>("DurationTypes");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.EmailTemplate>("EmailTemplates");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.FavoriteBillingCode>("FavoriteBillingCodes");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.FormCategory>("FormCategories");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.Form>("Forms");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.LiveLink>("LiveLinks");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.NoteTemplate>("NoteTemplates");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.ResourceCache>("ResourceCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.RoleCache>("RoleCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>("ServiceDeskRoleCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate>("TeamsMessageTemplates");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TeamsMessageType>("TeamsMessageTypes");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>("TicketEntityPicklistValueCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>("TicketNoteEntityPicklistValueCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TimeEntry>("TimeEntries");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>("TimeEntryTemplates");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TimeGuardSection>("TimeGuardSections");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.WaitingStatus>("WaitingStatuses");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.WorkflowRule>("WorkflowRules");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.WorkflowStep>("WorkflowSteps");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.WorkflowStepType>("WorkflowStepTypes");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.WorkflowTriggerType>("WorkflowTriggerTypes");
    opt.AddRouteComponents("odata/ATTime", oDataBuilderATTime.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddHttpClient("CrownATTime.Server").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddScoped<AuthenticationStateProvider, CrownATTime.Client.ApplicationAuthenticationStateProvider>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<CrownATTime.Client.SecurityService>();
builder.Services.AddScoped<CrownATTime.Client.AutotaskService>();
builder.Services.AddScoped<CrownATTime.Client.ThreeCxClientService>();
builder.Services.AddScoped<CrownATTime.Client.EmailService>();
builder.Services.AddScoped<CrownATTime.Client.TeamsChatService>();
builder.Services.AddScoped<CrownATTime.Client.TemplateTokenDiscoveryService>();
builder.Services.AddScoped<CrownATTime.Client.ATTimeService>();
builder.Services.AddScoped<CrownATTime.Client.ITGlueService>();
builder.Services.AddScoped<AiScenarioRunnerService>();
builder.Services.Configure<GraphApiOptions>(builder.Configuration.GetSection("GraphApi"));
builder.Services.AddScoped<GraphApi>();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin() //.SetIsOriginAllowed((host) => true)
    .WithOrigins(new[] { "https://timeguard.crown.software", "https://localhost:5001" }).AllowCredentials();
}));
builder.Services.AddScoped<CrownATTime.Server.DistributorProductsService>();
builder.Services.AddDbContext<CrownATTime.Server.Data.DistributorProductsContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(builder.Configuration.GetConnectionString("DistributorProductsConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderDistributorProducts = new ODataConventionModelBuilder();
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.IngramLenovoFamilyImage>("IngramLenovoFamilyImages");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.IngramLenovoScrapeSource>("IngramLenovoScrapeSources");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.IngramManufacturerContent>("IngramManufacturerContents").EntityType.HasKey(entity => new { entity.VendorName, entity.VendorPartNumber });
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.IngramProduct>("IngramProducts");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.IngramProductEnrichment>("IngramProductEnrichments");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.IngramSyncRule>("IngramSyncRules");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.IngramWarehouseInventory>("IngramWarehouseInventories").EntityType.HasKey(entity => new { entity.IngramPartNumber, entity.WarehouseLocation });
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefedition>("LenovoPsrefeditions");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamily>("LenovoPsreffamilies");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.LenovoPsreffamilyImage>("LenovoPsreffamilyImages");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageDatum>("LenovoPsrefimageData");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefskuFamilyMap>("LenovoPsrefskuFamilyMaps");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.StreakwaveProduct>("StreakwaveProducts");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductEnrichment>("StreakwaveProductEnrichments");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.StreakwaveSyncRule>("StreakwaveSyncRules");
    oDataBuilderDistributorProducts.EntitySet<CrownATTime.Server.Models.DistributorProducts.StreakwaveWarehouseStock>("StreakwaveWarehouseStocks").EntityType.HasKey(entity => new { entity.Sku, entity.WarehouseCode });
    oDataBuilderDistributorProducts.Function("LenovoPsrefgalleryBeforeV27S").Returns<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefgalleryBeforeV27>();
    oDataBuilderDistributorProducts.Function("IngramProductsForQuotings").Returns<CrownATTime.Server.Models.DistributorProducts.IngramProductsForQuoting>();
    oDataBuilderDistributorProducts.Function("IngramProductsWithDetails").Returns<CrownATTime.Server.Models.DistributorProducts.IngramProductsWithDetail>();
    oDataBuilderDistributorProducts.Function("LenovoPsrefapprovedSkuImages").Returns<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefapprovedSkuImage>();
    oDataBuilderDistributorProducts.Function("LenovoPsrefcurrentFamilies").Returns<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefcurrentFamily>();
    oDataBuilderDistributorProducts.Function("LenovoPsrefimageStorageSummaries").Returns<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefimageStorageSummary>();
    oDataBuilderDistributorProducts.Function("LenovoPsrefunmatchedSkus").Returns<CrownATTime.Server.Models.DistributorProducts.LenovoPsrefunmatchedSku>();
    oDataBuilderDistributorProducts.Function("StreakwaveProductsForQuotings").Returns<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsForQuoting>();
    oDataBuilderDistributorProducts.Function("StreakwaveProductsWithEnrichments").Returns<CrownATTime.Server.Models.DistributorProducts.StreakwaveProductsWithEnrichment>();
    opt.AddRouteComponents("odata/DistributorProducts", oDataBuilderDistributorProducts.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<CrownATTime.Client.DistributorProductsService>();
var app = builder.Build();
var forwardingOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
};
forwardingOptions.KnownIPNetworks.Clear();
forwardingOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardingOptions);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();
app.MapControllers();
app.UseHeaderPropagation();
app.MapStaticAssets();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(CrownATTime.Client._Imports).Assembly);
app.Run();