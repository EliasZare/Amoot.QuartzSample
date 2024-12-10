using Microsoft.Extensions.Logging;

namespace Quartz.Application.Jobs;

public class FileCreatorJob(ILogger<FileCreatorJob> _logger) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        Directory.CreateDirectory(folderPath);
        var filePath = Path.Combine(folderPath, $"JobFiles_{DateTime.Now.ToString("yyyy-MMMM-dd-HH-m")}.txt");
        try
        {
            File.WriteAllText(filePath, string.Empty);
            _logger.LogInformation("File Created!");
        }
        catch (Exception e)

        {
            if (e != null) _logger.LogError(e.Message);
            throw;
        }

        return Task.CompletedTask;
    }
}

