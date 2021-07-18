using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.Core.Aggregates.NoteAggregate.Specifications;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Notebook.UnitTests.Core.Aggregates.NoteAggregate.Specifications
{
    public class NoteBySearchSpecTests
    {
        [Fact]
        public void Evaluate_BySearchString_ReturnsExpected()
        {
            string searchString = "search";

            List<Note> expected = new()
            {
                new Note($"{searchString}test1234"),
                new Note($"te{searchString}st1234"),
                new Note($"test1234{searchString}")
            };

            List<Note> allNotes = expected
                .Concat(new List<Note>
                {
                    new Note("test1234")
                })
                .ToList();

            NotesBySearchSpec spec = new(searchString);

            List<Note> actual = spec.Evaluate(allNotes).ToList();

            Assert.Equal(expected.Count, actual.Count);
            Assert.True(expected.All(e => actual.Contains(e)));
        }

        [Fact]
        public void Evaluate_ByNullSearchString_ReturnsAll()
        {
            List<Note> expected = new()
            {
                new Note("test1234"),
                new Note("test1234")
            };

            NotesBySearchSpec spec = new(null);

            List<Note> actual = spec.Evaluate(expected).ToList();

            Assert.Equal(expected.Count, actual.Count);
            Assert.True(expected.All(e => actual.Contains(e)));
        }
    }
}
