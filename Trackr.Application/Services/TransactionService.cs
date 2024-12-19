using Mapster;
using Trackr.API.Infrastructure.Models;
using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _tranRepository;
    private readonly IUserService _userService;

    public TransactionService(ITransactionRepository tranRepository, IUserService userService)
    {
        _tranRepository = tranRepository;
        _userService = userService;
    }

    public async Task<List<Transaction>> GetUserTransactions(string userId, int count, int page, CancellationToken cancellationToken)
    {
        var userGuidId = new Guid(userId);
        var transactions = await _tranRepository.GetAll(userGuidId, count, page, cancellationToken);
        return transactions;
    }

    public async Task<Transaction> AddTransaction(TransactionRequestModel transaction, string userId, CancellationToken cancellationToken)
    {
        var guidId = new Guid(userId);
        var responseTransaction = await _tranRepository.Add(transaction, guidId, cancellationToken: cancellationToken);
        return responseTransaction;
    }

    public async Task<List<Transaction>> GetLatestTransactions(int transactionCount, string userId, CancellationToken cancellationToken)
    {
        var guidId = new Guid(userId);
        var responseTransactions = await _tranRepository.GetLatestTransaction(transactionCount, guidId, cancellationToken);
        return responseTransactions;
    }

    public async Task<Transaction> DeleteTransaction(Guid transactionId, string userId, CancellationToken cancellationToken)
    {
        var transactionFromDB = await _tranRepository.GetById(transactionId, cancellationToken);

        if (transactionFromDB is null)
        {
            throw new InvalidTransactionException("Transaction does not exist with that id", "InvalidTransaction");
        }

        if (transactionFromDB.UserId.ToString() != userId)
        {
            throw new UserUnauthorizedException("User is not authorized to delete this particular transaction");
        }
        
        var transaction = await _tranRepository.Remove(transactionFromDB, cancellationToken);
        return transaction;
    }

    public async Task<Transaction> EditTransaction(TransactionRequestModel newTransaction, Guid transactionId, string userId, CancellationToken cancellationToken)
    {
        var transactionFromDb = await _tranRepository.GetById(transactionId, cancellationToken);

        if (transactionFromDb is null)
        {
            throw new InvalidTransactionException("Transaction does not exist with that id", "InvalidTransaction");
        }

        if (transactionFromDb.UserId.ToString() != userId)
        {
            throw new UserUnauthorizedException("User is not authorized to edit this particular transaction");
        }

        var mappedTransaction = newTransaction.Adapt(transactionFromDb);
        
        var transaction = await _tranRepository.Update(mappedTransaction, cancellationToken: cancellationToken);

        return transaction;
    }

    public async Task<MoneySpentModel> GetMoneySpent(string userId, CancellationToken cancellationToken)
    {
        Guid userGuidId = new Guid(userId);
        var user = await _userService.GetCurrentUser(userId, cancellationToken);
        var transactions = await _tranRepository.GetAll(t => t.UserId == userGuidId && t.TranDate.Month == DateTime.UtcNow.Month, cancellationToken);
        var totalSpent = transactions.Aggregate(0m, (sum, t) => sum + t.Amount);

        return new MoneySpentModel()
        {
            MoneySpent = totalSpent,
            CostLimit = user.CostLimit,
        };
    }

    public async Task<CurrentAndPreviousMonthExpensesModel> GetCurrentAndPreviousMonthExpenses(string id, CancellationToken cancellationToken)
    {
        Guid userGuidId = new Guid(id);
        var currentAndPreviousMonthExpenses = await _tranRepository.GetCurrentAndPreviousMonthExpenses(userGuidId, cancellationToken);
        return currentAndPreviousMonthExpenses;
    }

    public async Task<SortedDictionary<int, decimal>> GetExpensesForTheWholeYear(string id, CancellationToken cancellationToken)
    {
        Guid userGuidId = new Guid(id);
        var expensesOfTheWholeYear = await _tranRepository.GetExpensesForTheWholeYear(userGuidId, cancellationToken);
        return expensesOfTheWholeYear;
    }
}