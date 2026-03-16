using BankingPaymentsApiDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingPaymentsApiDemo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<WebhookLog> WebhookLogs => Set<WebhookLog>();
}