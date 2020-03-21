using Eveneum;
using Eveneum.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveneumSample.Repositories
{
    public interface IEventStoreRepository
    {
        Task<Response> AddEventAsync(string streamId, Object body, Object metadata = null);
        Task<Response> AddSnapshot(string streamId, ulong version, object snapshot);
        Task<Response> DeleteSnapshots(string streamId, ulong version);
        Task<Stream?> GetStream(string streamId);
        Task<List<StreamHeader>> GetStreamHeaders();
        Task<DeleteResponse> DeleteStream(string streamId);

    }

    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IEventStore _eventStore;

        public EventStoreRepository(IEventStoreFactory eventStoreFactory)
        {
            _eventStore = eventStoreFactory.GetEventStore();
        }

        public async Task<Response> AddEventAsync(string streamId, Object body, Object metadata = null)
        {
            var expectedVersion = await GetStreamVersionAsync(streamId);
            var events = CreateEvents(streamId, body, metadata, expectedVersion);

            return await _eventStore.WriteToStream(streamId, events, expectedVersion);
        }

        public async Task<Response> AddSnapshot(string streamId, ulong version, object snapshot)
        {
            return await _eventStore.CreateSnapshot(streamId, version, snapshot);
        }

        public async Task<Response> DeleteSnapshots(string streamId, ulong version)
        {
            return await _eventStore.DeleteSnapshots(streamId, version);
        }

        public async Task<List<StreamHeader>> GetStreamHeaders()
        {
            var headers = new List<StreamHeader>();
            var query = "SELECT * from c";

            await ((IAdvancedEventStore)_eventStore).LoadStreamHeaders(query, e =>
            {
                headers.AddRange(e);
                return Task.CompletedTask;
            });

            return headers;
        }

        public async Task<Stream?> GetStream(string streamId)
        {
            var streams = await _eventStore.ReadStream(streamId);
            return streams.Stream;
        }

        public async Task<DeleteResponse> DeleteStream(string streamId)
        {
            var expectedVersion = await GetStreamVersionAsync(streamId);
            return await _eventStore.DeleteStream(streamId, expectedVersion.Value);
        }

        public async Task<List<EventData>> GetEvents(string streamId)
        {
            List<EventData> events = new List<EventData>();

            var query = $"SELECT * FROM c WHERE c.StreamId = '{streamId}'";
            await ((IAdvancedEventStore)_eventStore).LoadEvents(query, e =>
            {
                events.AddRange(e);
                return Task.CompletedTask;
            });

            return events;
        }

        private EventData[] CreateEvents(string streamId, Object body, Object metadata, ulong? existingVersion)
        {
            var version = existingVersion.GetValueOrDefault() + 1;
            var data = new EventData(streamId, body, metadata, version);

            return new List<EventData> { data }.ToArray();
        }

        private async Task<ulong?> GetStreamVersionAsync(string streamId)
        {
            ulong? version = null;
            var query = $"SELECT TOP 1 * from c WHERE c.StreamId = '{streamId}'";

            await ((IAdvancedEventStore)_eventStore).LoadStreamHeaders(query, e =>
            {
                if (e.Count > 0)
                    version = e.First().Version;
                return Task.CompletedTask;
            });

            return version;
        }
    }
}
