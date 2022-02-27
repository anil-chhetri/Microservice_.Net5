using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Extensions;
using Play.Common.Interface;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemRepository;
        private readonly IPublishEndpoint publicEndPoint;

        public ItemsController(IRepository<Item> itemRepository, IPublishEndpoint publicEndPoint)
        {
            this.publicEndPoint = publicEndPoint;
            this.itemRepository = itemRepository;
        }

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

            await publicEndPoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));
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
            await publicEndPoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description));

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
            await publicEndPoint.Publish(new CatalogItemDeleted(item.Id));

            return NoContent();

        }
    }
}
