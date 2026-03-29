using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_end_for_TMS.Migrations
{
    /// <inheritdoc />
    public partial class Base : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AppName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.SetNull);
                });

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

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Trucks_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickupAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CargoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CargoWeightKg = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    RequestedPickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuotedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CancelledAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlannedPickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlannedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualPickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FuelCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TollCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CostNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CancelledAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Trips_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "TruckId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "r-admin", "STATIC_STAMP_123456", "Admin", "ADMIN" },
                    { "r-user", "STATIC_STAMP_123456", "User", "USER" }
                });

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

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AppName", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiryTime", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "u-admin", 0, null, "STATIC_STAMP_123456", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@asp.app", true, false, null, "ADMIN@ASP.APP", "TOP1SERVER", "AQAAAAIAAYagAAAAEOf6Pb8v/8VwLIDv8T6/7UfVvJqR9Z0X5Y6vX5Y6vX", null, false, null, null, "STATIC_STAMP_123456", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), false, "Top1Server" },
                    { "u-user1", 0, null, "STATIC_STAMP_123456", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user1@asp.app", true, false, null, "USER1@ASP.APP", "USER1", "AQAAAAIAAYagAAAAEOf6Pb8v/8VwLIDv8T6/7UfVvJqR9Z0X5Y6vX5Y6vX", null, false, null, null, "STATIC_STAMP_123456", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), false, "User1" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "ContactPerson", "CreatedAt", "CustomerType", "Email", "Name", "Notes", "PhoneNumber", "Status", "TaxCode", "TenantId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0195d004-cccc-7000-8000-000000000001"), "123 Nguyễn Huệ, Quận 1, Tp. Hồ Chí Minh", "Nguyễn Văn Toàn", new DateTimeOffset(new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "info@thuanphat.vn", "Công ty TNHH Thuận Phát", null, "0281234567", 1, "0312345678", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d004-cccc-7000-8000-000000000002"), "456 Lê Lợi, Quận 3, Tp. Hồ Chí Minh", "Trần Thị Hoa", new DateTimeOffset(new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "contact@namviet.com.vn", "Công ty CP Nam Việt", null, "0282345678", 1, "0323456789", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d004-cccc-7000-8000-000000000003"), "789 Trần Hưng Đạo, Quận 5, Tp. Hồ Chí Minh", null, new DateTimeOffset(new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, null, "Nguyễn Minh Khoa", null, "0903456789", 1, null, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d004-cccc-7000-8000-000000000004"), "12 Đinh Tiên Hoàng, Bình Thạnh, Tp. Hồ Chí Minh", "Lê Văn Hưng", new DateTimeOffset(new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "hungthinhlogistics@gmail.com", "Doanh nghiệp Tư nhân Hưng Thịnh", null, "0284567890", 1, "0334567890", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d004-cccc-7000-8000-000000000005"), "34 Võ Văn Tần, Quận 3, Tp. Hồ Chí Minh", null, new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "lethu@gmail.com", "Lê Thị Thu", null, "0905678901", 1, null, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d004-cccc-7000-8000-000000000006"), "78 Lý Tự Trọng, Quận 1, Tp. Hồ Chí Minh", "Phạm Quốc Hải", new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "hailong.transport@vnn.vn", "Công ty TNHH Hải Long", "Khách hàng tạm dừng hợp đồng", "0286789012", 2, "0356789012", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0195d004-cccc-7000-8000-000000000007"), "90 Lê Văn Việt, Quận 9, Tp. Hồ Chí Minh", null, new DateTimeOffset(new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, null, "Phạm Văn Dũng", null, "0907890123", 1, null, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d004-cccc-7000-8000-000000000008"), "44 Hai Bà Trưng, Quận 1, Tp. Hồ Chí Minh", "Hoàng Thị Lan", new DateTimeOffset(new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "vietmy.co@gmail.com", "Công ty CP Việt Mỹ", null, "0288901234", 1, "0378901234", new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null }
                });

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "DriverId", "CreatedAt", "DateOfBirth", "FullName", "HireDate", "LicenseClass", "LicenseExpiry", "LicenseNumber", "Notes", "PhoneNumber", "Status", "TenantId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0195d003-dddd-7000-8000-000000000001"), new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1985, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Văn Minh", new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "B2", new DateTime(2028, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "B2-123456", null, "0901234567", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d003-dddd-7000-8000-000000000002"), new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1980, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trần Thanh Tùng", new DateTime(2019, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "FC", new DateTime(2027, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "FC-234567", null, "0912345678", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d003-dddd-7000-8000-000000000003"), new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1990, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lê Văn Hùng", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", new DateTime(2029, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "C-345678", null, "0923456789", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d003-dddd-7000-8000-000000000004"), new DateTimeOffset(new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1978, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phạm Quốc Bảo", new DateTime(2018, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "D", new DateTime(2027, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "D-456789", null, "0934567890", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d003-dddd-7000-8000-000000000005"), new DateTimeOffset(new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1992, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàng Văn Nam", new DateTime(2022, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "C-567890", "Bằng lái sắp hết hạn — cần gia hạn trước tháng 5/2026", "0945678901", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d003-dddd-7000-8000-000000000006"), new DateTimeOffset(new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1995, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nguyễn Đức Trí", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "B2", new DateTime(2030, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "B2-678901", null, "0956789012", 2, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d003-dddd-7000-8000-000000000007"), new DateTimeOffset(new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1983, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Võ Minh Phúc", new DateTime(2017, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "FC", new DateTime(2028, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "FC-789012", null, "0967890123", 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d003-dddd-7000-8000-000000000008"), new DateTimeOffset(new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTime(1988, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đặng Văn Long", new DateTime(2016, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "C", new DateTime(2027, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "C-890123", null, "0978901234", 3, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null }
                });

            migrationBuilder.InsertData(
                table: "Trucks",
                columns: new[] { "TruckId", "Brand", "CreatedAt", "CurrentStatus", "EngineNumber", "HeightMm", "LastMaintenanceDate", "LengthMm", "LicensePlate", "MaxPayloadKg", "ModelYear", "OdometerReading", "OwnershipType", "PurchaseDate", "TenantId", "TruckType", "UpdatedAt", "VinNumber", "WidthMm" },
                values: new object[,]
                {
                    { new Guid("0195d005-eeee-7000-8000-000000000001"), "Hino", new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "J08ETUA01234", 2800, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9600, "51C-123.45", 8000m, 2020, 85000m, 1, new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Thùng kín", null, "JHLRE4H77BC001234", 2490 },
                    { new Guid("0195d005-eeee-7000-8000-000000000002"), "Isuzu", new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "6HK1AUUA02345", null, new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 8200, "51C-234.56", 7500m, 2019, 120000m, 1, new DateTime(2019, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Mui bạt", null, "JAATV6512KA002345", 2490 },
                    { new Guid("0195d005-eeee-7000-8000-000000000003"), "Howo", new DateTimeOffset(new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 2, "WD615AUUA03456", 3200, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12000, "51D-345.67", 15000m, 2021, 65000m, 1, new DateTime(2021, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Thùng kín", null, "LZGAAHBN0M1003456", 2550 },
                    { new Guid("0195d005-eeee-7000-8000-000000000004"), "Volvo", new DateTimeOffset(new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "D13KAUUA04567", 3800, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 11500, "51D-456.78", 20000m, 2022, 40000m, 2, new DateTime(2022, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Bồn", null, "YV2R4A2A4NA004567", 2550 },
                    { new Guid("0195d005-eeee-7000-8000-000000000005"), "Scania", new DateTimeOffset(new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, "DC13AUUA05678", null, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12000, "51C-567.89", 18000m, 2018, 210000m, 1, new DateTime(2018, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Mui bạt", null, "XLERX441XKA005678", 2550 },
                    { new Guid("0195d005-eeee-7000-8000-000000000006"), "Mercedes-Benz", new DateTimeOffset(new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "OM471AUUA06789", 3500, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 12000, "29H-678.90", 25000m, 2023, 18000m, 2, new DateTime(2023, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Thùng kín", null, "WDB96300312006789", 2550 },
                    { new Guid("0195d005-eeee-7000-8000-000000000007"), "Man", new DateTimeOffset(new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1, "D2676AUUA07890", 3600, new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 11000, "29H-789.01", 22000m, 2020, 92000m, 1, new DateTime(2020, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Cẩu", null, "WMA06XZZ0KM007890", 2550 },
                    { new Guid("0195d005-eeee-7000-8000-000000000008"), "Daf", new DateTimeOffset(new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, "MX13AUUA08901", 3200, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 10500, "29C-890.12", 30000m, 2021, 155000m, 2, new DateTime(2021, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Tự đổ", null, "XLRTE45MS0E008901", 2550 },
                    { new Guid("0195d005-eeee-7000-8000-000000000009"), "Hino", new DateTimeOffset(new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5, "J08ETUA09012", null, new DateTime(2025, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 9200, "43A-901.23", 9000m, 2019, 280000m, 1, new DateTime(2019, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), "Mui bạt", null, "JHLRE4H52KA009012", 2490 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "r-admin", "u-admin" },
                    { "r-user", "u-user1" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CancellationReason", "CancelledAt", "CargoDescription", "CargoWeightKg", "CompletedAt", "CreatedAt", "CustomerId", "DeliveryAddress", "Notes", "OrderNumber", "PickupAddress", "QuotedPrice", "RequestedDeliveryDate", "RequestedPickupDate", "Status", "TenantId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0195d001-aaaa-7000-8000-000000000001"), null, null, "Nội thất gia đình — ghế sofa, bàn, ghế", 350m, null, new DateTimeOffset(new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d004-cccc-7000-8000-000000000001"), "456 Lê Lợi, Quận 3, Tp. Hồ Chí Minh", "Customer requests morning pickup", "ORD-000001", "123 Nguyễn Huệ, Quận 1, Tp. Hồ Chí Minh", 2500000m, new DateTime(2026, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d001-aaaa-7000-8000-000000000002"), null, null, "Thiết bị văn phòng — bàn làm việc, màn hình, máy in", 500m, null, new DateTimeOffset(new DateTime(2026, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d004-cccc-7000-8000-000000000002"), "12 Phạm Văn Đồng, Thủ Đức, Tp. Hồ Chí Minh", null, "ORD-000002", "789 Trần Hưng Đạo, Quận 5, Tp. Hồ Chí Minh", 4000000m, new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null },
                    { new Guid("0195d001-aaaa-7000-8000-000000000003"), null, null, "Tủ lạnh và máy giặt", 200m, null, new DateTimeOffset(new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d004-cccc-7000-8000-000000000003"), "56 Nguyễn Văn Linh, Quận 7, Tp. Hồ Chí Minh", null, "ORD-000003", "34 Võ Văn Tần, Quận 3, Tp. Hồ Chí Minh", 1800000m, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0195d001-aaaa-7000-8000-000000000004"), null, null, "Hành lý cá nhân — hộp carton, quần áo, dụng cụ nhà bếp", 150m, null, new DateTimeOffset(new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d004-cccc-7000-8000-000000000004"), "90 Lê Văn Việt, Quận 9, Tp. Hồ Chí Minh", null, "ORD-000004", "78 Lý Tự Trọng, Quận 1, Tp. Hồ Chí Minh", 1200000m, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0195d001-aaaa-7000-8000-000000000005"), null, null, "Vật liệu xây dựng — xi măng, thanh thép", 2000m, null, new DateTimeOffset(new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d004-cccc-7000-8000-000000000001"), "200 Quang Trung, Gò Vấp, Tp. Hồ Chí Minh", null, "ORD-000005", "100 Cách Mạng Tháng 8, Quận 10, Tp. Hồ Chí Minh", 5500000m, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0195d001-aaaa-7000-8000-000000000006"), null, null, "Điện tử gia dụng — TV, điều hòa không khí", 300m, new DateTimeOffset(new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d004-cccc-7000-8000-000000000002"), "88 Nguyễn Thị Minh Khai, Quận 3, Tp. Hồ Chí Minh", "Customer paid in full", "ORD-000006", "44 Hai Bà Trưng, Quận 1, Tp. Hồ Chí Minh", 3200000m, new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), new DateTimeOffset(new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "TripId", "ActualDeliveryDate", "ActualPickupDate", "CancellationReason", "CancelledAt", "CompletedAt", "CostNotes", "CreatedAt", "DriverId", "FuelCost", "Notes", "OrderId", "OtherCost", "PlannedDeliveryDate", "PlannedPickupDate", "Status", "TenantId", "TollCost", "TripNumber", "TruckId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0195d002-bbbb-7000-8000-000000000001"), null, null, null, null, null, null, new DateTimeOffset(new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d003-dddd-7000-8000-000000000001"), null, "Scheduled for morning pickup", new Guid("0195d001-aaaa-7000-8000-000000000003"), null, new DateTime(2026, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null, "TRP-000001", new Guid("0195d005-eeee-7000-8000-000000000001"), null },
                    { new Guid("0195d002-bbbb-7000-8000-000000000002"), null, null, null, null, null, null, new DateTimeOffset(new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d003-dddd-7000-8000-000000000002"), null, null, new Guid("0195d001-aaaa-7000-8000-000000000004"), null, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null, "TRP-000002", new Guid("0195d005-eeee-7000-8000-000000000002"), null },
                    { new Guid("0195d002-bbbb-7000-8000-000000000003"), new DateTime(2026, 3, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 15, 8, 30, 0, 0, DateTimeKind.Unspecified), null, null, new DateTimeOffset(new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Highway toll + parking fee", new DateTimeOffset(new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d003-dddd-7000-8000-000000000003"), 850000m, null, new Guid("0195d001-aaaa-7000-8000-000000000005"), 50000m, new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), 120000m, "TRP-000003", new Guid("0195d005-eeee-7000-8000-000000000003"), new DateTimeOffset(new DateTime(2026, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0195d002-bbbb-7000-8000-000000000004"), new DateTime(2026, 3, 11, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), null, null, new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, new DateTimeOffset(new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d003-dddd-7000-8000-000000000004"), 600000m, null, new Guid("0195d001-aaaa-7000-8000-000000000006"), null, new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), 80000m, "TRP-000004", new Guid("0195d005-eeee-7000-8000-000000000004"), new DateTimeOffset(new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("0195d002-bbbb-7000-8000-000000000005"), null, null, "Truck broke down before pickup", new DateTimeOffset(new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, new DateTimeOffset(new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("0195d003-dddd-7000-8000-000000000005"), null, null, new Guid("0195d001-aaaa-7000-8000-000000000005"), null, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new Guid("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e"), null, "TRP-000005", new Guid("0195d005-eeee-7000-8000-000000000005"), new DateTimeOffset(new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId",
                table: "Customers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_TenantId",
                table: "Drivers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId",
                table: "Orders",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId_OrderNumber",
                table: "Orders",
                columns: new[] { "TenantId", "OrderNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DriverId",
                table: "Trips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_OrderId",
                table: "Trips",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TenantId",
                table: "Trips",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TenantId_TripNumber",
                table: "Trips",
                columns: new[] { "TenantId", "TripNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TruckId",
                table: "Trips",
                column: "TruckId");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_TenantId",
                table: "Trucks",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
