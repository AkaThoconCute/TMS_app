using back_end_for_TMS.Models;
using Bogus;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class TruckDataSeeder
{
  public static List<Truck> Generate()
  {
    Randomizer.Seed = new Random(789012);
    var staticNow = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);

    var brands = new[] { "Hino", "Isuzu", "Howo", "Volvo", "Scania", "Mercedes-Benz", "Man", "Daf" };
    var types = new[] { "Thùng kín", "Mui bạt", "Bồn", "Cẩu", "Tự đổ" };

    var faker = new Faker<Truck>()
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
    return faker.Generate(10);
  }
}