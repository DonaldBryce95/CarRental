using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces
{
  public interface ICustomerService
  {
    Task<IEnumerable<BranchDto>> GetAllBranchesAsync();

    Task<PagedResult<AvailableCarDto>> GetAvailableCarsAsync(int branchId, DateOnly pickUpDate, DateOnly returnDate, TimeSpan pickUpTime, TimeSpan returnTime, int page, int pageSize);

    Task<AvailableCarDto> GetAvailableCarByCarIdAsync(int branchId, int carId, DateOnly pickUpDate, DateOnly returnDate, TimeSpan pickUpTime, TimeSpan returnTime);

    Task<IEnumerable<OurFleetDto>> GetAllCarsAsync();

    Task<BookingDto> CalculateBookingAsync(CalculateBookingDto calculateBookingDto);

    Task<GuestReservationDto> CreateReservationAsync(CreateReservationDto createReservationDto);

    Task<ReservationDto> CreateCustomerReservationAsync(CreateReservationDto createReservationDto, int customerId);

    Task<AdminReservationDto?> GetReservationByIdAsync(int reservationId);

    Task<PagedResult<CarGroupDto>> GetCarsByGroupAsync(int page, int pageSize);

    Task<PagedResult<MyReservationsDto>> GetReservationsByCustomerIdAsync(int customerId, int page, int pageSize);

    Task<CustomerProfileDto?> GetCustomerProfileByIdAsync(int customerId);
  }
}