using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using blazorEx;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient(ApiName.DinoApi, options =>
{
    options.BaseAddress = new Uri("http://localhost:5192/");
});

await builder.Build().RunAsync();

public static class ApiName
{
    public const string DinoApi = "DinoApi";
}
