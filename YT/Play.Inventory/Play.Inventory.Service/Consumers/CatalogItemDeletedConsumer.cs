using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Interface;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> repo;
        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repo)
        {
            this.repo = repo;

        }
        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            var item = repo.GetAsync(message.CatalogId);

            if (item == null)
            {
                return;
            }

            await repo.DeleteAsync(message.CatalogId);

        }
    }
}