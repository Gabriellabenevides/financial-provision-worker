using System.Text;
using System.Text.Json;
using FinancialProvision.Worker.Application.Events;
using FinancialProvision.Worker.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FinancialProvision.Worker.Infraestructure.Messaging.Consumers;

public class DevolucaoAprovadaConsumer
{
    private readonly IMessageHandler<DevolucaoAprovadaEvent> _handler;

    public DevolucaoAprovadaConsumer(IMessageHandler<DevolucaoAprovadaEvent> handler)
    {
        _handler = handler;
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
            autoDelete: false
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var mensagem = Encoding.UTF8.GetString(ea.Body.ToArray());

                var evento = JsonSerializer.Deserialize<DevolucaoAprovadaEvent>(mensagem);

                if (evento == null) return;

                await _handler.Handle(evento);

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch
            {
                channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        channel.BasicConsume("devolucao-aprovada", false, consumer);

        Console.WriteLine("Worker consumindo fila...");
    }
}