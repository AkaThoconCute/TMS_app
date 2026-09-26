using System.Linq.Expressions;
using back_end_for_TMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Models.Repository;

/// <summary>
/// Trip repository — stub for Sprint 4 Order implementation.
/// Full CRUD will be implemented in Trip tasks.
/// </summary>
public class TripRepo(AppDbContext dbContext)
{
  public Task<Trip?> FindAsync(Expression<Func<Trip, bool>> predicate)
    => dbContext.Trips.FirstOrDefaultAsync(predicate);

  public IQueryable<Trip> Query()
    => dbContext.Trips.AsQueryable();

  public void Add(Trip trip)
    => dbContext.Trips.Add(trip);

  public void Update(Trip trip)
    => dbContext.Trips.Update(trip);

  public Task SaveChangesAsync()
    => dbContext.SaveChangesAsync();
}
