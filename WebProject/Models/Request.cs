public class Request
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime SubmissionTime { get; set; } = DateTime.Now;
    public DateTime ResolutionDueDate { get; set; }
    public bool IsResolved { get; set; } = false;
}
