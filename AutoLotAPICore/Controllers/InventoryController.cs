using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoLotDALCore.Models;
using AutoLotDALCore.Repos;
using AutoMapper;
using Newtonsoft.Json;

namespace AutoLotAPICore.Controllers
{
    [Route("api/Inventory")]
    [Produces("application/json")]
    public class InventoryController : Controller
    {
        private readonly IInventoryRepo _repo;
        private readonly IMapper _mapper;

        public InventoryController(IInventoryRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        
        [HttpGet]
        public IEnumerable<Inventory> GetCars()
        {
            var inventories = _repo.GetAll();
            return _mapper.Map<List<Inventory>, List<Inventory>>(inventories);
        }

        [HttpGet("{id}", Name = "DisplayRoute")]
        public async Task<IActionResult> GetInventory([FromRoute] int id)
        {
            Inventory inventory = _repo.GetOne(id);
            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Inventory, Inventory>(inventory));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory([FromRoute] int id, [FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inventory.Id)
            {
                return BadRequest();
            }

            _repo.Update(inventory);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostInventory([FromBody] Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repo.Add(inventory);
            return CreatedAtRoute("DisplayRoute", new { id = inventory.Id }, inventory);
        }

        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> DeleteInventory([FromRoute] int id, [FromRoute] string timestamp)
        {
            if (!timestamp.StartsWith("\""))
            {
                timestamp = $"\"{timestamp}\"";
            }

            var ts = JsonConvert.DeserializeObject<byte[]>(timestamp);
            _repo.Delete(id, ts);
            return Ok();
        }
    }

    
}
