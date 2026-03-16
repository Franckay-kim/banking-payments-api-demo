namespace BankingPaymentsApiDemo.Models;

public class PaymentTransaction
{
    public int Id { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string PayerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Channel { get; set; } = string.Empty;
    public DateTime PaymentDateUtc { get; set; }
    public string Status { get; set; } = "Pending";
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAtUtc { get; set; }
}