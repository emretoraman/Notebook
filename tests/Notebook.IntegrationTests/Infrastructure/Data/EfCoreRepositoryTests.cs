using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.Core.Aggregates.NoteAggregate.Events;
using Notebook.Infrastructure.Data;
using Notebook.SharedKernel.BaseClasses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Notebook.IntegrationTests.Infrastructure.Data
{
    public class EfCoreRepositoryTests
    {
        private Mock<IMediator> _mockMediator;

        [Fact]
        public async Task UpdateAsync_ByNote_Updates()
        {
            string expected = "updated";
            EfCoreRepository<Note> noteRepository = GetNoteRepository();

            Note note = new("test");
            await noteRepository.AddAsync(note);

            note.Update(expected);
            await noteRepository.UpdateAsync(note);

            Note actual = (await noteRepository.ListAsync()).Single();

            Assert.Equal(expected, actual.Text);
            Assert.Equal(1, actual.Id);
            _mockMediator.Verify(
                m => m.Publish(
                    It.Is<BaseDomainEvent>(e => e is NoteUpdatedEvent), 
                    default
                ), 
                Times.Once
            );
        }

        private EfCoreRepository<Note> GetNoteRepository()
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            DbContextOptionsBuilder<EfCoreDbContext> builder = new DbContextOptionsBuilder<EfCoreDbContext>()
                .UseInMemoryDatabase("Notebook")
                .UseInternalServiceProvider(serviceProvider);

            _mockMediator = new Mock<IMediator>();

            EfCoreDbContext dbContext = new(builder.Options, _mockMediator.Object);
            return new EfCoreRepository<Note>(dbContext);
        }
    }
}
