using Microsoft.EntityFrameworkCore;
using Serilog;
using task;
using task.Options;
using task.Persistence;
using task.Services;
using task.Sources;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithUtcTime()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console(outputTemplate: "[{UtcTimestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services
       .AddOptions<SchedulerOptions>()
       .Bind(builder.Configuration.GetSection("Scheduler"))
       .Validate(o => !string.IsNullOrWhiteSpace(o.Cron), "Cron must be specified")
       .Validate(o => !string.IsNullOrWhiteSpace(o.TimeZone), "TimeZone must be specified")
       .ValidateOnStart();

builder.Services
       .AddOptions<TerminalSourceOptions>()
       .Bind(builder.Configuration.GetSection("TerminalSource"))
       .Validate(o => !string.IsNullOrWhiteSpace(o.FilePath))
       .ValidateOnStart();


builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<ITerminalSource, FileTerminalSource>();
builder.Services.AddScoped<ITerminalImportService, TerminalImportService>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();

var host = builder.Build();
host.Run();