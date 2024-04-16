public class Inquiry
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime SubmissionTime { get; set; } = DateTime.Now;
    public DateTime ResolutionDueDate { get; set; }
    public bool IsResolved { get; set; } = false;
}
