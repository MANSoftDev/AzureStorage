using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.WindowsAzure.StorageClient;

namespace Storage
{
    public class Blob : StorageBase
    {
        /// <summary>
        /// Upload blob from the given stream
        /// </summary>
        /// <param name="stream">Stream containing blob</param>
        /// <param name="fileName">Uri to blob</param>
        public static string PutBlob(Stream stream, string fileName)
        {
            CloudBlob blobRef = Blob.GetBlobReference(fileName);
            blobRef.UploadFromStream(stream);

            return blobRef.Uri.ToString();
        }

        /// <summary>
        /// Retrieve the specified blob
        /// </summary>
        /// <param name="blobAddress">Address of blob to retrieve</param>
        /// <returns>Stream containing blob</returns>
        public static Stream GetBlob(string blobAddress)
        {
            MemoryStream stream = new MemoryStream();

            Blob.GetBlobReference(blobAddress)
                .DownloadToStream(stream);                

            return stream;
        }
    }
}
