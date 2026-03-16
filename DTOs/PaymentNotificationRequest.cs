using System.ComponentModel.DataAnnotations;

namespace BankingPaymentsApiDemo.DTOs;

public class PaymentNotificationRequest
{
    [Required]
    public string TransactionReference { get; set; } = string.Empty;

    [Required]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    public string PayerName { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public string Channel { get; set; } = string.Empty;

    [Required]
    public DateTime PaymentDateUtc { get; set; }
}