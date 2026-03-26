using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace oop_s2_2_mvc_71739.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialFoodSafety : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Premises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Town = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    RiskRating = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Premises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PremisesId = table.Column<int>(type: "INTEGER", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    Outcome = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_Premises_PremisesId",
                        column: x => x.PremisesId,
                        principalTable: "Premises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowUps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InspectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ClosedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUps_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Premises",
                columns: new[] { "Id", "Address", "Name", "RiskRating", "Town" },
                values: new object[,]
                {
                    { 1, "12 Maple St", "Sunrise Deli", "Low", "Riverton" },
                    { 2, "5 Dock Rd", "Harbor Grill", "Medium", "Bayview" },
                    { 3, "44 Oak Ave", "Oakfield Bakery", "Low", "Oakfield" },
                    { 4, "18 River Ln", "Riverton Sushi", "High", "Riverton" },
                    { 5, "201 Coast Blvd", "Bayview Tacos", "Medium", "Bayview" },
                    { 6, "77 Pine Rd", "Oakfield Bistro", "High", "Oakfield" },
                    { 7, "3 Market Sq", "Riverton Cafe", "Medium", "Riverton" },
                    { 8, "9 Harbor St", "Bayview Pizza", "Low", "Bayview" },
                    { 9, "122 Main St", "Oakfield Diner", "Medium", "Oakfield" },
                    { 10, "65 Canal Way", "Riverbank Noodles", "High", "Riverton" },
                    { 11, "400 Ocean Dr", "Seaside Snacks", "High", "Bayview" },
                    { 12, "14 Pine St", "Pine Street Eatery", "Low", "Oakfield" }
                });

            migrationBuilder.InsertData(
                table: "Inspections",
                columns: new[] { "Id", "InspectionDate", "Notes", "Outcome", "PremisesId", "Score" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good hygiene practices.", "Pass", 1, 92 },
                    { 2, new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minor documentation gaps.", "Pass", 2, 78 },
                    { 3, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Clean storage areas.", "Pass", 3, 88 },
                    { 4, new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Temperature logs missing.", "Fail", 4, 62 },
                    { 5, new DateTime(2026, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sanitizer levels acceptable.", "Pass", 5, 75 },
                    { 6, new DateTime(2026, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cross-contamination risk.", "Fail", 6, 54 },
                    { 7, new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Improved from last visit.", "Pass", 7, 81 },
                    { 8, new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Excellent record keeping.", "Pass", 8, 95 },
                    { 9, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pest control issues.", "Fail", 9, 68 },
                    { 10, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Staff training updated.", "Pass", 10, 83 },
                    { 11, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Food storage violations.", "Fail", 11, 59 },
                    { 12, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Well maintained kitchen.", "Pass", 12, 90 },
                    { 13, new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "No major issues.", "Pass", 1, 86 },
                    { 14, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Needs label updates.", "Pass", 2, 73 },
                    { 15, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cleaning schedule inconsistent.", "Fail", 3, 64 },
                    { 16, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Improved compliance.", "Pass", 4, 70 },
                    { 17, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Handwashing sink issues.", "Fail", 5, 58 },
                    { 18, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ventilation cleaned.", "Pass", 6, 79 },
                    { 19, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "No critical findings.", "Pass", 7, 82 },
                    { 20, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Expired ingredients found.", "Fail", 8, 61 },
                    { 21, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good improvement.", "Pass", 9, 88 },
                    { 22, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Strong controls.", "Pass", 10, 91 },
                    { 23, new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minor repairs needed.", "Pass", 11, 76 },
                    { 24, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Routine visit.", "Pass", 12, 84 },
                    { 25, new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cooling unit maintenance.", "Fail", 6, 66 }
                });

            migrationBuilder.InsertData(
                table: "FollowUps",
                columns: new[] { "Id", "ClosedDate", "DueDate", "InspectionId", "Status" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Open" },
                    { 2, new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Closed" },
                    { 3, null, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Open" },
                    { 4, new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "Closed" },
                    { 5, null, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "Open" },
                    { 6, null, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, "Open" },
                    { 7, null, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, "Open" },
                    { 8, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, "Closed" },
                    { 9, null, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Open" },
                    { 10, null, new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "Open" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_InspectionId",
                table: "FollowUps",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PremisesId",
                table: "Inspections",
                column: "PremisesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUps");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "Premises");
        }
    }
}
