using Notebook.FunctionalTests.WebApi.Helpers;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Notebook.FunctionalTests.WebApi
{
    public class StartupTests : IClassFixture<SqlServerWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public StartupTests(SqlServerWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RootPath_ByNoParameters_GetsSwagger()
        {
            HttpResponseMessage response = await _client.GetAsync("");

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("/swagger/index.html", response.RequestMessage.RequestUri.AbsolutePath);
        }
    }
}