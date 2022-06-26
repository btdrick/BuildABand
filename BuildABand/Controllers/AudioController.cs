using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace BuildABand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AudioController(IConfiguration configuration)
        {
            // Retrieve the connection string for use with the application. The storage
            // connection string is stored in an environment variable on the machine
            // running the application called AZURE_STORAGE_CONNECTION_STRING. If the
            // environment variable is created after the application is launched in a
            // console or with Visual Studio, the shell or application needs to be closed
            // and reloaded to take the environment variable into account.
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            _configuration = configuration;
        }

        //todo: put audio files in backend Audio folder
        [HttpPost("blob")]
        public void postBlob()
        {

        }

        //todo: upload blob to azure
        public void uploadBlob()
        {
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
        }

        //todo: get blob from azure
        [HttpGet("blob")]
        public JsonResult getBlob()
        {
            return null;
        }
    }
}
