using Azure.Storage.Queues;

namespace SubscriptionExpiryEmail
{
    internal class QueueClientFactory
    {
        private readonly string _connectionString;

        public QueueClientFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public QueueClient CreateQueueClient(string expirequeue)
        {
            return new QueueClient(_connectionString, expirequeue);
        }
    }
}