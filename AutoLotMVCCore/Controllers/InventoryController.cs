using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoLotDALCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AutoLotMVCCore.Controllers
{
    public class InventoryController : Controller
    {
        private readonly string _baseUrl;

        public InventoryController(IConfiguration configuration)
        {
            _baseUrl = configuration.GetSection("ServiceAddress").Value;
        }

        private async Task<Inventory> GetInventoryRecord(int id)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var inventory = JsonConvert.DeserializeObject<Inventory>(await response.Content.ReadAsStringAsync());
                return inventory;
            }

            return null;
        }

        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var items = JsonConvert.DeserializeObject<List<Inventory>>(await response.Content.ReadAsStringAsync());
                return View(items);
            }

            return NotFound();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var inventory = await GetInventoryRecord(id.Value);
            return inventory != null ? (IActionResult) View(inventory) : NotFound();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Make,Color,PetName")] Inventory inventory)
        {
            if (!ModelState.IsValid) return View(inventory);
            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(inventory);
                var response =
                    await client.PostAsync(_baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,$"Unable to create record: {ex.Message}");
            }

            return View(inventory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var inventory = await GetInventoryRecord(id.Value);
            return inventory != null ? (IActionResult) View(inventory) : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Make,Color,PetName,Id,Timestamp")] Inventory inventory)
        {
            if (id != inventory.Id) return BadRequest();

            if (!ModelState.IsValid) return View(inventory);

            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(inventory);
            var response = await client.PutAsync($"{_baseUrl}/{inventory.Id}",
                new StringContent(json, Encoding.UTF8, "application/json"));
            
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(inventory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var inventory = await GetInventoryRecord(id.Value);
            return inventory != null ? (IActionResult)View(inventory) : NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("Id,Timestamp")] Inventory inventory)
        {
            var client = new HttpClient();
            var timeStampString = JsonConvert.SerializeObject(inventory.Timestamp);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{inventory.Id}/{timeStampString}");
            await client.SendAsync(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
