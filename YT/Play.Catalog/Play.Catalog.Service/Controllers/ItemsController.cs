using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("/items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDtos> items = new()
        {
            new ItemDtos(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDtos(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDtos(Guid.NewGuid(), "Bronze sword", "Deals a small amount of danger", 29, DateTimeOffset.UtcNow)

        };

        [HttpGet]
        public IEnumerable<ItemDtos> Get()
        {
            return items;
        }


        [HttpGet("{Id}")]
        public IActionResult GetById(Guid Id)
        {
            var item = items.Where(x => x.Id == Id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        [HttpPost]
        public IActionResult Post(CreateItemDto createdItem)
        {
            var item = new ItemDtos(Guid.NewGuid(), createdItem.Name, createdItem.Description, createdItem.Price, DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { Id = item.Id }, item);
        }


        [HttpPut("{Id}")]
        public IActionResult Put(Guid Id, UpdateItemDto updateItem)
        {
            var existingItem = items.Where(x => x.Id == Id).SingleOrDefault();

            if (existingItem == null)
            {
                return NotFound();
            }

            var updatedItem = existingItem with
            {
                Name = updateItem.Name,
                Description = updateItem.Description,
                Price = updateItem.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == Id);

            items[index] = updatedItem;

            return NoContent();
        }



        [HttpDelete]
        public IActionResult Delete(Guid Id)
        {
            var item = items.Where(x => x.Id == Id).SingleOrDefault();

            if (item == null)
            {
                return NotFound();
            }

            items.Remove(item);

            return NoContent();

        }
    }
}
