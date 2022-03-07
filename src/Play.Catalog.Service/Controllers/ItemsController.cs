using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Extensions;
using Play.Catalog.Service.Interfaces;
using Play.Catalog.Service.Repository;
using static Play.Catalog.Service.Dtos.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("Items")]
    public class ItemsController : ControllerBase
    {
        // private static readonly List<ItemDto> items = new()
        // {
        //     new ItemDto(Guid.NewGuid(), "Postion", "Restore a Small Amount of HP", 5, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Antidote", "Cures Poison", 7, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a Small Amount of Damage", 20, DateTimeOffset.UtcNow),
        // };
        private readonly IItemsRepository _itemRepository;
        public ItemsController(IItemsRepository itemsRepository)
        {
            _itemRepository = itemsRepository;
        }
        //metodo para leer todos los datos en la BD
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = (await _itemRepository.GetAllAsync()).Select(item => item.AsDto());
            return items;
        }
        //metodo para buscar un item en particular
        [HttpGet("{Id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid Id)
        {
            var item = await _itemRepository.GetAsync(Id);
            if (item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }
        //metodo para agregar un item
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto createItemDto)
        {
            // var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            // items.Add(item);
            // return CreatedAtAction(nameof(GetById), new { Id = item.Id }, item);
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreateDate = DateTimeOffset.UtcNow
            };
            await _itemRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { Id = item.Id }, item);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutAsync(Guid Id, UpdateItemDto updateItemDto)
        {
            // var ExistingItem = items.Where(x => x.Id == Id).SingleOrDefault();
            // if (ExistingItem == null)
            // {
            //     return NotFound();
            // }
            // var UpdatedItem = ExistingItem with
            // {
            //     Name = updateItemDto.Name,
            //     Description = updateItemDto.Description,
            //     Price = updateItemDto.Price
            // };
            // var index = items.FindIndex(ExistingItem => ExistingItem.Id == Id);
            // items[index] = UpdatedItem;
            // return NoContent();
            var existingItem = await _itemRepository.GetAsync(Id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _itemRepository.UpdateAsync(existingItem);
            return NoContent();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            // var index = items.FindIndex(x => x.Id == Id);
            // if (index < 0)
            // {
            //     return NotFound();
            // }
            // items.RemoveAt(index);
            // return NoContent();

            var item = await _itemRepository.GetAsync(Id);
            if (item == null)
            {
                return NotFound();
            }
            await _itemRepository.RemoveAsync(item.Id);
            return NoContent();
        }
    }
}