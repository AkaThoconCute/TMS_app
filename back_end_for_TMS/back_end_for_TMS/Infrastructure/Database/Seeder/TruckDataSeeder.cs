using back_end_for_TMS.Models;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class TruckDataSeeder : IDataSeeder<Truck>
{
  // Fixed IDs for stable migrations and cross-seeder references (UUIDv7 format)
  public static readonly Guid Truck1Id = Guid.Parse("0195d005-eeee-7000-8000-000000000001");
  public static readonly Guid Truck2Id = Guid.Parse("0195d005-eeee-7000-8000-000000000002");
  public static readonly Guid Truck3Id = Guid.Parse("0195d005-eeee-7000-8000-000000000003");
  public static readonly Guid Truck4Id = Guid.Parse("0195d005-eeee-7000-8000-000000000004");
  public static readonly Guid Truck5Id = Guid.Parse("0195d005-eeee-7000-8000-000000000005");
  public static readonly Guid Truck6Id = Guid.Parse("0195d005-eeee-7000-8000-000000000006");
  public static readonly Guid Truck7Id = Guid.Parse("0195d005-eeee-7000-8000-000000000007");
  public static readonly Guid Truck8Id = Guid.Parse("0195d005-eeee-7000-8000-000000000008");
  public static readonly Guid Truck9Id = Guid.Parse("0195d005-eeee-7000-8000-000000000009");

  public static List<Truck> Generate()
  {
    var tenantId = TenantDataSeeder.AlphaTenantId;

    return
    [
      new Truck
      {
        TruckId = Truck1Id, TenantId = tenantId,
        LicensePlate = "51C-123.45", VinNumber = "JHLRE4H77BC001234", EngineNumber = "J08ETUA01234",
        Brand = "Hino", ModelYear = 2020, TruckType = "Thùng kín",
        MaxPayloadKg = 8000m, LengthMm = 9600, WidthMm = 2490, HeightMm = 2800,
        OwnershipType = 1, CurrentStatus = 1, OdometerReading = 85000m,
        PurchaseDate = new DateTime(2020, 1, 15), LastMaintenanceDate = new DateTime(2026, 1, 10),
        CreatedAt = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero),
      },
      new Truck
      {
        TruckId = Truck2Id, TenantId = tenantId,
        LicensePlate = "51C-234.56", VinNumber = "JAATV6512KA002345", EngineNumber = "6HK1AUUA02345",
        Brand = "Isuzu", ModelYear = 2019, TruckType = "Mui bạt",
        MaxPayloadKg = 7500m, LengthMm = 8200, WidthMm = 2490, HeightMm = null,
        OwnershipType = 1, CurrentStatus = 1, OdometerReading = 120000m,
        PurchaseDate = new DateTime(2019, 3, 20), LastMaintenanceDate = new DateTime(2025, 12, 5),
        CreatedAt = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero),
      },
      new Truck
      {
        TruckId = Truck3Id, TenantId = tenantId,
        LicensePlate = "51D-345.67", VinNumber = "LZGAAHBN0M1003456", EngineNumber = "WD615AUUA03456",
        Brand = "Howo", ModelYear = 2021, TruckType = "Thùng kín",
        MaxPayloadKg = 15000m, LengthMm = 12000, WidthMm = 2550, HeightMm = 3200,
        OwnershipType = 1, CurrentStatus = 2, OdometerReading = 65000m,
        PurchaseDate = new DateTime(2021, 6, 10), LastMaintenanceDate = new DateTime(2026, 2, 1),
        CreatedAt = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero),
      },
      new Truck
      {
        TruckId = Truck4Id, TenantId = tenantId,
        LicensePlate = "51D-456.78", VinNumber = "YV2R4A2A4NA004567", EngineNumber = "D13KAUUA04567",
        Brand = "Volvo", ModelYear = 2022, TruckType = "Bồn",
        MaxPayloadKg = 20000m, LengthMm = 11500, WidthMm = 2550, HeightMm = 3800,
        OwnershipType = 2, CurrentStatus = 1, OdometerReading = 40000m,
        PurchaseDate = new DateTime(2022, 9, 5), LastMaintenanceDate = new DateTime(2026, 2, 15),
        CreatedAt = new DateTimeOffset(2026, 1, 16, 0, 0, 0, TimeSpan.Zero),
      },
      new Truck
      {
        TruckId = Truck5Id, TenantId = tenantId,
        LicensePlate = "51C-567.89", VinNumber = "XLERX441XKA005678", EngineNumber = "DC13AUUA05678",
        Brand = "Scania", ModelYear = 2018, TruckType = "Mui bạt",
        MaxPayloadKg = 18000m, LengthMm = 12000, WidthMm = 2550, HeightMm = null,
        OwnershipType = 1, CurrentStatus = 3, OdometerReading = 210000m,
        PurchaseDate = new DateTime(2018, 4, 22), LastMaintenanceDate = new DateTime(2026, 3, 1),
        CreatedAt = new DateTimeOffset(2026, 1, 16, 0, 0, 0, TimeSpan.Zero),
      },
      new Truck
      {
        TruckId = Truck6Id, TenantId = tenantId,
        LicensePlate = "29H-678.90", VinNumber = "WDB96300312006789", EngineNumber = "OM471AUUA06789",
        Brand = "Mercedes-Benz", ModelYear = 2023, TruckType = "Thùng kín",
        MaxPayloadKg = 25000m, LengthMm = 12000, WidthMm = 2550, HeightMm = 3500,
        OwnershipType = 2, CurrentStatus = 1, OdometerReading = 18000m,
        PurchaseDate = new DateTime(2023, 2, 14), LastMaintenanceDate = new DateTime(2026, 1, 20),
        CreatedAt = new DateTimeOffset(2026, 1, 16, 0, 0, 0, TimeSpan.Zero),
      },
      new Truck
      {
        TruckId = Truck7Id, TenantId = tenantId,
        LicensePlate = "29H-789.01", VinNumber = "WMA06XZZ0KM007890", EngineNumber = "D2676AUUA07890",
        Brand = "Man", ModelYear = 2020, TruckType = "Cẩu",
        MaxPayloadKg = 22000m, LengthMm = 11000, WidthMm = 2550, HeightMm = 3600,
        OwnershipType = 1, CurrentStatus = 1, OdometerReading = 92000m,
        PurchaseDate = new DateTime(2020, 7, 30), LastMaintenanceDate = new DateTime(2025, 11, 18),
        CreatedAt = new DateTimeOffset(2026, 1, 17, 0, 0, 0, TimeSpan.Zero),
      },
      new Truck
      {
        TruckId = Truck8Id, TenantId = tenantId,
        LicensePlate = "29C-890.12", VinNumber = "XLRTE45MS0E008901", EngineNumber = "MX13AUUA08901",
        Brand = "Daf", ModelYear = 2021, TruckType = "Tự đổ",
        MaxPayloadKg = 30000m, LengthMm = 10500, WidthMm = 2550, HeightMm = 3200,
        OwnershipType = 2, CurrentStatus = 4, OdometerReading = 155000m,
        PurchaseDate = new DateTime(2021, 11, 3), LastMaintenanceDate = new DateTime(2026, 3, 5),
        CreatedAt = new DateTimeOffset(2026, 1, 17, 0, 0, 0, TimeSpan.Zero),
      },
      new Truck
      {
        TruckId = Truck9Id, TenantId = tenantId,
        LicensePlate = "43A-901.23", VinNumber = "JHLRE4H52KA009012", EngineNumber = "J08ETUA09012",
        Brand = "Hino", ModelYear = 2019, TruckType = "Mui bạt",
        MaxPayloadKg = 9000m, LengthMm = 9200, WidthMm = 2490, HeightMm = null,
        OwnershipType = 1, CurrentStatus = 5, OdometerReading = 280000m,
        PurchaseDate = new DateTime(2019, 8, 17), LastMaintenanceDate = new DateTime(2025, 10, 12),
        CreatedAt = new DateTimeOffset(2026, 1, 17, 0, 0, 0, TimeSpan.Zero),
      },
    ];
  }
}