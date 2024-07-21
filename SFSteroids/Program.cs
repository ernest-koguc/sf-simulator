using Blazor.BrowserExtension;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFSteroids.Pages.InsideSFGame;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFSteroids
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.UseBrowserExtension(browserExtension =>
            {
                if (browserExtension.Mode == BrowserExtensionMode.ContentScript || builder.HostEnvironment.IsDevelopment())
                {
                    builder.RootComponents.Add<Main>("#SFSteroids");
                }
                else
                {
                    builder.RootComponents.Add<App>("#app");
                    builder.RootComponents.Add<HeadOutlet>("head::after");
                }
            });

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            await builder.Build().RunAsync();
        }
    }
}