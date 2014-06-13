using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace track_traffic_webjob
{
    class Program
    {
        const string TRAFFIC_ID = "335";

        static void Main(string[] args)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("traffic");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            var dtfi = new CultureInfo("sv-se");
            string blobName = string.Format(dtfi, "{0} {1}.jpg", TRAFFIC_ID, DateTime.Now.AddHours(2));
            string Uri = string.Format(@"http://www.trafiken.nu/cameraimages/{0}.jpg", TRAFFIC_ID);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.Properties.ContentType = "image/jpeg";

            WebClient wc = new WebClient();
            var image = wc.DownloadData(Uri);
            blockBlob.UploadFromByteArray(image, 0, image.Count());
        }
    }
}
