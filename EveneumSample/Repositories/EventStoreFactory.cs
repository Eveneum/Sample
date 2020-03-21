using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Eveneum;
using Microsoft.Azure.Cosmos;

namespace EveneumSample.Repositories
{
    public interface IEventStoreFactory
    {
        IEventStore GetEventStore();
    }

    public class EventStoreFactory : IEventStoreFactory
    {
        private Lazy<Task<IEventStore>> _lazyEventStore;
        private const string _partitionKey = "/StreamId";

        public EventStoreFactory(IConfiguration configuration)
        {
            _lazyEventStore = new Lazy<Task<IEventStore>>(() => CreateEventStore(configuration));
        }

        public IEventStore GetEventStore()
        {
            return _lazyEventStore.Value.Result;
        }

        private static async Task<IEventStore> CreateEventStore(IConfiguration configuration)
        {
            var cosmos = configuration.GetSection("CosmosDB");
            var database = cosmos["Database"];
            var collection = cosmos["Collection"];
            var connectionString = cosmos["ConnectionString"];

            var client = new CosmosClient(connectionString);
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(database);
            await databaseResponse.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(collection, _partitionKey));

            IEventStore eventStore = new EventStore(client, database, collection);
            await eventStore.Initialize();

            return eventStore;
        }
    }
}
