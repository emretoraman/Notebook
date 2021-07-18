using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.SharedKernel.BaseClasses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.Infrastructure.Data
{
    public class EfCoreDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public EfCoreDbContext(DbContextOptions<EfCoreDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Note> Notes { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result = await base.SaveChangesAsync(cancellationToken);

            BaseEntity[] entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (BaseEntity entity in entitiesWithEvents)
            {
                BaseDomainEvent[] domainEvents = entity.Events.ToArray();
                entity.Events.Clear();

                foreach (BaseDomainEvent domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
            }

            return result;
        }
    }
}
