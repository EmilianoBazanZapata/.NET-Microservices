using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using static Play.Catalog.Service.Dtos.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("Items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Postion", "Restore a Small Amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures Poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a Small Amount of Damage", 20, DateTimeOffset.UtcNow),
        };
        [HttpGet]
        public IEnumerable<ItemDto> GetAll()
        {
            return items;
        }
        [HttpGet("{Id}")]
        public ItemDto GetById(Guid Id)
        {
            var item = items.Where(x => x.Id == Id).SingleOrDefault();
            return item;
        }
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetById), new { Id = item.Id }, item);
        }
        [HttpPut("{Id}")]
        public IActionResult Put(Guid Id, UpdateItemDto updateItemDto)
        {
            var ExistingItem = items.Where(x => x.Id == Id).SingleOrDefault();
            var UpdatedItem = ExistingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };
            var index = items.FindIndex(ExistingItem => ExistingItem.Id == Id);
            items[index] = UpdatedItem;
            return NoContent();
        }
        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var index = items.FindIndex(x => x.Id == Id);
            items.RemoveAt(index);
            return NoContent();
        }
    }
}