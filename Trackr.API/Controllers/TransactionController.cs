using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Trackr.API.Infrastructure.Helpers;
using Trackr.API.Infrastructure.Models;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.API.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("/v{version:apiversion}/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _tranService;

    public TransactionController(ITransactionService tranService)
    {
        _tranService = tranService;
    }

    [Authorize]
    [HttpGet("GetUserTransactions")]
    public async Task<List<Transaction>> GetUserTransactions(int count, int page, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var transactions = await _tranService.GetUserTransactions(id, count, page, cancellationToken);
        return transactions;
    }

    [Authorize]
    [HttpPost("AddTransaction")]
    [Produces("application/json")]
    public async Task<Transaction> Add(TransactionRequestModel transaction, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var responseTransaction = await _tranService.AddTransaction(transaction, id, cancellationToken);
        return responseTransaction;
    }

    [Authorize]
    [HttpGet("GetLatestTransaction")]
    [Produces("application/json")]
    public async Task<List<Transaction>> GetLatestTransaction(int transactionCount, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var latestTransactions = await _tranService.GetLatestTransactions(transactionCount, id, cancellationToken);
        return latestTransactions;
    }

    [Authorize]
    [HttpDelete("DeleteTransaction/{transactionId}")]
    [Produces("application/json")]
    public async Task<Transaction> DeleteTransaction(Guid transactionId, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var transaction = await _tranService.DeleteTransaction(transactionId, id, cancellationToken);
        return transaction;
    }

    [Authorize]
    [HttpPut("EditTransaction/{transactionId}")]
    [Produces("application/json")]
    public async Task<Transaction> EditTransaction(TransactionRequestModel newTransaction, Guid transactionId
, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var transaction = await _tranService.EditTransaction(newTransaction, transactionId, id, cancellationToken);
        return transaction;
    }

    [Authorize]
    [HttpGet("GetMoneySpent")]
    [Produces("application/json")]
    public async Task<MoneySpentModel> GetMoneySpent(CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var moneySpent = await _tranService.GetMoneySpent(id, cancellationToken);
        return moneySpent;
    }

    [Authorize]
    [HttpGet("GetPreviousAndCurrentMonthExpenses")]
    [Produces("application/json")]
    public async Task<CurrentAndPreviousMonthExpensesModel> GetCurrentAndPreviousMonthExpenses(CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var currentAndPreviousMonthExpenses = await _tranService.GetCurrentAndPreviousMonthExpenses(id, cancellationToken);
        return currentAndPreviousMonthExpenses;
    }

    [Authorize]
    [HttpGet("GetExpensesForTheWholeYear")]
    [Produces("application/json")]
    public async Task<SortedDictionary<int, decimal>> GetExpensesOfTheWholeYear(CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var expensesOfTheWholeYear = await _tranService.GetExpensesForTheWholeYear(id, cancellationToken);
        return expensesOfTheWholeYear;
    }
}