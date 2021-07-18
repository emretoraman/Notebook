using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.Core.Aggregates.NoteAggregate.Events;
using Notebook.Core.Aggregates.NoteAggregate.ValueObjects;
using System.Linq;
using Xunit;

namespace Notebook.UnitTests.Core.Aggregates.NoteAggregate.Entities
{
    public class NoteTests
    {
        [Theory]
        [InlineData(null, TextType.EmptyOrNull)]
        [InlineData("", TextType.EmptyOrNull)]
        [InlineData("Test", TextType.Alphabetic)]
        [InlineData("1234", TextType.Numeric)]
        [InlineData("Test1234", TextType.Alphanumeric)]
        [InlineData("Test 1234", TextType.Mixed)]
        public void Constructor_ByText_Constructs(string text, TextType type)
        {
            Note note = new(text);

            Assert.Equal(note.Text, text);
            Assert.Equal(note.TextType, type);
        }

        [Fact]
        public void Update_ByText_Updates()
        {
            string expected = "updated";
            Note note = new("test");

            note.Update(expected);

            Assert.Equal(note.Text, expected);
            Assert.True(note.Events.Single() is NoteUpdatedEvent);
        }
    }
}
