using DigitalBanking.WebAPI;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Asp.Versioning;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    Log.Information("Starting Digital Banking Api");
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
        configuration.ReadFrom.Services(services);
        configuration.Enrich.FromLogContext();
    });

    builder.Services.AddControllers();
    builder.Services.ConfigureApiDI(builder.Configuration);

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) => {
            var result = new
            {
                Status = report.Status,
                TotalDuration = report.TotalDuration,
                Checks = report.Entries.Select(entry => new
                {
                    Name = entry.Key,
                    Status = entry.Value.Status,
                    Duration = entry.Value.Duration,
                    Description = entry.Value.Description
                })
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
