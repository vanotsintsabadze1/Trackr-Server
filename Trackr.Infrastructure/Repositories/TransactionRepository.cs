using Microsoft.EntityFrameworkCore;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Transactions;
using Trackr.Domain.Models;
using Trackr.Infrastructure.Context;

namespace Trackr.Infrastructure.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(TrackrDBContext dbContext) : base(dbContext)
    { }

    public async Task<Transaction> Add(TransactionRequestModel transaction, Guid userId,
        CancellationToken cancellationToken)
    {
        var newTransaction = new Transaction()
        {
            UserId = userId,
            Type = transaction.Type,
            Title = transaction.Title,
            Description = transaction.Description,
            Amount = transaction.Amount,
        };
        await base.Add(newTransaction, cancellationToken);
        return newTransaction!;
    }

    public new async Task<Transaction> Update(Transaction newTransaction,
        CancellationToken cancellationToken)
    {
        await base.Update(newTransaction, cancellationToken);
        return newTransaction;
    }

    public new async Task<Transaction?> GetById(Guid transactionId, CancellationToken cancellationToken)
    {
        var transaction = await base.GetById(transactionId, cancellationToken: cancellationToken);
        return transaction;
    }

    public async Task<List<Transaction>> GetAll(Guid userId, int count, int page, CancellationToken cancellationToken)
    {
        if (count == 0 || page == 0)
        {
            return new List<Transaction>();
        }

        var offset = (page - 1) * count;
        var transactions = await base.GetAll(t => t.UserId == userId, cancellationToken: cancellationToken);

        if (transactions is not null)
        {
            return transactions.Skip(offset).Take(count).ToList();
        }

        return new List<Transaction>();
    }

    public async Task<List<Transaction>> GetLatestTransaction(int transactionCount, Guid userId, CancellationToken cancellationToken)
    {
        var transactions = await _dbSet.Where(t => t.UserId == userId)
            .OrderByDescending(t => t.TranDate)
            .Take(transactionCount)
            .ToListAsync(cancellationToken: cancellationToken);
        return transactions;
    }

    public new async Task<Transaction> Remove(Transaction transaction, CancellationToken cancellationToken)
    {
        var deletedTransaction = await base.Remove(transaction, cancellationToken);
        return deletedTransaction;
    }

    public async Task<CurrentAndPreviousMonthExpensesModel> GetCurrentAndPreviousMonthExpenses(Guid id, CancellationToken cancellationToken)
    {
        var previousMonthTransactions = await base.GetAll(t => t.TranDate.Month == DateTime.UtcNow.AddMonths(-1).Month && t.UserId == id, cancellationToken);
        var previousMonthExpenses = previousMonthTransactions.Aggregate(0m, (sum, t) => sum + t.Amount);

        var currentMonthTransactions = await base.GetAll(t => t.TranDate.Month == DateTime.UtcNow.Month && t.UserId == id, cancellationToken);
        var currentMonthExpenses = currentMonthTransactions.Aggregate(0m, (sum, t) => sum + t.Amount);

        return new CurrentAndPreviousMonthExpensesModel()
        {
            PreviousMonth = previousMonthExpenses,
            CurrentMonth = currentMonthExpenses
        };
    }

    public async Task<SortedDictionary<int, decimal>> GetExpensesForTheWholeYear(Guid id, CancellationToken cancellationToken)
    {
        var expenses = await base.GetAll(t => t.TranDate.Year == DateTime.Now.Year && t.UserId == id, cancellationToken);
        SortedDictionary<int, decimal> expensesCollectionByMonths = new();

        await Task.Run(() =>
        {
            for (int i = 1; i < 13; i++)
            {
                var monthFullExpense = expenses.AsParallel().Where(t => t.TranDate.Month == i).Aggregate(0m, (sum, t) => sum + t.Amount);
                expensesCollectionByMonths.Add(i, monthFullExpense);
            }
        }, cancellationToken);

        return expensesCollectionByMonths;
    }
}