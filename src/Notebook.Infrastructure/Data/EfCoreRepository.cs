using Ardalis.Specification.EntityFrameworkCore;
using Notebook.SharedKernel.Interfaces;

namespace Notebook.Infrastructure.Data
{
	public class EfCoreRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
	{
		public EfCoreRepository(EfCoreDbContext dbContext) : base(dbContext)
		{
		}
	}
}
