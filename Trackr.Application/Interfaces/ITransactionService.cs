using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface ITransactionService
{
    Task<List<Transaction>> GetUserTransactions(int userId, int count, int page);
    Task<Transaction> AddTransaction(TransactionRequestModel transaction, int userId);
    Task<List<Transaction>> GetLatestTransactions(int transactionCount, int userId);
    Task<Transaction> DeleteTransaction(int transactionId);
}
