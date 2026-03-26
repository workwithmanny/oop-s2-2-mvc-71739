namespace oop_s2_2_mvc_71739.Models;

public class DashboardPremisesRow
{
    public int PremisesId { get; init; }
    public string PremisesName { get; init; } = string.Empty;
    public string Town { get; init; } = string.Empty;
    public RiskRating RiskRating { get; init; }
    public int LastScore { get; init; }
    public InspectionOutcome LastOutcome { get; init; }
    public int OpenFollowUps { get; init; }
}
