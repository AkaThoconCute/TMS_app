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
    var staticNow = new DateTimeOffset(2026, 3, 20, 0, 0, 0, TimeSpan.Zero);

    return
    [
      // Order 1: Created (no trip yet)
      new Order
      {
        TenantId = tenantId,
        OrderId = Order1Id,
        OrderNumber = "ORD-000001",
        CustomerId = CustomerDataSeeder.Customer1Id,
        PickupAddress = "123 Nguyễn Huệ, Quận 1, Tp. Hồ Chí Minh",
        DeliveryAddress = "456 Lê Lợi, Quận 3, Tp. Hồ Chí Minh",
        CargoDescription = "Nội thất gia đình — ghế sofa, bàn, ghế",
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
        CustomerId = CustomerDataSeeder.Customer2Id,
        PickupAddress = "789 Trần Hưng Đạo, Quận 5, Tp. Hồ Chí Minh",
        DeliveryAddress = "12 Phạm Văn Đồng, Thủ Đức, Tp. Hồ Chí Minh",
        CargoDescription = "Thiết bị văn phòng — bàn làm việc, màn hình, máy in",
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
        CustomerId = CustomerDataSeeder.Customer3Id,
        PickupAddress = "34 Võ Văn Tần, Quận 3, Tp. Hồ Chí Minh",
        DeliveryAddress = "56 Nguyễn Văn Linh, Quận 7, Tp. Hồ Chí Minh",
        CargoDescription = "Tủ lạnh và máy giặt",
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
        CustomerId = CustomerDataSeeder.Customer4Id,
        PickupAddress = "78 Lý Tự Trọng, Quận 1, Tp. Hồ Chí Minh",
        DeliveryAddress = "90 Lê Văn Việt, Quận 9, Tp. Hồ Chí Minh",
        CargoDescription = "Hành lý cá nhân — hộp carton, quần áo, dụng cụ nhà bếp",
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
        CustomerId = CustomerDataSeeder.Customer1Id,
        PickupAddress = "100 Cách Mạng Tháng 8, Quận 10, Tp. Hồ Chí Minh",
        DeliveryAddress = "200 Quang Trung, Gò Vấp, Tp. Hồ Chí Minh",
        CargoDescription = "Vật liệu xây dựng — xi măng, thanh thép",
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
        CustomerId = CustomerDataSeeder.Customer2Id,
        PickupAddress = "44 Hai Bà Trưng, Quận 1, Tp. Hồ Chí Minh",
        DeliveryAddress = "88 Nguyễn Thị Minh Khai, Quận 3, Tp. Hồ Chí Minh",
        CargoDescription = "Điện tử gia dụng — TV, điều hòa không khí",
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

}
