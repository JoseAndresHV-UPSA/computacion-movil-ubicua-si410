using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace FunctionEntrada
{
    public static class Function
    {
        [FunctionName("Function")]
        [return: ServiceBus("entrada-products", ServiceBusEntityType.Queue)]
        public static async Task<string> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous, "get", "post", "put", "delete",
                Route = "products/")]
                HttpRequest req,

            ILogger log)
        {
            string method = req.Method;
            string id = req.Query["id"];
            string body = await new StreamReader(req.Body).ReadToEndAsync();

            var product = JsonConvert.DeserializeObject<Product>(body);

            var request = new Request
            {
                Method = method,
                Id = id,
                Body = product
            };

            string message = JsonConvert.SerializeObject(request);
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
