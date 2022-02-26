using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{

    public class ItemRepository : IItemRepository
    {
        private readonly string collectionName = "items";

        private readonly IMongoCollection<Item> collections;

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemRepository(IMongoDatabase database)
        {
            collections = database.GetCollection<Item>(collectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await collections.Find(filterBuilder.Empty).ToListAsync();
        }


        public async Task<Item> GetAsync(Guid Id)
        {
            var filter = filterBuilder.Eq(entity => entity.Id, Id);
            return await collections.Find(filter).SingleOrDefaultAsync();
        }


        public async Task CreateAsync(Item item)
        {
            if (item == null)
            {
                throw new ArgumentException(nameof(item));
            }

            await collections.InsertOneAsync(item);
        }

        public async Task UpdateAsync(Item item)
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