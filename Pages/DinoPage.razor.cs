using System.Net.Http.Json;
using blazorEx.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace blazorEx.Pages;

public partial class DinoPage : ComponentBase
{
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; }

    private HttpClient HttpClient => HttpClientFactory.CreateClient(ApiName.DinoApi);
    private List<Dino>? _dinos;


    protected override async Task OnInitializedAsync()
    {
        _dinos = await HttpClient.GetFromJsonAsync<List<Dino>>("api/dino");
    }
}
