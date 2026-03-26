using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71739.Data;
using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Controllers;

[Authorize]
public class FollowUpsController(ApplicationDbContext context, ILogger<FollowUpsController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var followUps = await context.FollowUps
            .AsNoTracking()
            .Include(f => f.Inspection)
            .ThenInclude(i => i!.Premises)
            .OrderByDescending(f => f.DueDate)
            .ToListAsync();

        return View(followUps);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var followUp = await context.FollowUps
            .AsNoTracking()
            .Include(f => f.Inspection)
            .ThenInclude(i => i!.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (followUp is null)
        {
            return NotFound();
        }

        return View(followUp);
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public IActionResult Create()
    {
        ViewData["InspectionId"] = new SelectList(
            context.Inspections
                .AsNoTracking()
                .Include(i => i.Premises)
                .OrderByDescending(i => i.InspectionDate)
                .Select(i => new { i.Id, Label = (i.Premises == null ? "Unknown" : i.Premises.Name) + $" ({i.InspectionDate:yyyy-MM-dd})" }),
            "Id",
            "Label");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Create([Bind("InspectionId,DueDate,Status,ClosedDate")] FollowUp followUp)
    {
        var inspection = await context.Inspections
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == followUp.InspectionId);

        if (inspection is null)
        {
            ModelState.AddModelError(nameof(FollowUp.InspectionId), "Inspection not found.");
        }
        else if (followUp.DueDate < inspection.InspectionDate)
        {
            ModelState.AddModelError(nameof(FollowUp.DueDate), "Due date cannot be before inspection date.");
            logger.LogWarning("Follow-up due date before inspection {InspectionId}", inspection.Id);
        }

        if (followUp.Status == FollowUpStatus.Closed && followUp.ClosedDate is null)
        {
            ModelState.AddModelError(nameof(FollowUp.ClosedDate), "Closed date is required when status is Closed.");
        }

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Follow-up create failed validation");
            ViewData["InspectionId"] = new SelectList(
                context.Inspections
                    .AsNoTracking()
                    .Include(i => i.Premises)
                    .OrderByDescending(i => i.InspectionDate)
                    .Select(i => new { i.Id, Label = (i.Premises == null ? "Unknown" : i.Premises.Name) + $" ({i.InspectionDate:yyyy-MM-dd})" }),
                "Id",
                "Label",
                followUp.InspectionId);
            return View(followUp);
        }

        context.Add(followUp);
        await context.SaveChangesAsync();
        logger.LogInformation("Follow-up created {FollowUpId} Inspection {InspectionId} Status {Status}", followUp.Id, followUp.InspectionId, followUp.Status);

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var followUp = await context.FollowUps.FindAsync(id);
        if (followUp is null)
        {
            return NotFound();
        }

        ViewData["InspectionId"] = new SelectList(
            context.Inspections
                .AsNoTracking()
                .Include(i => i.Premises)
                .OrderByDescending(i => i.InspectionDate)
                .Select(i => new { i.Id, Label = (i.Premises == null ? "Unknown" : i.Premises.Name) + $" ({i.InspectionDate:yyyy-MM-dd})" }),
            "Id",
            "Label",
            followUp.InspectionId);

        return View(followUp);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,InspectionId,DueDate,Status,ClosedDate")] FollowUp followUp)
    {
        if (id != followUp.Id)
        {
            return NotFound();
        }

        var inspection = await context.Inspections
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == followUp.InspectionId);

        if (inspection is null)
        {
            ModelState.AddModelError(nameof(FollowUp.InspectionId), "Inspection not found.");
        }
        else if (followUp.DueDate < inspection.InspectionDate)
        {
            ModelState.AddModelError(nameof(FollowUp.DueDate), "Due date cannot be before inspection date.");
            logger.LogWarning("Follow-up due date before inspection {InspectionId}", inspection.Id);
        }

        if (followUp.Status == FollowUpStatus.Closed && followUp.ClosedDate is null)
        {
            ModelState.AddModelError(nameof(FollowUp.ClosedDate), "Closed date is required when status is Closed.");
        }

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Follow-up edit failed validation {FollowUpId}", followUp.Id);
            ViewData["InspectionId"] = new SelectList(
                context.Inspections
                    .AsNoTracking()
                    .Include(i => i.Premises)
                    .OrderByDescending(i => i.InspectionDate)
                    .Select(i => new { i.Id, Label = (i.Premises == null ? "Unknown" : i.Premises.Name) + $" ({i.InspectionDate:yyyy-MM-dd})" }),
                "Id",
                "Label",
                followUp.InspectionId);
            return View(followUp);
        }

        try
        {
            context.Update(followUp);
            await context.SaveChangesAsync();
            logger.LogInformation("Follow-up updated {FollowUpId} Status {Status}", followUp.Id, followUp.Status);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!FollowUpExists(followUp.Id))
            {
                return NotFound();
            }

            logger.LogError(ex, "Follow-up update failed concurrency {FollowUpId}", followUp.Id);
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var followUp = await context.FollowUps
            .AsNoTracking()
            .Include(f => f.Inspection)
            .ThenInclude(i => i!.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (followUp is null)
        {
            return NotFound();
        }

        return View(followUp);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var followUp = await context.FollowUps.FindAsync(id);
        if (followUp is null)
        {
            return RedirectToAction(nameof(Index));
        }

        context.FollowUps.Remove(followUp);
        await context.SaveChangesAsync();
        logger.LogInformation("Follow-up deleted {FollowUpId}", followUp.Id);

        return RedirectToAction(nameof(Index));
    }

    private bool FollowUpExists(int id)
    {
        return context.FollowUps.Any(e => e.Id == id);
    }
}
