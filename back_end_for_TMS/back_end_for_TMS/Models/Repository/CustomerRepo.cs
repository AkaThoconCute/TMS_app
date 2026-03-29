using System.Linq.Expressions;
using back_end_for_TMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Models.Repository;

public class CustomerRepo(AppDbContext dbContext)
{
  public Task<Customer?> FindAsync(Expression<Func<Customer, bool>> predicate)
    => dbContext.Customers.FirstOrDefaultAsync(predicate);

  public IQueryable<Customer> Query()
    => dbContext.Customers.AsQueryable();

  public void Add(Customer customer)
    => dbContext.Customers.Add(customer);

  public void Update(Customer customer)
    => dbContext.Customers.Update(customer);

  public void Remove(Customer customer)
    => dbContext.Customers.Remove(customer);

  public Task SaveChangesAsync()
    => dbContext.SaveChangesAsync();
}
