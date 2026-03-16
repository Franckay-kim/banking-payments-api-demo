using BankingPaymentsApiDemo.DTOs;
using BankingPaymentsApiDemo.Models;

namespace BankingPaymentsApiDemo.Services;

public interface IPaymentService
{
    Task<PaymentValidationResponse> ValidateAsync(PaymentNotificationRequest request);
    Task<(bool Success, string Message, PaymentTransaction? Transaction)> ReceivePaymentAsync(PaymentNotificationRequest request);
    Task<List<PaymentTransaction>> GetTransactionsAsync();
    Task<PaymentTransaction?> GetTransactionByReferenceAsync(string reference);
}