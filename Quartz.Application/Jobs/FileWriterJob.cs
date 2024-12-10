using Microsoft.Extensions.Logging;

namespace Quartz.Application.Jobs;

public class FileWriterJob(ILogger<FileWriterJob> _logger) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var textFiles = Directory.GetFiles(folderPath, "*.txt");
        try
        {
            foreach (var file in textFiles)
            {
                File.AppendAllText($"{folderPath}/{Path.GetFileName(file)}",
                    $"Hey sir I'm written on {DateTime.Now.ToString("f")}." + Environment.NewLine);
                _logger.LogInformation(
                    $"Hey sir I'm written on {DateTime.Now.ToString("f")}. on {Path.GetFileName(file)}");
            }
        }
        catch (Exception e)
        {
            if (e != null) _logger.LogError(e.Message);
            throw;
        }

        return Task.CompletedTask;
    }
}

