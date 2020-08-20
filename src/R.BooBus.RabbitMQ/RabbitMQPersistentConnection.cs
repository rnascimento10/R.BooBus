using RabbitMQ.Client;
using System;

namespace R.BooBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IPersistentConnection<IModel>, IDisposable
    {
        private readonly string _connectionString;

        public RabbitMQPersistentConnection(string connectionString)
        {
             

        }

        

        public IModel GetModel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
