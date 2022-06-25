using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PocketCqrs.Projections
{
    public class FileProjectionStore<Tid, T> : IProjectionStore<Tid, T>, IDisposable where T : new()
    {
        private Dictionary<string, StreamWriter> _fileStreams = new Dictionary<string, StreamWriter>();
        private string EventStoreContentPath;

        public FileProjectionStore(string fileBasePath)
        {
            var projectionName = typeof(T).ToString().Split('.').Last();
            EventStoreContentPath = $"{fileBasePath}/cqrs/projections/{projectionName}";
            if (!Directory.Exists(EventStoreContentPath))
            {
                Directory.CreateDirectory(EventStoreContentPath);
            }
        }

        public FileProjectionStore() : this(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        {
        }

        public T GetProjection(Tid id)
        {

            var filePath = $"{EventStoreContentPath}/{id}";
            if (!File.Exists(filePath)) return new T();
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public void Save(Tid id, T projection)
        {
            var filePath = $"{EventStoreContentPath}/{id}";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var stream = GetOrCreateStreamWriter(id.ToString());
            var jsonData = JsonConvert.SerializeObject(projection);
            stream.Write(jsonData);
        }

        private StreamWriter GetOrCreateStreamWriter(string id)
        {
            StreamWriter fileStream;
            if (_fileStreams.TryGetValue(id, out var existingStream))
            {
                fileStream = existingStream;
            }
            else
            {
                var filePath = $"{EventStoreContentPath}/{id}";
                fileStream = File.AppendText(filePath);
                fileStream.AutoFlush = true;
            }
            return fileStream;
        }

        public void Dispose()
        {
            foreach (var stream in _fileStreams)
            {
                stream.Value.Close();
            }
        }
    }
}