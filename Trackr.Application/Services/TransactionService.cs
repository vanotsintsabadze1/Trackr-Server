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

    public async Task<List<Transaction>> GetUserTransactions(int userId)
    {
        List<Transaction> transactions = await _tranRepository.GetUserTransactions(userId);
        return transactions;
    }

    public async Task<Transaction> AddTransaction(TransactionRequestModel transaction, int userId)
    {
        var responseTransaction = await _tranRepository.AddTransaction(transaction, userId);
        return responseTransaction;
    }
}
