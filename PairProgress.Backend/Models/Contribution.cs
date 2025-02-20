using System.Text.Json.Serialization;

namespace PairProgress.Backend.Models;

public class Contribution
{
    public int Id { get; set; }
    [JsonIgnore]
    public Goal Goal { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public string? ContributorName => User.UserName;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}