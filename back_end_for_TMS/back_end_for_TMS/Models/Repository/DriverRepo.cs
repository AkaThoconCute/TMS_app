using System.Linq.Expressions;
using back_end_for_TMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Models.Repository;

public class DriverRepo(AppDbContext dbContext)
{
  public Task<Driver?> FindAsync(Expression<Func<Driver, bool>> predicate)
    => dbContext.Drivers.FirstOrDefaultAsync(predicate);

  public IQueryable<Driver> Query()
    => dbContext.Drivers.AsQueryable();

  public void Add(Driver driver)
    => dbContext.Drivers.Add(driver);

  public void Update(Driver driver)
    => dbContext.Drivers.Update(driver);

  public void Remove(Driver driver)
    => dbContext.Drivers.Remove(driver);

  public Task SaveChangesAsync()
    => dbContext.SaveChangesAsync();
}
