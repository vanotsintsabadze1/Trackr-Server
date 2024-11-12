using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Trackr.Application.Interfaces;
using Trackr.Application.Models.Transactions;
using Dapper;
using Trackr.Domain.Models;

namespace Trackr.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository, IDisposable
{
    private string _conString;
    private SqlConnection _con;

    public TransactionRepository(IConfiguration configuration)
    {
        _conString = configuration.GetConnectionString(name: "DefaultConnection")!;
        _con = new(_conString);
        _con.Open();
    }
    public void Dispose()
    {
        _con.Close();
    }

    public async Task<Transaction> AddTransaction(TransactionRequestModel transaction, int userId)
    {
        var newTransaction = await _con.QuerySingleOrDefaultAsync<Transaction>("INSERT INTO Transactions (UserId, Type, Title, Description, Amount) OUTPUT INSERTED.* VALUES (@userId, @type, @title, @description, @amount)",
            new
            {
                userId,
                type = transaction.Type,
                title = transaction.Title,
                description = transaction.Description,
                amount = transaction.Amount,
            });

        return newTransaction!;
        
    }

    public Task<Transaction> DeleteTransaction(int transactionId)
    {
        throw new NotImplementedException();
    }


    public Task<Transaction> EditTransaction(TransactionRequestModel transaction)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction> GetTransactionById(int transactionId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Transaction>> GetUserTransactions(int userId)
    {
        var transactions = await _con.QueryAsync<Transaction>("SELECT * FROM Transactions WHERE UserId = @userId", new { userId });

        return transactions.ToList() ?? new List<Transaction>();
    }
}
