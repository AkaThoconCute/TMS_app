using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_end_for_TMS.Migrations
{
    /// <inheritdoc />
    public partial class Drivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                    table.ForeignKey(
                        name: "FK_Drivers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "DriverId", "CreatedAt", "DateOfBirth", "FullName", "HireDate", "LicenseClass", "LicenseExpiry", "LicenseNumber", "Notes", "PhoneNumber", "Status", "TenantId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01cd3d4f-c42a-6a52-7cde-d4aec7b8cc6b"), new DateTimeOffset(new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1949, 8, 7, 21, 2, 17, 632, DateTimeKind.Unspecified).AddTicks(7228), "Laverna Dickinson", new DateTime(2023, 5, 18, 13, 42, 3, 955, DateTimeKind.Unspecified).AddTicks(7824), "FC", new DateTime(2030, 2, 11, 18, 51, 28, 905, DateTimeKind.Unspecified).AddTicks(636), "B2-355779", null, "0910040980", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0b32d802-f3d1-a613-14cb-00c92641dd17"), new DateTimeOffset(new DateTime(2025, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1957, 3, 25, 11, 10, 14, 383, DateTimeKind.Unspecified).AddTicks(3020), "Greg Gerhold", new DateTime(2023, 8, 4, 11, 25, 18, 991, DateTimeKind.Unspecified).AddTicks(1497), "D", new DateTime(2027, 2, 18, 7, 43, 32, 489, DateTimeKind.Unspecified).AddTicks(8325), "B5-368735", null, "0941291143", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0c2228c0-194f-6849-1d6f-d490dc73afc5"), new DateTimeOffset(new DateTime(2026, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1954, 3, 23, 23, 29, 44, 150, DateTimeKind.Unspecified).AddTicks(1020), "Vince Gaylord", new DateTime(2023, 10, 25, 14, 35, 37, 97, DateTimeKind.Unspecified).AddTicks(6395), "FC", new DateTime(2031, 1, 23, 7, 15, 6, 716, DateTimeKind.Unspecified).AddTicks(8670), "B4-642798", null, "0968283708", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("362fc9fe-0f95-7cef-8c23-64b709074012"), new DateTimeOffset(new DateTime(2025, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1962, 3, 4, 8, 9, 37, 622, DateTimeKind.Unspecified).AddTicks(4824), "Arjun Smith", new DateTime(2024, 8, 8, 20, 2, 15, 82, DateTimeKind.Unspecified).AddTicks(5169), "C", new DateTime(2030, 12, 17, 0, 30, 44, 97, DateTimeKind.Unspecified).AddTicks(5019), "B2-641105", "Quos voluptatem officiis qui.", "0956354895", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("986b5a8f-bd55-6b0b-21e8-c413c8e7face"), new DateTimeOffset(new DateTime(2025, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1979, 11, 7, 13, 51, 33, 422, DateTimeKind.Unspecified).AddTicks(5390), "Devonte Stroman", new DateTime(2023, 11, 29, 11, 25, 58, 998, DateTimeKind.Unspecified).AddTicks(6978), "FC", new DateTime(2029, 4, 9, 0, 0, 53, 929, DateTimeKind.Unspecified).AddTicks(5003), "B3-655472", "Id atque occaecati autem aut aut mollitia sunt aut.", "0921870154", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("9a2dcb89-b8fb-4503-1b21-9b2941c7a483"), new DateTimeOffset(new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1979, 6, 23, 6, 24, 26, 465, DateTimeKind.Unspecified).AddTicks(3617), "George Hettinger", new DateTime(2024, 12, 24, 22, 0, 28, 385, DateTimeKind.Unspecified).AddTicks(5062), "C", new DateTime(2029, 7, 26, 14, 15, 3, 322, DateTimeKind.Unspecified).AddTicks(1141), "B5-390261", null, "0970115468", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("be330d54-4d92-9084-9d14-3db6617da349"), new DateTimeOffset(new DateTime(2025, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1950, 6, 10, 9, 57, 56, 183, DateTimeKind.Unspecified).AddTicks(528), "Larry Jakubowski", new DateTime(2021, 8, 29, 1, 35, 29, 930, DateTimeKind.Unspecified).AddTicks(176), "C", new DateTime(2029, 12, 6, 10, 57, 35, 312, DateTimeKind.Unspecified).AddTicks(2227), "B5-416834", "Possimus laboriosam nihil porro sed aut cum doloremque.", "0937826730", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("fda00fb1-a354-bddd-d628-93affb891e11"), new DateTimeOffset(new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1996, 11, 19, 10, 4, 49, 841, DateTimeKind.Unspecified).AddTicks(4810), "Hunter Turcotte", new DateTime(2025, 1, 22, 1, 15, 14, 820, DateTimeKind.Unspecified).AddTicks(1323), "C", new DateTime(2027, 9, 9, 2, 5, 13, 156, DateTimeKind.Unspecified).AddTicks(7975), "B5-211756", null, "0984144423", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_TenantId",
                table: "Drivers",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drivers");
        }
    }
}
