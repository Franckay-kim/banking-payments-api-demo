using BankingPaymentsApiDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingPaymentsApiDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReconciliationController : ControllerBase
{
    private readonly IReconciliationService _reconciliationService;

    public ReconciliationController(IReconciliationService reconciliationService)
    {
        _reconciliationService = reconciliationService;
    }

    [Authorize]
    [HttpPost("run")]
    public async Task<IActionResult> Run()
    {
        var result = await _reconciliationService.RunReconciliationAsync();
        return Ok(result);
    }
}