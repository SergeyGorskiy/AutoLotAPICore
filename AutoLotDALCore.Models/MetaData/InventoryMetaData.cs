using System.ComponentModel.DataAnnotations;

namespace AutoLotDALCore.Models.MetaData
{
    public class InventoryMetaData
    {
        [Display(Name = "PetName")] 
        public string PetName;

        [StringLength(50, ErrorMessage = "Please enter a value less than 50 characters long.")]
        public string Make;
    }
}