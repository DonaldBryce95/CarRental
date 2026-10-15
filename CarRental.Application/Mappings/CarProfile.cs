using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
  public class CarProfile : Profile
  {
    public CarProfile()
    {
      CreateMap<CreateCarDto, Car>();

      CreateMap<Car, CarDto>();

      CreateMap<Car, OurFleetDto>().ForMember(dest => dest.VehicleCategories, opt => opt.MapFrom(src => src.VehicleType.VehicleCategories));

      CreateMap<Car, AvailableCarDto>().ForMember(dest => dest.VehicleCategories, opt => opt.MapFrom(src => src.VehicleType.VehicleCategories)).ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.VehicleType.PricePerDay));

      CreateMap<Car, CarGroupDto>().ForMember(dest => dest.VehicleCategories, opt => opt.MapFrom(src => src.VehicleType.VehicleCategories));

      CreateMap<Car, AdminCarDto>().ForMember(dest => dest.VehicleCategories, opt => opt.MapFrom(src => src.VehicleType.VehicleCategories)).ForMember(dest => dest.PricePerDay, opt => opt.MapFrom(src => src.VehicleType.PricePerDay));
    }
  }
}