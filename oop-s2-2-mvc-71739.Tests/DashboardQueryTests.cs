using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Tests;

public class DashboardQueryTests
{
    [Fact]
    public async Task OverdueOpenFollowUps_ReturnsExpectedItems()
    {
        using var context = TestDbFactory.CreateContext(nameof(OverdueOpenFollowUps_ReturnsExpectedItems));

        var today = new DateTime(2026, 3, 18);

        var overdue = await context.FollowUps
            .AsNoTracking()
            .Where(f => f.Status == FollowUpStatus.Open && f.DueDate < today)
            .ToListAsync();

        Assert.Equal(2, overdue.Count);
        Assert.Contains(overdue, f => f.Id == 1);
        Assert.Contains(overdue, f => f.Id == 3);
    }

    [Fact]
    public async Task DashboardCounts_AreConsistentWithSeedData()
    {
        using var context = TestDbFactory.CreateContext(nameof(DashboardCounts_AreConsistentWithSeedData));

        var monthStart = new DateTime(2026, 3, 1);
        var nextMonth = monthStart.AddMonths(1);

        var inspectionsThisMonth = await context.Inspections
            .CountAsync(i => i.InspectionDate >= monthStart && i.InspectionDate < nextMonth);

        var failedThisMonth = await context.Inspections
            .CountAsync(i => i.InspectionDate >= monthStart && i.InspectionDate < nextMonth && i.Outcome == InspectionOutcome.Fail);

        Assert.Equal(3, inspectionsThisMonth);
        Assert.Equal(2, failedThisMonth);
    }
}
