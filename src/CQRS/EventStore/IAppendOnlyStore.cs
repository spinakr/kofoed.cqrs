using System;
using System.Collections.Generic;

namespace CQRS.EventStore
{
    public interface IAppendOnlyStore : IDisposable
    {
        void Append(string name, string jsonData, int expectedVersion);
        IEnumerable<VersionedData> ReadRecords(string name);
    }
}