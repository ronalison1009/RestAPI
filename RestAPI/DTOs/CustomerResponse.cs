public class CustomerResponse
{
    public bool Success { get; set; }
    public List<string> Errors { get; set; } = new();
}