using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using CrownATTime.Client;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "CrownATTimeTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<CrownATTime.Client.ATTimeService>();
builder.Services.AddScoped<CrownATTime.Client.AutotaskTicketService>();
builder.Services.AddScoped<CrownATTime.Client.AutotaskTimeEntryService>();
builder.Services.AddScoped<CrownATTime.Client.ThreeCxClientService>();
builder.Services.AddScoped<CrownATTime.Client.EmailService>();
builder.Services.AddScoped<CrownATTime.Client.TemplateTokenDiscoveryService>();
builder.Services.AddHttpClient("CrownATTime.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CrownATTime.Server"));
builder.Services.AddScoped<CrownATTime.Client.SecurityService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CrownATTime.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
await host.RunAsync();