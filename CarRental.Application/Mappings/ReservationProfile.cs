using AutoMapper;
using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class ReservationProfile : Profile
  {
    public ReservationProfile()
    {
      CreateMap<Reservation, ReservationDto>();

      CreateMap<Reservation, AdminReservationDto>().ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName)).ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Customer.LastName)).ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Customer.Email)).ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber)).ForMember(dest => dest.PickUpBranchName, opt => opt.MapFrom(src => src.PickUpBranch.BranchName)).ForMember(dest => dest.ReturnBranchName, opt => opt.MapFrom(src => src.ReturnBranch.BranchName)).ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Car.Make)).ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Car.Model)).ForMember(dest => dest.CarStatus, opt => opt.MapFrom(src => src.Car.CarStatus));

      CreateMap<Reservation, GuestReservationDto>().ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName)).ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Customer.LastName)).ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Customer.Email)).ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber));

      CreateMap<Reservation, MyReservationsDto>().ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Car.Make)).ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Car.Model)).ForMember(dest => dest.PickUpBranchName, opt => opt.MapFrom(src => src.PickUpBranch.BranchName)).ForMember(dest => dest.ReturnBranchName, opt => opt.MapFrom(src => src.ReturnBranch.BranchName));
    }
  }
}