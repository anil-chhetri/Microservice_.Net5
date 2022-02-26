using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{

    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> collections;

        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            collections = database.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await collections.Find(filterBuilder.Empty).ToListAsync();
        }


        public async Task<T> GetAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(entity => entity.Id, Id);
            return await collections.Find(filter).SingleOrDefaultAsync();
        }


        public async Task CreateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentException(nameof(item));
            }

            await collections.InsertOneAsync(item);
        }

        public async Task UpdateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentException(nameof(item));
            }

            var filter = filterBuilder.Eq(entity => entity.Id, item.Id);
            await collections.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(entity => entity.Id, Id);
            await collections.DeleteOneAsync(filter);
        }


    }
}