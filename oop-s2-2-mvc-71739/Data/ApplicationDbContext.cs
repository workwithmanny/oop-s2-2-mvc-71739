using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using oop_s2_2_mvc_71739.Models;

namespace oop_s2_2_mvc_71739.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Premises> Premises => Set<Premises>();
    public DbSet<Inspection> Inspections => Set<Inspection>();
    public DbSet<FollowUp> FollowUps => Set<FollowUp>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Premises>()
            .Property(p => p.RiskRating)
            .HasConversion<string>();

        builder.Entity<Inspection>()
            .Property(i => i.Outcome)
            .HasConversion<string>();

        builder.Entity<FollowUp>()
            .Property(f => f.Status)
            .HasConversion<string>();

        builder.Entity<Premises>()
            .HasMany(p => p.Inspections)
            .WithOne(i => i.Premises)
            .HasForeignKey(i => i.PremisesId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Inspection>()
            .HasMany(i => i.FollowUps)
            .WithOne(f => f.Inspection)
            .HasForeignKey(f => f.InspectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Premises>().HasData(
            new Premises { Id = 1, Name = "Sunrise Deli", Address = "12 Maple St", Town = "Riverton", RiskRating = RiskRating.Low },
            new Premises { Id = 2, Name = "Harbor Grill", Address = "5 Dock Rd", Town = "Bayview", RiskRating = RiskRating.Medium },
            new Premises { Id = 3, Name = "Oakfield Bakery", Address = "44 Oak Ave", Town = "Oakfield", RiskRating = RiskRating.Low },
            new Premises { Id = 4, Name = "Riverton Sushi", Address = "18 River Ln", Town = "Riverton", RiskRating = RiskRating.High },
            new Premises { Id = 5, Name = "Bayview Tacos", Address = "201 Coast Blvd", Town = "Bayview", RiskRating = RiskRating.Medium },
            new Premises { Id = 6, Name = "Oakfield Bistro", Address = "77 Pine Rd", Town = "Oakfield", RiskRating = RiskRating.High },
            new Premises { Id = 7, Name = "Riverton Cafe", Address = "3 Market Sq", Town = "Riverton", RiskRating = RiskRating.Medium },
            new Premises { Id = 8, Name = "Bayview Pizza", Address = "9 Harbor St", Town = "Bayview", RiskRating = RiskRating.Low },
            new Premises { Id = 9, Name = "Oakfield Diner", Address = "122 Main St", Town = "Oakfield", RiskRating = RiskRating.Medium },
            new Premises { Id = 10, Name = "Riverbank Noodles", Address = "65 Canal Way", Town = "Riverton", RiskRating = RiskRating.High },
            new Premises { Id = 11, Name = "Seaside Snacks", Address = "400 Ocean Dr", Town = "Bayview", RiskRating = RiskRating.High },
            new Premises { Id = 12, Name = "Pine Street Eatery", Address = "14 Pine St", Town = "Oakfield", RiskRating = RiskRating.Low }
        );

        builder.Entity<Inspection>().HasData(
            new Inspection { Id = 1, PremisesId = 1, InspectionDate = new DateTime(2025, 12, 15), Score = 92, Outcome = InspectionOutcome.Pass, Notes = "Good hygiene practices." },
            new Inspection { Id = 2, PremisesId = 2, InspectionDate = new DateTime(2025, 12, 20), Score = 78, Outcome = InspectionOutcome.Pass, Notes = "Minor documentation gaps." },
            new Inspection { Id = 3, PremisesId = 3, InspectionDate = new DateTime(2026, 1, 5), Score = 88, Outcome = InspectionOutcome.Pass, Notes = "Clean storage areas." },
            new Inspection { Id = 4, PremisesId = 4, InspectionDate = new DateTime(2026, 1, 12), Score = 62, Outcome = InspectionOutcome.Fail, Notes = "Temperature logs missing." },
            new Inspection { Id = 5, PremisesId = 5, InspectionDate = new DateTime(2026, 1, 18), Score = 75, Outcome = InspectionOutcome.Pass, Notes = "Sanitizer levels acceptable." },
            new Inspection { Id = 6, PremisesId = 6, InspectionDate = new DateTime(2026, 1, 22), Score = 54, Outcome = InspectionOutcome.Fail, Notes = "Cross-contamination risk." },
            new Inspection { Id = 7, PremisesId = 7, InspectionDate = new DateTime(2026, 1, 28), Score = 81, Outcome = InspectionOutcome.Pass, Notes = "Improved from last visit." },
            new Inspection { Id = 8, PremisesId = 8, InspectionDate = new DateTime(2026, 2, 2), Score = 95, Outcome = InspectionOutcome.Pass, Notes = "Excellent record keeping." },
            new Inspection { Id = 9, PremisesId = 9, InspectionDate = new DateTime(2026, 2, 5), Score = 68, Outcome = InspectionOutcome.Fail, Notes = "Pest control issues." },
            new Inspection { Id = 10, PremisesId = 10, InspectionDate = new DateTime(2026, 2, 8), Score = 83, Outcome = InspectionOutcome.Pass, Notes = "Staff training updated." },
            new Inspection { Id = 11, PremisesId = 11, InspectionDate = new DateTime(2026, 2, 10), Score = 59, Outcome = InspectionOutcome.Fail, Notes = "Food storage violations." },
            new Inspection { Id = 12, PremisesId = 12, InspectionDate = new DateTime(2026, 2, 14), Score = 90, Outcome = InspectionOutcome.Pass, Notes = "Well maintained kitchen." },
            new Inspection { Id = 13, PremisesId = 1, InspectionDate = new DateTime(2026, 2, 18), Score = 86, Outcome = InspectionOutcome.Pass, Notes = "No major issues." },
            new Inspection { Id = 14, PremisesId = 2, InspectionDate = new DateTime(2026, 2, 20), Score = 73, Outcome = InspectionOutcome.Pass, Notes = "Needs label updates." },
            new Inspection { Id = 15, PremisesId = 3, InspectionDate = new DateTime(2026, 2, 24), Score = 64, Outcome = InspectionOutcome.Fail, Notes = "Cleaning schedule inconsistent." },
            new Inspection { Id = 16, PremisesId = 4, InspectionDate = new DateTime(2026, 3, 3), Score = 70, Outcome = InspectionOutcome.Pass, Notes = "Improved compliance." },
            new Inspection { Id = 17, PremisesId = 5, InspectionDate = new DateTime(2026, 3, 5), Score = 58, Outcome = InspectionOutcome.Fail, Notes = "Handwashing sink issues." },
            new Inspection { Id = 18, PremisesId = 6, InspectionDate = new DateTime(2026, 3, 10), Score = 79, Outcome = InspectionOutcome.Pass, Notes = "Ventilation cleaned." },
            new Inspection { Id = 19, PremisesId = 7, InspectionDate = new DateTime(2026, 3, 12), Score = 82, Outcome = InspectionOutcome.Pass, Notes = "No critical findings." },
            new Inspection { Id = 20, PremisesId = 8, InspectionDate = new DateTime(2026, 3, 18), Score = 61, Outcome = InspectionOutcome.Fail, Notes = "Expired ingredients found." },
            new Inspection { Id = 21, PremisesId = 9, InspectionDate = new DateTime(2026, 3, 20), Score = 88, Outcome = InspectionOutcome.Pass, Notes = "Good improvement." },
            new Inspection { Id = 22, PremisesId = 10, InspectionDate = new DateTime(2026, 3, 25), Score = 91, Outcome = InspectionOutcome.Pass, Notes = "Strong controls." },
            new Inspection { Id = 23, PremisesId = 11, InspectionDate = new DateTime(2026, 3, 28), Score = 76, Outcome = InspectionOutcome.Pass, Notes = "Minor repairs needed." },
            new Inspection { Id = 24, PremisesId = 12, InspectionDate = new DateTime(2026, 1, 3), Score = 84, Outcome = InspectionOutcome.Pass, Notes = "Routine visit." },
            new Inspection { Id = 25, PremisesId = 6, InspectionDate = new DateTime(2026, 2, 26), Score = 66, Outcome = InspectionOutcome.Fail, Notes = "Cooling unit maintenance." }
        );

        builder.Entity<FollowUp>().HasData(
            new FollowUp { Id = 1, InspectionId = 4, DueDate = new DateTime(2026, 2, 5), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 2, InspectionId = 6, DueDate = new DateTime(2026, 2, 10), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 2, 8) },
            new FollowUp { Id = 3, InspectionId = 9, DueDate = new DateTime(2026, 3, 1), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 4, InspectionId = 11, DueDate = new DateTime(2026, 3, 5), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 3, 4) },
            new FollowUp { Id = 5, InspectionId = 15, DueDate = new DateTime(2026, 3, 15), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 6, InspectionId = 17, DueDate = new DateTime(2026, 3, 20), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 7, InspectionId = 20, DueDate = new DateTime(2026, 4, 5), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 8, InspectionId = 25, DueDate = new DateTime(2026, 3, 10), Status = FollowUpStatus.Closed, ClosedDate = new DateTime(2026, 3, 9) },
            new FollowUp { Id = 9, InspectionId = 9, DueDate = new DateTime(2026, 2, 20), Status = FollowUpStatus.Open, ClosedDate = null },
            new FollowUp { Id = 10, InspectionId = 11, DueDate = new DateTime(2026, 2, 25), Status = FollowUpStatus.Open, ClosedDate = null }
        );
    }
}
