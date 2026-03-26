using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71739.Data;
using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Controllers;

[Authorize]
public class PremisesController(ApplicationDbContext context, ILogger<PremisesController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var premises = await context.Premises
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync();

        return View(premises);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var premises = await context.Premises
            .AsNoTracking()
            .Include(p => p.Inspections)
            .ThenInclude(i => i.FollowUps)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (premises is null)
        {
            return NotFound();
        }

        return View(premises);
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Create([Bind("Name,Address,Town,RiskRating")] Premises premises)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Premises create failed validation");
            return View(premises);
        }

        context.Add(premises);
        await context.SaveChangesAsync();

        logger.LogInformation("Premises created {PremisesId} {Town} {RiskRating}", premises.Id, premises.Town, premises.RiskRating);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var premises = await context.Premises.FindAsync(id);
        if (premises is null)
        {
            return NotFound();
        }

        return View(premises);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Town,RiskRating")] Premises premises)
    {
        if (id != premises.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Premises edit failed validation {PremisesId}", premises.Id);
            return View(premises);
        }

        try
        {
            context.Update(premises);
            await context.SaveChangesAsync();
            logger.LogInformation("Premises updated {PremisesId}", premises.Id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!PremisesExists(premises.Id))
            {
                return NotFound();
            }

            logger.LogError(ex, "Premises update failed concurrency {PremisesId}", premises.Id);
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

        var premises = await context.Premises
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (premises is null)
        {
            return NotFound();
        }

        return View(premises);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Inspector}")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var premises = await context.Premises.FindAsync(id);
        if (premises is null)
        {
            return RedirectToAction(nameof(Index));
        }

        context.Premises.Remove(premises);
        await context.SaveChangesAsync();

        logger.LogInformation("Premises deleted {PremisesId}", premises.Id);
        return RedirectToAction(nameof(Index));
    }

    private bool PremisesExists(int id)
    {
        return context.Premises.Any(e => e.Id == id);
    }
}
