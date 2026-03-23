using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.ModelConfig;

public interface IModelConfig<T> where T : class
{
  static abstract void Setup(ModelBuilder builder, List<T> seeding);
}