using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagement.Tests
{
    public class ProjectTests: IClassFixture<TestClientProvider>
    {
        private TestServer _testServer;
        delegate Task<HttpStatusCode> RequestWithMethod(string uri, StringContent stringContent, string methodType);
        RequestWithMethod _requestWithMethod;
        public ProjectTests(TestClientProvider testClientProvider)
        {
            _testServer = testClientProvider._testServer;
            _requestWithMethod = new RequestWithMethod(testClientProvider.RequestWithMethod);
        }

        [Trait("Collection", "Project")]
        [Fact]
        public async void getTaskTest()
        {
            var response = await _testServer.CreateRequest("/api/Project").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/Project/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/Project/3").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Trait("Collection", "Project")]
        [Fact]
        public async void addTaskTest()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, dynamic>
            {
                { "Name", "P1"},
                { "Detail", "P1-Detail"}
            }), Encoding.Default, "application/json");
            var statusCode = await _requestWithMethod("/api/Project", content, "POST");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var response = await _testServer.CreateRequest("/api/Project/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Trait("Collection", "Project")]
        [Fact]
        public async void updateTaskTest()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, dynamic>
            {
                { "Name", "P1"},
                { "Detail", "P1-Detail"},
                { "id", 1 }
            }), Encoding.Default, "application/json");
            var statusCode = await _requestWithMethod("/api/Project", content, "PUT");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var response = await _testServer.CreateRequest("/api/Project/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Trait("Collection", "Project")]
        [Fact]
        public async void deleteTaskTest()
        {
            var response = await _testServer.CreateRequest("/api/Project/2").SendAsync("DELETE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/Project").SendAsync("DELETE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
