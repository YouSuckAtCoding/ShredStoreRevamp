using Application.Repositories.UserStorage;
using Application.Services.UserServices;
using DatabaseAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShredStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
