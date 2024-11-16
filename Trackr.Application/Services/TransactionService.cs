using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Services;

internal class TransactionService : ITransactionService
{
    private ITransactionRepository _tranRepository;

    public TransactionService(ITransactionRepository tranRepository)
    {
        _tranRepository = tranRepository;
    }

    public async Task<List<Transaction>> GetUserTransactions(int userId, int count, int page)
    {
        List<Transaction> transactions = await _tranRepository.GetUserTransactions(userId, count, page);
        return transactions;
    }

    public async Task<Transaction> AddTransaction(TransactionRequestModel transaction, int userId)
    {
        var responseTransaction = await _tranRepository.AddTransaction(transaction, userId);
        return responseTransaction;
    }

    public async Task<List<Transaction>> GetLatestTransactions(int transactionCount, int userId)
    {
        var responseTransactions = await _tranRepository.GetLatestTransaction(transactionCount, userId);
        return responseTransactions;
    }

    public async Task<Transaction> DeleteTransaction(int transactionId)
    {
        var doesRecordExist = await _tranRepository.GetTransactionById(transactionId);
        
        if (doesRecordExist is null)
        {
            throw new TransactionDoesNotExist("Transaction does not exist with that id", "InvalidTransactionId");
        }

        var transaction = await _tranRepository.DeleteTransaction(transactionId);
        return transaction;
    }
}
