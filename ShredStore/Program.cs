using Application;
using AspNetCoreRateLimit;
using Movies.Api.Mapping;
using Serilog;
using ShredStore.Jwt;

var builder = WebApplication.CreateBuilder(args);

string CorsPolicy = "AllowAngularApp";
string logFilePath = "logs/apilog-.txt";
string allowedHosts = "Cors:AllowedHosts";

var config = builder.Configuration;
var hosts = config.GetSection(allowedHosts).Get<string[]>()!;

builder.Services.AddJwtAuthorization(config);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

builder.Services.AddCors(o => o.AddPolicy(CorsPolicy,
                      policy =>
                      {
                          policy.WithOrigins(hosts)
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                      }));


var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

builder.Host.UseSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors(CorsPolicy);

app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

app.Run();
