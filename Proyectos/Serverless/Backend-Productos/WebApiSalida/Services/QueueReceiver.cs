using Azure.Messaging.ServiceBus;

namespace WebApiSalida.Services
{
    public class QueueReceiver
    {
        static string connectionString = "Endpoint=sb://busjoso.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=bbDa4H5RtuzVqy1G3w+xhnmFEtUWwyo1VkvoezzXoq8=";
        static string queueName = "salida-products";
        static ServiceBusClient? client;
        static ServiceBusProcessor? processor;
        static string? msg = string.Empty;

        public static async Task<string> Receive()
        {
            // Create the client object that will be used to create sender and receiver objects
            client = new ServiceBusClient(connectionString);
            // create a processor that we can use to process the messages
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += async (ProcessMessageEventArgs args) =>
                {
                    string body = args.Message.Body.ToString();
                    Console.WriteLine($"Received: {body}");

                    if (!string.IsNullOrEmpty(body))
                    {
                        msg = body;
                    }

                    // complete the message. messages is deleted from the queue. 
                    await args.CompleteMessageAsync(args.Message);
                };

                // add handler to process any errors
                processor.ProcessErrorAsync += (ProcessErrorEventArgs args) =>
                {
                    //Console.WriteLine(args.Exception.ToString());
                    return Task.CompletedTask;
                };

                // start processing 
                await processor.StartProcessingAsync();

                //Console.ReadKey();
                await Task.Delay(3000);

                // stop processing 
                await processor.StopProcessingAsync();

                return msg;
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
