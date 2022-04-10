using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FunctionGetById
{
    public static class Function
    {
        [FunctionName("Function")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous, "get",
                Route = "products/{id}")]
                HttpRequest req,

            [CosmosDB(
                databaseName: "dbdistribuida",
                collectionName: "products",
                ConnectionStringSetting = "CosmosDbConnection",
                SqlQuery = "select * from products p where p.id = {id}")]
                IEnumerable<Product> productsDocument,

            ILogger log)
        {
            Product product = null;
            var enumerator = productsDocument.GetEnumerator();
            if (enumerator.MoveNext())
            {
                product = enumerator.Current;
            }

            var response = new Response<Product>
            {
                Data = product,
                Success = true,
                Method = "GETBYID"
            };

            if (product == null)
            {
                response.Success = false;
            }
            
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
