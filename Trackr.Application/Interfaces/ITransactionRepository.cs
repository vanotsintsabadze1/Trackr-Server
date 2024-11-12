using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;
public interface ITransactionRepository
{
    Task<List<Transaction>> GetUserTransactions(int userId);
    Task<Transaction> GetTransactionById(int transactionId);
    Task<Transaction> AddTransaction(TransactionRequestModel transaction, int userId);
    Task<Transaction> DeleteTransaction(int transactionId);
    Task<Transaction> EditTransaction(TransactionRequestModel transaction);
}
