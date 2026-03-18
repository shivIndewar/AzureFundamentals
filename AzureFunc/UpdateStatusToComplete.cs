using System;
using AzureFunc.Data;
using AzureFunc.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunc;

public class UpdateStatusToComplete
{
    private readonly ILogger _logger;
    private readonly DBContext _dbContext;
    public UpdateStatusToComplete(ILoggerFactory loggerFactory, DBContext dbContext)
    {
        _logger = loggerFactory.CreateLogger<UpdateStatusToComplete>();
        _dbContext = dbContext;
    }

    [Function("UpdateStatusToComplete")]
    public void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);

        IEnumerable<Salesrequest> salesRequests = _dbContext.salesrequest.Where(u => u.Status == "Image Processed").ToList();

        foreach (var salesRequest in salesRequests)
        {
            salesRequest.Status = "Completed";
        }

        _dbContext.SaveChanges();

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}