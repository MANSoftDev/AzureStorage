using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Storage
{
    public class StorageSetup : StorageBase
    {
        /// <summary>
        /// Create the blob container, table and queue
        /// necessary for the application
        /// These should only be created once for the application
        /// so consolidate the creation here
        /// </summary>
        public static void CreateContainersQueuesTables()
        {
            // These methods return true if the container did not exist and was created
            bool didExist = Blob.CreateIfNotExist();
            didExist = Queue.CreateIfNotExist();
            didExist = Table.CreateTableIfNotExist(TABLE_NAME);
        }
    }
}
