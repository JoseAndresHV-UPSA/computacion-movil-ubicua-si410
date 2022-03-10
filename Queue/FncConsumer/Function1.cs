using System;
using System.Threading.Tasks;
using Biblioteca;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FncConsumer
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task Run(
            [ServiceBusTrigger(
                "queue", 
                Connection = "ConnString")] 
            string myQueueItem,

            [CosmosDB(
                databaseName: "dbdistribuida",
                collectionName: "tabloide",
                ConnectionStringSetting = "CosmosDbConnectionString")] 
            IAsyncCollector<dynamic> datos,

            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            dynamic data = JsonConvert.DeserializeObject<Tabloide>(myQueueItem);
         
            await datos.AddAsync(data);
        }
    }
}
