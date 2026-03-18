using AzureFunc.Data;
using AzureFunc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunc;

public class GroceryAPI
{
    private readonly ILogger<GroceryAPI> _logger;
    private readonly DBContext _dbContext;
    public GroceryAPI(ILogger<GroceryAPI> logger, DBContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Function("GetGrocery")]
    public IActionResult GetGrocery([HttpTrigger(AuthorizationLevel.Function, "get", Route ="GroceryList")] HttpRequest req)
    {
        _logger.LogInformation("Getting grocery items");
        return new OkObjectResult(_dbContext.GroceryItems.ToList());
    }


    [Function("CreateGrocery")]
    public async Task<IActionResult> CreateGrocery([HttpTrigger(AuthorizationLevel.Function, "post", Route = "GroceryList")] HttpRequest req)
    {
        _logger.LogInformation("Creating grocery item");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        GroceryItems_Upsert ? data = JsonConvert.DeserializeObject<GroceryItems_Upsert>(requestBody);

        GroceryItem groceryItem = new GroceryItem() {
            Name = data.Name
        };

        try
        {
            await _dbContext.GroceryItems.AddAsync(groceryItem);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw;
        }

        return new OkObjectResult(_dbContext.GroceryItems.ToList());
    }

    [Function("Update")]
    public async Task<IActionResult> Update([HttpTrigger(AuthorizationLevel.Function, "put", Route = "GroceryList/Update/{id}")] HttpRequest req, string id)
    {
        _logger.LogInformation("Creating grocery item");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        GroceryItems_Upsert? data = JsonConvert.DeserializeObject<GroceryItems_Upsert>(requestBody);

        var groceryIetm = _dbContext.GroceryItems.FirstOrDefault(x => x.Id == id);

        if (groceryIetm != null)
        {
            groceryIetm.Name = data.Name;
        }

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw;
        }

        return new OkObjectResult(_dbContext.GroceryItems.ToList());
    }

    [Function("Get")]
    public IActionResult Get(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList/Edit/{id}")] HttpRequest req,
    string id)
    {
        _logger.LogInformation($"Getting grocery item with id: {id}");

        var groceryItem = _dbContext.GroceryItems.FirstOrDefault(x => x.Id == id);

        if (groceryItem == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(groceryItem);
    }


    [Function("Delete")]
    public async Task<IActionResult> Delete(
    [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "GroceryList/Delete/{id}")] HttpRequest req,
    string id)
    {
        _logger.LogInformation($"Getting grocery item with id: {id}");



        var groceryItem = _dbContext.GroceryItems.FirstOrDefault(x => x.Id == id);
         _dbContext.GroceryItems.Remove(groceryItem);
        await _dbContext.SaveChangesAsync();    

        if (groceryItem == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(groceryItem);
    }

}