using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71739.Data;
using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Tests;

public static class TestDbFactory
{
    public static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new ApplicationDbContext(options);
        Seed(context);
        return context;
    }

    private static void Seed(ApplicationDbContext context)
    {
        var premises = new[]
        {
            new Premises { Id = 1, Name = "Alpha Cafe", Address = "1 Main St", Town = "Riverton", RiskRating = RiskRating.Low },
            new Premises { Id = 2, Name = "Beta Grill", Address = "2 Market St", Town = "Bayview", RiskRating = RiskRating.High },
            new Premises { Id = 3, Name = "Gamma Diner", Address = "3 High St", Town = "Oakfield", RiskRating = RiskRating.Medium }
        };

        var inspections = new[]
        {
            new Inspection { Id = 1, PremisesId = 1, InspectionDate = new DateTime(2026, 3, 5), Score = 80, Outcome = InspectionOutcome.Pass, Notes = "Ok" },
            new Inspection { Id = 2, PremisesId = 1, InspectionDate = new DateTime(2026, 3, 10), Score = 62, Outcome = InspectionOutcome.Fail, Notes = "Fail" },
            new Inspection { Id = 3, PremisesId = 2, InspectionDate = new DateTime(2026, 3, 12), Score = 59, Outcome = InspectionOutcome.Fail, Notes = "Fail" },
            new Inspection { Id = 4, PremisesId = 3, InspectionDate = new DateTime(2026, 2, 15), Score = 90, Outcome = InspectionOutcome.Pass, Notes = "Prev month" }
        };

        var followUps = new[]
        {
            new FollowUp { Id = 1, InspectionId = 2, DueDate = new DateTime(2026, 3, 15), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 2, InspectionId = 3, DueDate = new DateTime(2026, 3, 20), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 3, 18) },
            new FollowUp { Id = 3, InspectionId = 4, DueDate = new DateTime(2026, 3, 1), Status = FollowUpStatus.Open, ClosedDate = null }
        };

        context.Premises.AddRange(premises);
        context.Inspections.AddRange(inspections);
        context.FollowUps.AddRange(followUps);
        context.SaveChanges();
    }
}
