using System.Collections.Generic;
using AutoLotDALCore.Models;

namespace AutoLotDALCore.Repos
{
    public interface IInventoryRepo : IRepo<Inventory>
    {
        List<Inventory> Search(string searchString);

        List<Inventory> GetPinkCars();

        List<Inventory> GetRelatedData();
    }
}