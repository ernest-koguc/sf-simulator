using Magic.IndexedDb.Extensions;
using Magic.IndexedDb.Helpers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SFSimulator.Frontend;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.WebWorkers;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<BrowserService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddRadzenComponents();
builder.Services.AddBlazorJSRuntime();
builder.Services.AddWebWorkerService();

builder.Services.AddBlazorDB(options =>
{
    options.Name = Constants.DatabaseName;
    options.Version = Constants.DatabaseVersion;
    options.StoreSchemas = new() { SchemaHelper.GetStoreSchema<SavedResultEntity>() };
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.RegisterSimulatorCore();

await builder.Build().BlazorJSRunAsync();