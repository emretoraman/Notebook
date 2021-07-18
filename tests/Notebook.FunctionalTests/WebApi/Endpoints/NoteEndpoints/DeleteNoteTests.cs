using Notebook.FunctionalTests.WebApi.Helpers;
using Notebook.WebApi.Endpoints.NoteEndpoints;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Notebook.FunctionalTests.WebApi.Endpoints.NoteEndpoints
{
    public class DeleteNoteTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public DeleteNoteTests(InMemoryWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void HandleAsync_ByRequest_Handles()
        {
            int id = 1;

            await _client.PostAsync(CreateNote.BuildRoute(), JsonContent.Create(new { text = "Text1" }));

            HttpResponseMessage response = await _client.DeleteAsync(DeleteNote.BuildRoute(id));

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
