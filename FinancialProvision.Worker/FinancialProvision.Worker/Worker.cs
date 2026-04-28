using FinancialProvision.Worker.Infraestructure.Messaging.Consumers;

namespace FinancialProvision.Worker;

public class Worker : BackgroundService
{
    private readonly DevolucaoAprovadaConsumer _consumer;

    public Worker(DevolucaoAprovadaConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Consumir();
        return Task.CompletedTask;
    }
}