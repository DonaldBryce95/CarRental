using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using Moq;
using FluentAssertions;

namespace CarRental.Admin.Tests;

public class AdminServiceTests
{
    private readonly Mock<IAdminRepository> _adminRepositoryMock;

    private readonly Mock<IMapper> _mapperMock;

    private readonly AdminService _adminService;

    public AdminServiceTests()
    {
        _adminRepositoryMock = new Mock<IAdminRepository>();

        _mapperMock = new Mock<IMapper>();

        _adminService = new AdminService(_adminRepositoryMock.Object, _mapperMock.Object);
    }

    private static DateOnly GetDate(int days) => DateOnly.FromDateTime(DateTime.Now).AddDays(days);

    [Fact]
    public async Task CreateBranchAsync_ShouldThrow_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _adminService.CreateBranchAsync(null));
    }

    [Fact]
    public async Task CreateBranchAsync_ShouldCreate_WhenDataIsValid()
    {
        var dto = new CreateBranchDto
        {
            BranchName = "AirportBranch",
            Address = "123 MainStreet",
            PhoneNumber = "07898374652",
            OpeningHours = "08:00-18:00"
        };

        var branch = new Branch
        {
            BranchId = 1,
            BranchName = "AirportBranch",
            Address = "123 MainStreet",
            PhoneNumber = "07898374652",
            OpeningHours = "08:00-18:00"
        };

        var branchDto = new BranchDto
        {
            BranchId = 1,
            BranchName = "AirportBranch",
            Address = "123 MainStreet",
            PhoneNumber = "07898374652",
            OpeningHours = "08:00-18:00"
        };

        _mapperMock.Setup(x => x.Map<Branch>(dto)).Returns(branch);

        _mapperMock.Setup(x => x.Map<BranchDto>(branch)).Returns(branchDto);

        _adminRepositoryMock.Setup(x => x.CreateBranchAsync(branch)).ReturnsAsync(branch);

        var result = await _adminService.CreateBranchAsync(dto);

        result.Should().BeEquivalentTo(branchDto);
        _adminRepositoryMock.Verify(x => x.CreateBranchAsync(branch),Times.Once);
    }

    [Fact]
    public async Task CreateCarAsync_ShouldThrow_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _adminService.CreateCarAsync(null));
    }

    [Fact]
    public async Task CreateCarAsync_ShouldCreate_WhenDataIsValid()
    {
        var createCarDto = new CreateCarDto
        {
            BranchId = 1,
            VehicleTypeId = 1,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = TransmissionType.Automatic
        };

        var car = new Car
        {
            CarId = 1,
            BranchId = 1,
            VehicleTypeId = 1,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = TransmissionType.Automatic,
            CarStatus = CarStatus.Available,
            HasAC = true
        };

        var carDto = new CarDto
        {
            CarId = 1,
            BranchId = 1,
            VehicleTypeId = 1,
            Make = "Honda",
            Model = "Civic"
        };

        _mapperMock.Setup(x => x.Map<Car>(createCarDto)).Returns(car);

        _mapperMock.Setup(x => x.Map<CarDto>(car)).Returns(carDto);

        _adminRepositoryMock.Setup(x => x.CreateCarAsync(car)).ReturnsAsync(car);

        var result = await _adminService.CreateCarAsync(createCarDto);

        result.Should().BeEquivalentTo(carDto);

        _adminRepositoryMock.Verify(x => x.CreateCarAsync(car),Times.Once);
    }

    [Fact]
    public async Task CreateVehicleTypeAsync_ShouldThrow_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _adminService.CreateVehicleTypeAsync(null));
    }

    [Fact]
    public async Task CreateVehicleTypeAsync_ShouldCreate_WhenDataIsValid()
    {
        var createVehicleTypeDto = new CreateVehicleTypeDto
        {
            VehicleCategories = VehicleCategories.Economy,
            PricePerDay = 45
        };

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            VehicleCategories = VehicleCategories.Economy,
            PricePerDay = 45
        };

        var vehicleTypeDto = new VehicleTypeDto
        {
            VehicleTypeId = 1,
            VehicleCategories = VehicleCategories.Economy,
            PricePerDay = 45
        };

        _mapperMock.Setup(x => x.Map<VehicleType>(createVehicleTypeDto)).Returns(vehicleType);

        _mapperMock.Setup(x => x.Map<VehicleTypeDto>(vehicleType)).Returns(vehicleTypeDto);

        _adminRepositoryMock.Setup(x => x.CreateVehicleTypeAsync(vehicleType)).ReturnsAsync(vehicleType);

        var result = await _adminService.CreateVehicleTypeAsync(createVehicleTypeDto);

        result.Should().BeEquivalentTo(vehicleTypeDto);

        _adminRepositoryMock.Verify(x => x.CreateVehicleTypeAsync(vehicleType), Times.Once);
    }

    [Fact]
    public async Task GetAllBranchesAsync_ShouldReturnBranches_WhenBranchesExist()
    {
        var branches = new List<Branch>
        {
            new Branch
            {
                BranchId = 1,
                BranchName = "AirportBranch",
                Address = "123 MainStreet",
                PhoneNumber = "07847635465",
                OpeningHours = "08:00-18:00"
            },
            new Branch
            {
                BranchId = 2,
                BranchName = "DownTownBranch",
                Address = "321 MainStreet",
                PhoneNumber = "07847689874",
                OpeningHours = "09:00-18:00"
            }
        };

        var branchDto = new List<BranchDto>
        {
            new BranchDto
            {
                BranchId = 1,
                BranchName = "AirportBranch",
                Address = "123 MainStreet",
                PhoneNumber = "07847635465",
                OpeningHours = "08:00-18:00"
            },
            new BranchDto
            {
                BranchId = 2,
                BranchName = "DownTownBranch",
                Address = "321 MainStreet",
                PhoneNumber = "07847689874",
                OpeningHours = "09:00-18:00"
            }
        };

        _mapperMock.Setup(x => x.Map<IEnumerable<BranchDto>>(branches)).Returns(branchDto);

        _adminRepositoryMock.Setup(x => x.GetAllBranchesAsync()).ReturnsAsync(branches);

        var result = await _adminService.GetAllBranchesAsync();

        result.Should().BeEquivalentTo(branchDto);

        _adminRepositoryMock.Verify(x => x.GetAllBranchesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllCarsAsync_ShouldReturnCars_WhenCarsExist()
    {
        int page = 1;
        int pageSize = 10;

        var vehicleType = new VehicleType
        {
            VehicleTypeId = 1,
            VehicleCategories = VehicleCategories.Economy,
            PricePerDay = 40
        };

        var car1 = new Car
        {
            CarId = 1,
            BranchId = 1,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = TransmissionType.Automatic,
            CarStatus = CarStatus.Available,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var car2 = new Car
        {
            CarId = 2,
            BranchId = 1,
            VehicleTypeId = 1,
            VehicleType = vehicleType,
            Make = "Toyota",
            Model = "Yaris",
            NumberOfDoors = 5,
            TransmissionType = TransmissionType.Automatic,
            CarStatus = CarStatus.Available,
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

        var carDto1 = new AdminCarDto
        {
            CarId = 1,
            BranchId = 1,
            Make = "Honda",
            Model = "Civic",
            NumberOfDoors = 5,
            TransmissionType = TransmissionType.Automatic,
            CarStatus = CarStatus.Available,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var carDto2 = new AdminCarDto
        {
            CarId = 2,
            BranchId = 1,
            Make = "Toyota",
            Model = "Yaris",
            NumberOfDoors = 5,
            TransmissionType = TransmissionType.Automatic,
            CarStatus = CarStatus.Available,
            EngineCapacity = 1.0m,
            HasAC = true
        };

        var carDtos = new PagedResult<AdminCarDto>
        {
            Items = new List<AdminCarDto>
            {
                carDto1,
                carDto2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        _mapperMock.Setup(x => x.Map<PagedResult<AdminCarDto>>(cars)).Returns(carDtos);

        _adminRepositoryMock.Setup(x => x.GetAllCarsAsync(page, pageSize)).ReturnsAsync(cars);

        var result = await _adminService.GetAllCarsAsync(page, pageSize);

        result.Should().BeEquivalentTo(carDtos);

        _adminRepositoryMock.Verify(x => x.GetAllCarsAsync(page, pageSize), Times.Once);

        _mapperMock.Verify(x => x.Map<PagedResult<AdminCarDto>>(cars), Times.Once);
    }

    [Fact]
    public async Task GetAllCarsAsync_ShouldReturnEmpty_WhenNoCarsExist()
    {
        int page = 1;
        int pageSize = 10;

        var cars = new PagedResult<Car>
        {
           Items = new List<Car>(),
           Page = 1,
           PageSize = 10,
           TotalCount = 0
        };

        var carDto = new PagedResult<AdminCarDto>
        {
            Items = new List<AdminCarDto>(),
            Page = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _adminRepositoryMock.Setup(x => x.GetAllCarsAsync(page, pageSize)).ReturnsAsync(cars);

        _mapperMock.Setup(x => x.Map<PagedResult<AdminCarDto>>(cars)).Returns(carDto);

        var result = await _adminService.GetAllCarsAsync(page, pageSize);

        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);

        _mapperMock.Verify(x => x.Map<PagedResult<AdminCarDto>>(cars), Times.Once);

        _adminRepositoryMock.Verify(x => x.GetAllCarsAsync(page, pageSize),Times.Once);
    }

    [Fact]
    public async Task GetAllBranchesAsync_ShouldReturnEmpty_WhenNoBranchesExist()
    {
        var branches = new List<Branch>
        {

        };

        _adminRepositoryMock.Setup(x => x.GetAllBranchesAsync()).ReturnsAsync(branches);

        _mapperMock.Setup(x => x.Map<IEnumerable<BranchDto>>(branches)).Returns([]);

        var result = await _adminService.GetAllBranchesAsync();

        result.Should().BeEmpty();

        _adminRepositoryMock.Verify(x => x.GetAllBranchesAsync(),Times.Once);
    }

    [Fact]
    public async Task GetAllReservationsAsync_ShouldReturnAdminReservationDto_WhenDataIsValid()
    {
        int page = 1;
        int pageSize = 10;

        var reservation1 = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed
        };
        var reservation2 = new Reservation
        {
            ReservationId = 2,
            CustomerId = 2,
            CarId = 2,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed
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

        var reservationDto1 = new AdminReservationDto
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "JohnDoe@example.com",
            PhoneNumber = "07667373633",
            Make = "Honda",
            Model = "Civic",
            CarStatus = CarStatus.Available,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            PickUpBranchName = "Airport Branch",
            ReturnBranchName = "Downtown Branch",
            ReservationStatus = ReservationStatus.Confirmed
        };
        var reservationDto2 = new AdminReservationDto
        {
            ReservationId = 2,
            CustomerId = 2,
            CarId = 2,
            FirstName = "John",
            LastName = "Smith",
            Email = "JohnSmith@example.com",
            PhoneNumber = "07667732622",
            Make = "Toyota",
            Model = "Yaris",
            CarStatus = CarStatus.Available,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            PickUpBranchName = "Airport Branch",
            ReturnBranchName = "Downtown Branch",
            ReservationStatus = ReservationStatus.Confirmed
        };

        var reservationDtos = new PagedResult<AdminReservationDto>
        {
            Items = new List<AdminReservationDto>
            {
                reservationDto1,
                reservationDto2,
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        _adminRepositoryMock.Setup(x => x.GetAllReservationsAsync(page, pageSize)).ReturnsAsync(reservations);

        _mapperMock.Setup(x => x.Map<PagedResult<AdminReservationDto>>(reservations)).Returns(reservationDtos);

        var result = await _adminService.GetAllReservationsAsync(page, pageSize);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);

        _adminRepositoryMock.Verify(x => x.GetAllReservationsAsync(page , pageSize), Times.Once);

        _mapperMock.Verify(x => x.Map<PagedResult<AdminReservationDto>>(reservations), Times.Once);
    }

    [Fact]
    public async Task GetAllReservationsAsync_ShouldReturnEmpty_WhenNoReservationsExist()
    {
        int page = 1;
        int pageSize = 10;

        var reservations = new PagedResult<Reservation>
        {
            Items = new List<Reservation>(),
            Page = 1,
            PageSize = 10,
            TotalCount = 0
        };

        var reservationDtos = new PagedResult<AdminReservationDto>
        {
            Items = new List<AdminReservationDto>(),
            Page = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _adminRepositoryMock.Setup(x => x.GetAllReservationsAsync(page, pageSize)).ReturnsAsync(reservations);

        _mapperMock.Setup(x => x.Map<PagedResult<AdminReservationDto>>(reservations)).Returns(reservationDtos);

        var result = await _adminService.GetAllReservationsAsync(page, pageSize);

        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);

        _mapperMock.Verify(x => x.Map<PagedResult<AdminReservationDto>>(reservations), Times.Once);

        _adminRepositoryMock.Verify(x => x.GetAllReservationsAsync(page, pageSize),Times.Once);
    }

    [Fact]
    public async Task GetAllVehicleTypesAsync_ShouldReturnVehicleTypeDto_WhenDataIsValid()
    {
        var vehicleTypes = new List<VehicleType>
        {
            new VehicleType
            {
                VehicleTypeId = 1,
                PricePerDay = 40,
                VehicleCategories = VehicleCategories.Economy
            },
            new VehicleType
            {
               VehicleTypeId = 2,
               PricePerDay = 60,
               VehicleCategories = VehicleCategories.Compact
            }
        };

        var vehicleTypesDto = new List<VehicleTypeDto>
        {
            new VehicleTypeDto
            {
                VehicleTypeId = 1,
                PricePerDay = 40,
                VehicleCategories = VehicleCategories.Economy
            },
            new VehicleTypeDto
            {
                VehicleTypeId = 2,
                PricePerDay = 60,
                VehicleCategories = VehicleCategories.Compact
            }
        };


        _adminRepositoryMock.Setup(x => x.GetAllVehicleTypesAsync()).ReturnsAsync(vehicleTypes);

        _mapperMock.Setup(x => x.Map<IEnumerable<VehicleTypeDto>>(vehicleTypes)).Returns(vehicleTypesDto);

        var result = await _adminService.GetAllVehicleTypesAsync();

        result.Should().BeEquivalentTo(vehicleTypesDto);
        result.Should().HaveCount(2);

        _adminRepositoryMock.Verify(x => x.GetAllVehicleTypesAsync(), Times.Once);

        _mapperMock.Verify(x => x.Map<IEnumerable<VehicleTypeDto>>(vehicleTypes), Times.Once);
    }

    [Fact]
    public async Task GetAllVehicleTypesAsync_ShouldReturnEmpty_WhenNoVehicleTypesExist()
    {
        var vehicleTypes = new List<VehicleType>();

        var vehicleTypesDto = new List<VehicleTypeDto>();


        _adminRepositoryMock.Setup(x => x.GetAllVehicleTypesAsync()).ReturnsAsync(vehicleTypes);

        _mapperMock.Setup(x => x.Map<IEnumerable<VehicleTypeDto>>(vehicleTypes)).Returns(vehicleTypesDto);

        var result = await _adminService.GetAllVehicleTypesAsync();

        result.Should().BeEmpty();

        _adminRepositoryMock.Verify(x => x.GetAllVehicleTypesAsync(), Times.Once);

        _mapperMock.Verify(x => x.Map<IEnumerable<VehicleTypeDto>>(vehicleTypes), Times.Once);
    }

    [Fact]
    public async Task GetReservationByIdAsync_ShouldReturnNull_WhenReservationDoesNotExist()
    {

        _adminRepositoryMock.Setup(x => x.GetReservationByIdAsync(1)).ReturnsAsync((Reservation?)null);

        var result = await _adminService.GetReservationByIdAsync(1);

        result.Should().BeNull();

        _adminRepositoryMock.Verify(x => x.GetReservationByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetReservationByIdAsync_ShouldReturnAdminReservationDto_WhenReservationExists()
    {
        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed
        };

        var reservationDto = new AdminReservationDto
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "JohnDoe@example.com",
            PhoneNumber = "07667373633",
            Make = "Honda",
            Model = "Civic",
            CarStatus = CarStatus.Available,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            PickUpBranchName = "Airport Branch",
            ReturnBranchName = "Downtown Branch",
            ReservationStatus = ReservationStatus.Confirmed
        };

        _adminRepositoryMock.Setup(x => x.GetReservationByIdAsync(1)).ReturnsAsync(reservation);

        _mapperMock.Setup(x => x.Map<AdminReservationDto>(reservation)).Returns(reservationDto);

        var result = await _adminService.GetReservationByIdAsync(1);

        result.Should().BeEquivalentTo(reservationDto);

        _adminRepositoryMock.Verify(x => x.GetReservationByIdAsync(1), Times.Once);

        _mapperMock.Verify(x => x.Map<AdminReservationDto>(reservation), Times.Once);
    }

    [Fact]
    public async Task GetDashboardCarsUnderMaintenanceCountAsync_ShouldReturnCount_WhenDataIsValid()
    {
        var count = 5;

        _adminRepositoryMock.Setup(x => x.GetDashboardCarsUnderMaintenanceCountAsync()).ReturnsAsync(count);

        var result = await _adminService.GetDashboardCarsUnderMaintenanceCountAsync();

        result.Should().Be(count);

        _adminRepositoryMock.Verify(x => x.GetDashboardCarsUnderMaintenanceCountAsync(), Times.Once);
    }

    [Fact]
    public async Task GetDashboardMaintenanceWorkersCountAsync_ShouldReturnCount_WhenDataIsValid()
    {
        var count = 5;

        _adminRepositoryMock.Setup(x => x.GetDashboardMaintenanceWorkersCountAsync()).ReturnsAsync(count);

        var result = await _adminService.GetDashboardMaintenanceWorkersCountAsync();

        result.Should().Be(count);

        _adminRepositoryMock.Verify(x => x.GetDashboardMaintenanceWorkersCountAsync(), Times.Once);
    }

    [Fact]
    public async Task GetDashboardMaintenanceTasksCountAsync_ShouldReturnCount_WhenDataIsValid()
    {
        var count = 5;

        _adminRepositoryMock.Setup(x => x.GetDashboardMaintenanceTasksCountAsync()).ReturnsAsync(count);

        var result = await _adminService.GetDashboardMaintenanceTasksCountAsync();

        result.Should().Be(count);

        _adminRepositoryMock.Verify(x => x.GetDashboardMaintenanceTasksCountAsync(), Times.Once);
    }

    [Fact]
    public async Task GetDashboardReservationsCountAsync_ShouldReturnCount_WhenDataIsValid()
    {
        var count = 5;

        _adminRepositoryMock.Setup(x => x.GetDashboardReservationsCountAsync()).ReturnsAsync(count);

        var result = await _adminService.GetDashboardReservationsCountAsync();

        result.Should().Be(count);

        _adminRepositoryMock.Verify(x => x.GetDashboardReservationsCountAsync(), Times.Once);
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldThrow_WhenReservationDoesNotExist()
    {
        _adminRepositoryMock.Setup(x => x.GetReservationByIdAsync(1)).ReturnsAsync((Reservation?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.CancelReservationAsync(1));

        _adminRepositoryMock.Verify(x => x.GetReservationByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldCancelReservation_WhenReservationExists()
    {
        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed
        };

        var updatedReservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Cancelled
        };

        var updatedReservationDto = new AdminReservationDto
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "JohnDoe@example.com",
            PhoneNumber = "07667373633",
            Make = "Honda",
            Model = "Civic",
            CarStatus = CarStatus.Available,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            PickUpBranchName = "Airport Branch",
            ReturnBranchName = "Downtown Branch",
            ReservationStatus = ReservationStatus.Cancelled
        };

        _adminRepositoryMock.Setup(x => x.GetReservationByIdAsync(1)).ReturnsAsync(reservation);

        _adminRepositoryMock.Setup(x => x.UpdateReservationAsync(
        It.Is<Reservation>(r => r.ReservationStatus == ReservationStatus.Cancelled))).ReturnsAsync(updatedReservation);

        _mapperMock.Setup(x => x.Map<AdminReservationDto>(updatedReservation)).Returns(updatedReservationDto);

        var result = await _adminService.CancelReservationAsync(1);

        result.Should().BeEquivalentTo(updatedReservationDto);

        _adminRepositoryMock.Verify(x => x.GetReservationByIdAsync(1), Times.Once);

        _adminRepositoryMock.Verify(x => x.UpdateReservationAsync(
        It.Is<Reservation>(r => r.ReservationStatus == ReservationStatus.Cancelled)), Times.Once);

        _mapperMock.Verify(x => x.Map<AdminReservationDto>(updatedReservation), Times.Once);
    }

    [Fact]
    public async Task CreateMaintenanceTaskAsync_ShouldThrow_WhenCarDoesNotExist()
    {
        var carReturnedDto = new CarReturnedDto
        {
          CarId = 1
        };

        _adminRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync((Car?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.CreateMaintenanceTaskAsync(carReturnedDto));

        _adminRepositoryMock.Verify(x => x.GetCarByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateMaintenanceTaskAsync_ShouldThrow_WhenCarIsNotOutOnHire()
    {
        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available
        };

        var carReturnedDto = new CarReturnedDto
        {
          CarId = 1
        };

        _adminRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.CreateMaintenanceTaskAsync(carReturnedDto));

        _adminRepositoryMock.Verify(x => x.GetCarByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateMaintenanceTaskAsync_ShouldCreateMaintenanceTask_WithAwaitingAssignment_WhenWorkerIsNull()
    {
        var car = new Car
        {
            CarId = 1,
            Make = "Honda",
            Model = "Civic",
            CarStatus = CarStatus.OutOnHire
        };

        var carReturnedDto = new CarReturnedDto
        {
          CarId = 1
        };

        var createdTask = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            CarId = carReturnedDto.CarId,
            MaintenanceTaskStatus = MaintenanceTaskStatus.AwaitingAssignment
        };

        var createdReport = new MaintenanceReport
        {
            MaintenanceTaskId = createdTask.MaintenanceTaskId,
            OverallResult = InspectionResult.Pending,
            MaintenanceReportStatus = MaintenanceReportStatus.Pending
        };

        _adminRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        _adminRepositoryMock.Setup(x => x.GetAvailableMaintenanceWorkerAsync()).ReturnsAsync((MaintenanceWorker?)null);

        _adminRepositoryMock.Setup(x => x.CreateMaintenanceTaskAsync(It.Is<MaintenanceTask>(t => t.CarId == 1 && t.MaintenanceTaskStatus == MaintenanceTaskStatus.AwaitingAssignment))).ReturnsAsync(createdTask);

        _adminRepositoryMock.Setup(x => x.CreateMaintenanceReportAsync(It.Is<MaintenanceReport>(r => r.MaintenanceReportStatus == MaintenanceReportStatus.Pending && r.OverallResult == InspectionResult.Pending))).ReturnsAsync(createdReport);

        _adminRepositoryMock.Setup(x => x.SaveChangesAsync());

        var result = await _adminService.CreateMaintenanceTaskAsync(carReturnedDto);

        result.CarId.Should().Be(car.CarId);
        result.Make.Should().Be(car.Make);
        result.Model.Should().Be(car.Model);

        result.MaintenanceTaskStatus.Should().Be(MaintenanceTaskStatus.AwaitingAssignment);

        _adminRepositoryMock.Verify(x => x.GetCarByIdAsync(1), Times.Once);

        _adminRepositoryMock.Verify(x => x.CreateMaintenanceTaskAsync(
        It.Is<MaintenanceTask>(t => t.CarId == 1 && t.MaintenanceTaskStatus == MaintenanceTaskStatus.AwaitingAssignment)), Times.Once);

        _adminRepositoryMock.Verify(x => x.CreateMaintenanceReportAsync(
        It.Is<MaintenanceReport>(r => r.MaintenanceReportStatus == MaintenanceReportStatus.Pending && r.OverallResult == InspectionResult.Pending)), Times.Once);

        _adminRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateMaintenanceTaskAsync_ShouldCreateMaintenanceTask_ForAssignedWorker_WhenWorkerExists()
    {
        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1
        };

        var car = new Car
        {
            CarId = 1,
            Make = "Honda",
            Model = "Civic",
            CarStatus = CarStatus.OutOnHire
        };

        var carReturnedDto = new CarReturnedDto
        {
          CarId = 1
        };

        var assignedTask = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            CarId = carReturnedDto.CarId,
            MaintenanceWorkerId = worker.MaintenanceWorkerId,
            MaintenanceTaskStatus = MaintenanceTaskStatus.Pending
        };

        var assignedReport = new MaintenanceReport
        {
            MaintenanceTaskId = assignedTask.MaintenanceTaskId,
            OverallResult = InspectionResult.Pending,
            MaintenanceWorkerId = worker.MaintenanceWorkerId,
            MaintenanceReportStatus = MaintenanceReportStatus.Pending
        };

        _adminRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        _adminRepositoryMock.Setup(x => x.GetAvailableMaintenanceWorkerAsync()).ReturnsAsync(worker);

        _adminRepositoryMock.Setup(x => x.CreateMaintenanceTaskAsync(It.Is<MaintenanceTask>(t => t.CarId == 1 && t.MaintenanceTaskStatus == MaintenanceTaskStatus.Pending))).ReturnsAsync(assignedTask);

        _adminRepositoryMock.Setup(x => x.CreateMaintenanceReportAsync(It.Is<MaintenanceReport>(r => r.MaintenanceReportStatus == MaintenanceReportStatus.Pending && r.OverallResult == InspectionResult.Pending))).ReturnsAsync(assignedReport);

        _adminRepositoryMock.Setup(x => x.SaveChangesAsync());

        var result = await _adminService.CreateMaintenanceTaskAsync(carReturnedDto);

        result.CarId.Should().Be(car.CarId);
        result.Make.Should().Be(car.Make);
        result.Model.Should().Be(car.Model);
        result.MaintenanceWorkerId.Should().Be(worker.MaintenanceWorkerId);

        result.MaintenanceTaskStatus.Should().Be(MaintenanceTaskStatus.Pending);

        _adminRepositoryMock.Verify(x => x.GetCarByIdAsync(1), Times.Once);

        _adminRepositoryMock.Verify(x => x.CreateMaintenanceTaskAsync(
        It.Is<MaintenanceTask>(t => t.CarId == 1 && t.MaintenanceTaskStatus == MaintenanceTaskStatus.Pending)), Times.Once);

        _adminRepositoryMock.Verify(x => x.CreateMaintenanceReportAsync(
        It.Is<MaintenanceReport>(r => r.MaintenanceReportStatus == MaintenanceReportStatus.Pending && r.OverallResult == InspectionResult.Pending)), Times.Once);

        _adminRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateMaintenanceWorkerAsync_ShouldThrow_WhenDtoIsNull()
    {
        string userId = "1233";

        await Assert.ThrowsAsync<ArgumentNullException>(() => _adminService.CreateMaintenanceWorkerAsync(null, userId));
    }

    [Fact]
    public async Task CreateMaintenanceWorkerAsync_ShouldCreateMaintenanceWorker_WhenDataIsValid()
    {
        string userId = "1233";

        var assignRole = new AssignRoleDto
        {
            Role = "Maintenance",
            FirstName = "John",
            LastName = "Smith"
        };

        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = userId,
            FirstName = "John",
            LastName = "Smith",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available
        };

        var createdWorker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = userId,
            FirstName = "John",
            LastName = "Smith",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available
        };

        var workerDto = new MaintenanceWorkerDto
        {
            MaintenanceWorkerId = 1,
            FirstName = "John",
            LastName = "Smith",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available
        };

        _mapperMock.Setup(x => x.Map<MaintenanceWorker>(assignRole)).Returns(worker);

        _adminRepositoryMock.Setup(x => x.CreateMaintenanceWorkerAsync(worker)).ReturnsAsync(createdWorker);

        _mapperMock.Setup(x => x.Map<MaintenanceWorkerDto>(createdWorker)).Returns(workerDto);

        var result = await _adminService.CreateMaintenanceWorkerAsync(assignRole, userId);


        result.Should().BeEquivalentTo(workerDto);

        _adminRepositoryMock.Verify(x => x.CreateMaintenanceWorkerAsync(It.Is<MaintenanceWorker>(w => w.UserId == userId && w.MaintenanceWorkerStatus == MaintenanceWorkerStatus.Available)), Times.Once);
    }

    [Fact]
    public async Task PlaceCarOnHireAsync_ShouldThrow_WhenReservationDoesNotExist()
    {
        int reservationId = 1;

        _adminRepositoryMock.Setup(x => x.GetReservationCarByReservationIdAsync(1)).ReturnsAsync((ReservationCar?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.PlaceCarOnHireAsync(1));

        _adminRepositoryMock.Verify(x => x.GetReservationCarByReservationIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task PlaceCarOnHireAsync_ShouldThrow_WhenReservationStatus_IsNotConfirmed()
    {
        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Cancelled
        };

        var reservationCar = new ReservationCar
        {
            ReservationCarId = 1,
            ReservationId = 1,
            Reservation = reservation
        };

        _adminRepositoryMock.Setup(x => x.GetReservationCarByReservationIdAsync(1)).ReturnsAsync(reservationCar);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.PlaceCarOnHireAsync(1));

        _adminRepositoryMock.Verify(x => x.GetReservationCarByReservationIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task PlaceCarOnHireAsync_ShouldThrow_WhenReservationStatusPickUpDate_IsNotToday()
    {
        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(1),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed
        };

        var reservationCar = new ReservationCar
        {
            ReservationCarId = 1,
            ReservationId = 1,
            Reservation = reservation
        };

        _adminRepositoryMock.Setup(x => x.GetReservationCarByReservationIdAsync(1)).ReturnsAsync(reservationCar);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.PlaceCarOnHireAsync(1));

        _adminRepositoryMock.Verify(x => x.GetReservationCarByReservationIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task PlaceCarOnHireAsync_ShouldThrow_WhenReservationCar_IsNotAvailable()
    {
        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.UnderMaintenance
        };

        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(0),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed
        };

        var reservationCar = new ReservationCar
        {
            ReservationCarId = 1,
            ReservationId = 1,
            Reservation = reservation,
            Car = car
        };

        _adminRepositoryMock.Setup(x => x.GetReservationCarByReservationIdAsync(1)).ReturnsAsync(reservationCar);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.PlaceCarOnHireAsync(1));

        _adminRepositoryMock.Verify(x => x.GetReservationCarByReservationIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task PlaceCarOnHireAsync_ShouldReturnOnHireDto_WhenReservationCarDataIsValid()
    {
        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available
        };

        var reservation = new Reservation
        {
            ReservationId = 1,
            CustomerId = 1,
            CarId = 1,
            PickUpDate = GetDate(0),
            ReturnDate = GetDate(4),
            PickUpTime = TimeSpan.FromHours(8),
            ReturnTime = TimeSpan.FromHours(8),
            PickUpBranchId = 1,
            ReturnBranchId = 2,
            ReservationStatus = ReservationStatus.Confirmed
        };

        var reservationCar = new ReservationCar
        {
            ReservationCarId = 1,
            ReservationId = 1,
            Reservation = reservation,
            CarId = 1,
            Car = car
        };

        _adminRepositoryMock.Setup(x => x.GetReservationCarByReservationIdAsync(1)).ReturnsAsync(reservationCar);

        _adminRepositoryMock.Setup(x => x.SaveChangesAsync());

        var result = await _adminService.PlaceCarOnHireAsync(1);

        result.ReservationId.Should().Be(1);
        result.CarId.Should().Be(1);
        result.CarStatus.Should().Be(CarStatus.OutOnHire);

        _adminRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetDashboardCarsCountAsync_ShouldReturnCount()
    {
        int count = 5;

        _adminRepositoryMock.Setup(x => x.GetDashboardCarsCountAsync()).ReturnsAsync(count);

        var result = await _adminService.GetDashboardCarsCountAsync();

        result.Should().Be(count);

        _adminRepositoryMock.Verify(x => x.GetDashboardCarsCountAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllMaintenanceWorkersAsync_ShouldReturnMaintenanceWorkers_WhenDataIsValid()
    {
        int page = 1;
        int pageSize = 10;

        var maintenanceWorker1 = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1
        };

        var maintenanceWorker2 = new MaintenanceWorker
        {
            MaintenanceWorkerId = 2
        };

        var maintenanceWorkerDto1 = new MaintenanceWorkerDto
        {
            MaintenanceWorkerId = 1
        };

        var maintenanceWorkerDto2 = new MaintenanceWorkerDto
        {
            MaintenanceWorkerId = 2
        };

        var maintenanceWorkers = new PagedResult<MaintenanceWorker>
        {
            Items = new List<MaintenanceWorker>
            {
                maintenanceWorker1,
                maintenanceWorker2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        var maintenanceWorkersDto = new PagedResult<MaintenanceWorkerDto>
        {
            Items = new List<MaintenanceWorkerDto>
            {
                maintenanceWorkerDto1,
                maintenanceWorkerDto2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        _adminRepositoryMock.Setup(x => x.GetAllMaintenanceWorkersAsync(page, pageSize)).ReturnsAsync(maintenanceWorkers);

        _mapperMock.Setup(x => x.Map<PagedResult<MaintenanceWorkerDto>>(maintenanceWorkers)).Returns(maintenanceWorkersDto);

        var result = await _adminService.GetAllMaintenanceWorkersAsync(page, pageSize);

        result.Should().BeEquivalentTo(maintenanceWorkersDto);
        result.Items.Should().HaveCount(2);

        _adminRepositoryMock.Verify(x => x.GetAllMaintenanceWorkersAsync(page, pageSize), Times.Once);

        _mapperMock.Verify(x => x.Map<PagedResult<MaintenanceWorkerDto>>(maintenanceWorkers), Times.Once);
    }

    [Fact]
    public async Task GetTasksByMaintenanceWorkerIdAsync_ShouldThrow_WhenWorkerDoesNotExist()
    {
        int page = 1;
        int pageSize = 10;

        _adminRepositoryMock.Setup(x => x.GetMaintenanceWorkerByIdAsync(1)).ReturnsAsync((MaintenanceWorker?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.GetTasksByMaintenanceWorkerIdAsync(1, page, pageSize));

        _adminRepositoryMock.Verify(x => x.GetMaintenanceWorkerByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetTasksByMaintenanceWorkerIdAsync_ShouldReturnMaintenanceTasks_WhenDataIsValid()
    {
        int page = 1;
        int pageSize = 10;

        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1
        };

        var task1 = new MaintenanceTask
        {
            CarId = 1,
            MaintenanceWorkerId = 1,
        };

        var task2 = new MaintenanceTask
        {
            CarId = 2,
            MaintenanceWorkerId = 1,
        };

        var taskDto1 = new MaintenanceTaskDto
        {
            CarId = 1,
            MaintenanceWorkerId = 1,
            Make = "Honda",
            Model = "Civic",
            MaintenanceTaskStatus = MaintenanceTaskStatus.Pending,
            InspectionResult = InspectionResult.Pending,
            MaintenanceReportStatus = MaintenanceReportStatus.Pending
        };

        var taskDto2 = new MaintenanceTaskDto
        {
            CarId = 2,
            MaintenanceWorkerId = 1,
            Make = "Toyota",
            Model = "Yaris",
            MaintenanceTaskStatus = MaintenanceTaskStatus.Pending,
            InspectionResult = InspectionResult.Pending,
            MaintenanceReportStatus = MaintenanceReportStatus.Pending
        };

        var tasks = new PagedResult<MaintenanceTask>
        {
            Items = new List<MaintenanceTask>
            {
               task1,
               task2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        var tasksDto = new PagedResult<MaintenanceTaskDto>
        {
            Items = new List<MaintenanceTaskDto>
            {
               taskDto1,
               taskDto2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };


        _adminRepositoryMock.Setup(x => x.GetMaintenanceWorkerByIdAsync(1)).ReturnsAsync(worker);

        _adminRepositoryMock.Setup(x => x.GetTasksByMaintenanceWorkerIdAsync(1, page, pageSize)).ReturnsAsync(tasks);

        _mapperMock.Setup(x => x.Map<PagedResult<MaintenanceTaskDto>>(tasks)).Returns(tasksDto);

        var result = await _adminService.GetTasksByMaintenanceWorkerIdAsync(1, page, pageSize);

        result.Items.Should().HaveCount(2);
        result.Should().BeEquivalentTo(tasksDto);

        _adminRepositoryMock.Verify(x => x.GetMaintenanceWorkerByIdAsync(1), Times.Once);

        _adminRepositoryMock.Verify(x => x.GetTasksByMaintenanceWorkerIdAsync(1, page, pageSize), Times.Once);

        _mapperMock.Verify(x => x.Map<PagedResult<MaintenanceTaskDto>>(tasks), Times.Once);
    }

    [Fact]
    public async Task GetMaintenanceReportByMaintenanceTaskIdAsync_ShouldThrow_WhenReportDoesNotExist()
    {
        _adminRepositoryMock.Setup(x => x.GetMaintenanceReportByMaintenanceTaskIdAsync(1)).ReturnsAsync((MaintenanceReport?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.GetMaintenanceReportByMaintenanceTaskIdAsync(1));

        _adminRepositoryMock.Verify(x => x.GetMaintenanceReportByMaintenanceTaskIdAsync(1), Times.Once);
    }


    [Fact]
    public async Task GetMaintenanceReportByMaintenanceTaskIdAsync_ShouldReturnReportDto_WhenDataIsValid()
    {
        var report = new MaintenanceReport
        {
            MaintenanceTaskId = 1,
            MaintenanceWorkerId = 1
        };

        var reportDto = new ReportDto
        {
            MaintenanceWorkerId = 1,
        };

        _adminRepositoryMock.Setup(x => x.GetMaintenanceReportByMaintenanceTaskIdAsync(1)).ReturnsAsync(report);

        _mapperMock.Setup(x => x.Map<ReportDto>(report)).Returns(reportDto);

        var result = await _adminService.GetMaintenanceReportByMaintenanceTaskIdAsync(1);

        result.Should().BeEquivalentTo(reportDto);

        _adminRepositoryMock.Verify(x => x.GetMaintenanceReportByMaintenanceTaskIdAsync(1), Times.Once);

        _mapperMock.Verify(x => x.Map<ReportDto>(report), Times.Once);
    }

    [Fact]
    public async Task CompleteCarRepairAsync_ShouldThrow_WhenCarDoesNotExist()
    {
        _adminRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync((Car?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.CompleteCarRepairAsync(1));

        _adminRepositoryMock.Verify(x => x.GetCarByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CompleteCarRepairAsync_ShouldThrow_WhenCarIsNotUnderMaintenance()
    {
        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.Available
        };

        _adminRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        await Assert.ThrowsAsync<ArgumentException>(() => _adminService.CompleteCarRepairAsync(1));

        _adminRepositoryMock.Verify(x => x.GetCarByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CompleteCarRepairAsync_ShouldReturnCompletedRepairDto_WhenDataIsValid()
    {
        var car = new Car
        {
            CarId = 1,
            CarStatus = CarStatus.UnderMaintenance,
            Make = "Honda",
            Model = "Civic"
        };

        _adminRepositoryMock.Setup(x => x.GetCarByIdAsync(1)).ReturnsAsync(car);

        _adminRepositoryMock.Setup(x => x.SaveChangesAsync());

        var result = await _adminService.CompleteCarRepairAsync(1);

        result.Make.Should().Be("Honda");
        result.Model.Should().Be("Civic");

        car.CarStatus.Should().Be(CarStatus.Available);

        _adminRepositoryMock.Verify(x => x.GetCarByIdAsync(1), Times.Once);

        _adminRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
