using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.StorageClient;

namespace Storage
{
    public class Queue : StorageBase
    {
        /// <summary>
        /// Add message to queue
        /// </summary>
        /// <param name="msg">CloudQueueMessage to add</param>
        public static void Add(CloudQueueMessage msg)
        {
            Queue.AddMessage(msg);
        }

        /// <summary>
        /// Get next message in queue
        /// </summary>
        /// <returns>CloudQueueMessage or null</returns>
        public static CloudQueueMessage GetNextMessage()
        {
            return Queue.PeekMessage() != null ? Queue.GetMessage() : null;
        }

        /// <summary>
        /// Get all messages in the queue
        /// </summary>
        /// <remarks>Only returns one message for now</remarks>
        /// <returns>Collection of CloudQueueMessage</returns>
        public static List<CloudQueueMessage> GetAllMessages()
        {
            // If the number requested is greater than the actual
            // number of messages in the queue this will fail.
            // So get the approximate number of messages available

            // Although this would seem to a good property to use
            // it will always return null
            //int? count = Queue.ApproximateMessageCount;

            // To get the count use this method
            int count = Queue.RetrieveApproximateMessageCount();
            
            return Queue.GetMessages(count).ToList();
        }

        /// <summary>
        /// Delete given message
        /// </summary>
        /// <param name="msg">CloudQueueMessage to delete</param>
        public static void DeleteMessage(CloudQueueMessage msg)
        {
            Queue.DeleteMessage(msg);
        }
    }
}
