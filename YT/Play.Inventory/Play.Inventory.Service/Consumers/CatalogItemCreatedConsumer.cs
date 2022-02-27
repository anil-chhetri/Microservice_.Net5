using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Settings;
using Play.Common.Interface;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> dbContext;
        public CatalogItemCreatedConsumer(IRepository<CatalogItem> dbContext)
        {
            this.dbContext = dbContext;

        }
        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;
            var item = await dbContext.GetAsync(message.CatalogId);

            if (item != null)
            {
                return;
            }

            item = new CatalogItem() { Id = message.CatalogId, Name = message.name, Description = message.description };

            await dbContext.CreateAsync(item);

        }
    }
}