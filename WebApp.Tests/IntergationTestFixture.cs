using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApp.Tests
{
    /// <summary>
    /// A test fixture which hosts the target project (project we wish to test) in an in-memory server.
    /// </summary>
    public class IntegrationTestFixture<TStartup> : IDisposable
    {
        private const string SolutionName = "EFCoreInMemorySample.sln";

        // real Startup for project, containing Controllers and json configs
        private static readonly Type StartupType = typeof(WebApp.Startup);

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly TestServer _server;

        protected readonly DbContextOptions options;

        public IntegrationTestFixture()
            : this(Path.Combine(""))
        {
            options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(nameof(MyContext))
                .Options;
        }

        protected IntegrationTestFixture(string solutionRelativeTargetProjectParentDir)
        {
            var startupAssembly = StartupType.Assembly;
            var contentRoot = GetProjectPath(solutionRelativeTargetProjectParentDir, startupAssembly);

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot) // for json configs
                .ConfigureServices(OverrideServices)
                .UseEnvironment("Development")
                .UseStartup(typeof(TStartup))
                ;

            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost");
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }

        /// <summary>
        /// Allows to use mocks of services. Services must be added by TryAddScoped to use mocks.
        /// </summary>
        protected virtual void InitializeServices(IServiceCollection services)
        {
        }

        protected HttpRequestMessage GetRequest(string path, HttpResponseMessage response)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                var cookie = SetCookieHeaderValue.ParseList(values.ToList()).First();
                request.Headers.Add("Cookie", new CookieHeaderValue(cookie.Name, cookie.Value).ToString());
            }

            return request;
        }

        protected HttpRequestMessage GetRequest(string path, object obj, HttpResponseMessage response)
        {
            var queryStr = string.Join("&", obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead)
                .Select(x => $"{x.Name}={x.GetValue(obj)}"));

            var request = new HttpRequestMessage(HttpMethod.Get, $"{path}?{queryStr}");
            request.Content = MakeJsonContent(obj);
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                var cookie = SetCookieHeaderValue.ParseList(values.ToList()).First();
                request.Headers.Add("Cookie", new CookieHeaderValue(cookie.Name, cookie.Value).ToString());
            }

            return request;
        }

        protected HttpRequestMessage PostRequest(string path, object obj, HttpResponseMessage response)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Content = MakeJsonContent(obj);
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                var cookie = SetCookieHeaderValue.ParseList(values.ToList()).First();
                request.Headers.Add("Cookie", new CookieHeaderValue(cookie.Name, cookie.Value).ToString());
            }

            return request;
        }

        protected HttpRequestMessage DelRequest(string path, HttpResponseMessage response)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, path);
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                var cookie = SetCookieHeaderValue.ParseList(values.ToList()).First();
                request.Headers.Add("Cookie", new CookieHeaderValue(cookie.Name, cookie.Value).ToString());
            }

            return request;
        }

        protected HttpRequestMessage PutRequest(string path, object obj, HttpResponseMessage response)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, path);
            request.Content = MakeJsonContent(obj);
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                var cookie = SetCookieHeaderValue.ParseList(values.ToList()).First();
                request.Headers.Add("Cookie", new CookieHeaderValue(cookie.Name, cookie.Value).ToString());
            }

            return request;
        }

        private void OverrideServices(IServiceCollection services)
        {
            InitializeServices(services);

            var startupAssembly = StartupType.GetTypeInfo().Assembly;

            // Inject a custom application part manager. Overrides AddMvcCore() because that uses TryAdd().
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(startupAssembly));

            manager.FeatureProviders.Add(new ControllerFeatureProvider());
            manager.FeatureProviders.Add(new ViewComponentFeatureProvider());

            services.AddSingleton(manager);
        }

        /// <summary>
        /// Gets the full path to the target project path that we wish to test
        /// </summary>
        /// <param name="solutionRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = AppContext.BaseDirectory;

            // Find the folder which contains the solution file. We then use this information to find the target
            // project which we want to test.
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, SolutionName));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }

        private static HttpContent MakeJsonContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj, JsonSerializerSettings), System.Text.Encoding.Unicode, "application/json");
        }
    }
}