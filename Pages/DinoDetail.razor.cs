using System.Net.Http.Json;
using blazorEx.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace blazorEx.Pages;

public partial class DinoDetail : ComponentBase
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; }

    private HttpClient HttpClient => HttpClientFactory.CreateClient(ApiName.DinoApi);

    private Dino? _dino;

    protected override async Task OnInitializedAsync()
    {
        _dino = await HttpClient.GetFromJsonAsync<Dino>($"api/dino/{Id}");
    }
}
