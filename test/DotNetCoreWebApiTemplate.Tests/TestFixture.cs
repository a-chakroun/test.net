// -----------------------------------------------------------
// <copyright file="TestFixture.cs" company="Company Name">
// Copyright YEAR COPYRIGHT HOLDER
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall
// be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Test fixture to configure a TestServer for running functional tests.
// </summary>
// -----------------------------------------------------------

namespace DotNetCoreWebApiTemplate.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class TestFixture : IDisposable
    {
        private readonly TestServer testServer;

        public TestFixture()
        {
            var projectPath = GetProjectPath(nameof(DotNetCoreWebApiTemplate));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .AddInMemoryCollection(InitializeAppSettings())
                .Build();

            var builder = new WebHostBuilder()
                .UseConfiguration(configuration)
                .ConfigureServices(InitializeServices)
                .UseStartup<Startup>();

            testServer = new TestServer(builder);

            HttpClient = testServer.CreateClient();
            HttpClient.BaseAddress = new Uri("https://localhost");
        }

        public HttpClient HttpClient { get; }

        public void Dispose()
        {
            HttpClient.Dispose();
            testServer.Dispose();
        }

        /// <summary>
        /// Override to add test app settings.
        /// Settings registered here are added after those in appsettings.json.
        /// Each setting name must include the full name hierarchy. For example
        /// a setting called "ConectionString" in the section "DbConfig" would
        /// have the name "DbConfig:ConnectionString". Just like when overriding
        /// app settings using environment variables.
        /// </summary>
        /// <returns>The application settings to override</returns>
        protected virtual Dictionary<string, string> InitializeAppSettings()
        {
            return null;
        }

        /// <summary>
        /// Override to add test stubs.
        /// Services registered here are added before the services configured in
        /// the Startup class. In order to register an alternative implementation
        /// here for testing, the default implementation registered in the Startup
        /// class must use the TryAdd methods so that they are only added if an
        /// implementation doesn't already exist.
        /// </summary>
        /// <param name="services">The services to override</param>
        protected virtual void InitializeServices(IServiceCollection services)
        {
        }

        private static string GetProjectPath(string projectName)
        {
            // Get currently executing test project path
            var applicationBasePath = AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo?.FullName, "src"));

                if (!projectDirectoryInfo.Exists)
                {
                    continue;
                }

                var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));

                if (projectFileInfo.Exists)
                {
                    return Path.Combine(projectDirectoryInfo.FullName, projectName);
                }
            }
            while (directoryInfo?.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }
    }
}
