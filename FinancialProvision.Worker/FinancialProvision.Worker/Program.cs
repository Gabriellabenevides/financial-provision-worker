using FinancialProvision.Worker.Application.Events;
using FinancialProvision.Worker.Application.Interfaces;
using FinancialProvision.Worker.Infraestructure.Messaging.Consumers;
using FinancialProvision.Worker;
using FinancialProvision.Worker.Application.Handlers;

var builder = Host.CreateApplicationBuilder(args);

// HttpClient (Scoped automaticamente)
builder.Services.AddHttpClient<
    IMessageHandler<DevolucaoAprovadaEvent>,
    DevolucaoAprovadaHandler>();

// Consumer (Singleton)
builder.Services.AddSingleton<DevolucaoAprovadaConsumer>();

// Worker
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

await app.RunAsync();