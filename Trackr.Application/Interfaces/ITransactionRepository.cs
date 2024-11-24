using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;
public interface ITransactionRepository
{
    Task<List<Transaction>> GetUserTransactions(string userId, int count, int page);
    Task<Transaction?> GetTransactionById(string transactionId);
    Task<Transaction> AddTransaction(TransactionRequestModel transaction, string userId);
    Task<Transaction> DeleteTransaction(string transactionId);
    Task<Transaction> EditTransaction(TransactionRequestModel transaction, string transactionId);
    Task<List<Transaction>> GetLatestTransaction(int transactionCount, string userId);
}
