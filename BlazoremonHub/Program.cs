using BlazoremonHub.Pages;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Refit;

namespace BlazoremonHub;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddMudServices();
        builder.Services.AddRefitClient<IPokeApi>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://pokeapi.co"));

        await builder.Build().RunAsync();
    }
}
