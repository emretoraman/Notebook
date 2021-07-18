using System.Collections.Generic;

namespace Notebook.SharedKernel.BaseClasses
{
	public abstract class BaseEntity
	{
		public virtual int Id { get; protected set; }

		public List<BaseDomainEvent> Events = new();
	}
}
