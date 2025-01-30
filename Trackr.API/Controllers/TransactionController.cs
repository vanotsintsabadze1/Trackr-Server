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

    /// <summary>
    /// Gets all the user's transactions
    /// </summary>
    /// <param name="count"></param>
    /// <param name="page"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Transactions were retrieved successfully</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpGet]
    public async Task<List<Transaction>> GetUserTransactions(int count, int page, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var transactions = await _tranService.GetUserTransactions(id, count, page, cancellationToken);
        return transactions;
    }


    /// <summary>
    /// Adds the user transaction to the history
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Transactions was added successfully</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpPost]
    [Produces("application/json")]
    public async Task<Transaction> Add(TransactionRequestModel transaction, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var responseTransaction = await _tranService.AddTransaction(transaction, id, cancellationToken);
        return responseTransaction;
    }

    /// <summary>
    /// Gets a given amount of the latest transactions
    /// </summary>
    /// <param name="transactionCount"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Transactions were retrieved successfully</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpGet("latest-transactions")]
    [Produces("application/json")]
    public async Task<List<Transaction>> GetLatestTransaction(int transactionCount, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var latestTransactions = await _tranService.GetLatestTransactions(transactionCount, id, cancellationToken);
        return latestTransactions;
    }

    /// <summary>
    /// Deletes a transaction with a given id
    /// </summary>
    /// <param name="transactionId"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Transaction was deleted successfully</response>
    /// <response code="404">Transaction is invalid</response>
    /// <response code="401">Unauthorized to delete the transaction</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpDelete("{transactionId}")]
    [Produces("application/json")]
    public async Task<Transaction> DeleteTransaction(Guid transactionId, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var transaction = await _tranService.DeleteTransaction(transactionId, id, cancellationToken);
        return transaction;
    }

    /// <summary>
    /// Edits the transaction with a given id
    /// </summary>
    /// <param name="newTransaction"></param>
    /// <param name="transactionId"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Transaction was edited successfully</response>
    /// <response code="404">Transaction is invalid</response>
    /// <response code="401">Unauthorized to edit the transaction</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpPut("{transactionId}")]
    [Produces("application/json")]
    public async Task<Transaction> EditTransaction(TransactionRequestModel newTransaction, Guid transactionId
, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var transaction = await _tranService.EditTransaction(newTransaction, transactionId, id, cancellationToken);
        return transaction;
    }

    /// <summary>
    /// Gets the amount of money that the user spent in this month
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Spent money was retrieved successfully</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpGet("money-spent")]
    [Produces("application/json")]
    public async Task<MoneySpentModel> GetMoneySpent(CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var moneySpent = await _tranService.GetMoneySpent(id, cancellationToken);
        return moneySpent;
    }

    /// <summary>
    /// Returns an object with previous and current month total expenses
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Expenses were retrieved successfully</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpGet("previous-and-current-month")]
    [Produces("application/json")]
    public async Task<CurrentAndPreviousMonthExpensesModel> GetCurrentAndPreviousMonthExpenses(CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var currentAndPreviousMonthExpenses = await _tranService.GetCurrentAndPreviousMonthExpenses(id, cancellationToken);
        return currentAndPreviousMonthExpenses;
    }

    /// <summary>
    /// Returns expense of every month in a previous year
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Expenses were retrieved successfully</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpGet("yearly-expense")]
    [Produces("application/json")]
    public async Task<SortedDictionary<int, decimal>> GetExpensesOfTheWholeYear(CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var expensesOfTheWholeYear = await _tranService.GetExpensesForTheWholeYear(id, cancellationToken);
        return expensesOfTheWholeYear;
    }
}