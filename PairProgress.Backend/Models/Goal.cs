namespace PairProgress.Backend.Models;

public class Goal
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; } 
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; } 
    public DateTime Date { get; set; }
    public decimal RecommendedInvestmentPerMonth { get; set; }
    public User User { get; set; }
}