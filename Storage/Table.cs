using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Storage
{
    public class Table : StorageBase
    {
        public const string PARTITION_KEY = "MetaData";

        /// <summary>
        /// Add object to table
        /// </summary>
        /// <param name="data">MetaData object to add</param>
        public static void Add(MetaData data)
        {
            Context.Add(data);
        }

        /// <summary>
        /// Get MetaData object matching key
        /// </summary>
        /// <param name="key">Key of object to retrieve</param>
        /// <returns>MetaData if found</returns>
        public static MetaData GetMetaData(string key)
        {
            return (from e in Context.Data
                    where e.RowKey == key && e.PartitionKey == PARTITION_KEY
                   select e).SingleOrDefault();
        }

        /// <summary>
        /// Get all MetaData object in storage
        /// </summary>
        /// <returns></returns>
        public static List<MetaData> GetAll()
        {
            return (from e in Context.Data
                    select e).ToList();
        }


        #region Properties

        private static MetaDataContext Context
        {
            get { return new MetaDataContext(Account.TableEndpoint.AbsoluteUri, Account.Credentials); }
        }

        #endregion
    }

    public class MetaData : TableServiceEntity
    {
        public MetaData()
        {
            PartitionKey = Table.PARTITION_KEY;
            RowKey = "Not Set";
        }

        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImageURL { get; set; }
    }

    public class MetaDataContext : TableServiceContext
    {
        public MetaDataContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        {
            // Alternative method of creating table
            //CloudTableClient.CreateTablesFromModel(typeof(MetaDataContext),
            //                           baseAddress, credentials);

            // Prevent DataServiceQueryException when no records
            // match a query
            IgnoreResourceNotFoundException = true;
        }

        public IQueryable<MetaData> Data
        {
            get { return CreateQuery<MetaData>(StorageBase.TABLE_NAME); }
        }

        /// <summary>
        /// Add object to table
        /// </summary>
        /// <param name="data">MetaData object to add</param>
        public void Add(MetaData data)
        {
            // RowKey can't have / so replace it
            data.RowKey = data.RowKey.Replace("/", "_");

            MetaData original = (from e in Data
                                where e.RowKey == data.RowKey 
                                && e.PartitionKey == Table.PARTITION_KEY
                                select e).FirstOrDefault();

            // Check if the object already exists
            // and update if so
            if(original != null)
            {
                Update(original, data);
            }
            else
            {
                AddObject(StorageBase.TABLE_NAME, data);
            }

            SaveChanges();
        }

        /// <summary>
        /// Update object
        /// </summary>
        /// <param name="original">Original MetaData object</param>
        /// <param name="data">Updated object</param>
        public void Update(MetaData original, MetaData data)
        {
            original.Description = data.Description;
            original.Date = data.Date;
            original.ImageURL = data.ImageURL;
            UpdateObject(original);

            SaveChanges();
        }
    }
}
