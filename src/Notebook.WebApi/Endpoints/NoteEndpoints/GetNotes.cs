using Ardalis.ApiEndpoints;
using Notebook.Core.Aggregates.NoteAggregate.Entities;
using Notebook.Core.Interfaces;
using Notebook.WebApi.Endpoints.NoteEndpoints.Records;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.WebApi.Endpoints.NoteEndpoints
{
    public class GetNotes : BaseAsyncEndpoint
        .WithRequest<GetNotesRequest>
        .WithResponse<GetNotesResponse>
    {
        private const string Route = "/notes";
        public static string BuildRoute(string searchString) => $"{Route}?SearchString={searchString}";

        private readonly INoteSearchService _service;

        public GetNotes(INoteSearchService service)
        {
            _service = service;
        }

        [HttpGet(Route)]
        [SwaggerOperation(Tags = new[] { "Notes" })]
        public override async Task<ActionResult<GetNotesResponse>> HandleAsync([FromQuery] GetNotesRequest request, CancellationToken cancellationToken = default)
        {
            List<Note> notes = await _service.GetNotesBySearch(request.SearchString, cancellationToken);

            GetNotesResponse response = new()
            {
                Notes = notes.Select(n => new NoteRecord(n.Id, n.Text, n.TextType.ToString())).ToList()
            };

            return Ok(response);
        }
    }

    public class GetNotesRequest
    {
        public string SearchString { get; set; }
    }

    public class GetNotesResponse
    {
        public List<NoteRecord> Notes { get; set; }
    }
}
