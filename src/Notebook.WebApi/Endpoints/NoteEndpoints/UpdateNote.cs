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
    public class UpdateNote : BaseAsyncEndpoint
        .WithRequest<UpdateNoteRequest>
        .WithResponse<UpdateNoteResponse>
    {
        private const string Route = "/notes";
        public static string BuildRoute() => Route;

        private readonly IRepository<Note> _repository;

        public UpdateNote(IRepository<Note> repository)
        {
            _repository = repository;
        }

        [HttpPut(Route)]
        [SwaggerOperation(Tags = new[] { "Notes" })]
        public override async Task<ActionResult<UpdateNoteResponse>> HandleAsync([FromBody] UpdateNoteRequest request, CancellationToken cancellationToken = default)
        {
            Note note = await _repository.GetByIdAsync(request.Id, cancellationToken);
            note.Update(request.Text);

            await _repository.UpdateAsync(note, cancellationToken);

            UpdateNoteResponse response = new()
            {
                Note = new NoteRecord(note.Id, note.Text, note.TextType.ToString())
            };

            return Ok(response);
        }
    }

    public class UpdateNoteRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }
    }

    public class UpdateNoteResponse
    {
        public NoteRecord Note { get; set; }
    }
}
