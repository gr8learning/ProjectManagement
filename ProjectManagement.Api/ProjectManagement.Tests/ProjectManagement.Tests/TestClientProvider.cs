using Microsoft.AspNetCore.Mvc.Testing;
using ProjectManagement.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using RESTFulSense.Clients;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;

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

        public void Dispose()
        {
            _testServer.Dispose();
        }
    }
}
