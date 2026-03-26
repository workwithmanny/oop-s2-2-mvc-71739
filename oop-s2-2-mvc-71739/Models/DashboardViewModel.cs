namespace oop_s2_2_mvc_71739.Models;

public class DashboardViewModel
{
    public DashboardFilter Filter { get; init; } = new();
    public IReadOnlyList<string> TownOptions { get; init; } = Array.Empty<string>();
    public IReadOnlyList<DashboardPremisesRow> PremisesRows { get; init; } = Array.Empty<DashboardPremisesRow>();
    public IReadOnlyList<DashboardPremisesSnippet> PremisesSnippet { get; init; } = Array.Empty<DashboardPremisesSnippet>();
    public IReadOnlyList<DashboardInspectionSnippet> InspectionsSnippet { get; init; } = Array.Empty<DashboardInspectionSnippet>();
    public IReadOnlyList<DashboardFollowUpSnippet> FollowUpsSnippet { get; init; } = Array.Empty<DashboardFollowUpSnippet>();
    public int InspectionsThisMonth { get; init; }
    public int FailedInspectionsThisMonth { get; init; }
    public int OpenFollowUps { get; init; }
    public int OverdueOpenFollowUps { get; init; }
}
