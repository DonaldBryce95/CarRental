using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
  public class VehicleTypeProfile : Profile
  {
    public VehicleTypeProfile()
    {
      CreateMap<CreateVehicleTypeDto, VehicleType>();

      CreateMap<VehicleType, VehicleTypeDto>();
    }
  }
}