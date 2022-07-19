﻿using Azure.Storage.Blobs;
using BuildABand.DAL;
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
        private MusicDAL musicDal;

        public AudioController(IConfiguration configuration)
        {
            musicDal = new MusicDAL(configuration);
        }

        [HttpPost]
        [Consumes("application/octet-stream")]
        public async Task<int> postBlobAsync([FromQuery][Required][MinLength(3)] string fileName, [Required] int musicianID)
        {
            var guid = Guid.NewGuid();
            var audioID = musicDal.addUserFileNameToAzureFileNameMapping(guid, fileName, musicianID);
            await uploadBlobAsync(guid.ToString(), Request.Body);
            return audioID;
        }

        //todo: upload blob to azure
        public async Task uploadBlobAsync(string azureFileName, Stream content)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=babstorage;AccountKey=FAwXuMf3l7RjNTZbOhO+kVqIoCQtJQkRttiJFD8H+f326b3JS2DL+YlODgL95C6pbTFZs49J4kCR+AStwiF8Hw==;EndpointSuffix=core.windows.net");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("audio");
            BlobClient blobClient = containerClient.GetBlobClient(azureFileName);
            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            await blobClient.UploadAsync(content, overwrite: true);
        }

        [HttpGet("fileInfo")]
        public JsonResult getFileInfo([FromQuery] int musicianID)
        {
            if (musicianID < 1)
            {
                throw new ArgumentException("Musician ID must be 1 or greater");
            }

            var results = musicDal.getFileInfo(musicianID);
            return results;
        }

        [HttpGet("blob")]
        public async Task<FileStreamResult> getBlob([FromQuery] string azureFileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=babstorage;AccountKey=FAwXuMf3l7RjNTZbOhO+kVqIoCQtJQkRttiJFD8H+f326b3JS2DL+YlODgL95C6pbTFZs49J4kCR+AStwiF8Hw==;EndpointSuffix=core.windows.net");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("audio");
            BlobClient blobClient = containerClient.GetBlobClient(azureFileName);

            var result = await blobClient.DownloadStreamingAsync();
            return new FileStreamResult(result.Value.Content, "application/octet-stream");
        }
    }
}
