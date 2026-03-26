using back_end_for_TMS.Models;
using Bogus;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class DriverDataSeeder : IDataSeeder<Driver>
{
  public static List<Driver> Generate()
  {
    Randomizer.Seed = new Random(456789);
    var staticNow = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);

    var licenseClasses = new[] { "B2", "C", "D", "FC" };

    var faker = new Faker<Driver>()
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

    return faker.Generate(8);
  }
}
