using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces
{
  public interface ICustomerRepository
  {
    Task<IEnumerable<Branch>> GetAllBranchesAsync();

    Task<PagedResult<Car>> GetAvailableCarsAsync(int branchId, DateOnly pickUpDate, DateOnly returnDate, int page, int pageSize);

    Task<Car?> GetAvailableCarAsync(int branchId, int carId,
    DateOnly pickUpDate, DateOnly returnDate);

    Task<IEnumerable<Car>> GetAllCarsAsync();

    Task<PagedResult<Car>> GetCarsByGroupAsync(int page, int pageSize);

    Task<Car?> GetCarByIdAsync(int carId);

    Task<Reservation> CreateReservationAsync(Reservation reservation);

    Task<Branch?> GetBranchByIdAsync(int branchId);

    Task<Customer?> GetCustomerByEmailAsync(string email);

    Task<Customer> CreateCustomerAsync(Customer customer);

    Task<Reservation?> GetReservationByIdAsync(int reservationId);

    Task<PagedResult<Reservation>> GetReservationsByCustomerIdAsync(int customerId, int page, int pageSize);

    Task<Customer?> GetCustomerByIdAsync(int customerId);
  }

}