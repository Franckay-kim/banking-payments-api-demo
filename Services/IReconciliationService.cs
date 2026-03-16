namespace BankingPaymentsApiDemo.Services;

public interface IReconciliationService
{
    Task<object> RunReconciliationAsync();
}