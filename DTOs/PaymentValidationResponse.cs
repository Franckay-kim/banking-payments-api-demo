namespace BankingPaymentsApiDemo.DTOs;

public class PaymentValidationResponse
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
}