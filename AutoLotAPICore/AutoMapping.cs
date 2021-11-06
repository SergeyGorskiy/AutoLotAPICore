using AutoLotDALCore.Models;
using AutoMapper;

namespace AutoLotAPICore
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Inventory, Inventory>()
                .ForMember(x => x.Orders, 
                    opt => opt.Ignore());
        }
    }
}