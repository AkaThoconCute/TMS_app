using back_end_for_TMS.Models;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class OrderDataSeeder : IDataSeeder<Order>
{
  // Expose order IDs for TripDataSeeder to reference
  public static readonly Guid Order1Id = Guid.Parse("0195d001-aaaa-7000-8000-000000000001");
  public static readonly Guid Order2Id = Guid.Parse("0195d001-aaaa-7000-8000-000000000002");
  public static readonly Guid Order3Id = Guid.Parse("0195d001-aaaa-7000-8000-000000000003");
  public static readonly Guid Order4Id = Guid.Parse("0195d001-aaaa-7000-8000-000000000004");
  public static readonly Guid Order5Id = Guid.Parse("0195d001-aaaa-7000-8000-000000000005");
  public static readonly Guid Order6Id = Guid.Parse("0195d001-aaaa-7000-8000-000000000006");

  public static List<Order> Generate()
  {
    var tenantId = TenantDataSeeder.AlphaTenantId;

    // We need customer IDs from seeded customers — CustomerDataSeeder uses Bogus with seed 987654
    // Generate them deterministically to get the same GUIDs
    var customerIds = GetSeededCustomerIds();
    var staticNow = new DateTimeOffset(2026, 3, 20, 0, 0, 0, TimeSpan.Zero);

    return
    [
      // Order 1: Created (no trip yet)
      new Order
      {
        TenantId = tenantId,
        OrderId = Order1Id,
        OrderNumber = "ORD-000001",
        CustomerId = customerIds[0],
        PickupAddress = "123 Nguyen Hue, District 1, HCMC",
        DeliveryAddress = "456 Le Loi, District 3, HCMC",
        CargoDescription = "Household furniture — sofa, table, chairs",
        CargoWeightKg = 350m,
        RequestedPickupDate = new DateTime(2026, 3, 25),
        RequestedDeliveryDate = new DateTime(2026, 3, 26),
        QuotedPrice = 2500000m,
        Status = 1, // Created
        Notes = "Customer requests morning pickup",
        CreatedAt = staticNow.AddDays(-5),
      },
      // Order 2: Created (no trip yet)
      new Order
      {
        TenantId = tenantId,
        OrderId = Order2Id,
        OrderNumber = "ORD-000002",
        CustomerId = customerIds[1],
        PickupAddress = "789 Tran Hung Dao, District 5, HCMC",
        DeliveryAddress = "12 Pham Van Dong, Thu Duc, HCMC",
        CargoDescription = "Office equipment — desks, monitors, printers",
        CargoWeightKg = 500m,
        RequestedPickupDate = new DateTime(2026, 3, 27),
        RequestedDeliveryDate = new DateTime(2026, 3, 28),
        QuotedPrice = 4000000m,
        Status = 1, // Created
        CreatedAt = staticNow.AddDays(-3),
      },
      // Order 3: Assigned (has a Planned trip)
      new Order
      {
        TenantId = tenantId,
        OrderId = Order3Id,
        OrderNumber = "ORD-000003",
        CustomerId = customerIds[2],
        PickupAddress = "34 Vo Van Tan, District 3, HCMC",
        DeliveryAddress = "56 Nguyen Van Linh, District 7, HCMC",
        CargoDescription = "Refrigerator and washing machine",
        CargoWeightKg = 200m,
        RequestedPickupDate = new DateTime(2026, 3, 22),
        RequestedDeliveryDate = new DateTime(2026, 3, 23),
        QuotedPrice = 1800000m,
        Status = 2, // Assigned
        CreatedAt = staticNow.AddDays(-7),
        UpdatedAt = staticNow.AddDays(-6),
      },
      // Order 4: Assigned (has a Planned trip)
      new Order
      {
        TenantId = tenantId,
        OrderId = Order4Id,
        OrderNumber = "ORD-000004",
        CustomerId = customerIds[3],
        PickupAddress = "78 Ly Tu Trong, District 1, HCMC",
        DeliveryAddress = "90 Le Van Viet, District 9, HCMC",
        CargoDescription = "Personal belongings — boxes, clothing, kitchenware",
        CargoWeightKg = 150m,
        RequestedPickupDate = new DateTime(2026, 3, 21),
        RequestedDeliveryDate = new DateTime(2026, 3, 22),
        QuotedPrice = 1200000m,
        Status = 2, // Assigned
        CreatedAt = staticNow.AddDays(-8),
        UpdatedAt = staticNow.AddDays(-7),
      },
      // Order 5: Delivered (trip completed, awaiting manager finalization)
      new Order
      {
        TenantId = tenantId,
        OrderId = Order5Id,
        OrderNumber = "ORD-000005",
        CustomerId = customerIds[0],
        PickupAddress = "100 Cach Mang Thang 8, District 10, HCMC",
        DeliveryAddress = "200 Quang Trung, Go Vap, HCMC",
        CargoDescription = "Construction materials — cement, steel bars",
        CargoWeightKg = 2000m,
        RequestedPickupDate = new DateTime(2026, 3, 15),
        RequestedDeliveryDate = new DateTime(2026, 3, 16),
        QuotedPrice = 5500000m,
        Status = 4, // Delivered
        CreatedAt = staticNow.AddDays(-12),
        UpdatedAt = staticNow.AddDays(-4),
      },
      // Order 6: Completed (fully finalized)
      new Order
      {
        TenantId = tenantId,
        OrderId = Order6Id,
        OrderNumber = "ORD-000006",
        CustomerId = customerIds[1],
        PickupAddress = "44 Hai Ba Trung, District 1, HCMC",
        DeliveryAddress = "88 Nguyen Thi Minh Khai, District 3, HCMC",
        CargoDescription = "Electronic appliances — TVs, air conditioners",
        CargoWeightKg = 300m,
        RequestedPickupDate = new DateTime(2026, 3, 10),
        RequestedDeliveryDate = new DateTime(2026, 3, 11),
        QuotedPrice = 3200000m,
        Status = 5, // Completed
        CompletedAt = staticNow.AddDays(-6),
        Notes = "Customer paid in full",
        CreatedAt = staticNow.AddDays(-15),
        UpdatedAt = staticNow.AddDays(-6),
      },
    ];
  }

  /// <summary>
  /// Regenerate the same customer GUIDs that CustomerDataSeeder produces with seed 987654.
  /// </summary>
  private static List<Guid> GetSeededCustomerIds()
  {
    Bogus.Randomizer.Seed = new Random(987654);

    var faker = new Bogus.Faker<Customer>()
        .RuleFor(c => c.TenantId, _ => TenantDataSeeder.AlphaTenantId)
        .RuleFor(c => c.CustomerId, f => f.Random.Guid())
        .RuleFor(c => c.CustomerType, f => f.Random.WeightedRandom([1, 2], [0.6f, 0.4f]))
        .RuleFor(c => c.Name, f => f.Company.CompanyName())
        .RuleFor(c => c.ContactPerson, f => f.Random.Bool(0.7f) ? f.Name.FullName() : null)
        .RuleFor(c => c.PhoneNumber, f => $"09{f.Random.Number(10000000, 99999999)}")
        .RuleFor(c => c.Email, f => f.Random.Bool(0.6f) ? f.Internet.Email() : null)
        .RuleFor(c => c.Address, f => f.Random.Bool(0.8f) ? f.Address.FullAddress() : null)
        .RuleFor(c => c.TaxCode, (f, c) => f.Random.Bool(0.5f) ? f.Random.Number(1000000000, 1999999999).ToString() : null)
        .RuleFor(c => c.Status, f => f.Random.WeightedRandom([1, 2], [0.8f, 0.2f]))
        .RuleFor(c => c.Notes, f => f.Random.Bool(0.2f) ? f.Lorem.Sentence() : null)
        .RuleFor(c => c.CreatedAt, f => new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero).AddDays(-f.Random.Number(1, 365)))
        .RuleFor(c => c.UpdatedAt, f => f.Random.Bool(0.5f) ? new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero).AddDays(-f.Random.Number(1, 30)) : (DateTimeOffset?)null);

    var customers = faker.Generate(8);
    return customers.Select(c => c.CustomerId).ToList();
  }
}
