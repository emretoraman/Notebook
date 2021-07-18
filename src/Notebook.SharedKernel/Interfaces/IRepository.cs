using Ardalis.Specification;

namespace Notebook.SharedKernel.Interfaces
{
	public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
	{
	}
}
