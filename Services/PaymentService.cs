using System.Text.Json;
using BankingPaymentsApiDemo.Data;
using BankingPaymentsApiDemo.DTOs;
using BankingPaymentsApiDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingPaymentsApiDemo.Services;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(AppDbContext context, ILogger<PaymentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PaymentValidationResponse> ValidateAsync(PaymentNotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.TransactionReference))
        {
            return new PaymentValidationResponse
            {
                IsValid = false,
                Message = "Transaction reference is required."
            };
        }

        if (request.Amount <= 0)
        {
            return new PaymentValidationResponse
            {
                IsValid = false,
                Message = "Amount must be greater than zero."
            };
        }

        var exists = await _context.PaymentTransactions
            .AnyAsync(x => x.TransactionReference == request.TransactionReference);

        if (exists)
        {
            return new PaymentValidationResponse
            {
                IsValid = false,
                Message = "Duplicate transaction reference detected."
            };
        }

        return new PaymentValidationResponse
        {
            IsValid = true,
            Message = "Transaction is valid."
        };
    }

    public async Task<(bool Success, string Message, PaymentTransaction? Transaction)> ReceivePaymentAsync(PaymentNotificationRequest request)
    {
        var validation = await ValidateAsync(request);

        if (!validation.IsValid)
        {
            await LogWebhookAsync(request.TransactionReference, request, validation.Message, false);
            return (false, validation.Message, null);
        }

        var transaction = new PaymentTransaction
        {
            TransactionReference = request.TransactionReference,
            AccountNumber = request.AccountNumber,
            PayerName = request.PayerName,
            Amount = request.Amount,
            Channel = request.Channel,
            PaymentDateUtc = request.PaymentDateUtc,
            Status = "Pending",
            ProcessedAtUtc = null
        };

        _context.PaymentTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        var processingResult = await ProcessPaymentAsync(transaction);

        await LogWebhookAsync(
            request.TransactionReference,
            request,
            processingResult.Message,
            processingResult.Success
        );

        return (processingResult.Success, processingResult.Message, transaction);
    }

    public async Task<List<PaymentTransaction>> GetTransactionsAsync()
    {
        return await _context.PaymentTransactions
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<PaymentTransaction?> GetTransactionByReferenceAsync(string reference)
    {
        return await _context.PaymentTransactions
            .FirstOrDefaultAsync(x => x.TransactionReference == reference);
    }

    private async Task<(bool Success, string Message)> ProcessPaymentAsync(PaymentTransaction transaction)
    {
        try
        {
            if (transaction.Amount > 100000)
            {
                transaction.Status = "Failed";
                transaction.ErrorMessage = "Transaction exceeds demo processing threshold.";
                transaction.ProcessedAtUtc = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogWarning(
                    "Payment processing failed for reference {Reference}",
                    transaction.TransactionReference
                );

                return (false, "Payment received but processing failed.");
            }

            transaction.Status = "Posted";
            transaction.ErrorMessage = null;
            transaction.ProcessedAtUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Payment processed successfully for reference {Reference}",
                transaction.TransactionReference
            );

            return (true, "Payment received and posted successfully.");
        }
        catch (Exception ex)
        {
            transaction.Status = "Failed";
            transaction.ErrorMessage = ex.Message;
            transaction.ProcessedAtUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogError(
                ex,
                "Unexpected error while processing payment for reference {Reference}",
                transaction.TransactionReference
            );

            return (false, "Payment received but processing encountered an unexpected error.");
        }
    }

    private async Task LogWebhookAsync(string reference, object request, string responseMessage, bool isSuccess)
    {
        var log = new WebhookLog
        {
            TransactionReference = reference,
            RequestPayload = JsonSerializer.Serialize(request),
            ResponseMessage = responseMessage,
            IsSuccess = isSuccess
        };

        _context.WebhookLogs.Add(log);
        await _context.SaveChangesAsync();
    }
}