using Notebook.FunctionalTests.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
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
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions()
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task RootPath_ByNoParameters_GetsSwagger()
        {
            HttpResponseMessage response = await _client.GetAsync("");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/swagger/index.html", response.Headers.Location.ToString());

            response = await _client.GetAsync(response.Headers.Location);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}