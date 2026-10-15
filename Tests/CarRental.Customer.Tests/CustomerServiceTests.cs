using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using FluentAssertions;
using Moq;

namespace CarRental.Customer.Tests;
public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;

    private readonly Mock<IMapper> _mapperMock;

    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();

        _mapperMock = new Mock<IMapper>();

        _customerService = new CustomerService(_customerRepositoryMock.Object, _mapperMock.Object);
    }

    private static DateOnly GetDate(int days) => DateOnly.FromDateTime(DateTime.Now).AddDays(days);

    private static TimeSpan GetTime(int hours) => TimeSpan.FromHours(hours);

    [Fact]
    public async Task GetAvailableCarsAsync_ShouldThrow_WhenPickUpDate_IsBeforeToday()
    {
        //Arrange
        int branchId = 1;
        DateOnly pickUpDate = GetDate(-1);
        DateOnly returnDate = GetDate(1);
        TimeSpan pickUpTime = GetTime(8);
        TimeSpan returnTime = GetTime(10);

        int page = 1;
        int pageSize = 10;

        // Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, pickUpTime, returnTime, page, pageSize));

        _customerRepositoryMock.Verify(
        x => x.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, page, pageSize),Times.Never);
    }

    [Fact]
    public async Task GetAvailableCarsAsync_ShouldThrow_WhenReturnDate_IsBeforePickUpDate()
    {
        //Arrange
        int branchId = 1;
        DateOnly pickUpDate = GetDate(2);
        DateOnly returnDate = GetDate(1);
        TimeSpan pickUpTime = GetTime(8);
        TimeSpan returnTime = GetTime(10);

        int page = 1;
        int pageSize = 10;

        //Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, pickUpTime, returnTime, page, pageSize));

        _customerRepositoryMock.Verify(
        x => x.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, page, pageSize),Times.Never);
    }

    [Fact]
    public async Task GetAvailableCarsAsync_ShouldThrow_WhenPickUpDateAndReturnDateAreEqual_ButPickUpTimeIsAfterReturnTime()
    {
        //Arrange
        int branchId = 1;
        DateOnly pickUpDate = GetDate(1);
        DateOnly returnDate = GetDate(1);
        TimeSpan pickUpTime = GetTime(10);
        TimeSpan returnTime = GetTime(8);

        int page = 1;
        int pageSize = 10;

        //Act + Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, pickUpTime, returnTime, page, pageSize));

        _customerRepositoryMock.Verify(
        x => x.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, page, pageSize),Times.Never);
    }

    [Fact]
    public async Task GetAvailableCarsAsync_ShouldReturn_WhenDataIsValid()
    {
        //Arrange
        int branchId = 1;
        DateOnly pickUpDate = GetDate(2);
        DateOnly returnDate = GetDate(5);
        TimeSpan pickUpTime = GetTime(10);
        TimeSpan returnTime = GetTime(8);
        int page = 1;
        int pageSize = 10;

        var cars = new PagedResult<Car>()
        {
            Items = new List<Car>
            {
                new Car
                {
                    CarId = 1,
                    CarStatus = CarStatus.Available,
                    BranchId = 1,
                    VehicleTypeId = 1,
                    Make = "Honda",
                    Model = "Civic",
                    NumberOfDoors = 5,
                    TransmissionType = 0,
                    EngineCapacity = 1.0m,
                    HasAC = true
                }
            },
            Page = page,
            PageSize = pageSize,
            TotalCount = 1
        };

        var carDto = new PagedResult<AvailableCarDto>
        {
            Items = new List<AvailableCarDto>
            {
                new AvailableCarDto
                {
                    CarId = 1,
                    Make = "Honda",
                    Model = "Civic",
                    VehicleCategories = VehicleCategories.Economy,
                    NumberOfDoors = 5,
                    TransmissionType = 0,
                    EngineCapacity = 1.0m,
                    HasAC = true,
                    PricePerDay = 45
                }
            },
            Page = page,
            PageSize = pageSize,
            TotalCount = 1
        };

        _customerRepositoryMock.Setup(x => x.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, page, pageSize)).ReturnsAsync(cars);

        _mapperMock.Setup(x => x.Map<PagedResult<AvailableCarDto>>(cars)).Returns(carDto);

        var result = await _customerService.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, pickUpTime, returnTime, page, pageSize);

        result.Should().NotBeNull();
        var car = result.Items.First();

        car.CarId.Should().Be(1);
        car.Make.Should().Be("Honda");
        car.Model.Should().Be("Civic");

        _customerRepositoryMock.Verify(
        x => x.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, page, pageSize),Times.Once);
    }

    [Fact]
    public async Task GetAvailableCarsAsync_ShouldReturnEmpty_WhenNoCarExists()
    {
        //Arrange
        int branchId = 1;
        DateOnly pickUpDate = GetDate(2);
        DateOnly returnDate = GetDate(5);
        TimeSpan pickUpTime = GetTime(10);
        TimeSpan returnTime = GetTime(8);

        int page = 1;
        int pageSize = 10;

        var cars = new PagedResult<Car>()
        {
            Items = new List<Car>(),
            Page = page,
            PageSize = pageSize,
            TotalCount = 0
        };

        var carDto = new PagedResult<AvailableCarDto>
        {
            Items = new List<AvailableCarDto>(),
            Page = page,
            PageSize = pageSize,
            TotalCount = 0
        };

        _customerRepositoryMock.Setup(x => x.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, page, pageSize)).ReturnsAsync(cars);

        _mapperMock.Setup(x => x.Map<PagedResult<AvailableCarDto>>(cars)).Returns(carDto);

        var result = await _customerService.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, pickUpTime, returnTime, page, pageSize);

        result.Items.Should().BeEmpty();
        result.Page.Should().Be(page);
        result.PageSize.Should().Be(pageSize);
        result.TotalCount.Should().Be(0);

        _customerRepositoryMock.Verify(
        x => x.GetAvailableCarsAsync(branchId, pickUpDate, returnDate, page, pageSize),Times.Once);
    }

    [Fact]
    public async Task CalculateBookingAsync_ShouldThrow_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _customerService.CalculateBookingAsync(null));
    }

    [Fact]
    public async Task CalculateBookingAsync_ShouldThrow_WhenCarIdIsNotFound()
    {
        var dto = new CalculateBookingDto
        {
            CarId = 999,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10)
        };

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(999)).ReturnsAsync((Car?) null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CalculateBookingAsync(dto));
    }

    [Fact]
    public async Task CalculateBookingAsync_ShouldCalculateBooking_WhenDataIsValid()
    {
        var dto = new CalculateBookingDto
        {
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            PricePerDay = 40,
            VehicleCategories = VehicleCategories.Economy
        };

        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 1,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        var result = await _customerService.CalculateBookingAsync(dto);

        var expectedNumberOfDays = dto.ReturnDate.DayNumber - dto.PickUpDate.DayNumber;

        result.NumberOfDays.Should().Be(expectedNumberOfDays);
        result.PricePerDay.Should().Be(vehicleType.PricePerDay);
        result.TotalPrice.Should().Be(expectedNumberOfDays * vehicleType.PricePerDay);

        result.CarId.Should().Be(dto.CarId);
        result.PickUpDate.Should().Be(dto.PickUpDate);
        result.ReturnDate.Should().Be(dto.ReturnDate);
        result.PickUpTime.Should().Be(dto.PickUpTime);
        result.ReturnTime.Should().Be(dto.ReturnTime);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrow_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _customerService.CreateReservationAsync(null));
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrow_WhenPickUpBranchDoesNotExist()
    {
        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07898763564",
            Email = "johndoe@Example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(999)).ReturnsAsync((Branch?) null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateReservationAsync(dto));
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrow_WhenReturnBranchDoesNotExist()
    {
        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07898763564",
            Email = "johndoe@Example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        var branch = new Branch
        {
            BranchId = 999,
            BranchName = "Airport Branch",
            Address = "123 Main Street",
            PhoneNumber = "07847363535",
            OpeningHours = "08:00-16:00"
        };

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(999)).ReturnsAsync(branch);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(111)).ReturnsAsync((Branch?) null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateReservationAsync(dto));
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrow_WhenCarDoesNotExist()
    {
        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07898763564",
            Email = "johndoe@Example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        var branch1 = new Branch
        {
            BranchId = 999,
            BranchName = "Airport Branch",
            Address = "123 Main Street",
            PhoneNumber = "07847363535",
            OpeningHours = "08:00-16:00"
        };

        var branch2 = new Branch
        {
            BranchId = 111,
            BranchName = "Downtown Branch",
            Address = "321 Main Street",
            PhoneNumber = "07872525636",
            OpeningHours = "08:00-16:00"
        };


        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(999)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(111)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync((Car?) null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateReservationAsync(dto));
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldThrow_WhenCarIsNotAvailable()
    {

        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07898763564",
            Email = "johndoe@Example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var branch1 = new Branch
        {
            BranchId = 999,
            BranchName = "Airport Branch",
            Address = "123 Main Street",
            PhoneNumber = "07847363535",
            OpeningHours = "08:00-16:00"
        };

        var branch2 = new Branch
        {
            BranchId = 111,
            BranchName = "Downtown Branch",
            Address = "321 Main Street",
            PhoneNumber = "07872525636",
            OpeningHours = "08:00-16:00"
        };


        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(999)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(111)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(dto.PickUpBranchId, dto.CarId, dto.PickUpDate, dto.ReturnDate)).ReturnsAsync((Car?) null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateReservationAsync(dto));
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldCreateCustomer_WhenCustomerDoesNotExist()
    {
        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07898763564",
            Email = "johndoe@Example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        var branch1 = new Branch
        {
            BranchId = 999,
            BranchName = "Airport Branch",
            Address = "123 Main Street",
            PhoneNumber = "07847363535",
            OpeningHours = "08:00-16:00"
        };

        var branch2 = new Branch
        {
            BranchId = 111,
            BranchName = "Downtown Branch",
            Address = "321 Main Street",
            PhoneNumber = "07872525636",
            OpeningHours = "08:00-16:00"
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            PricePerDay = 40,
            VehicleCategories = VehicleCategories.Economy
        };

        var car1 = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var customer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 1
        };

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(999)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(111)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car1);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(dto.PickUpBranchId, dto.CarId, dto.PickUpDate, dto.ReturnDate)).ReturnsAsync(car1);

        _customerRepositoryMock.Setup(x => x.GetCustomerByEmailAsync(dto.Email)).ReturnsAsync((CarRental.Domain.Entities.Customer?) null);

        _customerRepositoryMock.Setup(x => x.CreateCustomerAsync(It.IsAny<CarRental.Domain.Entities.Customer>())).ReturnsAsync(customer);

        await _customerService.CreateReservationAsync(dto);

        _customerRepositoryMock.Verify(x => x.CreateCustomerAsync(It.Is<CarRental.Domain.Entities.Customer>(c => c.FirstName == dto.FirstName && c.LastName == dto.LastName && c.Email == dto.Email && c.PhoneNumber == dto.PhoneNumber)), Times.Once);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldUseExistingCustomer_WhenCustomerExists()
    {
        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07898763564",
            Email = "johndoe@Example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        var branch1 = new Branch
        {
            BranchId = 999,
            BranchName = "Airport Branch",
            Address = "123 Main Street",
            PhoneNumber = "07847363535",
            OpeningHours = "08:00-16:00"
        };

        var branch2 = new Branch
        {
            BranchId = 111,
            BranchName = "Downtown Branch",
            Address = "321 Main Street",
            PhoneNumber = "07872525636",
            OpeningHours = "08:00-16:00"
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            PricePerDay = 40,
            VehicleCategories = VehicleCategories.Economy
        };

        var car1 = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var existingCustomer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 1
        };

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(999)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(111)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car1);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(dto.PickUpBranchId, dto.CarId, dto.PickUpDate, dto.ReturnDate)).ReturnsAsync(car1);

        _customerRepositoryMock.Setup(x => x.GetCustomerByEmailAsync(dto.Email)).ReturnsAsync(existingCustomer);

        await _customerService.CreateReservationAsync(dto);

        _customerRepositoryMock.Verify(x => x.CreateCustomerAsync(It.IsAny<CarRental.Domain.Entities.Customer>()), Times.Never);

        _customerRepositoryMock.Verify(x => x.CreateReservationAsync(It.Is<Reservation>(r => r.CustomerId == existingCustomer.CustomerId && r.CarId == dto.CarId && r.PickUpDate == dto.PickUpDate && r.ReturnDate == dto.ReturnDate && r.PickUpTime == dto.PickUpTime && r.ReturnTime == dto.ReturnTime && r.PickUpBranchId == dto.PickUpBranchId && r.ReturnBranchId == dto.ReturnBranchId)), Times.Once);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldCreateReservation_WhenDataIsValid()
    {
        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07898763564",
            Email = "johndoe@Example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        var branch1 = new Branch
        {
            BranchId = 999,
            BranchName = "Airport Branch",
            Address = "123 Main Street",
            PhoneNumber = "07847363535",
            OpeningHours = "08:00-16:00"
        };

        var branch2 = new Branch
        {
            BranchId = 111,
            BranchName = "Downtown Branch",
            Address = "321 Main Street",
            PhoneNumber = "07872525636",
            OpeningHours = "08:00-16:00"
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            PricePerDay = 40,
            VehicleCategories = VehicleCategories.Economy
        };

        var car1 = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var existingCustomer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 1
        };

        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = existingCustomer.CustomerId,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReservationStatus = ReservationStatus.Confirmed,
        };

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(999)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(111)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car1);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(dto.PickUpBranchId, dto.CarId, dto.PickUpDate, dto.ReturnDate)).ReturnsAsync(car1);

        _customerRepositoryMock.Setup(x => x.GetCustomerByEmailAsync(dto.Email)).ReturnsAsync(existingCustomer);

        _customerRepositoryMock.Setup(x => x.CreateReservationAsync(It.IsAny<Reservation>())).ReturnsAsync(reservation);

        await _customerService.CreateReservationAsync(dto);

        _customerRepositoryMock.Verify(x => x.CreateReservationAsync(
        It.Is<Reservation>(r => r.TotalPrice == 160
        )), Times.Once);

        _customerRepositoryMock.Verify(x => x.CreateReservationAsync(It.Is<Reservation>(r => r.CustomerId == existingCustomer.CustomerId && r.CarId == dto.CarId && r.PickUpDate == dto.PickUpDate && r.ReturnDate == dto.ReturnDate && r.PickUpTime == dto.PickUpTime && r.ReturnTime == dto.ReturnTime && r.PickUpBranchId == dto.PickUpBranchId && r.ReturnBranchId == dto.ReturnBranchId && r.ReservationStatus == ReservationStatus.Confirmed)), Times.Once);
    }

    [Fact]
    public async Task CreateReservationAsync_ShouldReturnGuestReservationDto_WhenReservationIsCreated()
    {
        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07898763564",
            Email = "johndoe@Example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        var reservationDto = new GuestReservationDto
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReturnBranchId = 111,
            PickUpBranchId = 999
        };

        var branch1 = new Branch
        {
            BranchId = 999,
            BranchName = "Airport Branch",
            Address = "123 Main Street",
            PhoneNumber = "07847363535",
            OpeningHours = "08:00-16:00"
        };

        var branch2 = new Branch
        {
            BranchId = 111,
            BranchName = "Downtown Branch",
            Address = "321 Main Street",
            PhoneNumber = "07872525636",
            OpeningHours = "08:00-16:00"
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            PricePerDay = 40,
            VehicleCategories = VehicleCategories.Economy
        };

        var car1 = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var existingCustomer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 1
        };

        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = existingCustomer.CustomerId,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            ReservationStatus = ReservationStatus.Confirmed,
        };

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(999)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(111)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car1);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(dto.PickUpBranchId, dto.CarId, dto.PickUpDate, dto.ReturnDate)).ReturnsAsync(car1);

        _customerRepositoryMock.Setup(x => x.GetCustomerByEmailAsync(dto.Email)).ReturnsAsync(existingCustomer);

        _customerRepositoryMock.Setup(x => x.CreateReservationAsync(It.IsAny<Reservation>())).ReturnsAsync(reservation);

        _mapperMock.Setup(x => x.Map<GuestReservationDto>(reservation)).Returns(reservationDto);

        var result = await _customerService.CreateReservationAsync(dto);

        result.Should().NotBeNull();
        result.ReservationId.Should().Be(reservationDto.ReservationId);
        result.CustomerId.Should().Be(reservationDto.CustomerId);
        result.CarId.Should().Be(reservationDto.CarId);

        _mapperMock.Verify(x => x.Map<GuestReservationDto>(reservation),
        Times.Once);

        _customerRepositoryMock.Verify(x => x.CreateReservationAsync(It.Is<Reservation>(r => r.CustomerId == existingCustomer.CustomerId && r.CarId == dto.CarId && r.PickUpDate == dto.PickUpDate && r.ReturnDate == dto.ReturnDate && r.PickUpTime == dto.PickUpTime && r.ReturnTime == dto.ReturnTime && r.PickUpBranchId == dto.PickUpBranchId && r.TotalPrice == 160 && r.ReturnBranchId == dto.ReturnBranchId && r.ReservationStatus == ReservationStatus.Confirmed)), Times.Once);
    }

    [Fact]
    public async Task GetAllBranchesAsync_ShouldReturnBranches_WhenBranchesExist()
    {
        var branches = new List<Branch>
        {
            new Branch
            {
                BranchId = 1,
                BranchName = "Airport Branch",
                Address = "123 Main Street",
                PhoneNumber = "07898767836",
                OpeningHours = "08:00-16:00"
            },
            new Branch
            {
                BranchId = 2,
                BranchName = "Downtown Branch",
                Address = "321 Main Street",
                PhoneNumber = "07898765435",
                OpeningHours = "08:00-16:00"
            }
        };

        var branchesDto = new List<BranchDto>
        {
            new BranchDto
            {
                BranchId = 1,
                BranchName = "Airport Branch",
                Address = "123 Main Street",
                PhoneNumber = "07898767836",
                OpeningHours = "08:00-16:00"
            },
            new BranchDto
            {
                BranchId = 2,
                BranchName = "Downtown Branch",
                Address = "321 Main Street",
                PhoneNumber = "07898765435",
                OpeningHours = "08:00-16:00"
            }
        };

        _customerRepositoryMock.Setup(x => x.GetAllBranchesAsync()).ReturnsAsync(branches);

        _mapperMock.Setup(x => x.Map<IEnumerable<BranchDto>>(branches)).Returns(branchesDto);

        var result = await _customerService.GetAllBranchesAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var branch = result.First();

        branch.BranchId.Should().Be(1);
        branch.BranchName.Should().Be("Airport Branch");

        _customerRepositoryMock.Verify(x => x.GetAllBranchesAsync(), Times.Once);

        _mapperMock.Verify(x => x.Map<IEnumerable<BranchDto>>(branches), Times.Once);
    }

    [Fact]
    public async Task GetAllCarsAsync_ShouldReturnCars_WhenCarsExist()
    {
        var cars = new List<Car>
        {
            new Car
            {
                CarId = 1,
                CarStatus = CarStatus.Available,
                BranchId = 999,
                VehicleTypeId = 1,
                Make = "Honda",
                Model = "Civic",
                NumberOfDoors = 5,
                TransmissionType = 0,
                EngineCapacity = 1.0m,
                HasAC = true
            },
            new Car
            {
                CarId = 2,
                CarStatus = CarStatus.Available,
                BranchId = 999,
                VehicleTypeId = 1,
                Make = "Toyota",
                Model = "Yaris",
                NumberOfDoors = 5,
                TransmissionType = 0,
                EngineCapacity = 1.0m,
                HasAC = true
            }
        };

        var fleetDto = new List<OurFleetDto>
        {
            new OurFleetDto
            {
                Make = "Honda",
                Model = "Civic",
                EngineCapacity = 1.0m,
            },
            new OurFleetDto
            {
                Make = "Toyota",
                Model = "Yaris",
                EngineCapacity = 1.0m,
            }
        };

        _customerRepositoryMock.Setup(x => x.GetAllCarsAsync()).ReturnsAsync(cars);

        _mapperMock.Setup(x => x.Map<IEnumerable<OurFleetDto>>(cars)).Returns(fleetDto);

        var result = await _customerService.GetAllCarsAsync();

        var car = result.First();

        result.Should().HaveCount(2);

        car.Make.Should().Be("Honda");
        car.Model.Should().Be("Civic");

        _customerRepositoryMock.Verify(x => x.GetAllCarsAsync(), Times.Once);

        _mapperMock.Verify(x => x.Map<IEnumerable<OurFleetDto>>(cars), Times.Once);
    }

    [Fact]
    public async Task GetAvailableCarByCarIdAsync_ShouldThrow_WhenCarDoesNotExist()
    {
        int branchId = 1;
        DateOnly pickUpDate = GetDate(2);
        DateOnly returnDate = GetDate(2);
        TimeSpan pickUpTime = GetTime(8);
        TimeSpan returnTime = GetTime(10);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(999)).ReturnsAsync((Car?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.GetAvailableCarByCarIdAsync(branchId, 999, pickUpDate, returnDate, pickUpTime, returnTime));
    }

    [Fact]
    public async Task GetAvailableCarByCarIdAsync_ShouldThrow_WhenDatesAreNotValid()
    {
        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        DateOnly pickUpDate = GetDate(2);
        DateOnly returnDate = GetDate(1);
        TimeSpan pickUpTime = GetTime(8);
        TimeSpan returnTime = GetTime(10);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.GetAvailableCarByCarIdAsync(999, 1, pickUpDate, returnDate, pickUpTime, returnTime));
    }

    [Fact]
    public async Task GetAvailableCarByCarIdAsync_ShouldThrow_WhenNoCarsAvailable()
    {
        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        DateOnly pickUpDate = GetDate(3);
        DateOnly returnDate = GetDate(6);
        TimeSpan pickUpTime = GetTime(8);
        TimeSpan returnTime = GetTime(10);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(999, 1, pickUpDate, returnDate)).ReturnsAsync((Car?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.GetAvailableCarByCarIdAsync(999, 1, pickUpDate, returnDate, pickUpTime, returnTime));
    }

    [Fact]
    public async Task GetAvailableCarByCarIdAsync_ShouldReturnDto_WhenDataIsValid()
    {
        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            PricePerDay = 40,
            VehicleCategories = VehicleCategories.Economy
        };

        var availableCar = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        DateOnly pickUpDate = GetDate(3);
        DateOnly returnDate = GetDate(6);
        TimeSpan pickUpTime = GetTime(8);
        TimeSpan returnTime = GetTime(10);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(999, 1, pickUpDate, returnDate)).ReturnsAsync(availableCar);

        var result = await _customerService.GetAvailableCarByCarIdAsync(999, 1, pickUpDate, returnDate, pickUpTime, returnTime);

        result.Should().NotBeNull();
        result.CarId.Should().Be(1);

        result.Make.Should().Be("Honda");
        result.Model.Should().Be("Civic");
        result.NumberOfDoors.Should().Be(5);
        result.TransmissionType.Should().Be(0);
        result.EngineCapacity.Should().Be(1.0m);
        result.HasAC.Should().BeTrue();
        result.VehicleCategories.Should().Be(VehicleCategories.Economy);
        result.PricePerDay.Should().Be(40);
    }

    [Fact]
    public async Task GetCarsByGroupAsync_ShouldReturnOrderedCars_WhenDataIsValid()
    {
        int page = 1;
        int pageSize = 10;

        var vehicleType2 = new VehicleType
        {
            VehicleTypeId = 2,
            PricePerDay = 60,
            VehicleCategories = VehicleCategories.SUV
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            PricePerDay = 40,
            VehicleCategories = VehicleCategories.Economy
        };

        var car1 = new Car
        {
            CarId = 2,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 2,
            VehicleType = vehicleType2,
            Make = "Kia",
            Model = "Sportage",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var car2 = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available,
            BranchId = 999,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var cars = new PagedResult<Car>
        {
            Items = new List<Car>
            {
                car1,
                car2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        var carDto1 = new CarGroupDto
        {
            CarId = 1,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var carDto2 = new CarGroupDto
        {
            CarId = 2,
            Make = "Kia",
            Model = "Sportage",
            NumberOfDoors = 5,
            TransmissionType = 0,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var cargroupDto = new PagedResult<CarGroupDto>
        {
            Items = new List<CarGroupDto>
            {
                carDto1,
                carDto2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };


        _customerRepositoryMock.Setup(x => x.GetCarsByGroupAsync(page, pageSize)).ReturnsAsync(cars);

        _mapperMock.Setup(x => x.Map<PagedResult<CarGroupDto>>(It.IsAny<PagedResult<Car>>())).Returns(cargroupDto);

        var result = await _customerService.GetCarsByGroupAsync(page, pageSize);

        result.Items.Should().HaveCount(2);

        _customerRepositoryMock.Verify(x => x.GetCarsByGroupAsync(page, pageSize), Times.Once);

        _mapperMock.Verify(x => x.Map<PagedResult<CarGroupDto>>(It.IsAny<PagedResult<Car>>()), Times.Once);
    }

    [Fact]
    public async Task GetReservationByIdAsync_ShouldReturnNull_WhenReservationIdDoesNotExist()
    {

        _customerRepositoryMock.Setup(x => x.GetReservationByIdAsync(1)).ReturnsAsync((Reservation?)null);

        var result = await _customerService.GetReservationByIdAsync(1);

        result.Should().Be(null);

        _customerRepositoryMock.Verify(x => x.GetReservationByIdAsync(1),
        Times.Once);
    }

    [Fact]
    public async Task GetReservationByIdAsync_ShouldReturnReservation_WhenReservationExists()
    {
        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(2),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            PickUpBranchId = 1,
            ReturnBranchId = 1,
            ReservationStatus = ReservationStatus.Confirmed,
            TotalPrice = 180
        };

        var adminReservationDto = new AdminReservationDto
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(2),
            ReturnDate = GetDate(5),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(10),
            PickUpBranchId = 1,
            ReturnBranchId = 1,
            ReservationStatus = ReservationStatus.Confirmed,
            TotalPrice = 180,
            FirstName = "John",
            LastName = "Smith",
            Email = "JohnSmith@example.com",
            PhoneNumber = "07898765678"
        };

        _customerRepositoryMock.Setup(x => x.GetReservationByIdAsync(1)).ReturnsAsync(reservation);

        _mapperMock.Setup(x => x.Map<AdminReservationDto>(reservation)).Returns(adminReservationDto);

        var result = await _customerService.GetReservationByIdAsync(1);

        result.Should().NotBeNull();
        result.ReservationId.Should().Be(1);
        result.CustomerId.Should().Be(1);
        result.CarId.Should().Be(1);
        result.PickUpDate.Should().Be(reservation.PickUpDate);
        result.ReturnDate.Should().Be(reservation.ReturnDate);
        result.PickUpTime.Should().Be(reservation.PickUpTime);
        result.ReturnTime.Should().Be(reservation.ReturnTime);
        result.PickUpBranchId.Should().Be(reservation.PickUpBranchId);
        result.ReturnBranchId.Should().Be(reservation.ReturnBranchId);
        result.ReservationStatus.Should().Be(reservation.ReservationStatus);
        result.TotalPrice.Should().Be(reservation.TotalPrice);

        _customerRepositoryMock.Verify(x => x.GetReservationByIdAsync(1), Times.Once);

        _mapperMock.Verify(x => x.Map<AdminReservationDto>(reservation),
        Times.Once);
    }

    [Fact]
    public async Task CreateCustomerReservationAsync_ShouldThrow_WhenDtoIsNull()
    {
        int customerId = 2;

       await Assert.ThrowsAnyAsync<ArgumentNullException>(() => _customerService.CreateCustomerReservationAsync(null, customerId));
    }

    [Fact]
    public async Task CreateCustomerReservationAsync_ShouldThrow_WhenCustomerDoesNotExist()
    {
        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07886372662",
            Email = "JohnDoe@example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 1
        };

        _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(2)).ReturnsAsync((CarRental.Domain.Entities.Customer?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateCustomerReservationAsync(dto, 2));
    }

    [Fact]
    public async Task CreateCustomerReservationAsync_ShouldThrow_WhenPickUpBranch_DoesNotExist()
    {
        var customer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 2,
        };

        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07886372662",
            Email = "JohnDoe@example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 1
        };

        _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(2)).ReturnsAsync(customer);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(1)).ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateCustomerReservationAsync(dto, 2));
    }

    [Fact]
    public async Task CreateCustomerReservationAsync_ShouldThrow_WhenReturnBranch_DoesNotExist()
    {
        var customer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 2,
        };

        var branch1 = new Branch
        {
            BranchId = 1
        };

        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07886372662",
            Email = "JohnDoe@example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2
        };

        _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(2)).ReturnsAsync(customer);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(1)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(2)).ReturnsAsync((Branch?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateCustomerReservationAsync(dto, 2));
    }

    [Fact]
    public async Task CreateCustomerReservationAsync_ShouldThrow_WhenCar_DoesNotExist()
    {
        var customer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 2,
        };

        var branch1 = new Branch
        {
            BranchId = 1
        };

        var branch2 = new Branch
        {
            BranchId = 2
        };

        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07886372662",
            Email = "JohnDoe@example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2
        };

        _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(2)).ReturnsAsync(customer);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(1)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(2)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync((Car?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateCustomerReservationAsync(dto, 2));
    }

    [Fact]
    public async Task CreateCustomerReservationAsync_ShouldThrow_WhenCar_IsNotAvailable()
    {
        var customer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 2,
        };

        var branch1 = new Branch
        {
            BranchId = 1
        };

        var branch2 = new Branch
        {
            BranchId = 2
        };

        var car = new Car
        {
            CarId = 1
        };

        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07886372662",
            Email = "JohnDoe@example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2
        };

        _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(2)).ReturnsAsync(customer);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(1)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(2)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(1, 1, dto.PickUpDate, dto.ReturnDate)).ReturnsAsync((Car?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _customerService.CreateCustomerReservationAsync(dto, 2));
    }

    [Fact]
    public async Task CreateCustomerReservationAsync_ShouldReturnReservationDto_WhenDataIsValid()
    {
        var customer = new CarRental.Domain.Entities.Customer
        {
            CustomerId = 2,
        };

        var branch1 = new Branch
        {
            BranchId = 1
        };

        var branch2 = new Branch
        {
            BranchId = 2
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            VehicleCategories = VehicleCategories.Economy,
            PricePerDay = 40
        };

        var car = new Car
        {
            CarId = 1,
            VehicleType = vehicleType,
            VehicleTypeId = 1
        };

        var reservation = new Reservation
        {
            CustomerId = customer.CustomerId,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed
        };

        var reservationDto = new ReservationDto
        {
            CustomerId = 2,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed,
            TotalPrice = 120
        };

        var dto = new CreateReservationDto
        {
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "07886372662",
            Email = "JohnDoe@example.com",
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2
        };

        _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(2)).ReturnsAsync(customer);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(1)).ReturnsAsync(branch1);

        _customerRepositoryMock.Setup(x => x.GetBranchByIdAsync(2)).ReturnsAsync(branch2);

        _customerRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        _customerRepositoryMock.Setup(x => x.GetAvailableCarAsync(1, 1, dto.PickUpDate, dto.ReturnDate)).ReturnsAsync(car);

        _customerRepositoryMock.Setup(x => x.CreateReservationAsync(It.IsAny<Reservation>())).ReturnsAsync(reservation);

        _mapperMock.Setup(x => x.Map<ReservationDto>(reservation)).Returns(reservationDto);

        var result = await _customerService.CreateCustomerReservationAsync(dto, 2);

        result.Should().NotBeNull();
        result.CustomerId.Should().Be(reservationDto.CustomerId);
        result.CarId.Should().Be(reservationDto.CarId);
        result.PickUpDate.Should().Be(reservationDto.PickUpDate);
        result.ReturnDate.Should().Be(reservationDto.ReturnDate);
        result.PickUpTime.Should().Be(reservationDto.PickUpTime);
        result.ReturnTime.Should().Be(reservationDto.ReturnTime);
        result.PickUpBranchId.Should().Be(reservationDto.PickUpBranchId);
        result.ReturnBranchId.Should().Be(reservationDto.ReturnBranchId);

        result.TotalPrice.Should().Be(120);

        _customerRepositoryMock.Verify(x => x.CreateReservationAsync(It.Is<Reservation>(r => r.CustomerId == 2 && r.CarId == 1 && r.TotalPrice == 120 && r.PickUpBranchId == 1 && r.ReturnBranchId == 2 && r.ReservationStatus == ReservationStatus.Confirmed && r.ReservationCars.Any(rc => rc.CarId == 1))), Times.Once);
    }

    [Fact]
    public async Task GetReservationsByCustomerIdAsync_ShouldReturnMyReservationsDto_WhenRservationsExist()
    {
        var reservation1 = new Reservation
        {
            ReservationId = 1,
            CustomerId = 2,
            CarId = 1
        };

        var reservation2 = new Reservation
        {
            ReservationId = 2,
            CustomerId = 2,
            CarId = 2
        };
        var reservations = new PagedResult<Reservation>
        {
            Items = new List<Reservation>
            {
                reservation1,
                reservation2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        var reservationDto1 = new MyReservationsDto
        {
            ReservationId = 1,
            CustomerId = 2,
            CarId = 1
        };

        var reservationDto2 = new MyReservationsDto
        {
            ReservationId = 2,
            CustomerId = 2,
            CarId = 2
        };

        var reservationDtos = new PagedResult<MyReservationsDto>
        {
            Items = new List<MyReservationsDto>
            {
                reservationDto1,
                reservationDto2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };



        _customerRepositoryMock.Setup(x => x.GetReservationsByCustomerIdAsync(2, 1, 10)).ReturnsAsync(reservations);

        _mapperMock.Setup(x => x.Map<PagedResult<MyReservationsDto>>(reservations)).Returns(reservationDtos);

        var result = await _customerService.GetReservationsByCustomerIdAsync(2, 1, 10);

        result.Should().NotBeNull();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.TotalCount.Should().Be(2);

        result.Items.Count.Should().Be(2);

        result.Items.First().ReservationId.Should().Be(1);
        result.Items.First().CustomerId.Should().Be(2);
        result.Items.First().CarId.Should().Be(1);

        result.Items.Last().ReservationId.Should().Be(2);
        result.Items.Last().CustomerId.Should().Be(2);
        result.Items.Last().CarId.Should().Be(2);

        _customerRepositoryMock.Verify(x => x.GetReservationsByCustomerIdAsync(2, 1, 10), Times.Once);

        _mapperMock.Verify(x => x.Map<PagedResult<MyReservationsDto>>(reservations), Times.Once);
    }

    [Fact]
    public async Task GetCustomerProfileByIdAsync_ShouldReturnNull_WhenCustomerIsNull()
    {
        int customerId = 1;

        _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync((CarRental.Domain.Entities.Customer?)null);

        var result = await _customerService.GetCustomerProfileByIdAsync(1);

        result.Should().BeNull();

        _customerRepositoryMock.Verify(x => x.GetCustomerByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetCustomerProfileByIdAsync_ShouldReturnCustomerProfileDto_WhenCustomerExists()
    {
        var customer = new CarRental.Domain.Entities.Customer
        {
           CustomerId = 1,
           FirstName = "John",
           LastName = "Doe",
           Email = "JohnDoe@example.com",
           PhoneNumber = "07464637373"
        };

        var customerDto = new CustomerProfileDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "JohnDoe@example.com",
            PhoneNumber = "07464637373"
        };

        _customerRepositoryMock.Setup(x => x.GetCustomerByIdAsync(1)).ReturnsAsync(customer);

        _mapperMock.Setup(x => x.Map<CustomerProfileDto>(customer)).Returns(customerDto);

        var result = await _customerService.GetCustomerProfileByIdAsync(1);

        result.Should().NotBeNull();

        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be("JohnDoe@example.com");
        result.PhoneNumber.Should().Be("07464637373");

        _mapperMock.Verify(x => x.Map<CustomerProfileDto>(customer), Times.Once);

        _customerRepositoryMock.Verify(x => x.GetCustomerByIdAsync(1), Times.Once);
    }
}
