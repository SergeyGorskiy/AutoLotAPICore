using System.ComponentModel.DataAnnotations;
using AutoLotDALCore.Models.Base;

namespace AutoLotDALCore.Models
{
    public partial class CreditRisk : EntityBase
    {
        [StringLength(50)] 
        public string FirstName { get; set; }

        [StringLength(50)] 
        public string LastName { get; set; }
    }
}