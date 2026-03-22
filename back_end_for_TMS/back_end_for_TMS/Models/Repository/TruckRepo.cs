using System.Linq.Expressions;
using back_end_for_TMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Models.Repository;

public class TruckRepo(AppDbContext dbContext)
{
  public Task<Truck?> FindAsync(Expression<Func<Truck, bool>> predicate)
    => dbContext.Trucks.FirstOrDefaultAsync(predicate);

  public IQueryable<Truck> Query()
    => dbContext.Trucks.AsQueryable();

  public void Add(Truck truck)
    => dbContext.Trucks.Add(truck);

  public void Update(Truck truck)
    => dbContext.Trucks.Update(truck);

  public void Remove(Truck truck)
    => dbContext.Trucks.Remove(truck);

  public Task SaveChangesAsync()
    => dbContext.SaveChangesAsync();
}
