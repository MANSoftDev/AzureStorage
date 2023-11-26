using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Storage
{
    public abstract class StorageBase
    {
        // Must be lower case and conform to DNS
        // naming conventions
        public const string QUEUE_NAME = "metadataqueue";
        public const string BLOB_CONTAINER_NAME = "photos";

        // Does not need to be lower case
        public const string TABLE_NAME = "MetaData";

        static StorageBase()
        {
            // This code is necessary to use CloudStorageAccount.FromConfigurationSetting
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
                RoleEnvironment.Changed += (sender, arg) =>
                {
                    if(arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                        .Any((change) => (change.ConfigurationSettingName == configName)))
                    {
                        if(!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                        {
                            RoleEnvironment.RequestRecycle();
                        }
                    }
                };
            });
        }

        #region Properties

        #region Blob

        protected static CloudBlobContainer Blob
        {
            get { return BlobClient.GetContainerReference(BLOB_CONTAINER_NAME); }
        }

        private static CloudBlobClient BlobClient
        {
            get
            {
                //return new CloudBlobClient(Account.BlobEndpoint.AbsoluteUri, Account.Credentials);

                // More direct method
                return Account.CreateCloudBlobClient();
            }
        }

        #endregion

        #region Queue

        protected static CloudQueue Queue
        {
            get { return QueueClient.GetQueueReference(QUEUE_NAME); }
        }

        private static CloudQueueClient QueueClient
        {
            get { return Account.CreateCloudQueueClient(); }
        }

        #endregion

        #region Table

        protected static CloudTableClient Table
        {
            get { return Account.CreateCloudTableClient(); }
        }
        
        #endregion

        protected static CloudStorageAccount Account
        {
            get
            {
                // For development this can be used
                //return CloudStorageAccount.DevelopmentStorageAccount; 
                // or this so code doesn't need to be changed before deployment 
                return CloudStorageAccount.FromConfigurationSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString");
            }
        }

        #endregion
    }
}
