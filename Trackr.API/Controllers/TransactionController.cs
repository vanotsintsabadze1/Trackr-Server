using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.API.Controllers;


[ApiController]
[ApiVersion(1)]
[Route("/v{version:apiversion}/[controller]")]
public class TransactionController : ControllerBase
{
    private ITransactionService _tranService;

    public TransactionController(ITransactionService tranService)
    {
        _tranService = tranService;
    }

    [Authorize]
    [ApiVersion(1)]
    [HttpGet("GetUserTransactions")]
    public async Task<List<Transaction>> GetAll()
    {
        int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var transactions = await _tranService.GetUserTransactions(id);
        return transactions;
    }

    [Authorize]
    [ApiVersion(1)]
    [HttpPost("AddTransaction")]
    [Produces("application/json")]
    public async Task<Transaction> Add(TransactionRequestModel transaction)
    {
        int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var responseTransaction = await _tranService.AddTransaction(transaction, id);
        return responseTransaction;
    }
}
