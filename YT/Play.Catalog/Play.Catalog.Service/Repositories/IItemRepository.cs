using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{
    public interface IItemRepository
    {
        Task CreateAsync(Item item);
        Task DeleteAsync(Guid Id);
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid Id);
        Task UpdateAsync(Item item);
    }
}