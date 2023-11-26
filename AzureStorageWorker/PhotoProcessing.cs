using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Drawing;
using System.IO;

namespace AzureStorageWorker
{
    public class PhotoProcessing
    {
        public static void Run()
        {
            // Read from queue
            CloudQueueMessage msg = Storage.Queue.GetNextMessage();

            while(msg != null)
            {
                string[] message = msg.AsString.Split('$');
                if(message.Length == 2)
                {
                    AddWatermark(message[0], message[1]);
                }                

                // Message has been read so remove it
                Storage.Queue.DeleteMessage(msg);

                // Get next message if any
                msg = Storage.Queue.GetNextMessage();   
            }
        }

        private static void AddWatermark(string blobURI, string fileName)
        {
            Stream stream = Storage.Blob.GetBlob(blobURI);
            Image img = Image.FromStream(stream);

            Graphics g = Graphics.FromImage(img);

            Font font = new Font("Arial", 24);
            SolidBrush brush = new SolidBrush(Color.Red);

            g.DrawString("WATERMARK", font, brush, new PointF(100, 100));

            Storage.Blob.PutBlob(stream, fileName);
        }
    }
}
