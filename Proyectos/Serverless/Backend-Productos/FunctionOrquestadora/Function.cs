using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace FunctionOrquestadora
{
    public static class Function
    {
        private static HttpClient httpClient = new HttpClient();
        [FunctionName("Orquestador")]
        [return: ServiceBus("salida-products", EntityType.Queue)]
        public static async Task<string> RunAsync(
            [ServiceBusTrigger("entrada-products", 
                Connection = "AzureWebJobsServiceBus")]
                string myQueueItem, 
            
            ILogger log)
        {
            var request = JsonConvert.DeserializeObject<Request>(myQueueItem);
            string message = string.Empty;

            if (request.Method == "GET" && request.Id == null)
            {
                var response = await httpClient.GetAsync("https://functiongetproducts.azurewebsites.net/api/products");
                message = await response.Content.ReadAsStringAsync();
            }
            if (request.Method == "GET" && request.Id != null)
            {
                var response = await httpClient.GetAsync("https://functiongetbyidproducts.azurewebsites.net/api/products/" + request.Id);
                message = await response.Content.ReadAsStringAsync();
            }
            if (request.Method == "POST")
            {
                var response = await httpClient.PostAsJsonAsync("https://functionpostproducts.azurewebsites.net/api/products", request.Body);
                message = await response.Content.ReadAsStringAsync();
            }
            if (request.Method == "PUT")
            {
                var response = await httpClient.PutAsJsonAsync("https://functionupdateproducts.azurewebsites.net/api/products/" + request.Id, request.Body);
                message = await response.Content.ReadAsStringAsync();
            }
            if (request.Method == "DELETE" && request.Id != null)
            {
                var response = await httpClient.DeleteAsync("https://functiondeleteproducts.azurewebsites.net/api/products/" + request.Id);
                message = await response.Content.ReadAsStringAsync();
            }

            return message;
        }
    }

    public class Request
    {
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("body")]
        public Product Body { get; set; }
    }

    public class Product
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("price")]
        public double Price { get; set; }
        [JsonProperty("stock")]
        public int Stock { get; set; }
    }
}
