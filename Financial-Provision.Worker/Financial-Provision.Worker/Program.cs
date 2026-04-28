using FinancialProvision.Worker.Application.Events;
using Financial_Provision.Worker.Workers;
using FinancialProvision.Provision.Application.Interfaces;
using FinancialProvision.Provision.Application.Interfaces.Repositories;
using FinancialProvision.Worker.Application.Handlers;
using FinancialProvision.Worker.Application.Interfaces;
using FinancialProvision.Worker.Infraestructure.Messaging.Consumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);

// ?? Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ?? DbContext
builder.Services.AddDbContext<FinancialProvisionDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

// ?? Repositórios
builder.Services.AddScoped<IProvisaoDevolucaoRepository, ProvisaoDevolucaoRepository>();
builder.Services.AddScoped<IMovimentacaoProvisaoRepository, MovimentacaoProvisaoRepository>();

// ?? Handler (regra de negócio)
builder.Services.AddScoped<IMessageHandler<DevolucaoAprovadaEvent>, DevolucaoAprovadaHandler>();

// ?? Consumer
builder.Services.AddSingleton<DevolucaoAprovadaConsumer>();

// ?? Worker
builder.Services.AddHostedService<DevolucaoAprovadaWorker>();

var app = builder.Build();

await app.RunAsync();