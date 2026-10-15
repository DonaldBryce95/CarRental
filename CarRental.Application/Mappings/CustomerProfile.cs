using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
  public class CustomerProfile : Profile
  {
    public CustomerProfile()
    {
      CreateMap<Customer, CustomerProfileDto>();
    }
  }
}