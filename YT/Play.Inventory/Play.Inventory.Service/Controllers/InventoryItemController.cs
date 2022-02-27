using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Interface;
using Play.Inventory.Service.Entities;
using Play.Inventory.Service.Properties;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("inventory")]
    public class InventoryItemController : ControllerBase
    {
        private readonly IRepository<InventoryItem> repository;

        public InventoryItemController(IRepository<InventoryItem> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(Guid UserId)
        {
            var inventory = (await repository.GetAllAsync(entity => entity.UserId == UserId))
                        .Select(item => item.AsDto());
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