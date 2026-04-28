using System.Net.Http.Json;
using FinancialProvision.Worker.Application.Events;
using FinancialProvision.Worker.Application.Interfaces;

namespace FinancialProvision.Worker.Application.Handlers;

public class DevolucaoAprovadaHandler : IMessageHandler<DevolucaoAprovadaEvent>
{
    private readonly HttpClient _httpClient;

    public DevolucaoAprovadaHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task Handle(DevolucaoAprovadaEvent message)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "https://localhost:7001/api/provisao/utilizar",
            message
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao chamar Provision API");
        }
    }
}
