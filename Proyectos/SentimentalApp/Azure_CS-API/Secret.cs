namespace AzureCS_API
{
    public class Secret
    {
        public static string apiEndpoint = "https://mood-recognition.cognitiveservices.azure.com/";
        public static string secretEndpoint = "b5fe18dbb19f454b909a3c7df90094c2";
        // COSMOSDB
        public static string dbName = "cognitive_services";
        public static string containerName = "messages";
        public static string uri = "https://dbjoso.documents.azure.com:443/";
        public static string primaryKey = "SCheqckSxI1aUifLrGc76V6yrNCdVwgxL8DoZlxKKUrnDWrEKgAAbRH9OIyBU2WWCI5YgizO5BvuSGjDMy5M0A==";
        //QUEUE
        public static string busConnString = "Endpoint=sb://busjoso.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=bbDa4H5RtuzVqy1G3w+xhnmFEtUWwyo1VkvoezzXoq8=";
        public static string queueName = "iot-queue";
    }
}
