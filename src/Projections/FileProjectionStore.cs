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
        private string ProjectionsPath;

        public FileProjectionStore()
        {
            var projectionName = typeof(T).ToString().Split('.').Last();
            var myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var projectName = System.Reflection.Assembly.GetEntryAssembly().FullName;

            ProjectionsPath = $"{myDocumentsPath}/{projectName}/projections/{projectionName}";
            if (!Directory.Exists(ProjectionsPath))
            {
                Directory.CreateDirectory(ProjectionsPath);
            }
        }

        public T GetProjection(Tid id)
        {

            var filePath = $"{ProjectionsPath}/{id}";
            if (!File.Exists(filePath)) return new T();
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public void Save(Tid id, T projection)
        {
            var filePath = $"{ProjectionsPath}/{id}";
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
                var filePath = $"{ProjectionsPath}/{id}";
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