namespace oop_s2_2_mvc_71739.Models;

public class DashboardFollowUpSnippet
{
    public int FollowUpId { get; init; }
    public string PremisesName { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public FollowUpStatus Status { get; init; }
}
