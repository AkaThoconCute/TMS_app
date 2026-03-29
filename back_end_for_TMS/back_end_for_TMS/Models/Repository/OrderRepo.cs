using System.Linq.Expressions;
using back_end_for_TMS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Models.Repository;

public class OrderRepo(AppDbContext dbContext)
{
  public Task<Order?> FindAsync(Expression<Func<Order, bool>> predicate)
    => dbContext.Orders.FirstOrDefaultAsync(predicate);

  public IQueryable<Order> Query()
    => dbContext.Orders.AsQueryable();

  public void Add(Order order)
    => dbContext.Orders.Add(order);

  public void Update(Order order)
    => dbContext.Orders.Update(order);

  public Task SaveChangesAsync()
    => dbContext.SaveChangesAsync();
}
