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

namespace FncProductor
{
    public static class Function1
    {
        [FunctionName("SendMessage")]
        [return: ServiceBus("queue", EntityType.Queue)]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SendMessage function requested");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            return requestBody;
        }
    }
}
