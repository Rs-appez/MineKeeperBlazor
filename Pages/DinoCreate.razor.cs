using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace blazorEx.Pages;

public partial class DinoCreate : ComponentBase
{
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; }
    [Inject]
    public NavigationManager Navigation { get; set; }

    private HttpClient HttpClient => HttpClientFactory.CreateClient(ApiName.DinoApi);

    private Dino _dino = new();


    private async Task CreateDino()
    {
        var response = await HttpClient.PostAsJsonAsync("api/dino", _dino);
        if (response.IsSuccessStatusCode)
        {
            Navigation.NavigateTo("/dino");
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error creating dino: {error}");
        }
    }

    private class Dino
    {
        public string? Espece { get; set; }
        public float LengthMeters { get; set; }
        public float WeightKg { get; set; } 
    }
}
