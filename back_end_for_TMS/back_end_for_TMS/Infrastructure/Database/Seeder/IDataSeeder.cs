namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public interface IDataSeeder<T> where T : class
{
  static abstract List<T> Generate();
}