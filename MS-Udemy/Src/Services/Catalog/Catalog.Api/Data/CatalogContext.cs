using Catalog.Api.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var connectionstring = configuration.GetSection("DatabaseSettings")["ConnectionString"];
            var databaseName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
            var collectionName = configuration.GetValue<string>("DatabaseSettings:CollectionName");

            var client = new MongoClient(connectionstring);

            //creating a database.
            var database = client.GetDatabase(databaseName);
            Products = database.GetCollection<Product>(collectionName);
            CatalogContextSeed.SeedData(Products);
            
        }
        public IMongoCollection<Product> Products { get; }




    }
}
