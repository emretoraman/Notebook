using Notebook.FunctionalTests.WebApi.Helpers;
using Notebook.WebApi.Endpoints.NoteEndpoints;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Notebook.FunctionalTests.WebApi.Endpoints.NoteEndpoints
{
    public class CreateNoteTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CreateNoteTests(InMemoryWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void HandleAsync_ByRequest_Handles()
        {
            var expected = new { id = 1, text = "Text1", textType = "Alphanumeric" };

            JsonContent requestContent = JsonContent.Create(new { expected.text });

            HttpResponseMessage response = await _client.PostAsync(CreateNote.BuildRoute(), requestContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            CreateNoteResponse actual = JsonConvert.DeserializeObject<CreateNoteResponse>(responseContent);

            Assert.Equal(expected.id, actual.Note.Id);
            Assert.Equal(expected.text, actual.Note.Text);
            Assert.Equal(expected.textType, actual.Note.TextType);
        }
    }
}
