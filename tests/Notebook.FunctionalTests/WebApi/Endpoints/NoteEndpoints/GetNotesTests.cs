using Notebook.FunctionalTests.WebApi.Helpers;
using Notebook.WebApi.Endpoints.NoteEndpoints;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Notebook.FunctionalTests.WebApi.Endpoints.NoteEndpoints
{
    public class GetNotesTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GetNotesTests(InMemoryWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void HandleAsync_ByNullSearchString_ReturnsAll()
        {
            await _client.PostAsync(CreateNote.BuildRoute(), JsonContent.Create(new { text = "note1" }));
            await _client.PostAsync(CreateNote.BuildRoute(), JsonContent.Create(new { text = "note2" }));

            HttpResponseMessage response = await _client.GetAsync(GetNotes.BuildRoute(null));
            string responseContent = await response.Content.ReadAsStringAsync();
            GetNotesResponse actual = JsonConvert.DeserializeObject<GetNotesResponse>(responseContent);

            Assert.Equal(2, actual.Notes.Count);
        }
    }
}
