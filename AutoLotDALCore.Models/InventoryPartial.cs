using System.ComponentModel.DataAnnotations.Schema;
using AutoLotDALCore.Models.MetaData;
using Microsoft.AspNetCore.Mvc;

namespace AutoLotDALCore.Models
{
    [ModelMetadataType(typeof(InventoryMetaData))]
    public partial class Inventory
    {
        public override string ToString()
        {
            return $"{this.PetName ?? "**No Name**"} is a {this.Color} {this.Make} with ID {this.Id}.";
        }

        [NotMapped] 
        public string MakeColor => $"{Make} + ({Color})";
    }
}