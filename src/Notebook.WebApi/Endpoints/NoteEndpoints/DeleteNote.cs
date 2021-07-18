using Ardalis.ApiEndpoints;
using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.WebApi.Endpoints.NoteEndpoints
{
    public class DeleteNote : BaseAsyncEndpoint
        .WithRequest<DeleteNoteRequest>
        .WithoutResponse
    {
        private const string Route = "/notes/{Id:int}";
        public static string BuildRoute(int id) => Route.Replace("{Id:int}", id.ToString());

        private readonly IRepository<Note> _repository;

        public DeleteNote(IRepository<Note> repository)
        {
            _repository = repository;
        }

        [HttpDelete(Route)]
        [SwaggerOperation(Tags = new[] { "Notes" })]
        public override async Task<ActionResult> HandleAsync([FromRoute] DeleteNoteRequest request, CancellationToken cancellationToken = default)
        {
            Note note = await _repository.GetByIdAsync(request.Id, cancellationToken);

            await _repository.DeleteAsync(note, cancellationToken);

            return Ok();
        }
    }

    public class DeleteNoteRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
