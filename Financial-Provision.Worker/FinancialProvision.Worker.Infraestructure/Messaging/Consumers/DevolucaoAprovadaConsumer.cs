using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using FinancialProvision.Worker.Application.Events;
using FinancialProvision.Provision.Application.Entities;
using FinancialProvision.Provision.Application.Interfaces;
using FinancialProvision.Provision.Application.Interfaces.Repositories;
using FinancialProvision.Worker.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialProvision.Worker.Infraestructure.Messaging.Consumers;

public class DevolucaoAprovadaConsumer
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DevolucaoAprovadaConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void Consumir()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "devolucao-aprovada",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var mensagem = Encoding.UTF8.GetString(ea.Body.ToArray());

                Console.WriteLine("📥 Evento recebido:");
                Console.WriteLine(mensagem);

                var evento = JsonSerializer.Deserialize<DevolucaoAprovadaEvent>(mensagem);

                if (evento == null)
                    return;

                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider
                    .GetRequiredService<IMessageHandler<DevolucaoAprovadaEvent>>();

                await handler.Handle(evento);

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro:");
                Console.WriteLine(ex.Message);

                channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        channel.BasicConsume(
            queue: "devolucao-aprovada",
            autoAck: false,
            consumer: consumer
        );

        Console.WriteLine("🚀 Worker consumindo fila...");
    }
}