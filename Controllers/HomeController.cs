using ConsumeInventaory.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace ConsumeInventaory.Controllers
{
    public class HomeController : Controller
    {


        Uri baseAddreess = new Uri("https://localhost:7008/api/inventory/Items");
        HttpClient client;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddreess;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string selectedType = "Electrical")
        {
            List<Items> items = new List<Items>();
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress +"/" + selectedType);
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<List<Items>>(data);

                Console.WriteLine(items.ToString());

            }
            
            return View(items);
        }

        public async Task<IActionResult> Addbill( string customerName, decimal totalAmount)
        {
            var apiUrl = client.BaseAddress + "/add_bill/";

            //HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/AddBills/");
            var body = new
            {
               
                customerName = customerName,
                totalAmount = totalAmount
            };
            var requestJson = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiUrl, httpContent);


            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                // Handle successful response
                return Ok(responseContent);


            }
            else
            {
                return BadRequest();
            }


        }
        [HttpPatch]
        public async Task UpdateQuantity(string updateQuantityRequest)
        {
            List<UpdateQuantity> quantitiesList = JsonConvert.DeserializeObject<List<UpdateQuantity>>(updateQuantityRequest);

            var jsonContent = JsonConvert.SerializeObject(quantitiesList);


            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            var response = await client.PatchAsync(client.BaseAddress , content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content);
                string data = await response.Content.ReadAsStringAsync();


                Console.WriteLine(data);
                //return Ok("good");

            }
            else
            {
                //return Error();
            }


            //var apiUrl = "https://localhost:7008/api/Items/UpdateQuantity";


            //var jsonContent = JsonConvert.SerializeObject(updateQuantities);


            //var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));


            //var response = await client.PatchAsync(apiUrl, content);


            //var apiUrl = client.BaseAddress + "/UpdateQuantity/";
            //var json = JsonConvert.SerializeObject(item);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            //var response = await client.PatchAsync(apiUrl, content);

            //if (response.IsSuccessStatusCode)
            //{
            //    var responseContent = await response.Content.ReadAsStringAsync();
            //    // Handle successful response
            //    return Ok(responseContent);


            //}
            //else
            //{
            //    return BadRequest();
            //}

        }


        public IActionResult Thansks()
        {
            return View("thanks");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}