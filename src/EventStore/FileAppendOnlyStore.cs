using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PocketCqrs.EventStore
{
    public class FileAppendOnlyStore : IAppendOnlyStore
    {
        private Dictionary<string, StreamWriter> _fileStreams = new Dictionary<string, StreamWriter>();
        private string EventStoreContentPath;

        public FileAppendOnlyStore(string storeName)
        {
            var myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            EventStoreContentPath = $"{myDocumentsPath}/{storeName}/eventstore";
            if (!Directory.Exists(EventStoreContentPath))
            {
                System.Console.WriteLine($"Creating directory {EventStoreContentPath}");
                Directory.CreateDirectory(EventStoreContentPath);
            }
        }

        public void Append(string name, string jsonData, int expectedVersion)
        {
            var filePath = $"{EventStoreContentPath}/{name}";
            var fileExists = File.Exists(filePath);
            var lastLine = fileExists ? File.ReadLines(filePath).Last() : null;
            var version = lastLine == null ? 0 : int.Parse(lastLine.Split(';')[0]);
            if (version != expectedVersion) throw new Exception();
            var stream = GetOrCreateStreamWriter(name);
            stream.WriteLine($"{expectedVersion + 1};{jsonData}");
        }

        public IEnumerable<VersionedData> ReadRecords(string name)
        {
            var filePath = $"{EventStoreContentPath}/{name}";
            if (!File.Exists(filePath)) return new List<VersionedData>();
            return File.ReadAllLines(filePath).Select(x =>
            {
                var splitLine = x.Split(';');
                return new VersionedData { Version = int.Parse(splitLine[0]), JsonData = splitLine[1] };
            });
        }

        public void Dispose()
        {
            foreach (var stream in _fileStreams)
            {
                stream.Value.Close();
            }
        }

        private StreamWriter GetOrCreateStreamWriter(string name)
        {
            StreamWriter fileStream;
            if (_fileStreams.TryGetValue(name, out var existingStream))
            {
                fileStream = existingStream;
            }
            else
            {
                var filePath = $"{EventStoreContentPath}/{name}";
                fileStream = File.AppendText(filePath);
                fileStream.AutoFlush = true;
            }
            return fileStream;
        }
    }
}