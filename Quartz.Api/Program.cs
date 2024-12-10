using Quartz;
using Quartz.Application.Jobs;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddQuartz(q =>
{
    var fileCreatorJob = new JobKey("CreateFileJob");
    q.AddJob<FileCreatorJob>(opts => opts.WithIdentity(fileCreatorJob));
    q.AddTrigger(opts => opts
        .ForJob(fileCreatorJob)
        .WithIdentity("CreateFileJob-trigger")
        .WithCronSchedule("0 * * ? * *"));

    var fileWriterJob = new JobKey("FileWriterJob");
    q.AddJob<FileWriterJob>(opts => opts.WithIdentity(fileWriterJob));
    q.AddTrigger(opts => opts
        .ForJob(fileWriterJob)
        .WithIdentity("FileWriterJob-trigger")
        .WithCronSchedule("0 */2 * * * ?"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("wwwroot/logs/Error/e.txt", LogEventLevel.Error)
    .WriteTo.File("wwwroot/logs/Info/i.txt", LogEventLevel.Information)
    .CreateLogger();

builder.Logging.AddSerilog();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
