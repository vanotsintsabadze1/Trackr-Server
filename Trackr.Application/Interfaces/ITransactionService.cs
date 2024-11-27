using Trackr.API.Infrastructure.Models;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface ITransactionService
{
    Task<List<Transaction>> GetUserTransactions(string userId, int count, int page, CancellationToken cancellationToken);
    Task<Transaction> AddTransaction(TransactionRequestModel transaction, string userId, CancellationToken cancellationToken);
    Task<List<Transaction>> GetLatestTransactions(int transactionCount, string userId, CancellationToken cancellationToken);
    Task<Transaction> DeleteTransaction(Guid transactionId, string userId, CancellationToken cancellationToken);
    Task<Transaction> EditTransaction(TransactionRequestModel transaction, Guid transactionId, string userId, CancellationToken cancellationToken);
    Task<MoneySpentModel> GetMoneySpent(string userId, CancellationToken cancellationToken);
}
