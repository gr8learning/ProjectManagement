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
    public class TaskTests: IClassFixture<TestClientProvider>
    {
        private TestServer _testServer;
        delegate Task<HttpStatusCode> RequestWithMethod(string uri, StringContent stringContent, string methodType);
        RequestWithMethod _requestWithMethod;
        public TaskTests(TestClientProvider testClientProvider)
        {
            _testServer = testClientProvider._testServer;
            _requestWithMethod = new RequestWithMethod(testClientProvider.RequestWithMethod);
        }

        [Trait("Collection", "Task")]
        [Fact]
        public async void getTaskTest()
        {
            var response = await _testServer.CreateRequest("/api/Task").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/Task/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/Task/3").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Trait("Collection", "Task")]
        [Fact]
        public async void addTaskTest()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, dynamic>
            {
                { "ProjectID", 4},
                { "Detail", "Task-1"},
                { "Status", "New" },
                { "AssignedToUserId", 1 }
            }), Encoding.Default, "application/json");
            var statusCode = await _requestWithMethod("/api/Task", content, "POST");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var response = await _testServer.CreateRequest("/api/Task/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Trait("Collection", "Task")]
        [Fact]
        public async void updateTaskTest()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, dynamic>
            {
                { "ProjectID", 4},
                { "Detail", "Task-1"},
                { "Status", "New" },
                { "AssignedToUserId", 1 },
                { "id", 1 }
            }), Encoding.Default, "application/json");
            var statusCode = await _requestWithMethod("/api/Task", content, "PUT");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var response = await _testServer.CreateRequest("/api/Task/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Trait("Collection", "Task")]
        [Fact]
        public async void deleteTaskTest()
        {
            var response = await _testServer.CreateRequest("/api/Task/2").SendAsync("DELETE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/Task").SendAsync("DELETE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
