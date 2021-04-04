using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
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
        public UserTests(TestClientProvider testClientProvider)
        {
            _testServer = testClientProvider._testServer;
        }

        public async Task<HttpStatusCode> RequestWithMethod(string uri, StringContent stringContent, string methodType)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            switch(methodType)
            {
                case "PUT": request = new HttpRequestMessage(HttpMethod.Put, uri); break;
                case "DELETE": request = new HttpRequestMessage(HttpMethod.Delete, uri); break;
                default: request = new HttpRequestMessage(HttpMethod.Post, uri); break;
            }
            request.Content = stringContent;

            var client = _testServer.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.SendAsync(request);
            return response.StatusCode;
        }

        [Fact]
        public async void getUserDetailsTest()
        {
            var response = await _testServer.CreateRequest("/api/User").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/User/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/User/3").SendAsync("GET");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void addUserTest()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "FirstName", "Nitin"},
                { "LastName", "Kumar"},
                { "Email", "nitinkumar6912@gmail.com" }
            }), Encoding.Default, "application/json");
            var statusCode = await RequestWithMethod("/api/User", content, "POST");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var response = await _testServer.CreateRequest("/api/User/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

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
            var statusCode = await RequestWithMethod("/api/User", content, "PUT");
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var response = await _testServer.CreateRequest("/api/User/1").SendAsync("GET");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void deleteUserDetailsTest()
        {
            var response = await _testServer.CreateRequest("/api/User/2").SendAsync("DELETE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            response = await _testServer.CreateRequest("/api/User").SendAsync("DELETE");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
