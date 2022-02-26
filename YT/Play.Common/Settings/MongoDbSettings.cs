namespace Play.Common.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; init; }

        public int Port { get; set; }

        public string CononectionString => $"mongodb://{Host}:{Port}";
    }
}