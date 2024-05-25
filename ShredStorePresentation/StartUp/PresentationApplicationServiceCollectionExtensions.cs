using Serilog;
using ShredStorePresentation.Extensions.Cache;
using ShredStorePresentation.Services.CartItemServices;
using ShredStorePresentation.Services.CartServices;
using ShredStorePresentation.Services.Images;
using ShredStorePresentation.Services.JtwService;
using ShredStorePresentation.Services.JtwServices;
using ShredStorePresentation.Services.ProductServices;
using ShredStorePresentation.Services.UserService;

namespace ShredStorePresentation.StartUp
{
    public static class PresentationApplicationServiceCollectionExtensions
    {
        private const string RedisCnn = "Redis";
        private const string RedisInstanceName = "ShredStore_";
        private const string LogFilePath = "logs/log.txt";
        public static IServiceCollection RegisterServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddTransient<IUserHttpService, UserHttpService>();
            services.AddTransient<IProductHttpService, ProductHttpService>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddTransient<ICartHttpService, CartHttpService>();
            services.AddTransient<ICartItemHttpService, CartItemHttpService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IJwtHttpService, JwtHttpService>();
            services.AddTransient<IJwtGenerationService, JwtGenerationService>();
            services.AddSingleton<CacheRecordKeys>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString(RedisCnn);
                options.InstanceName = RedisInstanceName;
            });

            var logger = new LoggerConfiguration()
                .WriteTo.Console().WriteTo.File(LogFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog(logger);

            return services;

        }
    }
}
