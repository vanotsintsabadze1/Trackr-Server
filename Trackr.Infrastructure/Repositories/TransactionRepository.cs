using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Transactions;
using Dapper;
using Trackr.Domain.Models;

namespace Trackr.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository, IDisposable
{
    private readonly SqlConnection _con;

    public TransactionRepository(IConfiguration configuration)
    {
        var conString = configuration.GetConnectionString(name: "DefaultConnection")!;
        _con = new(conString);
        _con.Open();
    }
    public void Dispose()
    {
        _con.Close();
    }

    public async Task<Transaction> AddTransaction(TransactionRequestModel transaction, string userId)
    {
        var newTransaction = await _con.QuerySingleOrDefaultAsync<Transaction>("INSERT INTO Transactions (Id, UserId, Type, Title, Description, Amount) OUTPUT INSERTED.* VALUES (@id, @userId, @type, @title, @description, @amount)",
            new
            {
                id = Guid.NewGuid(),
                userId,
                type = transaction.Type,
                title = transaction.Title,
                description = transaction.Description,
                amount = transaction.Amount,
            });
        return newTransaction!;
    }

    public async Task<Transaction> DeleteTransaction(string transactionId)
    {
        var transaction = await _con.QueryFirstOrDefaultAsync<Transaction>("DELETE FROM Transactions OUTPUT DELETED.* WHERE Id = @transactionid", new { transactionId });
        return transaction!;
    }

    public async Task<Transaction> EditTransaction(TransactionRequestModel newTransaction, string transactionId)
    {
        var transaction = await _con.QueryFirstOrDefaultAsync<Transaction>("UPDATE Transactions SET Title = @title, Description = @description, Type = @type, Amount = @amount OUTPUT INSERTED.* WHERE Id = @id", new
        {
            title = newTransaction.Title,
            description = newTransaction.Description,
            type = newTransaction.Type,
            amount = newTransaction.Amount,
            id = transactionId,
        });

        return transaction!;
    }

    public Task<Transaction?> GetTransactionById(string transactionId)
    {
        var transaction = _con.QueryFirstOrDefaultAsync<Transaction>("SELECT * FROM Transactions WHERE Id = @transactionId", new { transactionId });
        return transaction;
    }

    public async Task<List<Transaction>> GetUserTransactions(string userId, int count, int page)
    {

        if (count == 0 || page == 0)
        {
            return new List<Transaction>();
        }

        var offset = (page - 1) * count;
        var transactions = await _con.QueryAsync<Transaction>(
            "SELECT * FROM Transactions WHERE UserId = @userId ORDER BY TranDate DESC OFFSET @offset ROWS FETCH NEXT @count ROWS ONLY",
            new { userId, offset, count }
        );

        return transactions.ToList() ?? new List<Transaction>();
    }

    public async Task<List<Transaction>> GetLatestTransaction(int transactionCount, string userId)
    {
        var transactions = await _con.QueryAsync<Transaction>("SELECT TOP (@transactionCount) * FROM Transactions WHERE UserId = @userId ORDER BY TranDate DESC",
            new
            {
                transactionCount,
                userId
            });
        return transactions.ToList() ?? new List<Transaction>();
    }
}
