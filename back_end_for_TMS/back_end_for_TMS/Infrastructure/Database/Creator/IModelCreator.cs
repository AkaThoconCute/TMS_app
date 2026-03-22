using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.Creator;

public interface IModelCreator<T> where T : class
{
  static abstract void Setup(ModelBuilder builder, List<T> seeding);
}