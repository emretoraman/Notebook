using Notebook.Core.Aggregates.NoteAggregate.Events;
using Notebook.Core.Aggregates.NoteAggregate.ValueObjects;
using Notebook.SharedKernel.BaseClasses;
using Notebook.SharedKernel.Interfaces;
using System.Text.RegularExpressions;

namespace Notebook.Core.Aggregates.NoteAggregate.Entities
{
    public class Note : BaseEntity, IAggregateRoot
    {
        public Note(string text)
        {
            Text = text;
        }

        public TextType TextType => GetTextType();

        public string Text { get; private set; }

        public void Update(string text)
        {
            string oldText = Text;
            Text = text;

            Events.Add(new NoteUpdatedEvent(this, oldText));
        }

        private TextType GetTextType()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return TextType.EmptyOrNull;
            }
            if (Regex.IsMatch(Text, "^[A-Za-z]+$"))
            {
                return TextType.Alphabetic;
            }
            if (Regex.IsMatch(Text, "^[0-9]+$"))
            {
                return TextType.Numeric;
            }
            if (Regex.IsMatch(Text, "^[A-Za-z0-9]+$"))
            {
                return TextType.Alphanumeric;
            }
            return TextType.Mixed;
        }
    }
}
