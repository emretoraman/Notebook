using Notebook.FunctionalTests.WebApi.Helpers;
using Notebook.WebApi.Endpoints.NoteEndpoints;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit;

namespace Notebook.FunctionalTests.WebApi.Endpoints.NoteEndpoints
{
    public class UpdateNoteTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public UpdateNoteTests(InMemoryWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async void HandleAsync_ByRequest_Handles()
        {
            var expected = new { id = 1, text = "Text1 Updated", textType = "Mixed" };

            await _client.PostAsync(CreateNote.BuildRoute(), JsonContent.Create(new { text = "Text1" }));

            HttpResponseMessage response = await _client.PutAsync(UpdateNote.BuildRoute(), JsonContent.Create(new { expected.id, expected.text }));
            string responseContent = await response.Content.ReadAsStringAsync();
            UpdateNoteResponse actual = JsonConvert.DeserializeObject<UpdateNoteResponse>(responseContent);

            Assert.Equal(expected.id, actual.Note.Id);
            Assert.Equal(expected.text, actual.Note.Text);
            Assert.Equal(expected.textType, actual.Note.TextType);
        }
    }
}
