using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Play.Inventory.Service.Properties;

namespace Play.Inventory.Service.Client
{
    public class CatalogClient
    {
        private readonly HttpClient client;

        public CatalogClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IReadOnlyCollection<CatalogItem>> GetItemsAsync()
        {
            return await client.GetFromJsonAsync<IReadOnlyCollection<CatalogItem>>("/items");
        }
    }
}