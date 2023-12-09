using Cnab.Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Cnab.Application.Jobs;
public class ProccessCnabItemsJob : IJob
{
    private readonly IProcessCnabServices _processCnabServices;
    private readonly ILogger<ProccessCnabItemsJob> _logger;
    public ProccessCnabItemsJob(IProcessCnabServices processCnabServices, ILogger<ProccessCnabItemsJob> logger)
    {
        _processCnabServices = processCnabServices;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Started ProccessCnabItemsJob - {datetime}", DateTime.Now);

        await _processCnabServices.Execute();

        _logger.LogInformation("Finished ProccessCnabItemsJob - {datetime}", DateTime.Now);
    }
}
