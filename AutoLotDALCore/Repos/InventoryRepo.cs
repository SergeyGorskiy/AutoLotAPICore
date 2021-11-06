using System.Collections.Generic;
using System.Linq;
using AutoLotDALCore.EF;
using AutoLotDALCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLotDALCore.Repos
{
    public class InventoryRepo : BaseRepo<Inventory>, IInventoryRepo
    {
        public InventoryRepo() { }

        public InventoryRepo(AutoLotContext context) : base(context) { }

        public List<Inventory> Search(string searchString)
        {
            return Context.Cars.Where(c => c.PetName.Contains(searchString)).ToList();
        }

        public List<Inventory> GetPinkCars()
        {
            return GetSome(x => x.Color == "Pink");
        }

        public List<Inventory> GetRelatedData()
        {
            return Context.Cars.FromSqlRaw("SELECT * FROM Inventory")
                .Include(x => x.Orders)
                .ThenInclude(x => x.Customer)
                .ToList();
        }

        public override List<Inventory> GetAll()
        {
            return GetAll(x => x.PetName, true).ToList();
        }
    }
}