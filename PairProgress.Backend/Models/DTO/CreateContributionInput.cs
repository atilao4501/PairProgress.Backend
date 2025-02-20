namespace PairProgress.Backend.Models;

public class CreateContributionInput
{
    public int GoalId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}