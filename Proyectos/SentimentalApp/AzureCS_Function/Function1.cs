using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureCS_Function
{
    public class Function1
    {
        [FunctionName("Function1")]
        public async Task RunAsync([ServiceBusTrigger("iot-queue", Connection = "AzureWebJobsServiceBus")]string myQueueItem, ILogger log)
        {
            string iotConnString = "HostName=joso-hub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Y2xUh/0CaHYeZlEm2zWhtpD3ZP5BbwSULhwFf0+EYso=";
            string targetDevice1 = "joso-rasp";
            string targetDevice2 = "esp8266";

            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(iotConnString);
            string mensaje = myQueueItem;
            log.LogInformation(mensaje);
            var commandMessage = new Message(Encoding.ASCII.GetBytes(mensaje));
            commandMessage.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync(targetDevice1, commandMessage);
            await serviceClient.SendAsync(targetDevice2, commandMessage);
        }
    }
}
