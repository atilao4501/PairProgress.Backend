namespace PairProgress.Backend.Models;

public class CreateGoalInput
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; } = 0;
    public DateTime Date { get; set; }
    public decimal RecommendedInvestmentPerMonth { get; set; }
}