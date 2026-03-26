using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71739.Data;
using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Controllers;

[Authorize]
public class InspectionsController(ApplicationDbContext context, ILogger<InspectionsController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var inspections = await context.Inspections
            .AsNoTracking()
            .Include(i => i.Premises)
            .OrderByDescending(i => i.InspectionDate)
            .ToListAsync();

        return View(inspections);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var inspection = await context.Inspections
            .AsNoTracking()
            .Include(i => i.Premises)
            .Include(i => i.FollowUps)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (inspection is null)
        {
            return NotFound();
        }

        return View(inspection);
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public IActionResult Create()
    {
        ViewData["PremisesId"] = new SelectList(context.Premises.AsNoTracking().OrderBy(p => p.Name), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Create([Bind("PremisesId,InspectionDate,Score,Outcome,Notes")] Inspection inspection)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Inspection create failed validation");
            ViewData["PremisesId"] = new SelectList(context.Premises.AsNoTracking().OrderBy(p => p.Name), "Id", "Name", inspection.PremisesId);
            return View(inspection);
        }

        context.Add(inspection);
        await context.SaveChangesAsync();
        logger.LogInformation("Inspection created {InspectionId} Premises {PremisesId} Outcome {Outcome}", inspection.Id, inspection.PremisesId, inspection.Outcome);

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var inspection = await context.Inspections.FindAsync(id);
        if (inspection is null)
        {
            return NotFound();
        }

        ViewData["PremisesId"] = new SelectList(context.Premises.AsNoTracking().OrderBy(p => p.Name), "Id", "Name", inspection.PremisesId);
        return View(inspection);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,PremisesId,InspectionDate,Score,Outcome,Notes")] Inspection inspection)
    {
        if (id != inspection.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Inspection edit failed validation {InspectionId}", inspection.Id);
            ViewData["PremisesId"] = new SelectList(context.Premises.AsNoTracking().OrderBy(p => p.Name), "Id", "Name", inspection.PremisesId);
            return View(inspection);
        }

        try
        {
            context.Update(inspection);
            await context.SaveChangesAsync();
            logger.LogInformation("Inspection updated {InspectionId}", inspection.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!InspectionExists(inspection.Id))
            {
                return NotFound();
            }

            logger.LogError(ex, "Inspection update failed concurrency {InspectionId}", inspection.Id);
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

        var inspection = await context.Inspections
            .AsNoTracking()
            .Include(i => i.Premises)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (inspection is null)
        {
            return NotFound();
        }

        return View(inspection);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var inspection = await context.Inspections.FindAsync(id);
        if (inspection is null)
        {
            return RedirectToAction(nameof(Index));
        }

        context.Inspections.Remove(inspection);
        await context.SaveChangesAsync();
        logger.LogInformation("Inspection deleted {InspectionId}", inspection.Id);

        return RedirectToAction(nameof(Index));
    }

    private bool InspectionExists(int id)
    {
        return context.Inspections.Any(e => e.Id == id);
    }
}
