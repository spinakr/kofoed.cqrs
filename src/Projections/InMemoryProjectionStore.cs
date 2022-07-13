using System.Collections.Generic;

namespace PocketCqrs.Projections
{
    public class InMemoryProjectionStore<Tid, T> : IProjectionStore<Tid, T> where T : new()
    {
        private Dictionary<Tid, T> _store = new Dictionary<Tid, T>();

        public T GetProjection(Tid id)
        {
            if (!_store.TryGetValue(id, out T projection))
            {
                return new T();
            }
            else
            {
                return projection;
            }
        }

        public void Save(Tid id, T projection)
        {
            _store.Remove(id);
            _store.Add(id, projection);
        }
    }
}