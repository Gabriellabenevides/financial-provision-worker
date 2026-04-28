using FinancialProvision.Worker.Infraestructure.Messaging.Consumers;
using Microsoft.Extensions.Hosting;

namespace Financial_Provision.Worker.Workers;

public class DevolucaoAprovadaWorker : BackgroundService
{
    private readonly DevolucaoAprovadaConsumer _consumer;

    public DevolucaoAprovadaWorker(DevolucaoAprovadaConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Consumir();
        return Task.CompletedTask;
    }
}
