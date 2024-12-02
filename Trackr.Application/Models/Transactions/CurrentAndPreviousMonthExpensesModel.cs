namespace Trackr.Application.Models.Transactions;

public class CurrentAndPreviousMonthExpensesModel
{
    public required decimal PreviousMonth { get; set; }
    public required decimal CurrentMonth { get; set; }
}
