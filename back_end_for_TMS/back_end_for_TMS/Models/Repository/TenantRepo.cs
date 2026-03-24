using System.Linq.Expressions;
using back_end_for_TMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Models.Repository;

public class TenantRepo(AppDbContext dbContext)
{
  public Task<Tenant?> FindAsync(Expression<Func<Tenant, bool>> predicate)
    => dbContext.Tenants.FirstOrDefaultAsync(predicate);

  public IQueryable<Tenant> Query()
    => dbContext.Tenants.AsQueryable();

  public void Add(Tenant tenant)
    => dbContext.Tenants.Add(tenant);

  public void Update(Tenant tenant)
    => dbContext.Tenants.Update(tenant);

  public void Remove(Tenant tenant)
    => dbContext.Tenants.Remove(tenant);

  public Task SaveChangesAsync()
    => dbContext.SaveChangesAsync();
}
