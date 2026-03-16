using BankingPaymentsApiDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingPaymentsApiDemo.Services;

public class ReconciliationService : IReconciliationService
{
    private readonly AppDbContext _context;

    public ReconciliationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> RunReconciliationAsync()
    {
        var total = await _context.PaymentTransactions.CountAsync();
        var posted = await _context.PaymentTransactions.CountAsync(x => x.Status == "Posted");
        var failed = await _context.PaymentTransactions.CountAsync(x => x.Status == "Failed");
        var pending = await _context.PaymentTransactions.CountAsync(x => x.Status == "Pending");

        var totalAmount = await _context.PaymentTransactions.SumAsync(x => x.Amount);
        var postedAmount = await _context.PaymentTransactions
            .Where(x => x.Status == "Posted")
            .SumAsync(x => x.Amount);

        var failedAmount = await _context.PaymentTransactions
            .Where(x => x.Status == "Failed")
            .SumAsync(x => x.Amount);

        return new
        {
            TotalTransactions = total,
            PostedTransactions = posted,
            FailedTransactions = failed,
            PendingTransactions = pending,
            TotalAmount = totalAmount,
            PostedAmount = postedAmount,
            FailedAmount = failedAmount,
            GeneratedAtUtc = DateTime.UtcNow
        };
    }
}