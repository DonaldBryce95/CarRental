using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
  public class MaintenanceReportProfile : Profile
  {
    public MaintenanceReportProfile()
    {
      CreateMap<MaintenanceReport, ReportDto>().ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.MaintenanceWorker.FirstName)).ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.MaintenanceWorker.LastName)).ForMember(dest => dest.MaintenanceWorkerId, opt => opt.MapFrom(src => src.MaintenanceWorker.MaintenanceWorkerId)).ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.MaintenanceTask.Car.Make)).ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.MaintenanceTask.Car.Model));
    }
  }
}