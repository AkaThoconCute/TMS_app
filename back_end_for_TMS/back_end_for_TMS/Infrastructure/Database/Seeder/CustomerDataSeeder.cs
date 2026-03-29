using back_end_for_TMS.Models;
using Bogus;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class CustomerDataSeeder : IDataSeeder<Customer>
{
  public static List<Customer> Generate()
  {
    Randomizer.Seed = new Random(987654);
    var staticNow = new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);

    var faker = new Faker<Customer>()
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
            .RuleFor(c => c.CreatedAt, f => staticNow.AddDays(-f.Random.Number(1, 365)))
            .RuleFor(c => c.UpdatedAt, f => f.Random.Bool(0.5f) ? staticNow.AddDays(-f.Random.Number(1, 30)) : (DateTimeOffset?)null);

    return faker.Generate(8);
  }
}
