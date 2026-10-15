using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories
{
  public class CustomerRepository : ICustomerRepository
  {
    private readonly CarRentalDbContext _context;

    public CustomerRepository(CarRentalDbContext context)
    {
      _context = context;
    }

    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
      await _context.Customers.AddAsync(customer);
      await _context.SaveChangesAsync();

      return customer;
    }

    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
      await _context.AddAsync(reservation);
      await _context.SaveChangesAsync();

      return reservation;
    }

    public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
    {

      var branches = await _context.Branches.AsNoTracking().OrderBy(b => b.BranchId).ToListAsync();

      return branches;
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {

      var cars = await _context.Cars.Include(c => c.VehicleType).AsNoTracking().GroupBy(c => new
      {
        c.Make,
        c.Model,
        c.EngineCapacity,
        c.VehicleType.VehicleCategories
      }).Select(g => g.First()).ToListAsync();

      return cars;
    }

    public async Task<PagedResult<Car>> GetAvailableCarsAsync(int branchId, DateOnly pickUpDate, DateOnly returnDate, int page, int pageSize)
    {
      var totalCount = await _context.Cars.Where(c => c.BranchId == branchId && c.CarStatus == CarStatus.Available && !c.ReservationCars.Any(rc => rc.Reservation.ReservationStatus == ReservationStatus.Confirmed && rc.Reservation.PickUpDate < returnDate && rc.Reservation.ReturnDate > pickUpDate)).CountAsync();

      var skip = (page - 1) * pageSize;

      var availableCars = await _context.Cars.Include(c => c.VehicleType).Where(c => c.BranchId == branchId && c.CarStatus == CarStatus.Available && !c.ReservationCars.Any(rc => rc.Reservation.ReservationStatus == ReservationStatus.Confirmed && rc.Reservation.PickUpDate < returnDate && rc.Reservation.ReturnDate > pickUpDate)).Skip(skip).Take(pageSize).ToListAsync();

      return new PagedResult<Car>
      {
        Items = availableCars,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }

    public async Task<Branch?> GetBranchByIdAsync(int branchId)
    {
      return await _context.Branches.FirstOrDefaultAsync(b => b.BranchId == branchId);
    }

    public async Task<Car?> GetCarByIdAsync(int carId)
    {
      return await _context.Cars.Include(cv => cv.VehicleType).FirstOrDefaultAsync(c => c.CarId == carId);
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
      var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

      return customer;
    }

    public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
    {
      return await _context.Reservations.Include(r => r.Customer).FirstOrDefaultAsync(r => r.ReservationId == reservationId);
    }

    public async Task<PagedResult<Reservation>> GetReservationsByCustomerIdAsync(int customerId, int page, int pageSize)
    {
      var totalCount = await _context.Reservations.Where(r => r.CustomerId == customerId).CountAsync();

      var skip = (page - 1) * pageSize;

      var reservations = await _context.Reservations.Include(r => r.Car).Include(r => r.PickUpBranch).Include(r => r.ReturnBranch).Where(r => r.CustomerId == customerId)
      .Skip(skip).Take(pageSize).ToListAsync();

      return new PagedResult<Reservation>
      {
        Items = reservations,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
      return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }

    public async Task<Car?> GetAvailableCarAsync(int branchId, int carId, DateOnly pickUpDate, DateOnly returnDate)
    {

      var availableCar = _context.Cars.Include(c => c.VehicleType).Where(c => c.BranchId == branchId && c.CarId == carId && c.CarStatus == CarStatus.Available && !c.ReservationCars.Any(rc => rc.Reservation.ReservationStatus == ReservationStatus.Confirmed && rc.Reservation.PickUpDate < returnDate && rc.Reservation.ReturnDate > pickUpDate));

      var car = await availableCar.FirstOrDefaultAsync();

      return car;
    }

    public async Task<PagedResult<Car>> GetCarsByGroupAsync(int page, int pageSize)
    {
      var totalCount = await _context.Cars.CountAsync();

      var skip = (page - 1) * pageSize;

      var groupedCars = await _context.Cars.Include(c => c.VehicleType).OrderBy(c => c.VehicleType.VehicleCategories).Skip(skip).Take(pageSize).ToListAsync();

      return new PagedResult<Car>
      {
        Items = groupedCars,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }
  }
}