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
    public async Task<List<Transaction>> GetUserTransactions(int count, int page)
    {
        int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var transactions = await _tranService.GetUserTransactions(id, count, page);
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

    [Authorize]
    [ApiVersion(1)]
    [HttpGet("GetLatestTransaction")]
    [Produces("application/json")]
    public async Task<List<Transaction>> GetLatestTransaction(int transactionCount)
    {
        int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var latestTransactions = await _tranService.GetLatestTransactions(transactionCount, id);
        return latestTransactions;
    }

    [Authorize]
    [ApiVersion(1)]
    [HttpDelete("DeleteTransaction")]
    [Produces("application/json")]
    public async Task<Transaction> DeleteTransaction(int transactionId)
    {
        var transaction = await _tranService.DeleteTransaction(transactionId);
        return transaction;
    }
}
