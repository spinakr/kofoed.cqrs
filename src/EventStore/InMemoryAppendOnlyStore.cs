using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketCqrs.EventStore
{
    public class InMemoryAppendOnlyStore : IAppendOnlyStore
    {
        private List<StorageRecord> _store = new List<StorageRecord>();

        public void Append(string name, string jsonData, int expectedVersion)
        {
            var stream = _store.Where(x => x.StreamName == name);
            var version = stream.Any() ? stream.Max(x => x.Version) : 0;
            if (version != expectedVersion) throw new Exception();
            _store.Add(new StorageRecord { StreamName = name, JsonData = jsonData, Version = expectedVersion + 1 });
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<VersionedData> ReadAllRecords()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VersionedData> ReadRecords(string name)
        {
            var storedRecords = _store.Where(x => x.StreamName == name);
            return storedRecords.Select(x => new VersionedData { JsonData = x.JsonData, Version = x.Version });
        }

        private class StorageRecord
        {
            public string StreamName { get; set; }
            public string JsonData { get; set; }
            public int Version { get; set; }
        }
    }
}