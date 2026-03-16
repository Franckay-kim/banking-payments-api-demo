using BankingPaymentsApiDemo.DTOs;
using BankingPaymentsApiDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingPaymentsApiDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate(PaymentNotificationRequest request)
    {
        var result = await _paymentService.ValidateAsync(request);
        return Ok(result);
    }

    [HttpPost("notify")]
    public async Task<IActionResult> Notify(PaymentNotificationRequest request)
    {
        var result = await _paymentService.ReceivePaymentAsync(request);

        if (!result.Success)
        {
            return BadRequest(new { result.Message });
        }

        return Ok(new
        {
            result.Message,
            result.Transaction
        });
    }

    [Authorize]
    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var transactions = await _paymentService.GetTransactionsAsync();
        return Ok(transactions);
    }

    [Authorize]
    [HttpGet("transactions/{reference}")]
    public async Task<IActionResult> GetTransactionByReference(string reference)
    {
        var transaction = await _paymentService.GetTransactionByReferenceAsync(reference);

        if (transaction == null)
        {
            return NotFound(new { message = "Transaction not found." });
        }

        return Ok(transaction);
    }
}