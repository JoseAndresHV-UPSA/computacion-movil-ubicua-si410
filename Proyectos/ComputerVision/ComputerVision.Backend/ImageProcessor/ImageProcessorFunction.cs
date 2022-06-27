using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ImageProcessor.Models;
using System.Collections.Generic;
using ImageProcessor.Services;
using ImageProcessor.Utils;

namespace ImageProcessor
{
    public static class ImageProcessorFunction
    {
        [FunctionName("ImageProcessor")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                "post",
                Route = "image-processor")]
            HttpRequest req,

            [CosmosDB(
                databaseName: "dbdistribuida",
                collectionName: "image-data",
                ConnectionStringSetting = "CosmosDbConn")]
             IEnumerable<ImageInfo> imageDataCollectionGet,

            [CosmosDB(
                databaseName: "dbdistribuida",
                collectionName: "image-data",
                ConnectionStringSetting = "CosmosDbConn")]
            IAsyncCollector<ImageInfo> imageDataCollectionPost,

            ILogger log)
        {
            if (req.Method == "GET")
            {
                Response<IEnumerable<ImageInfo>> response = new()
                {
                    Data = imageDataCollectionGet,
                    Success = true,
                    Message = "Lista de datos extraidos"
                };

                return new OkObjectResult(response); 
            }

            else if (req.Method == "POST")
            {
                var image = req.Form.Files[0].OpenReadStream();
                var extractedText = ComputerVision.OCR(image);
                var imageInfo = new ImageInfo
                {
                    Text = extractedText,
                    Urls = FilterProcess.GetUrls(extractedText),
                    PhoneNumbers = FilterProcess.GetPhoneNumbers(extractedText),
                    Emails = FilterProcess.GetEmails(extractedText),
                };

                await imageDataCollectionPost.AddAsync(imageInfo);

                Response<ImageInfo> response = new()
                {
                    Data = imageInfo,
                    Success = true,
                    Message = "Informacion extraida de la imagen"
                };

                return new OkObjectResult(response);
            }
            
            return new BadRequestResult();
        }
    }
}
