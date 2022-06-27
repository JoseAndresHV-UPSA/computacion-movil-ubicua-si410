using Azure.Messaging.ServiceBus;
using AzureCS_API.models;

namespace AzureCS_API.Services
{
    public static class QueueService
    {
        public static async Task<bool> SendMessage(Message data)
        {
            try
            {
                await using (ServiceBusClient client = new ServiceBusClient(Secret.busConnString))
                {
                    ServiceBusSender sender = client.CreateSender(Secret.queueName);
                    ServiceBusMessage message = new ServiceBusMessage(data.MessageSentiment);
                    await sender.SendMessageAsync(message);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

    }
}
