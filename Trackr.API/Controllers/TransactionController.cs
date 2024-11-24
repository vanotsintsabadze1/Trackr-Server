using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackr.API.Infrastructure.Helpers;
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
    private HttpContext _ctx;

    public TransactionController(ITransactionService tranService, IHttpContextAccessor ctx)
    {
        _tranService = tranService;
        _ctx = ctx.HttpContext!;
    }

    [Authorize]
    [ApiVersion(1)]
    [HttpGet("GetUserTransactions")]
    public async Task<List<Transaction>> GetUserTransactions(int count, int page)
    {
        var id = CredentialRetriever.GetUserId(_ctx);
        var transactions = await _tranService.GetUserTransactions(id, count, page);
        return transactions;
    }

    [Authorize]
    [ApiVersion(1)]
    [HttpPost("AddTransaction")]
    [Produces("application/json")]
    public async Task<Transaction> Add(TransactionRequestModel transaction)
    {
        var id = CredentialRetriever.GetUserId(_ctx);
        var responseTransaction = await _tranService.AddTransaction(transaction, id);
        return responseTransaction;
    }

    [Authorize]
    [ApiVersion(1)]
    [HttpGet("GetLatestTransaction")]
    [Produces("application/json")]
    public async Task<List<Transaction>> GetLatestTransaction(int transactionCount)
    {
        var id = CredentialRetriever.GetUserId(_ctx);
        var latestTransactions = await _tranService.GetLatestTransactions(transactionCount, id);
        return latestTransactions;
    }

    [Authorize]
    [ApiVersion(1)]
    [HttpDelete("DeleteTransaction/{transactionId}")]
    [Produces("application/json")]
    public async Task<Transaction> DeleteTransaction(int transactionId)
    {
        var id = CredentialRetriever.GetUserId(_ctx);
        var transaction = await _tranService.DeleteTransaction(transactionId, id);
        return transaction;
    }

    [Authorize]
    [ApiVersion(1)]
    [HttpPut("EditTransaction/{transactionId}")]
    [Produces("application/json")]
    public async Task<Transaction> EditTransaction(TransactionRequestModel newTransaction, int transactionId)
    {
        var id = CredentialRetriever.GetUserId(_ctx);
        var transaction = await _tranService.EditTransaction(newTransaction, transactionId, id);
        return transaction;
    }
}
