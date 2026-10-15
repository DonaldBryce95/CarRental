using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;

namespace CarRental.Application.Services
{
  public class CustomerService : ICustomerService
  {
    private readonly ICustomerRepository _customerRepository;

    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _mapper = mapper;
    }

    public async Task<BookingDto> CalculateBookingAsync(CalculateBookingDto calculateBookingDto)
    {
      if(calculateBookingDto == null)
      {
        throw new ArgumentNullException(nameof(calculateBookingDto));
      }

      if(calculateBookingDto.CarId <= 0)
      {
        throw new ArgumentException("Invalid Car ID.");
      }

      var car = await _customerRepository.GetCarByIdAsync(calculateBookingDto.CarId);

      if(car == null)
      {
        throw new ArgumentException($"Car ID: {calculateBookingDto.CarId}, could not be found.");
      }

      if(!ValidateBookingDates(
      calculateBookingDto.PickUpDate,
      calculateBookingDto.ReturnDate,
      calculateBookingDto.PickUpTime,
      calculateBookingDto.ReturnTime))
    {
        throw new ArgumentException("Invalid booking dates or times.");
    }

      var numberOfDays = calculateBookingDto.ReturnDate.DayNumber - calculateBookingDto.PickUpDate.DayNumber;

      var totalPrice = numberOfDays * car.VehicleType.PricePerDay;

      var calculatedBooking = new BookingDto
      {
        CarId = calculateBookingDto.CarId,
        PickUpDate = calculateBookingDto.PickUpDate,
        ReturnDate = calculateBookingDto.ReturnDate,
        PickUpTime = calculateBookingDto.PickUpTime,
        ReturnTime = calculateBookingDto.ReturnTime,
        NumberOfDays = numberOfDays,
        PricePerDay = car.VehicleType.PricePerDay,
        TotalPrice = totalPrice
      };

      return calculatedBooking;
    }


  public async Task<GuestReservationDto> CreateReservationAsync(CreateReservationDto createReservationDto)
  {
    if (createReservationDto == null)
    {
      throw new ArgumentNullException(nameof(createReservationDto));
    }

    if(!ValidateBookingDates(
    createReservationDto.PickUpDate,
    createReservationDto.ReturnDate,
    createReservationDto.PickUpTime,
    createReservationDto.ReturnTime))

    {
      throw new ArgumentException("Invalid booking dates or times.");
    }

    var pickUpBranch = await _customerRepository.GetBranchByIdAsync(createReservationDto.PickUpBranchId);

    if (pickUpBranch == null)
    {
      throw new ArgumentException($"Pick Up Branch ID: {createReservationDto.PickUpBranchId} could not be found");
    }

    var returnBranch = await _customerRepository.GetBranchByIdAsync(createReservationDto.ReturnBranchId);

    if (returnBranch == null)
    {
      throw new ArgumentException($"Return Branch ID: {createReservationDto.ReturnBranchId} could not be found");
    }

    var car = await _customerRepository.GetCarByIdAsync(createReservationDto.CarId);

    if (car == null)
    {
      throw new ArgumentException($"Car ID: {createReservationDto.CarId} could not be found");
    }

    var isCarAvailable = await _customerRepository.GetAvailableCarAsync(createReservationDto.PickUpBranchId, createReservationDto.CarId, createReservationDto.PickUpDate, createReservationDto.ReturnDate);

    if(isCarAvailable == null)
    {
      throw new ArgumentException($"Car ID: {createReservationDto.CarId} is not available");
    }

    Customer customer;

    var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(createReservationDto.Email);

    if (existingCustomer == null)
    {
      var newCustomer = new Customer
      {
        FirstName = createReservationDto.FirstName,
        LastName = createReservationDto.LastName,
        Email = createReservationDto.Email,
        PhoneNumber = createReservationDto.PhoneNumber
      };

      customer = await _customerRepository.CreateCustomerAsync(newCustomer);
    }
    else
    {
      customer = existingCustomer;
    }

    var reservation = new Reservation
    {
      CustomerId = customer.CustomerId,
      Customer = customer,
      CarId = createReservationDto.CarId,
      PickUpDate = createReservationDto.PickUpDate,
      ReturnDate = createReservationDto.ReturnDate,
      PickUpTime = createReservationDto.PickUpTime,
      ReturnTime = createReservationDto.ReturnTime,
      PickUpBranchId = createReservationDto.PickUpBranchId,
      ReturnBranchId = createReservationDto.ReturnBranchId,
      ReservationStatus = ReservationStatus.Confirmed
    };

    var calculateBooking = new CalculateBookingDto
    {
      CarId = createReservationDto.CarId,
      PickUpDate = createReservationDto.PickUpDate,
      ReturnDate = createReservationDto.ReturnDate,
      PickUpTime = createReservationDto.PickUpTime,
      ReturnTime = createReservationDto.ReturnTime
    };

    var calculatedBooking = await CalculateBookingAsync(calculateBooking);

    reservation.TotalPrice = calculatedBooking.TotalPrice;

    var reservationCar = new ReservationCar
    {
      Reservation = reservation,
      CarId = createReservationDto.CarId
    };

    reservation.ReservationCars.Add(reservationCar);

    var createdReservation = await _customerRepository.CreateReservationAsync(reservation);

    return _mapper.Map<GuestReservationDto>(createdReservation);
  }

    public async Task<ReservationDto> CreateCustomerReservationAsync(CreateReservationDto createReservationDto, int customerId)
    {
      if(createReservationDto == null)
      {
        throw new ArgumentNullException(nameof(createReservationDto));
      }

      if(!ValidateBookingDates(
      createReservationDto.PickUpDate,
      createReservationDto.ReturnDate,
      createReservationDto.PickUpTime,
      createReservationDto.ReturnTime))

      {
        throw new ArgumentException("Invalid booking dates or times.");
      }

      var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

      if(customer == null)
      {
        throw new ArgumentException($"Customer ID: {customerId} was not found.");
      }

      var pickUpBranch = await _customerRepository.GetBranchByIdAsync(createReservationDto.PickUpBranchId);

      if(pickUpBranch == null)
      {
        throw new ArgumentException($"Pick Up Branch ID: {createReservationDto.PickUpBranchId} could not be found");
      }

        var returnBranch = await _customerRepository.GetBranchByIdAsync(createReservationDto.ReturnBranchId);

      if(returnBranch == null)
      {
        throw new ArgumentException($"Return Branch ID: {createReservationDto.ReturnBranchId} could not be found");
      }

      var car = await _customerRepository.GetCarByIdAsync(createReservationDto.CarId);

      if(car == null)
      {
        throw new ArgumentException($"Car ID: {createReservationDto.CarId} could not be found");
      }

      var isCarAvailable = await _customerRepository.GetAvailableCarAsync(createReservationDto.PickUpBranchId, createReservationDto.CarId, createReservationDto.PickUpDate, createReservationDto.ReturnDate);

      if(isCarAvailable == null)
      {
        throw new ArgumentException($"Car ID: {createReservationDto.CarId} is not available");
      }

      var reservation = new Reservation
      {
        CustomerId = customerId,
        CarId = createReservationDto.CarId,
        PickUpDate = createReservationDto.PickUpDate,
        ReturnDate = createReservationDto.ReturnDate,
        PickUpTime = createReservationDto.PickUpTime,
        ReturnTime = createReservationDto.ReturnTime,
        PickUpBranchId = createReservationDto.PickUpBranchId,
        ReturnBranchId = createReservationDto.ReturnBranchId,
        ReservationStatus = ReservationStatus.Confirmed
      };

      var calculateBooking = new CalculateBookingDto
      {
        CarId = createReservationDto.CarId,
        PickUpDate = createReservationDto.PickUpDate,
        ReturnDate = createReservationDto.ReturnDate,
        PickUpTime = createReservationDto.PickUpTime,
        ReturnTime = createReservationDto.ReturnTime
      };

      var calculatedBooking = await CalculateBookingAsync(calculateBooking);

      reservation.TotalPrice = calculatedBooking.TotalPrice;

      var reservationCar = new ReservationCar
      {
        Reservation = reservation,
        CarId = createReservationDto.CarId
      };

      reservation.ReservationCars.Add(reservationCar);

      var createdReservation = await _customerRepository.CreateReservationAsync(reservation);

      return _mapper.Map<ReservationDto>(createdReservation);
    }

    public async Task<IEnumerable<BranchDto>> GetAllBranchesAsync()
    {
      var branches = await _customerRepository.GetAllBranchesAsync();

      return _mapper.Map<IEnumerable<BranchDto>>(branches);
    }

    public async Task<IEnumerable<OurFleetDto>> GetAllCarsAsync()
    {
      var cars = await _customerRepository.GetAllCarsAsync();

      return _mapper.Map<IEnumerable<OurFleetDto>>(cars);
    }

    public async Task<PagedResult<AvailableCarDto>> GetAvailableCarsAsync(int branchId, DateOnly pickUpDate, DateOnly returnDate, TimeSpan pickUpTime, TimeSpan returnTime, int page, int pageSize)
    {
      if(branchId <= 0)
      {
        throw new ArgumentException("Invalid branch ID.");
      }

      if(!ValidateBookingDates(pickUpDate, returnDate, pickUpTime, returnTime))
      {
        throw new ArgumentException("Invalid Dates Or Times");
      }

      var cars = await _customerRepository.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, page, pageSize);

      var availableCars = _mapper.Map<PagedResult<AvailableCarDto>>(cars);

      return availableCars;
    }

    public async Task<AvailableCarDto> GetAvailableCarByCarIdAsync(int branchId, int carId, DateOnly pickUpDate, DateOnly returnDate, TimeSpan pickUpTime, TimeSpan returnTime)
    {
      if(carId <= 0)
      {
        throw new ArgumentException("Invalid car ID.");
      }

      if(branchId <= 0)
      {
        throw new ArgumentException("Invalid branch ID.");
      }

      var car = await _customerRepository.GetCarByIdAsync(carId);

      if (car == null)
      {
        throw new ArgumentException($"Car ID: {carId} was not found.");
      }

      if(!ValidateBookingDates(pickUpDate, returnDate, pickUpTime, returnTime))
      {
        throw new ArgumentException("Invalid Dates Or Times.");
      }

      var availableCar = await _customerRepository.GetAvailableCarAsync(branchId, carId, pickUpDate, returnDate);

      if(availableCar == null)
      {
        throw new ArgumentException($"No Cars available for these dates");
      }

      var availableCarDto = new AvailableCarDto
      {
        CarId = availableCar.CarId,
        Make = availableCar.Make,
        Model = availableCar.Model,
        VehicleCategories = availableCar.VehicleType.VehicleCategories,
        NumberOfDoors = availableCar.NumberOfDoors,
        TransmissionType = availableCar.TransmissionType,
        EngineCapacity = availableCar.EngineCapacity,
        HasAC = availableCar.HasAC,
        PricePerDay = availableCar.VehicleType.PricePerDay
      };

      return availableCarDto;
    }

    public async Task<PagedResult<CarGroupDto>> GetCarsByGroupAsync(int page, int pageSize)
    {
      var cars = await _customerRepository.GetCarsByGroupAsync(page, pageSize);

      return _mapper.Map<PagedResult<CarGroupDto>>(cars);
    }

    public async Task<AdminReservationDto?> GetReservationByIdAsync(int reservationId)
    {
      var reservation = await _customerRepository.GetReservationByIdAsync(reservationId);

      if(reservation == null)
      {
        return null;
      }

      return _mapper.Map<AdminReservationDto>(reservation);
    }

    private bool ValidateBookingDates(DateOnly pickUpDate, DateOnly returnDate, TimeSpan pickUpTime, TimeSpan returnTime)
    {
      return pickUpDate >= DateOnly.FromDateTime(DateTime.Now) && returnDate >= pickUpDate && (pickUpDate != returnDate || pickUpTime < returnTime);
    }

    public async Task<PagedResult<MyReservationsDto>> GetReservationsByCustomerIdAsync(int customerId, int page, int pageSize)
    {
      var reservations = await _customerRepository.GetReservationsByCustomerIdAsync(customerId, page, pageSize);

      return _mapper.Map<PagedResult<MyReservationsDto>>(reservations);
    }

    public async Task<CustomerProfileDto?> GetCustomerProfileByIdAsync(int customerId)
    {
      var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

      if(customer == null)
      {
        return null;
      }

      return _mapper.Map<CustomerProfileDto>(customer);
    }
  }
}