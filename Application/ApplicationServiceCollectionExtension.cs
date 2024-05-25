using Application.Repositories;
using Application.Repositories.CartItemStorage;
using Application.Repositories.CartStorage;
using Application.Repositories.OrderItemStorage;
using Application.Repositories.ProductStorage;
using Application.Repositories.UserStorage;
using Application.Services.CartItemServices;
using Application.Services.CartServices;
using Application.Services.JwtServices;
using Application.Services.OrderItemServices;
using Application.Services.OrderServices;
using Application.Services.ProductServices;
using Application.Services.UserServices;
using AspNetCoreRateLimit;
using DatabaseAccess;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Application
{
    public static class ApplicationServiceCollectionExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = (int)HttpStatusCode.TooManyRequests;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint ="*",
                        Period = "20s",
                        Limit = 3
                    }
                };
            });

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();


            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<ICartItemRepository, CartItemRepository>();
            services.AddTransient<ICartItemService, CartItemService>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IOrderItemService, OrderItemService>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            services.AddSingleton<IJwtService, JwtService>();

            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

            
            return services;
        }
    }
}

