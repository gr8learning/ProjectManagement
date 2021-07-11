using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagement.Tests
{
    public class UserTests: IClassFixture<TestClientProvider>
    {
        private TestServer _testServer;
        delegate Task<HttpStatusCode> RequestWithMethod(string uri, StringContent stringContent, string methodType);
        RequestWithMethod _requestWithMethod;
        public UserTests(TestClientProvider testClientProvider)
        {
            _testServer = testClientProvider._testServer;
            _requestWithMethod = new RequestWithMethod(testClientProvider.RequestWithMethod);
        }

        [Trait("Collection", "User")]
        [Fact]
        public async void getUserTest()
        {
            var response = await _testServer.CreateRequest("/api/User").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/User/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/User/3").SendAsync("GET");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Trait("Collection", "User")]
        [Fact]
        public async void addUserTest()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "FirstName", "Nitin"},
                { "LastName", "Kumar"},
                { "Email", "nitinkumar6912@gmail.com" }
            }), Encoding.Default, "application/json");
            var statusCode = await _requestWithMethod("/api/User", content, "POST");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var response = await _testServer.CreateRequest("/api/User/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Trait("Collection", "User")]
        [Fact]
        public async void updateUserTest()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, dynamic>
            {
                { "FirstName", "Nitin1"},
                { "LastName", "Kumar1"},
                { "Email", "nitinkumar6912@gmail.com" },
                { "id", 1 }
            }), Encoding.Default, "application/json");
            var statusCode = await _requestWithMethod("/api/User", content, "PUT");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var response = await _testServer.CreateRequest("/api/User/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Trait("Collection", "User")]
        [Fact]
        public async void deleteUserTest()
        {
            var response = await _testServer.CreateRequest("/api/User/2").SendAsync("DELETE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/User").SendAsync("DELETE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
