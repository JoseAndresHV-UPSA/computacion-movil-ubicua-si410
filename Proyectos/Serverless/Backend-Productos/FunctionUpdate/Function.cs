using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using System.Linq;

namespace FunctionUpdate
{
    public static class Function
    {
        [FunctionName("Function")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous, "put",
                Route = "products/{id}")]
                HttpRequest req,
                string id,    

            [CosmosDB(
                ConnectionStringSetting = "CosmosDbConnection")]
                DocumentClient client,

            ILogger log)
        {
            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(reqBody);

            var option = new FeedOptions { EnableCrossPartitionQuery = true };
            var colUri = UriFactory.CreateDocumentCollectionUri("dbdistribuida", "products");

            var document = client.CreateDocumentQuery(colUri, option)
                            .Where(x => x.Id == id).AsEnumerable().FirstOrDefault();

            var response = new Response<Product>
            {
                Data = product,
                Success = false,
                Method = "PUT",
            };

            if (document != null)
            {
                document.SetPropertyValue("id", product.Id);
                document.SetPropertyValue("name", product.Name);
                document.SetPropertyValue("category", product.Category);
                document.SetPropertyValue("description", product.Description);
                document.SetPropertyValue("imageUrl", product.ImageUrl);
                document.SetPropertyValue("price", product.Price);
                document.SetPropertyValue("stock", product.Stock);

                response.Success = true;
                await client.ReplaceDocumentAsync(document);
            }

            return new OkObjectResult(response);
        }
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
