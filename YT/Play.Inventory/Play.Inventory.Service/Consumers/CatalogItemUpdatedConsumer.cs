using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Interface;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> repo;
        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repo)
        {
            this.repo = repo;

        }
        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;

            var item = await repo.GetAsync(message.CatalogId);

            if (item == null)
            {
                item = new CatalogItem() { Id = message.CatalogId, Name = message.name, Description = message.description };
                await repo.CreateAsync(item);
            }
            else
            {

                item.Name = message.name;
                item.Description = message.description;

                await repo.UpdateAsync(item);
            }

        }
    }
}
