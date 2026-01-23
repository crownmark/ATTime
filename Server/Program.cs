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

builder.Services.AddSingleton<CrownATTime.Server.Services.NotificationService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<CrownATTime.Server.Services.NotificationService>());

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
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.BillingCodeCache>("BillingCodeCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.CompanyCache>("CompanyCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.ContractCache>("ContractCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.EmailTemplate>("EmailTemplates");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.NoteTemplate>("NoteTemplates");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.ResourceCache>("ResourceCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.RoleCache>("RoleCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache>("ServiceDeskRoleCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache>("TicketEntityPicklistValueCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache>("TicketNoteEntityPicklistValueCaches");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TimeEntry>("TimeEntries");
    oDataBuilderATTime.EntitySet<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>("TimeEntryTemplates");
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
builder.Services.AddScoped<CrownATTime.Client.TemplateTokenDiscoveryService>();
builder.Services.AddScoped<CrownATTime.Client.ATTimeService>();
builder.Services.Configure<GraphApiOptions>(builder.Configuration.GetSection("GraphApi"));
builder.Services.AddScoped<GraphApi>();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin() //.SetIsOriginAllowed((host) => true)
    .WithOrigins(new[] { "https://timeguard.crown.software", "https://localhost:5001" }).AllowCredentials();
}));
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
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(CrownATTime.Client._Imports).Assembly);
app.Run();