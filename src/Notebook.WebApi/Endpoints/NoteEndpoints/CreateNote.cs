using Ardalis.ApiEndpoints;
using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.SharedKernel.Interfaces;
using Notebook.WebApi.Endpoints.NoteEndpoints.Records;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.WebApi.Endpoints.NoteEndpoints
{
    public class CreateNote : BaseAsyncEndpoint
        .WithRequest<CreateNoteRequest>
        .WithResponse<CreateNoteResponse>
    {
        private const string Route = "/notes";
        public static string BuildRoute() => Route;

        private readonly IRepository<Note> _repository;

        public CreateNote(IRepository<Note> repository)
        {
            _repository = repository;
        }

        [HttpPost(Route)]
        [SwaggerOperation(Tags = new[] { "Notes" })]
        public override async Task<ActionResult<CreateNoteResponse>> HandleAsync([FromBody] CreateNoteRequest request, CancellationToken cancellationToken = default)
        {
            Note note = new(request.Text);

            await _repository.AddAsync(note, cancellationToken);

            CreateNoteResponse response = new()
            {
                Note = new NoteRecord(note.Id, note.Text, note.TextType.ToString())
            };

            return Ok(response);
        }
    }

    public class CreateNoteRequest
    {
        [Required]
        public string Text { get; set; }
    }

    public class CreateNoteResponse
    {
        public NoteRecord Note { get; set; }
    }
}
