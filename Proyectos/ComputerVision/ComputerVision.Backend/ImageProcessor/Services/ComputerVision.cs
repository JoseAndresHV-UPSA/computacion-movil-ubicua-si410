using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProcessor.Services
{
    public static class ComputerVision
    {
        static string subscriptionKey = "4396f2335ddc422cacc63590e5f0e189";
        static string endpoint = "https://cv-ocr-card.cognitiveservices.azure.com/";

        public static List<string> OCR(Stream image)
        {
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            return ReadFile(client, image).Result;
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public static async Task<List<string>> ReadFile(ComputerVisionClient client, Stream file)
        {
            var textHeaders = await client.ReadInStreamAsync(file);
            string operationLocation = textHeaders.OperationLocation;
            Thread.Sleep(2000);

            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
            ReadOperationResult results;

            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            var textUrlFileResults = results.AnalyzeResult.ReadResults;

            List<string> textList = new();
            foreach (ReadResult page in textUrlFileResults)
            {
                foreach (Line line in page.Lines)
                {
                    textList.Add(line.Text);
                }
            }

            return textList;
        }
    }
}
