using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.Core.Aggregates.NoteAggregate.Specifications;
using Notebook.Core.Services;
using Notebook.SharedKernel.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Notebook.UnitTests.Core.Services
{
    public class NoteSearchServiceTests
    {
        [Fact]
        public async Task GetNotesBySearch_BySearchString_Gets()
        {
            List<Note> expected = new() { new Note("test") };
            Mock<IRepository<Note>> mockRepository = new();
            mockRepository
                .Setup(r => r.ListAsync(It.IsAny<NotesBySearchSpec>(), default))
                .ReturnsAsync(expected);
            NoteSearchService service = new(mockRepository.Object);

            List<Note> actual = await service.GetNotesBySearch("test");

            Assert.Same(expected, actual);
        }
    }
}
