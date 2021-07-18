using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.Core.Aggregates.NoteAggregate.Specifications;
using Notebook.Core.Interfaces;
using Notebook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.Core.Services
{
    public class NoteSearchService : INoteSearchService
    {
        private readonly IRepository<Note> _repository;

        public NoteSearchService(IRepository<Note> repository)
        {
            _repository = repository;
        }

        public async Task<List<Note>> GetNotesBySearch(string searchString, CancellationToken cancellationToken = default)
        {
            NotesBySearchSpec spec = new(searchString);
            return await _repository.ListAsync(spec, cancellationToken);
        }
    }
}
