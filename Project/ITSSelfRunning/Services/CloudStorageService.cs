using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITSSelfRunning.Models.Run;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ITSSelfRunning.Services
{
    /// <summary>
    /// Class to manage uploads to Storage Account
    /// </summary>
    public class CloudStorageService : ICloudStorageService
    {

        private readonly CloudBlobClient _cbc;

        public CloudStorageService(string cs)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(cs);
            _cbc = storageAccount.CreateCloudBlobClient();
        }


        /// <summary>
        /// Uploads a photo in b64 into storage blob
        /// </summary>
        /// <param name="photo">photo in base 64</param>
        /// <returns>the uri of the blob</returns>
        public async Task<String> UploadPhotoFromB64(string photo)
        {
            var stringa = photo.Remove(0, photo.IndexOf(',') + 1);
            var arrayBytesPhoto = Convert.FromBase64String(stringa);

            // Get a reference to a container named "mycontainer."
            CloudBlobContainer container = _cbc.GetContainerReference("selphies");
            string uniqueBlobName = string.Format("selfies/image_{0}.jpeg", Guid.NewGuid().ToString());
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(uniqueBlobName);
            await blockBlob.UploadFromByteArrayAsync(arrayBytesPhoto, 0, arrayBytesPhoto.Length);
            return blockBlob.Uri.AbsoluteUri;
        }


        /// <summary>
        /// Uploads a photo from file to blob storage
        /// </summary>
        /// <param name="photo">the file</param>
        /// <returns>the uri of the blob</returns>
        public async Task<String> UploadProfilePhoto(IFormFile photo)
        {
            var camerasContainer = _cbc.GetContainerReference("selphies");


            var id = Guid.NewGuid();
            var fileExtension = Path.GetExtension(photo.FileName);

            var blobName = $"photo/{id}{fileExtension}";

            var blobRef = camerasContainer.GetBlockBlobReference(blobName);


            using (var stream = photo.OpenReadStream())
            {
                await blobRef.UploadFromStreamAsync(stream);
            }


            return blobRef.Uri.AbsoluteUri;
        }

    }
}
