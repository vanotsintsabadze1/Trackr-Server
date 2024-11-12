namespace Trackr.Application.Models.Transactions;

public class TransactionRequestModel
{
    public required int Type { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Amount { get; set; }
}
