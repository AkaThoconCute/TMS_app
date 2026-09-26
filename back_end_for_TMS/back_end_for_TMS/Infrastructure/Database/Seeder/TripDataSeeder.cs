using back_end_for_TMS.Models;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class TripDataSeeder : IDataSeeder<Trip>
{
  public static List<Trip> Generate()
  {
    var tenantId = TenantDataSeeder.AlphaTenantId;
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
        TruckId = TruckDataSeeder.Truck1Id,
        DriverId = DriverDataSeeder.Driver1Id,
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
        TruckId = TruckDataSeeder.Truck2Id,
        DriverId = DriverDataSeeder.Driver2Id,
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
        TruckId = TruckDataSeeder.Truck3Id,
        DriverId = DriverDataSeeder.Driver3Id,
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
        TruckId = TruckDataSeeder.Truck4Id,
        DriverId = DriverDataSeeder.Driver4Id,
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
        TruckId = TruckDataSeeder.Truck5Id,
        DriverId = DriverDataSeeder.Driver5Id,
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
}
