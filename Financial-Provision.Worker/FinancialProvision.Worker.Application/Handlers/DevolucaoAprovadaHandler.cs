using FinancialProvision.Worker.Application.Events;
using FinancialProvision.Provision.Application.Entities;
using FinancialProvision.Provision.Application.Interfaces;
using FinancialProvision.Provision.Application.Interfaces.Repositories;
using FinancialProvision.Worker.Application.Interfaces;

namespace FinancialProvision.Worker.Application.Handlers;

public class DevolucaoAprovadaHandler : IMessageHandler<DevolucaoAprovadaEvent>
{
    private readonly IProvisaoDevolucaoRepository _repository;
    private readonly IMovimentacaoProvisaoRepository _movimentacaoRepository;

    public async Task Handle(DevolucaoAprovadaEvent evento)
    {
        var provisao = await _repository
            .GetByMesAnoAsync(evento.Mes, evento.Ano);

        if (provisao == null)
            return;

        provisao.RegistrarUtilizacao(evento.Valor);

        var movimentacao = new MovimentacaoProvisao(
            provisao.Id,
            evento.Valor,
            $"Devolução aprovada ID: {evento.DevolucaoId}"
        );

        await _repository.UpdateAsync(provisao);
        await _movimentacaoRepository.AddAsync(movimentacao);
    }
}
