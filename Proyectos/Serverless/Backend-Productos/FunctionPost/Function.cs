using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionPost
{
    public static class Function
    {
        [FunctionName("Function")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous, "post",
                Route = "products/")]
                HttpRequest req,

            [CosmosDB(
                databaseName: "dbdistribuida",
                collectionName: "products",
                ConnectionStringSetting = "CosmosDbConnection")]
                IAsyncCollector<Product> productDocuments,

            ILogger log)
        {
            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(reqBody);
            await productDocuments.AddAsync(product);

            var response = new Response<Product>
            {
                Data = product,
                Success = true,
                Method = "POST",
            };

            return new OkObjectResult(response);
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

        public class Response<T>
        {
            [JsonProperty("data")]
            public T Data { get; set; }
            [JsonProperty("success")]
            public bool Success { get; set; }
            [JsonProperty("method")]
            public string Method { get; set; }
        }
    }
}
