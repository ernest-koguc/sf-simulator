using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SFSimulator.Frontend;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.WebWorkers;
using System.Diagnostics;


try
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddRadzenComponents();
    builder.Services.AddBlazorJSRuntime();
    builder.Services.AddWebWorkerService();
    builder.Services.AddScoped<Stopwatch>();
    builder.Services.AddScoped<DatabaseService>();

    builder.UseSentry(opt =>
    {
        opt.Dsn = "https://d103b6e2ce6e44741ec35aa5fbff9e69@o4509968167403520.ingest.de.sentry.io/4509968173170768";
    });
    builder.Services.RegisterSimulatorCore();
    builder.Logging.SetMinimumLevel(LogLevel.Warning);

    await builder.Build().BlazorJSRunAsync();
}
catch (Exception ex)
{
    SentrySdk.CaptureException(ex);
    await SentrySdk.FlushAsync();
    throw;
}