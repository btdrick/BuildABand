using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace BuildABand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public AudioController(IConfiguration configuration)
        {
            // Retrieve the connection string for use with the application. The storage
            // connection string is stored in an environment variable on the machine
            // running the application called AZURE_STORAGE_CONNECTION_STRING. If the
            // environment variable is created after the application is launched in a
            // console or with Visual Studio, the shell or application needs to be closed
            // and reloaded to take the environment variable into account.
            connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            _configuration = configuration;
        }

        //todo: put audio files in backend Audio folder
        // used data annotations: https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-6.0
        [HttpPost]
        [Consumes("application/octet-stream")]
        public async Task postBlobAsync([FromQuery][Required][MinLength(3)] string filename)
        {
            //var guid = Guid.NewGuid();

            //await MusicDAL.addUserFileNameToAzureFileNameMapping(guid, userFileName);

            //add to sql table
            await uploadBlobAsync(filename, Request.Body);
        }

        //todo: upload blob to azure
        public async Task uploadBlobAsync(string azureFileName, Stream content)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("audio");
            BlobClient blobClient = containerClient.GetBlobClient(azureFileName);
            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            await blobClient.UploadAsync(content, overwrite: true);
        }

        [HttpGet("info")]
        public JsonResult getFileInfo([FromQuery] int memberID)
        {
            return null;
        }

        [HttpGet("blob")]
        public async Task<FileStreamResult> getBlob([FromQuery] string azureFileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("audio");
            BlobClient blobClient = containerClient.GetBlobClient(azureFileName);

            var result = await blobClient.DownloadStreamingAsync();
            return new FileStreamResult(result.Value.Content, "application/octet-stream"); // assuming wave format
        }
    }
}
