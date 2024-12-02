using System.Linq.Expressions;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;
public interface ITransactionRepository 
{
    Task<List<Transaction>> GetAll(Guid userId, int count, int page, CancellationToken cancellationToken);
    Task<List<Transaction>> GetAll(Expression<Func<Transaction, bool>> predicate, CancellationToken cancellationToken);
    Task<Transaction> Add(TransactionRequestModel transaction, Guid userId, CancellationToken cancellationToken);
    Task<Transaction> Remove(Transaction transaction, CancellationToken cancellationToken);
    Task<Transaction> Update(Transaction transaction, CancellationToken cancellationToken);
    Task<List<Transaction>> GetLatestTransaction(int transactionCount, Guid userId, CancellationToken cancellationToken);
    Task<Transaction?> GetById(Guid transactionId, CancellationToken cancellationToken);
    Task<CurrentAndPreviousMonthExpensesModel> GetCurrentAndPreviousMonthExpenses(Guid id, CancellationToken cancellationToken);
}
