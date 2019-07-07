namespace Kofoed.CQRS.Projections
{
    public interface IProjectionStore<Tid, T> where T : new()
    {
        T GetProjection(Tid id);
        void Save(Tid id, T projection);
    }
}