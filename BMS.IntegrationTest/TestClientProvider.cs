using BMS.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace BMS.IntegrationTest
{
    public class TestClientProvider : IDisposable
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {
            var projectDir = Directory.GetCurrentDirectory();

            _server = new TestServer(new WebHostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile(projectDir + "\\..\\..\\..\\..\\BMS.WebAPI\\appsettings.json", optional: true, reloadOnChange: true);
            }).UseStartup<Startup>());

            Client = _server.CreateClient();
        }

        public void Dispose()
        {
            _server?.Dispose();

            Client?.Dispose();
        }
    }
}
