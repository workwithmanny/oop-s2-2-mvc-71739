namespace oop_s2_2_mvc_71739.Models;

public class DashboardInspectionSnippet
{
    public int InspectionId { get; init; }
    public string PremisesName { get; init; } = string.Empty;
    public DateTime InspectionDate { get; init; }
    public int Score { get; init; }
    public InspectionOutcome Outcome { get; init; }
}
