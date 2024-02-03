using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ShredStore;
using Application;

namespace ShredStoreApiTests
{
    public class ApiFactory : WebApplicationFactory<IApiAssemblyMarker>
    {

        private readonly Action<IServiceCollection> _configure;
        public ApiFactory(Action<IServiceCollection> configure)
        {
            _configure = configure;
        }

        public Action<IServiceCollection> Configure { get; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureTestServices(services =>
            {
                _configure(services);

            });

        }
      
    }

    public static class CreateApi
    {
        public static ApiFactory CreateOfficialApi()
        {
            var api = new ApiFactory(services =>
            {
                services.AddApplication();
            });
            return api;
        }
    }
}
