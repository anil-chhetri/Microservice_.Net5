using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Interface;
using Play.Inventory.Service.Client;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Properties;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("inventory")]
    public class InventoryItemController : ControllerBase
    {
        private readonly IRepository<InventoryItem> repository;
        private readonly CatalogClient catalogClient;

        public InventoryItemController(IRepository<InventoryItem> repository,
                                       CatalogClient CatalogClient)
        {
            this.repository = repository;
            this.catalogClient = CatalogClient;
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetAsync(Guid UserId)
        {
            // var inventory = (await repository.GetAllAsync(entity => entity.UserId == UserId))
            //             .Select(item => item.AsDto());

            var catalogItem = await catalogClient.GetItemsAsync();
            var inventory = (await repository.GetAllAsync(entity => entity.UserId == UserId))
                            .Select(inventoryItem =>
                            {
                                var item = catalogItem.Single(e => e.Id == inventoryItem.CatalogItemId);
                                return inventoryItem.AsDto(item.name, item.description);
                            });


            return Ok(inventory);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(GrantItemDto item)
        {
            var inventory = await repository.GetAsync(
                    entity => entity.UserId == item.UserId && entity.CatalogItemId == item.CatalogItemId);

            if (inventory == null)
            {
                var itemToCreate = new InventoryItem()
                {
                    CatalogItemId = item.CatalogItemId,
                    UserId = item.UserId,
                    AcquiredDate = DateTimeOffset.UtcNow,
                    Quantity = item.Quantity
                };

                await repository.CreateAsync(itemToCreate);

                return CreatedAtAction(nameof(GetAsync), new { UserId = item.UserId }, itemToCreate);
            }
            else
            {
                inventory.Quantity += item.Quantity;
                await repository.UpdateAsync(inventory);
                return CreatedAtAction(nameof(GetAsync), new { UserId = item.UserId }, inventory);
            }
        }
    }
}