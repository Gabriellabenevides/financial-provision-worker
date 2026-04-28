namespace FinancialProvision.Worker.Application.Interfaces;

public interface IMessageHandler<T>
{
    Task Handle(T message);
}
