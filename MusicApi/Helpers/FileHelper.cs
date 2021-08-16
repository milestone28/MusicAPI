using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Helpers
{
    public static class FileHelper
    {

        //for Image
        public static async Task<string> UploadImage(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=gyustorageaccount;AccountKey=aCn0DbXSvw783OGgzCW+UkZLPNSdVMw13I/YPbzErcpFfRqKPc7N2XehumaBwkJ2bQnOyh8dS38cp9+2pRUP0w==;EndpointSuffix=core.windows.net";
            string containerName = "songcover";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream, overwrite : true);

            //save file path inside DB
            return  blobClient.Uri.AbsoluteUri;
        }

        //UploadFile audio etc
        public static async Task<string> UploadFile(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=gyustorageaccount;AccountKey=aCn0DbXSvw783OGgzCW+UkZLPNSdVMw13I/YPbzErcpFfRqKPc7N2XehumaBwkJ2bQnOyh8dS38cp9+2pRUP0w==;EndpointSuffix=core.windows.net";
            string containerName = "audiofiles";

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream, overwrite: true);

            //save file path inside DB
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
