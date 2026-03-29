using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_end_for_TMS.Migrations
{
    /// <inheritdoc />
    public partial class Customers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "ContactPerson", "CreatedAt", "CustomerType", "Email", "Name", "Notes", "PhoneNumber", "Status", "TaxCode", "TenantId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("04d3bfa6-b46a-f416-2903-20646d4e1127"), "587 Frami Vista, South Mittieburgh, Ethiopia", "Vilma Williamson", new DateTimeOffset(new DateTime(2025, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "Schuyler53@hotmail.com", "Willms - Paucek", null, "0976651161", 2, null, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("261eb9cd-8089-bf45-4488-002f939f4ebd"), "790 Tillman Plaza, Lisettemouth, Ethiopia", "Kelly Fadel", new DateTimeOffset(new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, null, "Pagac Inc", null, "0990846327", 1, null, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("3edc66db-dce0-de4e-4f5c-93db157673d3"), "58959 Haag Islands, Ilamouth, Sao Tome and Principe", "Emerald Gerlach", new DateTimeOffset(new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "Davin23@gmail.com", "Veum, Green and Huels", null, "0917606082", 1, null, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("541a49b7-ae45-6dbe-7ac8-2614a840c837"), "19866 Cassin Passage, North Liana, Sweden", "Eugene Smith", new DateTimeOffset(new DateTime(2025, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, null, "Runte - Bode", null, "0970392197", 2, null, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("5977a2a4-c086-1d7b-657d-4e79db53e71a"), "6037 Beier Fork, Harryshire, United Kingdom", "Jamie Torphy", new DateTimeOffset(new DateTime(2025, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, null, "Stehr, Runolfsson and Weber", "Aliquid ullam earum qui eum molestiae dolorem animi.", "0949139052", 1, "1540476821", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("60e49800-498a-cf5e-836b-67a18060651f"), "015 Harvey Highway, South Kali, Bosnia and Herzegovina", "Travis Herzog", new DateTimeOffset(new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, null, "Bergstrom LLC", "Ipsa quod consequatur nesciunt odio sint quod ullam quidem.", "0986538432", 1, null, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("78c12a97-b692-13e6-3bf7-e28788421b75"), "0132 Kuhlman Branch, Conroyburgh, Lesotho", "Domenica Hansen", new DateTimeOffset(new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, null, "Bauch - Gorczany", null, "0943557763", 2, "1939408095", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("e7da23d9-6724-8bc7-70ad-eacbc6b149d2"), "975 Barrows River, Katarinaton, Finland", "Genesis Toy", new DateTimeOffset(new DateTime(2025, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "Edgar35@hotmail.com", "Kshlerin Group", null, "0965461423", 1, "1490432197", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId",
                table: "Customers",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
