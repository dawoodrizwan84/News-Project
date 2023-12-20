using Azure.Data.Tables;

namespace HighestLowestElectricityPrice.Services
{
    internal class TableServiceClient
    {
        private string v;

        public TableServiceClient(string v)
        {
            this.v = v;
        }

        internal TableClient GetTableClient(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}