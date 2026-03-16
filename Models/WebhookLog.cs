namespace BankingPaymentsApiDemo.Models;

public class WebhookLog
{
    public int Id { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public string RequestPayload { get; set; } = string.Empty;
    public string ResponseMessage { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}