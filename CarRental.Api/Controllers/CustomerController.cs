using System.Security.Claims;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CustomerController : ControllerBase
  {
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
      _customerService = customerService;
    }

    [HttpGet("branches")]
    public async Task<ActionResult<IEnumerable<BranchDto>>> GetAllBranches()
    {
      var branches = await _customerService.GetAllBranchesAsync();

      return Ok(branches);
    }

    [HttpGet("cars")]
    public async Task<ActionResult<IEnumerable<OurFleetDto>>> GetAllCars()
    {
      var cars = await _customerService.GetAllCarsAsync();

      return Ok(cars);
    }

    [HttpGet("available-cars/{branchId}")]
    public async Task<ActionResult<PagedResult<AvailableCarDto>>> GetAvailableCars(int branchId, DateOnly pickUpDate, DateOnly returnDate, TimeSpan pickUpTime, TimeSpan returnTime, int page = 1, int pageSize = 10)
    {
      var availableCars = await _customerService.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, pickUpTime, returnTime, page, pageSize);

      return Ok(availableCars);
    }

    [HttpGet("available-cars/{carId}/check")]
    public async Task<ActionResult<AvailableCarDto>> GetAvailableCarById(int branchId, int carId, DateOnly pickUpDate, DateOnly returnDate, TimeSpan pickUpTime, TimeSpan returnTime)
    {

      var car = await _customerService.GetAvailableCarByCarIdAsync(branchId, carId, pickUpDate, returnDate, pickUpTime, returnTime);

      if(car == null)
      {
        return NotFound();
      }

      return Ok(car);
    }

    [HttpPost("calculate-booking")]
    public async Task<ActionResult<BookingDto>> CalculateBooking(CalculateBookingDto calculateBookingDto)
    {
      var calculatedBooking = await _customerService.CalculateBookingAsync(calculateBookingDto);

      return Ok(calculatedBooking);
    }

    [HttpPost("create-reservation")]
    public async Task<ActionResult<GuestReservationDto>> CreateReservation(CreateReservationDto createReservationDto)
    {
      var reservation = await _customerService.CreateReservationAsync(createReservationDto);

      return Ok(reservation);
    }

    [Authorize(Roles = "Customer")]
    [HttpPost("create-customer-reservation")]
    public async Task<ActionResult<ReservationDto>> CreateCustomerReservation(CreateReservationDto createReservationDto)
    {

      var customerIdClaim = User.FindFirst("CustomerId");

      if(customerIdClaim == null)
      {
        return Forbid();
      }

      var customerId = int.Parse(customerIdClaim.Value);

      var reservation = await _customerService.CreateCustomerReservationAsync(createReservationDto, customerId);

      return Ok(reservation);
    }

    [HttpGet("grouped-cars")]
    public async Task<ActionResult<PagedResult<CarGroupDto>>> GetCarsByGroup(int page, int pageSize = 10)
    {
      var cars = await _customerService.GetCarsByGroupAsync(page, pageSize);

      return Ok(cars);
    }

    [Authorize(Roles = "Customer,Admin")]
    [HttpGet("reservations/{reservationId}")]
    public async Task<ActionResult<AdminReservationDto>> GetReservationById(int reservationId)
    {
      var reservation = await _customerService.GetReservationByIdAsync(reservationId);

      if(reservation == null)
      {
        return NotFound();
      }

      if(User.IsInRole("Admin"))
      {
          return Ok(reservation);
      }

      var customerIdClaim = User.FindFirstValue("CustomerId");

      if(customerIdClaim == null)
      {
          return Forbid();
      }

      var customerId = int.Parse(customerIdClaim);

      if(reservation.CustomerId != customerId)
      {
          return Forbid();
      }

      return Ok(reservation);
    }

    [Authorize(Roles = "Customer")]
    [HttpGet("my-reservations")]
    public async Task<ActionResult<PagedResult<MyReservationsDto>>> GetMyReservations(int page = 1, int pageSize = 10)
    {
      var customerIdClaim = User.FindFirstValue("CustomerId");

      if (customerIdClaim == null)
      {
        return Forbid();
      }

      var customerId = int.Parse(customerIdClaim);

      var reservations = await _customerService.GetReservationsByCustomerIdAsync(customerId, page, pageSize);

      return Ok(reservations);
    }

    [Authorize(Roles = "Customer")]
    [HttpGet("customer-profile")]
    public async Task<ActionResult<CustomerProfileDto>> GetCustomerProfile()
    {
      var customerIdClaim = User.FindFirst("CustomerId");

      if(customerIdClaim == null)
      {
        return Forbid();
      }

       var customerId = int.Parse(customerIdClaim.Value);

       var customer = await _customerService.GetCustomerProfileByIdAsync(customerId);

      if(customer == null)
      {
        return NotFound();
      }

       return Ok(customer);
    }
  }
}