namespace oop_s2_2_mvc_71739.Models;

public class DashboardPremisesSnippet
{
    public int PremisesId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Town { get; init; } = string.Empty;
    public RiskRating RiskRating { get; init; }
}
