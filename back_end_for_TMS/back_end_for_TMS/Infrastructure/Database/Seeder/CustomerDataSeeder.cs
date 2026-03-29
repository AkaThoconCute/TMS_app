using back_end_for_TMS.Models;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class CustomerDataSeeder : IDataSeeder<Customer>
{
  // Fixed IDs for stable migrations and cross-seeder references
  public static readonly Guid Customer1Id = Guid.Parse("0195d004-cccc-7000-8000-000000000001");
  public static readonly Guid Customer2Id = Guid.Parse("0195d004-cccc-7000-8000-000000000002");
  public static readonly Guid Customer3Id = Guid.Parse("0195d004-cccc-7000-8000-000000000003");
  public static readonly Guid Customer4Id = Guid.Parse("0195d004-cccc-7000-8000-000000000004");
  public static readonly Guid Customer5Id = Guid.Parse("0195d004-cccc-7000-8000-000000000005");
  public static readonly Guid Customer6Id = Guid.Parse("0195d004-cccc-7000-8000-000000000006");
  public static readonly Guid Customer7Id = Guid.Parse("0195d004-cccc-7000-8000-000000000007");
  public static readonly Guid Customer8Id = Guid.Parse("0195d004-cccc-7000-8000-000000000008");

  public static List<Customer> Generate()
  {
    var tenantId = TenantDataSeeder.AlphaTenantId;

    return
    [
      new Customer
      {
        CustomerId = Customer1Id, TenantId = tenantId,
        Name = "Công ty TNHH Thuận Phát", ContactPerson = "Nguyễn Văn Toàn",
        PhoneNumber = "0281234567", Email = "info@thuanphat.vn",
        Address = "123 Nguyễn Huệ, Quận 1, Tp. Hồ Chí Minh",
        TaxCode = "0312345678", CustomerType = 2, Status = 1,
        CreatedAt = new DateTimeOffset(2026, 1, 10, 0, 0, 0, TimeSpan.Zero),
      },
      new Customer
      {
        CustomerId = Customer2Id, TenantId = tenantId,
        Name = "Công ty CP Nam Việt", ContactPerson = "Trần Thị Hoa",
        PhoneNumber = "0282345678", Email = "contact@namviet.com.vn",
        Address = "456 Lê Lợi, Quận 3, Tp. Hồ Chí Minh",
        TaxCode = "0323456789", CustomerType = 2, Status = 1,
        CreatedAt = new DateTimeOffset(2026, 1, 10, 0, 0, 0, TimeSpan.Zero),
      },
      new Customer
      {
        CustomerId = Customer3Id, TenantId = tenantId,
        Name = "Nguyễn Minh Khoa",
        PhoneNumber = "0903456789",
        Address = "789 Trần Hưng Đạo, Quận 5, Tp. Hồ Chí Minh",
        CustomerType = 1, Status = 1,
        CreatedAt = new DateTimeOffset(2026, 1, 12, 0, 0, 0, TimeSpan.Zero),
      },
      new Customer
      {
        CustomerId = Customer4Id, TenantId = tenantId,
        Name = "Doanh nghiệp Tư nhân Hưng Thịnh", ContactPerson = "Lê Văn Hưng",
        PhoneNumber = "0284567890", Email = "hungthinhlogistics@gmail.com",
        Address = "12 Đinh Tiên Hoàng, Bình Thạnh, Tp. Hồ Chí Minh",
        TaxCode = "0334567890", CustomerType = 2, Status = 1,
        CreatedAt = new DateTimeOffset(2026, 1, 12, 0, 0, 0, TimeSpan.Zero),
      },
      new Customer
      {
        CustomerId = Customer5Id, TenantId = tenantId,
        Name = "Lê Thị Thu",
        PhoneNumber = "0905678901", Email = "lethu@gmail.com",
        Address = "34 Võ Văn Tần, Quận 3, Tp. Hồ Chí Minh",
        CustomerType = 1, Status = 1,
        CreatedAt = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero),
      },
      new Customer
      {
        CustomerId = Customer6Id, TenantId = tenantId,
        Name = "Công ty TNHH Hải Long", ContactPerson = "Phạm Quốc Hải",
        PhoneNumber = "0286789012", Email = "hailong.transport@vnn.vn",
        Address = "78 Lý Tự Trọng, Quận 1, Tp. Hồ Chí Minh",
        TaxCode = "0356789012", CustomerType = 2, Status = 2,
        Notes = "Khách hàng tạm dừng hợp đồng",
        CreatedAt = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero),
        UpdatedAt = new DateTimeOffset(2026, 2, 20, 0, 0, 0, TimeSpan.Zero),
      },
      new Customer
      {
        CustomerId = Customer7Id, TenantId = tenantId,
        Name = "Phạm Văn Dũng",
        PhoneNumber = "0907890123",
        Address = "90 Lê Văn Việt, Quận 9, Tp. Hồ Chí Minh",
        CustomerType = 1, Status = 1,
        CreatedAt = new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero),
      },
      new Customer
      {
        CustomerId = Customer8Id, TenantId = tenantId,
        Name = "Công ty CP Việt Mỹ", ContactPerson = "Hoàng Thị Lan",
        PhoneNumber = "0288901234", Email = "vietmy.co@gmail.com",
        Address = "44 Hai Bà Trưng, Quận 1, Tp. Hồ Chí Minh",
        TaxCode = "0378901234", CustomerType = 2, Status = 1,
        CreatedAt = new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero),
      },
    ];
  }
}
