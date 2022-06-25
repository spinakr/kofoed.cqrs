namespace PocketCqrs.EventStore
{
    public class VersionedData
    {
        public int Version { get; set; }
        public string JsonData { get; set; }
    }
}