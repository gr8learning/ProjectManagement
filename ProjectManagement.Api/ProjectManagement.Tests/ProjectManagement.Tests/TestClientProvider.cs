using Microsoft.AspNetCore.Mvc.Testing;
using ProjectManagement.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using RESTFulSense.Clients;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjectManagement.Tests
{
    public class TestClientProvider : IDisposable
    {
        public readonly WebApplicationFactory<Startup> webApplicationFactory;
        public readonly HttpClient baseClient;
        public readonly IRESTFulApiFactoryClient apiFactoryClient;
        public TestServer _testServer;
        public TestClientProvider()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
            this.baseClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.baseClient);
            this._testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        public async Task<HttpStatusCode> RequestWithMethod(string uri, StringContent stringContent, string methodType)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            switch (methodType)
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

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}
