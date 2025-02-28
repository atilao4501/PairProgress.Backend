using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PairProgress.Backend.Models;

public class Goal
{
    [MaxLength(255)]
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Description { get; set; } 
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; } 
    public DateTime Date { get; set; }
    public decimal RecommendedInvestmentPerMonth { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    
    public List<Contribution> Contributions { get; set; }
    public string AutorName => User.UserName;
    
    
}