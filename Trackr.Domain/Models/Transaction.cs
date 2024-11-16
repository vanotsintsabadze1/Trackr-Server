namespace Trackr.Domain.Models;

public class Transaction
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required int Type { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime TranDate { get; set; }
}

