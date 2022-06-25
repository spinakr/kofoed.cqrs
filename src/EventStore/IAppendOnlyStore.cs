using System;
using System.Collections.Generic;

namespace PocketCqrs.EventStore
{
    public interface IAppendOnlyStore : IDisposable
    {
        void Append(string name, string jsonData, int expectedVersion);
        IEnumerable<VersionedData> ReadRecords(string name);
    }
}