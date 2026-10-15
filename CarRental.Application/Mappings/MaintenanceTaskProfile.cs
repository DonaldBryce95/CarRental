using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Mappings
{
  public class MaintenanceTaskProfile : Profile
  {
    public MaintenanceTaskProfile()
    {
      CreateMap<MaintenanceTask, MaintenanceTaskDto>().ForMember(dest => dest.MaintenanceReportStatus, opt => opt.MapFrom(src => src.MaintenanceReport.MaintenanceReportStatus)).ForMember(dest => dest.InspectionResult, opt => opt.MapFrom(src => src.MaintenanceReport.OverallResult)).ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Car.Make)).ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Car.Model));
    }
  }
}