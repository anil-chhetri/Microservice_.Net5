using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Extensions;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("/items")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemRepository itemRepository = new();

        [HttpGet]
        public async Task<IEnumerable<ItemDtos>> GetAsync()
        {
            var items = (await itemRepository.GetAllAsync()).Select(item => item.AsDTO());
            return items;
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync(Guid Id)
        {
            var item = (await itemRepository.GetAsync(Id)).AsDTO();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateItemDto createdItem)
        {
            var item = new Item
            {
                Name = createdItem.Name,
                Description = createdItem.Description,
                Price = createdItem.Price,
                CreatedDateTime = DateTimeOffset.UtcNow
            };

            await itemRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new { Id = item.Id }, item);
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> PutAsync(Guid Id, UpdateItemDto updateItem)
        {
            var existingItem = (await itemRepository.GetAsync(Id));

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItem.Name;
            existingItem.Description = updateItem.Description;
            existingItem.Price = updateItem.Price;

            await itemRepository.UpdateAsync(existingItem);

            return NoContent();
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            var item = await itemRepository.GetAsync(Id);

            if (item == null)
            {
                return NotFound();
            }

            await itemRepository.DeleteAsync(Id);

            return NoContent();

        }
    }
}
