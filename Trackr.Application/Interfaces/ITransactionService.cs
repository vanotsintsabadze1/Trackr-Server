using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface ITransactionService
{
    Task<List<Transaction>> GetUserTransactions(int userId);
    Task<Transaction> AddTransaction(TransactionRequestModel transaction, int userId);
}
