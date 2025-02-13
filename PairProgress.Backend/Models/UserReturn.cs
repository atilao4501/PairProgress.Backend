namespace PairProgress.Backend.Models;

public class UserReturn
{
    public string UserCode { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? PairName { get; set; }
    public string? PairEmail { get; set; }
    public string? PairCode { get; set; }
}