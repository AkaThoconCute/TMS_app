namespace back_end_for_TMS.Models;

public class Truck
{
  // 1. Identity Group (Hầu như không đổi)
  public Guid TruckId { get; set; } = Guid.NewGuid();
  public string LicensePlate { get; set; } = string.Empty; // Biển số xe
  public string? VinNumber { get; set; }                   // Số khung
  public string? EngineNumber { get; set; }                // Số máy
  public string? Brand { get; set; }                       // Hãng xe
  public int? ModelYear { get; set; }                      // Đời xe
  public DateTime? PurchaseDate { get; set; }              // Ngày mua

  // 2. Specification Group (Hầu như không đổi)
  public string? TruckType { get; set; }                   // Thùng kín, mui bạt...
  public decimal? MaxPayloadKg { get; set; }               // Tải trọng
  public int? LengthMm { get; set; }
  public int? WidthMm { get; set; }
  public int? HeightMm { get; set; }
  public int OwnershipType { get; set; } = 1;             // Xe nhà hoặc xe ngoài

  // 3. Operational Data (Thay đổi thường xuyên)
  public int CurrentStatus { get; set; } = 1;             // Trạng thái hiện tại
  public decimal OdometerReading { get; set; } = 0;       // Số km hiện tại
  public DateTime? LastMaintenanceDate { get; set; }      // Ngày bảo trì gần nhất

  // 4. Metadata
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? UpdatedAt { get; set; }          // Cập nhật mỗi khi Status hoặc Odometer thay đổi
}
