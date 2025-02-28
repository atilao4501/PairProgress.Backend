namespace PairProgress.Backend.Models;

public class DefaultReturn
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}