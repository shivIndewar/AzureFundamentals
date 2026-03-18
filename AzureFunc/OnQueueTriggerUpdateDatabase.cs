using System;
using Azure.Storage.Queues.Models;
using AzureFunc.Data;
using AzureFunc.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunc;

public class OnQueueTriggerUpdateDatabase
{
    private readonly ILogger<OnQueueTriggerUpdateDatabase> _logger;
    private readonly DBContext _dBContext;

    public OnQueueTriggerUpdateDatabase(ILogger<OnQueueTriggerUpdateDatabase> logger, DBContext dBContext)
    {
        _logger = logger;
        _dBContext = dBContext;
    }

    [Function(nameof(OnQueueTriggerUpdateDatabase))]
    public void Run([QueueTrigger("SalesRequestInBound")] QueueMessage message)
    {
        string messageBody = message.Body.ToString();
        Salesrequest salesrequest = JsonConvert.DeserializeObject<Salesrequest>(messageBody);

        if (salesrequest != null)
        {
            salesrequest.Status = "";
            _dBContext.salesrequest.Add(salesrequest);
            _dBContext.SaveChanges();
        }
        else
        {
            _logger.LogInformation("Failed to desirialize the data into a salesrequest object");
        }

        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
    }
}