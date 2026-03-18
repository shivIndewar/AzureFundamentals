using AzureFunctionWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace AzureFunctionTangyWeb.Controllers
{
    public class GroceryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GroceryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        string MasterKey = "dummy master key";
        string GroceryAPIUrl = "azure function api url";
        // GET: GroceryController
        public async Task<ActionResult> Index()
        {
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(GroceryAPIUrl);
            HttpResponseMessage response = await client.GetAsync(GroceryAPIUrl + "?code=" + MasterKey);
            string returnValue = response.Content.ReadAsStringAsync().Result;
            List<GroceryItem> groceryListToReturn = JsonConvert.DeserializeObject<List<GroceryItem>>(returnValue);
            return View(groceryListToReturn);
        }


        // GET: GroceryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GroceryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GroceryItem obj)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(obj);
                using (var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"))
                {
                    using var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri(GroceryAPIUrl);
                    HttpResponseMessage response = await client.PostAsync(GroceryAPIUrl + MasterKey, content);
                    string returnValue = response.Content.ReadAsStringAsync().Result;
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GroceryController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(GroceryAPIUrl);
            HttpResponseMessage response = await client.GetAsync(GroceryAPIUrl + "/Edit/" + id + MasterKey);
            string returnValue = response.Content.ReadAsStringAsync().Result;
            GroceryItem groceryItem = JsonConvert.DeserializeObject<GroceryItem>(returnValue);
            return View(groceryItem);
        }

        // POST: GroceryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GroceryItem obj)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(obj);
                using (var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"))
                {
                    using var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri(GroceryAPIUrl);
                    HttpResponseMessage response = await client.PutAsync(GroceryAPIUrl + "/Update/" + obj.Id + MasterKey, content);
                    string returnValue = response.Content.ReadAsStringAsync().Result;
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GroceryController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(GroceryAPIUrl);
            HttpResponseMessage response = await client.GetAsync(GroceryAPIUrl + "/Edit/" + id + MasterKey);
            string returnValue = response.Content.ReadAsStringAsync().Result;
            GroceryItem groceryItem = JsonConvert.DeserializeObject<GroceryItem>(returnValue);
            return View(groceryItem);
        }

        // POST: GroceryController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePOST(string id)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(GroceryAPIUrl);
                HttpResponseMessage response = await client.DeleteAsync(GroceryAPIUrl + "/Delete/" + id + MasterKey);
                string returnValue = response.Content.ReadAsStringAsync().Result;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}