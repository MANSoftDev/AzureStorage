using System;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureStorageWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnUpload(object sender, EventArgs e)
        {
            if(FileUpload.HasFile)
            {
                DateTime dt = DateTime.Parse(Date.Text);
                string fileName = string.Format("{0}_{1}", dt.ToString("yyyy/MM/dd"), FileUpload.FileName);

                // Upload the blob
                string blobURI = Storage.Blob.PutBlob(FileUpload.PostedFile.InputStream, fileName);


                // Add entry to table
                Storage.Table.Add(new Storage.MetaData
                {
                    Description = Description.Text,
                    Date = dt,
                    ImageURL = blobURI,
                    RowKey = fileName
                }
                );

                // Add message to queue
                Storage.Queue.Add(new CloudQueueMessage(blobURI + "$" + fileName));

                // Reset fields
                Description.Text = "";
                Date.Text = "";
            }
        }
    }
}
