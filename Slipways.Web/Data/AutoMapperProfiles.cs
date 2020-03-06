using AutoMapper;
using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Models;

namespace com.b_velop.Slipways.Web.Data
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Slipway, SlipwayDto>();
            CreateMap<SlipwayDto, Slipway>();

            CreateMap<Water, WaterDto>();
            CreateMap<WaterDto, Water>();
        }
    }
}
