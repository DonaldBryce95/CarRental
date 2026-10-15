using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
  public class MaintenanceWorkerProfile : Profile
  {
    public MaintenanceWorkerProfile()
    {
      CreateMap<MaintenanceWorker, MaintenanceWorkerDto>();

      CreateMap<AssignRoleDto, MaintenanceWorker>();
    }
  }
}