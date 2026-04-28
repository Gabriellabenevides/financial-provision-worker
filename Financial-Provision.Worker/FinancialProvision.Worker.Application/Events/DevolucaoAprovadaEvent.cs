namespace FinancialProvision.Worker.Application.Events;

public class DevolucaoAprovadaEvent
{
    public int DevolucaoId { get; set; }
    public decimal Valor { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
}
