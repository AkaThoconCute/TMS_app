using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end_for_TMS.Migrations
{
    /// <inheritdoc />
    public partial class Tenants_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("08a20656-efdc-ce61-cf1f-e0cfb319623e"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("1c7617df-b9c2-fa5a-3a7d-3f4b4bb7ee18"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("2df255c4-273c-da52-1071-9ed79e5f13e0"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("3a5c6033-a167-8abb-9156-2d55bbd81297"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("40b25403-8b4a-d9de-148e-cf8707e18517"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("4eb6cb58-dc2b-89b4-1223-5ae808fbfdbb"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("617b43e9-052f-826b-454f-dd540671ba7f"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("af57e96b-cbd7-e3f1-de60-408edf3ff3fd"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("c4889f9e-759c-efc3-070c-02ceb8cd064a"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));

            migrationBuilder.UpdateData(
                table: "Trucks",
                keyColumn: "TruckId",
                keyValue: new Guid("d20513a0-3f37-4c43-ebb3-bbca0ca8bdb0"),
                column: "TenantId",
                value: new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
