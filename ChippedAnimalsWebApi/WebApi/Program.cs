using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Services.Dtos.Mapping;
using Serilog;
using WebApi.Middleware;
using WebApi.Authentication.Extensions;
using FluentValidation;
using System.Reflection;
using FluentValidation.AspNetCore;
using Destructurama;
using Services.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

AddLogger(builder);

AddDbContext(builder);

AddMiddleware(builder);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddAuthentication("Basic").AddBasic();
builder.Services.AddAuthorization();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(AddSwaggerBasicAuthentication);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await MigrateDbDevelopmentAsync(app);
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    await MigrateDbProductionAsync(app);
}

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandling();
app.UseHttpContextLogging();
app.UseHttpLogging();

app.MapControllers();

app.Run();

static void AddLogger(WebApplicationBuilder builder)
{
    builder.Services.AddHttpContextAccessor();
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Destructure.UsingAttributes()
        .CreateLogger());
}

static void AddDbContext(WebApplicationBuilder builder)
{
    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ChippedAnimalsDbContext>(
        options => options.UseNpgsql(connectionString));
}

static async Task MigrateDbProductionAsync(WebApplication application)
{
    using (IServiceScope scope = application.Services.CreateScope())
    {
        using (ChippedAnimalsDbContext context = scope.ServiceProvider
            .GetRequiredService<ChippedAnimalsDbContext>())
        {
            bool canConnect;
            do
            {
                canConnect = await context.Database.CanConnectAsync();
                if (canConnect)
                {
                    await context.Database.MigrateAsync();
                }
            }
            while (!canConnect);
        }
    }
}

static async Task MigrateDbDevelopmentAsync(WebApplication application)
{
    using (IServiceScope scope = application.Services.CreateScope())
    {
        using (ChippedAnimalsDbContext context = scope.ServiceProvider
            .GetRequiredService<ChippedAnimalsDbContext>())
        {
            await context.Database.MigrateAsync();
        }
    }
}

static void AddMiddleware(WebApplicationBuilder builder)
{
    builder.Services.AddExceptionHandlingMiddleware();
    builder.Services.AddHttpContextLoggingMiddleware();
}

static void AddSwaggerBasicAuthentication(SwaggerGenOptions options)
{
    options.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Basic",
        In = ParameterLocation.Header
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Basic",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new string[0]
        }
    });
}