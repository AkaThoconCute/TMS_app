using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_end_for_TMS.Migrations
{
    /// <inheritdoc />
    public partial class Tenants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trucks_LicensePlate",
                table: "Trucks");

            migrationBuilder.AlterColumn<string>(
                name: "LicensePlate",
                table: "Trucks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Trucks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "u-admin",
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "u-user1",
                column: "TenantId",
                value: null);

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "CreatedAt", "Name", "OwnerId" },
                values: new object[,]
                {
                    { new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Alpha Logistics", "u-admin" },
                    { new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8f"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Bravo Trans", "u-user1" },
                    { new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d90"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Charlie Freight", "u-admin" },
                    { new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d91"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Delta Moving", "u-user1" },
                    { new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d92"), new DateTimeOffset(new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Echo Shipping", "u-admin" }
                });

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("08a20656-efdc-ce61-cf1f-e0cfb319623e"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("1c7617df-b9c2-fa5a-3a7d-3f4b4bb7ee18"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("2df255c4-273c-da52-1071-9ed79e5f13e0"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("3a5c6033-a167-8abb-9156-2d55bbd81297"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("40b25403-8b4a-d9de-148e-cf8707e18517"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("4eb6cb58-dc2b-89b4-1223-5ae808fbfdbb"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("617b43e9-052f-826b-454f-dd540671ba7f"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("af57e96b-cbd7-e3f1-de60-408edf3ff3fd"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("c4889f9e-759c-efc3-070c-02ceb8cd064a"),
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("d20513a0-3f37-4c43-ebb3-bbca0ca8bdb0"),
                column: "TenantId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_TenantId",
                table: "Trucks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Tenants_TenantId",
                table: "Trucks",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Tenants_TenantId",
                table: "Trucks");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_TenantId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LicensePlate",
                table: "Trucks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_LicensePlate",
                table: "Trucks",
                column: "LicensePlate",
                unique: true);
        }
    }
}
