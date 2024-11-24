using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface ITransactionService
{
    Task<List<Transaction>> GetUserTransactions(string userId, int count, int page);
    Task<Transaction> AddTransaction(TransactionRequestModel transaction, string userId);
    Task<List<Transaction>> GetLatestTransactions(int transactionCount, string userId);
    Task<Transaction> DeleteTransaction(string transactionId, string userId);
    Task<Transaction> EditTransaction(TransactionRequestModel transaction, string transactionId, string userId);
}
