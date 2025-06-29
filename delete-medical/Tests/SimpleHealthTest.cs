using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace CreateMedical.Tests
{
    public class SimpleHealthTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public SimpleHealthTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Service_Should_Be_Running()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK || 
                       response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}