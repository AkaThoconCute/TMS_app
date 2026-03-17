using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_end_for_TMS.Migrations
{
    /// <inheritdoc />
    public partial class Truck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VinNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EngineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelYear = table.Column<int>(type: "int", nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TruckType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxPayloadKg = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    LengthMm = table.Column<int>(type: "int", nullable: true),
                    WidthMm = table.Column<int>(type: "int", nullable: true),
                    HeightMm = table.Column<int>(type: "int", nullable: true),
                    OwnershipType = table.Column<int>(type: "int", nullable: false),
                    CurrentStatus = table.Column<int>(type: "int", nullable: false),
                    OdometerReading = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.TruckId);
                });

            migrationBuilder.InsertData(
                table: "Trucks",
                columns: new[] { "TruckId", "Brand", "CreatedAt", "CurrentStatus", "EngineNumber", "HeightMm", "LastMaintenanceDate", "LengthMm", "LicensePlate", "MaxPayloadKg", "ModelYear", "OdometerReading", "OwnershipType", "PurchaseDate", "TruckType", "UpdatedAt", "VinNumber", "WidthMm" },
                values: new object[,]
                {
                    { new Guid("08a20656-efdc-ce61-cf1f-e0cfb319623e"), "Scania", new DateTimeOffset(new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, "CRBRZ130E4IC", 3026, new DateTime(2025, 10, 5, 7, 45, 43, 232, DateTimeKind.Unspecified).AddTicks(8369), 7197, "9376-E361", 25695.97m, 2020, 125867.50m, 2, new DateTime(2024, 7, 14, 16, 40, 36, 809, DateTimeKind.Unspecified).AddTicks(6497), "Mui bạt", null, "XNVAW6LCBKO098514", 2553 },
                    { new Guid("1c7617df-b9c2-fa5a-3a7d-3f4b4bb7ee18"), "Volvo", new DateTimeOffset(new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5, "E5J1248ROIH9", 3328, new DateTime(2024, 10, 26, 19, 22, 50, 806, DateTimeKind.Unspecified).AddTicks(7459), 9892, "2965-KD11", 23643.01m, 2018, 195850.70m, 1, new DateTime(2022, 2, 2, 12, 24, 39, 865, DateTimeKind.Unspecified).AddTicks(8222), "Mui bạt", null, "WEZBRTAJULHJ86716", 2424 },
                    { new Guid("2df255c4-273c-da52-1071-9ed79e5f13e0"), "Daf", new DateTimeOffset(new DateTime(2025, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "65JQZKK27JHW", 2595, new DateTime(2025, 6, 25, 7, 26, 30, 743, DateTimeKind.Unspecified).AddTicks(5736), 10235, "1795-PY91", 12589.03m, 2018, 345576.58m, 1, new DateTime(2019, 9, 7, 5, 38, 14, 623, DateTimeKind.Unspecified).AddTicks(5856), "Cẩu", new DateTimeOffset(new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "1OSM8M5T9YNG92381", 2474 },
                    { new Guid("3a5c6033-a167-8abb-9156-2d55bbd81297"), "Isuzu", new DateTimeOffset(new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, "XSLI9KHOAOGI", 3354, new DateTime(2026, 1, 11, 9, 1, 2, 593, DateTimeKind.Unspecified).AddTicks(7211), 7837, "7448-YP20", 19024.92m, 2015, 419693.81m, 1, new DateTime(2017, 3, 22, 6, 6, 49, 619, DateTimeKind.Unspecified).AddTicks(2731), "Cẩu", new DateTimeOffset(new DateTime(2026, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "LR5H42KG7NI092672", 2542 },
                    { new Guid("40b25403-8b4a-d9de-148e-cf8707e18517"), "Mercedes-Benz", new DateTimeOffset(new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5, "SKH9XD1C5SZR", 2613, new DateTime(2024, 9, 12, 5, 5, 53, 19, DateTimeKind.Unspecified).AddTicks(5515), 6198, "7061-IZ47", 27952.52m, 2020, 308775.86m, 1, new DateTime(2024, 3, 1, 19, 29, 37, 763, DateTimeKind.Unspecified).AddTicks(4202), "Bồn", null, "WTGBCOF8M6T213769", 2512 },
                    { new Guid("4eb6cb58-dc2b-89b4-1223-5ae808fbfdbb"), "Isuzu", new DateTimeOffset(new DateTime(2025, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "G72MT8Q1ND57", 2661, new DateTime(2025, 8, 7, 23, 51, 10, 902, DateTimeKind.Unspecified).AddTicks(2862), 9169, "3108-VI14", 23866.81m, 2020, 229930.14m, 1, new DateTime(2019, 2, 27, 0, 45, 11, 257, DateTimeKind.Unspecified).AddTicks(4336), "Cẩu", null, "CHV52V6QTKUV78068", 2499 },
                    { new Guid("617b43e9-052f-826b-454f-dd540671ba7f"), "Volvo", new DateTimeOffset(new DateTime(2025, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, "9IZRW4X7MEVO", 3334, new DateTime(2024, 10, 21, 11, 44, 2, 924, DateTimeKind.Unspecified).AddTicks(8649), 5252, "5666-ID94", 4558.34m, 2018, 459416.72m, 1, new DateTime(2022, 4, 15, 12, 3, 47, 86, DateTimeKind.Unspecified).AddTicks(7788), "Bồn", new DateTimeOffset(new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AMF2H9SHDSN471834", 2544 },
                    { new Guid("af57e96b-cbd7-e3f1-de60-408edf3ff3fd"), "Isuzu", new DateTimeOffset(new DateTime(2025, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5, "1T5IPYOV9UHM", 3045, new DateTime(2025, 12, 21, 15, 9, 19, 279, DateTimeKind.Unspecified).AddTicks(6312), 8840, "7310-3Q96", 8310.22m, 2016, 39006.62m, 1, new DateTime(2017, 10, 11, 7, 38, 22, 846, DateTimeKind.Unspecified).AddTicks(3298), "Mui bạt", null, "XCH11ZAPJ7TS67845", 2468 },
                    { new Guid("c4889f9e-759c-efc3-070c-02ceb8cd064a"), "Howo", new DateTimeOffset(new DateTime(2025, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5, "0RZK45ET2Q1F", 2531, new DateTime(2024, 11, 8, 5, 15, 31, 994, DateTimeKind.Unspecified).AddTicks(9268), 7271, "2917-WJ65", 14910.21m, 2015, 69673.32m, 1, new DateTime(2025, 10, 19, 16, 9, 53, 115, DateTimeKind.Unspecified).AddTicks(2034), "Cẩu", new DateTimeOffset(new DateTime(2026, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "9GZ4DNTSWGWL93896", 2557 },
                    { new Guid("d20513a0-3f37-4c43-ebb3-bbca0ca8bdb0"), "Mercedes-Benz", new DateTimeOffset(new DateTime(2025, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5, "ATQN5TNE4LRT", 2716, new DateTime(2025, 10, 21, 21, 29, 16, 263, DateTimeKind.Unspecified).AddTicks(909), 8943, "2093-BP36", 17234.84m, 2017, 129874.43m, 1, new DateTime(2019, 2, 15, 12, 55, 23, 370, DateTimeKind.Unspecified).AddTicks(1670), "Cẩu", new DateTimeOffset(new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AI6X76CHTGFT55593", 2405 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_LicensePlate",
                table: "Trucks",
                column: "LicensePlate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trucks");
        }
    }
}
