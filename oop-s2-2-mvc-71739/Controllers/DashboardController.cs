using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71739.Data;
using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Controllers;

[Authorize]
public class DashboardController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index([FromQuery] DashboardFilter filter)
    {
        var townOptions = await context.Premises
            .AsNoTracking()
            .Select(p => p.Town)
            .Distinct()
            .OrderBy(t => t)
            .ToListAsync();

        var premisesQuery = context.Premises.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(filter.Town))
        {
            premisesQuery = premisesQuery.Where(p => p.Town == filter.Town);
        }

        if (filter.RiskRating is not null)
        {
            premisesQuery = premisesQuery.Where(p => p.RiskRating == filter.RiskRating);
        }

        var filteredPremisesIds = await premisesQuery
            .Select(p => p.Id)
            .ToListAsync();
        var hasFilter = !string.IsNullOrWhiteSpace(filter.Town) || filter.RiskRating is not null;

        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);
        var nextMonthStart = monthStart.AddMonths(1);

        var inspectionsQuery = context.Inspections
            .AsNoTracking()
            .Where(i => i.InspectionDate >= monthStart && i.InspectionDate < nextMonthStart);

        if (hasFilter && filteredPremisesIds.Count == 0)
        {
            inspectionsQuery = inspectionsQuery.Where(_ => false);
        }
        else if (filteredPremisesIds.Count > 0)
        {
            inspectionsQuery = inspectionsQuery.Where(i => filteredPremisesIds.Contains(i.PremisesId));
        }

        var inspectionsThisMonth = await inspectionsQuery.CountAsync();
        var failedInspectionsThisMonth = await inspectionsQuery
            .Where(i => i.Outcome == InspectionOutcome.Fail)
            .CountAsync();

        var openFollowUpsQuery = context.FollowUps
            .AsNoTracking()
            .Where(f => f.Status == FollowUpStatus.Open);

        var overdueFollowUpsQuery = openFollowUpsQuery.Where(f => f.DueDate < today);

        if (hasFilter && filteredPremisesIds.Count == 0)
        {
            openFollowUpsQuery = openFollowUpsQuery.Where(_ => false);
            overdueFollowUpsQuery = overdueFollowUpsQuery.Where(_ => false);
        }
        else if (filteredPremisesIds.Count > 0)
        {
            openFollowUpsQuery = openFollowUpsQuery.Where(f =>
                filteredPremisesIds.Contains(f.Inspection!.PremisesId));
            overdueFollowUpsQuery = overdueFollowUpsQuery.Where(f =>
                filteredPremisesIds.Contains(f.Inspection!.PremisesId));
        }

        var openFollowUps = await openFollowUpsQuery.CountAsync();
        var overdueOpenFollowUps = await overdueFollowUpsQuery.CountAsync();

        var inspectionsList = await inspectionsQuery
            .AsNoTracking()
            .ToListAsync();

        var latestInspectionDetails = inspectionsList
            .GroupBy(i => i.PremisesId)
            .Select(g => g
                .OrderByDescending(i => i.InspectionDate)
                .ThenByDescending(i => i.Id)
                .First())
            .Select(i => new { i.PremisesId, i.Score, i.Outcome })
            .ToList();

        var premisesLookup = await context.Premises
            .AsNoTracking()
            .Where(p => !hasFilter || filteredPremisesIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        var openFollowUpsByPremises = await openFollowUpsQuery
            .GroupBy(f => f.Inspection!.PremisesId)
            .Select(g => new { PremisesId = g.Key, Count = g.Count() })
            .ToListAsync();

        var openFollowUpsMap = openFollowUpsByPremises.ToDictionary(x => x.PremisesId, x => x.Count);

        var premisesRows = latestInspectionDetails
            .Where(x => premisesLookup.ContainsKey(x.PremisesId))
            .Select(x =>
            {
                var premises = premisesLookup[x.PremisesId];
                return new DashboardPremisesRow
                {
                    PremisesId = premises.Id,
                    PremisesName = premises.Name,
                    Town = premises.Town,
                    RiskRating = premises.RiskRating,
                    LastScore = x.Score,
                    LastOutcome = x.Outcome,
                    OpenFollowUps = openFollowUpsMap.GetValueOrDefault(premises.Id, 0)
                };
            })
            .OrderBy(x => x.PremisesName)
            .ToList();

        var premisesSnippet = await premisesQuery
            .OrderByDescending(p => p.RiskRating)
            .ThenBy(p => p.Name)
            .Take(5)
            .Select(p => new DashboardPremisesSnippet
            {
                PremisesId = p.Id,
                Name = p.Name,
                Town = p.Town,
                RiskRating = p.RiskRating
            })
            .ToListAsync();

        var inspectionsSnippet = await inspectionsQuery
            .Include(i => i.Premises)
            .OrderByDescending(i => i.InspectionDate)
            .Take(5)
            .Select(i => new DashboardInspectionSnippet
            {
                InspectionId = i.Id,
                PremisesName = i.Premises == null ? "Unknown" : i.Premises.Name,
                InspectionDate = i.InspectionDate,
                Score = i.Score,
                Outcome = i.Outcome
            })
            .ToListAsync();

        var followUpsSnippet = await openFollowUpsQuery
            .Include(f => f.Inspection)
            .ThenInclude(i => i!.Premises)
            .OrderBy(f => f.DueDate)
            .Take(5)
            .Select(f => new DashboardFollowUpSnippet
            {
                FollowUpId = f.Id,
                PremisesName = f.Inspection == null || f.Inspection.Premises == null
                    ? "Unknown"
                    : f.Inspection.Premises.Name,
                DueDate = f.DueDate,
                Status = f.Status
            })
            .ToListAsync();

        var viewModel = new DashboardViewModel
        {
            Filter = filter,
            TownOptions = townOptions,
            PremisesRows = premisesRows,
            PremisesSnippet = premisesSnippet,
            InspectionsSnippet = inspectionsSnippet,
            FollowUpsSnippet = followUpsSnippet,
            InspectionsThisMonth = inspectionsThisMonth,
            FailedInspectionsThisMonth = failedInspectionsThisMonth,
            OpenFollowUps = openFollowUps,
            OverdueOpenFollowUps = overdueOpenFollowUps
        };

        return View(viewModel);
    }
}
