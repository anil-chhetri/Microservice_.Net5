using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Interface;
using Play.Inventory.Service.Client;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("inventory")]
    public class InventoryItemController : ControllerBase
    {
        private readonly IRepository<InventoryItem> repository;
        // private readonly CatalogClient catalogClient;
        private readonly IRepository<CatalogItem> CatalogRepo;

        public InventoryItemController(IRepository<InventoryItem> repository,
                                       IRepository<CatalogItem> CatalogRepo,
                                       CatalogClient CatalogClient)
        {
            this.CatalogRepo = CatalogRepo;
            this.repository = repository;
            // this.catalogClient = CatalogClient;
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetAsync(Guid UserId)
        {
            var inventory = (await repository.GetAllAsync(entity => entity.UserId == UserId));
            var ItemsId = inventory.Select(x => x.CatalogItemId);
            var CatalogItemsEntities = await CatalogRepo.GetAllAsync(item => ItemsId.Contains(item.Id));


            var inventoryDto = inventory
                            .Select(inventoryItem =>
                            {
                                var item = CatalogItemsEntities.Single(e => e.Id == inventoryItem.CatalogItemId);
                                return inventoryItem.AsDto(item.Name, item.Description);
                            });


            return Ok(inventoryDto);
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