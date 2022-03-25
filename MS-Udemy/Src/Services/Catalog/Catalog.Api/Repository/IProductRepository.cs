using Catalog.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repository
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);
        Task<bool> DeleteProductAsync(string id);
        Task<Product> GetProductAsync(string id);
        Task<IEnumerable<Product>> GetProductByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<bool> UpdateProductAsync(Product product);
    }
}