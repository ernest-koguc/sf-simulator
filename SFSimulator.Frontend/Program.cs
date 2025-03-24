using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SFSimulator.Frontend;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.WebWorkers;
using System.Diagnostics;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();
builder.Services.AddBlazorJSRuntime();
builder.Services.AddWebWorkerService();
builder.Services.AddScoped<Stopwatch>();
builder.Services.AddScoped<DatabaseService>();

builder.Services.RegisterSimulatorCore();

await builder.Build().BlazorJSRunAsync();