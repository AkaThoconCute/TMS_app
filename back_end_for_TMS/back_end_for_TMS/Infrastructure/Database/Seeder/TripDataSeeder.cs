using back_end_for_TMS.Models;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class TripDataSeeder : IDataSeeder<Trip>
{
  public static List<Trip> Generate()
  {
    var tenantId = TenantDataSeeder.AlphaTenantId;

    // Get truck/driver IDs from their seeders (same Bogus seed → same GUIDs)
    var truckIds = GetSeededTruckIds();
    var driverIds = GetSeededDriverIds();
    var staticNow = new DateTimeOffset(2026, 3, 20, 0, 0, 0, TimeSpan.Zero);

    return
    [
      // Trip 1: Planned — linked to Order 3 (Assigned)
      new Trip
      {
        TenantId = tenantId,
        TripId = Guid.Parse("0195d002-bbbb-7000-8000-000000000001"),
        TripNumber = "TRP-000001",
        OrderId = OrderDataSeeder.Order3Id,
        TruckId = truckIds[0],
        DriverId = driverIds[0],
        PlannedPickupDate = new DateTime(2026, 3, 22),
        PlannedDeliveryDate = new DateTime(2026, 3, 23),
        Status = 1, // Planned
        Notes = "Scheduled for morning pickup",
        CreatedAt = staticNow.AddDays(-6),
      },
      // Trip 2: Planned — linked to Order 4 (Assigned)
      new Trip
      {
        TenantId = tenantId,
        TripId = Guid.Parse("0195d002-bbbb-7000-8000-000000000002"),
        TripNumber = "TRP-000002",
        OrderId = OrderDataSeeder.Order4Id,
        TruckId = truckIds[1],
        DriverId = driverIds[1],
        PlannedPickupDate = new DateTime(2026, 3, 21),
        PlannedDeliveryDate = new DateTime(2026, 3, 22),
        Status = 1, // Planned
        CreatedAt = staticNow.AddDays(-7),
      },
      // Trip 3: Completed — linked to Order 5 (Delivered)
      new Trip
      {
        TenantId = tenantId,
        TripId = Guid.Parse("0195d002-bbbb-7000-8000-000000000003"),
        TripNumber = "TRP-000003",
        OrderId = OrderDataSeeder.Order5Id,
        TruckId = truckIds[2],
        DriverId = driverIds[2],
        PlannedPickupDate = new DateTime(2026, 3, 15),
        PlannedDeliveryDate = new DateTime(2026, 3, 16),
        ActualPickupDate = new DateTime(2026, 3, 15, 8, 30, 0),
        ActualDeliveryDate = new DateTime(2026, 3, 16, 14, 0, 0),
        Status = 3, // Completed
        FuelCost = 850000m,
        TollCost = 120000m,
        OtherCost = 50000m,
        CostNotes = "Highway toll + parking fee",
        CompletedAt = staticNow.AddDays(-4),
        CreatedAt = staticNow.AddDays(-10),
        UpdatedAt = staticNow.AddDays(-4),
      },
      // Trip 4: Completed — linked to Order 6 (Completed)
      new Trip
      {
        TenantId = tenantId,
        TripId = Guid.Parse("0195d002-bbbb-7000-8000-000000000004"),
        TripNumber = "TRP-000004",
        OrderId = OrderDataSeeder.Order6Id,
        TruckId = truckIds[3],
        DriverId = driverIds[3],
        PlannedPickupDate = new DateTime(2026, 3, 10),
        PlannedDeliveryDate = new DateTime(2026, 3, 11),
        ActualPickupDate = new DateTime(2026, 3, 10, 9, 0, 0),
        ActualDeliveryDate = new DateTime(2026, 3, 11, 11, 0, 0),
        Status = 3, // Completed
        FuelCost = 600000m,
        TollCost = 80000m,
        CompletedAt = staticNow.AddDays(-9),
        CreatedAt = staticNow.AddDays(-14),
        UpdatedAt = staticNow.AddDays(-9),
      },
      // Trip 5: Cancelled — linked to Order 5 (previous attempt before Trip 3)
      new Trip
      {
        TenantId = tenantId,
        TripId = Guid.Parse("0195d002-bbbb-7000-8000-000000000005"),
        TripNumber = "TRP-000005",
        OrderId = OrderDataSeeder.Order5Id,
        TruckId = truckIds[4],
        DriverId = driverIds[4],
        PlannedPickupDate = new DateTime(2026, 3, 14),
        PlannedDeliveryDate = new DateTime(2026, 3, 15),
        Status = 4, // Cancelled
        CancellationReason = "Truck broke down before pickup",
        CancelledAt = staticNow.AddDays(-11),
        CreatedAt = staticNow.AddDays(-12),
        UpdatedAt = staticNow.AddDays(-11),
      },
    ];
  }

  private static List<Guid> GetSeededTruckIds()
  {
    // Must match TruckDataSeeder exactly: seed=789012, same rules, generate(10)
    Bogus.Randomizer.Seed = new Random(789012);
    var staticNow = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);

    var brands = new[] { "Hino", "Isuzu", "Howo", "Volvo", "Scania", "Mercedes-Benz", "Man", "Daf" };
    var types = new[] { "Thùng kín", "Mui bạt", "Bồn", "Cẩu", "Tự đổ" };

    var faker = new Bogus.Faker<Truck>()
        .RuleFor(t => t.TenantId, _ => TenantDataSeeder.AlphaTenantId)
        .RuleFor(t => t.TruckId, f => f.Random.Guid())
        .RuleFor(t => t.LicensePlate, f => $"{f.Random.Number(1000, 9999)}-{f.Random.AlphaNumeric(2).ToUpper()}{f.Random.Number(10, 99)}")
        .RuleFor(t => t.VinNumber, f => f.Vehicle.Vin())
        .RuleFor(t => t.EngineNumber, f => f.Random.AlphaNumeric(12).ToUpper())
        .RuleFor(t => t.Brand, f => f.PickRandom(brands))
        .RuleFor(t => t.ModelYear, f => f.Random.Number(2015, 2025))
        .RuleFor(t => t.PurchaseDate, f => f.Date.Past(10, staticNow.DateTime))
        .RuleFor(t => t.TruckType, f => f.PickRandom(types))
        .RuleFor(t => t.MaxPayloadKg, f => Math.Round(f.Random.Decimal(3000, 30000), 2))
        .RuleFor(t => t.LengthMm, f => f.Random.Number(5000, 12000))
        .RuleFor(t => t.WidthMm, f => f.Random.Number(2400, 2600))
        .RuleFor(t => t.HeightMm, f => f.Random.Number(2500, 3500))
        .RuleFor(t => t.OwnershipType, f => f.Random.Number(1, 2))
        .RuleFor(t => t.CurrentStatus, f => f.Random.Number(1, 5))
        .RuleFor(t => t.OdometerReading, f => Math.Round(f.Random.Decimal(10000, 500000), 2))
        .RuleFor(t => t.LastMaintenanceDate, f => f.Date.Past(2, staticNow.DateTime))
        .RuleFor(t => t.CreatedAt, f => staticNow.AddDays(-f.Random.Number(1, 365)))
        .RuleFor(t => t.UpdatedAt, f => f.Random.Bool() ? staticNow.AddDays(-f.Random.Number(1, 30)) : (DateTimeOffset?)null);

    var trucks = faker.Generate(10);
    return trucks.Select(t => t.TruckId).ToList();
  }

  private static List<Guid> GetSeededDriverIds()
  {
    // Must match DriverDataSeeder exactly: seed=456789, same rules, generate(8)
    Bogus.Randomizer.Seed = new Random(456789);
    var staticNow = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);

    var licenseClasses = new[] { "B2", "C", "D", "FC" };

    var faker = new Bogus.Faker<Driver>()
        .RuleFor(d => d.TenantId, _ => TenantDataSeeder.AlphaTenantId)
        .RuleFor(d => d.DriverId, f => f.Random.Guid())
        .RuleFor(d => d.FullName, f => f.Name.FullName())
        .RuleFor(d => d.PhoneNumber, f => $"09{f.Random.Number(10000000, 99999999)}")
        .RuleFor(d => d.LicenseNumber, f => $"B{f.Random.Number(1, 9)}-{f.Random.Number(100000, 999999)}")
        .RuleFor(d => d.LicenseClass, f => f.PickRandom(licenseClasses))
        .RuleFor(d => d.LicenseExpiry, f => f.Date.Future(5, staticNow.DateTime))
        .RuleFor(d => d.DateOfBirth, f => f.Date.Past(55, staticNow.DateTime.AddYears(-25)))
        .RuleFor(d => d.Status, f => f.Random.WeightedRandom([1, 2, 3], [0.7f, 0.2f, 0.1f]))
        .RuleFor(d => d.HireDate, f => f.Date.Past(5, staticNow.DateTime))
        .RuleFor(d => d.Notes, f => f.Random.Bool(0.2f) ? f.Lorem.Sentence() : null)
        .RuleFor(d => d.CreatedAt, f => staticNow.AddDays(-f.Random.Number(1, 365)))
        .RuleFor(d => d.UpdatedAt, f => f.Random.Bool() ? staticNow.AddDays(-f.Random.Number(1, 30)) : (DateTimeOffset?)null);

    var drivers = faker.Generate(8);
    return drivers.Select(d => d.DriverId).ToList();
  }
}
