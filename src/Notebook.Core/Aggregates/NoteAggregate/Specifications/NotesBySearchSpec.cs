using Ardalis.Specification;
using Notebook.Core.Aggregates.NoteAggregate.Entities;

namespace Notebook.Core.Aggregates.NoteAggregate.Specifications
{
    public class NotesBySearchSpec : Specification<Note>
    {
        public NotesBySearchSpec(string searchString)
        {
            searchString ??= string.Empty;

            Query.Where(n => n.Text.Contains(searchString));
        }
    }
}
