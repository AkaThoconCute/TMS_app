using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end_for_TMS.Migrations
{
    /// <inheritdoc />
    public partial class Tenant_FK_Nonnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Trucks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "u-admin",
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "u-user1",
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Trucks",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

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
        }
    }
}
