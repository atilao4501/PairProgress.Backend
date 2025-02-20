namespace PairProgress.Backend.Models;

public class HistoryOfContributionsPerGoal
{
    public int GoalId { get; set; }
    public int GoalName { get; set; }
    public List<Contribution> Contributions { get; set; }
}