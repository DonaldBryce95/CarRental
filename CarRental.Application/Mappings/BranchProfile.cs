using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
  public class BranchProfile : Profile
  {
    public BranchProfile()
    {
      CreateMap<CreateBranchDto, Branch>();

      CreateMap<Branch, BranchDto>();

      CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
    }
  }
}