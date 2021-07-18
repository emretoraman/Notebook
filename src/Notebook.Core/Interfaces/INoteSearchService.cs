using Notebook.Core.Aggregates.NoteAggregate.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.Core.Interfaces
{
    public interface INoteSearchService
    {
        Task<List<Note>> GetNotesBySearch(string searchString, CancellationToken cancellationToken = default);
    }
}
