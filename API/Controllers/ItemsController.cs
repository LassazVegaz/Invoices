using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORE.Models;
using CORE.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private readonly IItemService itemService;

        public ItemsController(IItemService itemService)
        {
            this.itemService = itemService;
        }


        // GET: api/items
        // Get all items
        [HttpGet]
        public Item[] Get()
        {
            return itemService.GetAllItems();
        }

        // GET api/items/id
        // get item by ID
        [HttpGet("{id}")]
        public ActionResult<Item> Get(int id)
        {

            var item = itemService.GetItem(id);

            return item is null ? NotFound($"Item {id} is not found") : item;

        }

        // POST api/items
        // create item
        [HttpPost]
        public Item Post([FromBody] Item item)
        {

            var newItem = itemService.CreateItem(item);

            return newItem;

        }

        // PUT api/items/id
        // update item
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Item item)
        {

            // check if item exists
            if (itemService.ItemExists(id))
            {

                // set ID to sent item
                item.Id = id;
                // update item
                itemService.UpdateItem(item);

                return Ok(item);

            }
            else
                return NotFound($"Item {id} is not found");

        }

        // DELETE api/items/id
        // delete item
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            // check if item exists
            if (itemService.ItemExists(id))
            {
                itemService.DeleteItem(id);
                return Ok();
            }
            else
                return NotFound($"Item {id} is not found");

        }
    }
}
