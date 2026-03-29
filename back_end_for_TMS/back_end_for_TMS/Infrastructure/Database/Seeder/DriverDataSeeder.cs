using back_end_for_TMS.Models;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class DriverDataSeeder : IDataSeeder<Driver>
{
  // Fixed IDs for stable migrations and cross-seeder references
  public static readonly Guid Driver1Id = Guid.Parse("0195d003-dddd-7000-8000-000000000001");
  public static readonly Guid Driver2Id = Guid.Parse("0195d003-dddd-7000-8000-000000000002");
  public static readonly Guid Driver3Id = Guid.Parse("0195d003-dddd-7000-8000-000000000003");
  public static readonly Guid Driver4Id = Guid.Parse("0195d003-dddd-7000-8000-000000000004");
  public static readonly Guid Driver5Id = Guid.Parse("0195d003-dddd-7000-8000-000000000005");
  public static readonly Guid Driver6Id = Guid.Parse("0195d003-dddd-7000-8000-000000000006");
  public static readonly Guid Driver7Id = Guid.Parse("0195d003-dddd-7000-8000-000000000007");
  public static readonly Guid Driver8Id = Guid.Parse("0195d003-dddd-7000-8000-000000000008");

  public static List<Driver> Generate()
  {
    var tenantId = TenantDataSeeder.AlphaTenantId;

    return
    [
      new Driver
      {
        DriverId = Driver1Id, TenantId = tenantId,
        FullName = "Nguyễn Văn Minh", PhoneNumber = "0901234567",
        LicenseNumber = "B2-123456", LicenseClass = "B2",
        LicenseExpiry = new DateTime(2028, 6, 15), DateOfBirth = new DateTime(1985, 4, 10),
        Status = 1, HireDate = new DateTime(2020, 3, 1),
        CreatedAt = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero),
      },
      new Driver
      {
        DriverId = Driver2Id, TenantId = tenantId,
        FullName = "Trần Thanh Tùng", PhoneNumber = "0912345678",
        LicenseNumber = "FC-234567", LicenseClass = "FC",
        LicenseExpiry = new DateTime(2027, 9, 20), DateOfBirth = new DateTime(1980, 7, 25),
        Status = 1, HireDate = new DateTime(2019, 6, 15),
        CreatedAt = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero),
      },
      new Driver
      {
        DriverId = Driver3Id, TenantId = tenantId,
        FullName = "Lê Văn Hùng", PhoneNumber = "0923456789",
        LicenseNumber = "C-345678", LicenseClass = "C",
        LicenseExpiry = new DateTime(2029, 3, 5), DateOfBirth = new DateTime(1990, 1, 18),
        Status = 1, HireDate = new DateTime(2021, 9, 1),
        CreatedAt = new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero),
      },
      new Driver
      {
        DriverId = Driver4Id, TenantId = tenantId,
        FullName = "Phạm Quốc Bảo", PhoneNumber = "0934567890",
        LicenseNumber = "D-456789", LicenseClass = "D",
        LicenseExpiry = new DateTime(2027, 11, 30), DateOfBirth = new DateTime(1978, 12, 3),
        Status = 1, HireDate = new DateTime(2018, 1, 20),
        CreatedAt = new DateTimeOffset(2026, 1, 16, 0, 0, 0, TimeSpan.Zero),
      },
      new Driver
      {
        DriverId = Driver5Id, TenantId = tenantId,
        FullName = "Hoàng Văn Nam", PhoneNumber = "0945678901",
        LicenseNumber = "C-567890", LicenseClass = "C",
        LicenseExpiry = new DateTime(2026, 5, 10), DateOfBirth = new DateTime(1992, 8, 22),
        Status = 1, HireDate = new DateTime(2022, 4, 10),
        Notes = "Bằng lái sắp hết hạn — cần gia hạn trước tháng 5/2026",
        CreatedAt = new DateTimeOffset(2026, 1, 16, 0, 0, 0, TimeSpan.Zero),
      },
      new Driver
      {
        DriverId = Driver6Id, TenantId = tenantId,
        FullName = "Nguyễn Đức Trí", PhoneNumber = "0956789012",
        LicenseNumber = "B2-678901", LicenseClass = "B2",
        LicenseExpiry = new DateTime(2030, 2, 14), DateOfBirth = new DateTime(1995, 3, 7),
        Status = 2, HireDate = new DateTime(2023, 7, 1), // OnLeave
        CreatedAt = new DateTimeOffset(2026, 1, 16, 0, 0, 0, TimeSpan.Zero),
      },
      new Driver
      {
        DriverId = Driver7Id, TenantId = tenantId,
        FullName = "Võ Minh Phúc", PhoneNumber = "0967890123",
        LicenseNumber = "FC-789012", LicenseClass = "FC",
        LicenseExpiry = new DateTime(2028, 8, 19), DateOfBirth = new DateTime(1983, 6, 14),
        Status = 1, HireDate = new DateTime(2017, 11, 5),
        CreatedAt = new DateTimeOffset(2026, 1, 17, 0, 0, 0, TimeSpan.Zero),
      },
      new Driver
      {
        DriverId = Driver8Id, TenantId = tenantId,
        FullName = "Đặng Văn Long", PhoneNumber = "0978901234",
        LicenseNumber = "C-890123", LicenseClass = "C",
        LicenseExpiry = new DateTime(2027, 4, 25), DateOfBirth = new DateTime(1988, 11, 29),
        Status = 3, HireDate = new DateTime(2016, 5, 12), // Terminated
        CreatedAt = new DateTimeOffset(2026, 1, 17, 0, 0, 0, TimeSpan.Zero),
      },
    ];
  }
}
