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
using Microsoft.Azure.Documents;

namespace FunctionDelete
{
    public static class Function
    {
        [FunctionName("Function")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous, "delete",
                Route = "products/{id}")]
                HttpRequest req,
                string id,

            [CosmosDB(
                databaseName: "dbdistribuida",
                collectionName: "products",
                ConnectionStringSetting = "CosmosDbConnection")]
                DocumentClient client,

            ILogger log)
        {
            await client.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri("dbdistribuida", "products", id),
                new RequestOptions() { PartitionKey = new PartitionKey(id) });

            var response = new Response<string>
            {
                Data = id,
                Success = true,
                Method = "DELETE"
            };

            return new OkObjectResult(response);
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
