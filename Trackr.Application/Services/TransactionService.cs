using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Services;

internal class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _tranRepository;

    public TransactionService(ITransactionRepository tranRepository)
    {
        _tranRepository = tranRepository;
    }

    public async Task<List<Transaction>> GetUserTransactions(string userId, int count, int page)
    {
        List<Transaction> transactions = await _tranRepository.GetUserTransactions(userId, count, page);
        return transactions;
    }

    public async Task<Transaction> AddTransaction(TransactionRequestModel transaction, string userId)
    {
        var responseTransaction = await _tranRepository.AddTransaction(transaction, userId);
        return responseTransaction;
    }

    public async Task<List<Transaction>> GetLatestTransactions(int transactionCount, string userId)
    {
        var responseTransactions = await _tranRepository.GetLatestTransaction(transactionCount, userId);
        return responseTransactions;
    }

    public async Task<Transaction> DeleteTransaction(string transactionId, string userId)
    {
        var transactionFromDB = await _tranRepository.GetTransactionById(transactionId);

        if (transactionFromDB is null)
        {
            throw new InvalidTransactionException("Transaction does not exist with that id", "InvalidTransaction");
        }

        if (transactionFromDB.UserId.ToString() != userId)
        {
            throw new UserUnauthorizedException("User is not authorized to delete this particular transaction");
        }

        var transaction = await _tranRepository.DeleteTransaction(transactionId);
        return transaction;
    }

    public async Task<Transaction> EditTransaction(TransactionRequestModel newTransaction, string transactionId, string userId)
    {
        var transactionFromDb = await _tranRepository.GetTransactionById(transactionId);

        if (transactionFromDb is null)
        {
            throw new InvalidTransactionException("Transaction does not exist with that id", "InvalidTransaction");
        }

        if (transactionFromDb.UserId.ToString() != userId)
        {
            throw new UserUnauthorizedException("User is not authorized to edit this particular transaction");
        }

        var transaction = await _tranRepository.EditTransaction(newTransaction, transactionId);

        return transaction;
    }
}