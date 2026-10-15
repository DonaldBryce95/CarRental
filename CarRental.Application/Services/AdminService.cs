using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;

namespace CarRental.Application.Services
{
  public class AdminService : IAdminService
  {
    private readonly IAdminRepository _adminRepository;

    private readonly IMapper _mapper;

    public AdminService(IAdminRepository adminRepository, IMapper mapper)
    {
      _adminRepository = adminRepository;
      _mapper = mapper;
    }

    public async Task<AdminReservationDto>CancelReservationAsync(int reservationId)
    {
      var reservation = await _adminRepository.GetReservationByIdAsync(reservationId);

      if(reservation == null)
      {
        throw new ArgumentException($"Reservation ID: {reservationId} was not found.");
      }

      if(reservation.ReservationStatus == ReservationStatus.Cancelled)
      {
          throw new ArgumentException("Reservation already cancelled.");
      }

      reservation.ReservationStatus = ReservationStatus.Cancelled;

      var updatedReservation = await _adminRepository.UpdateReservationAsync(reservation);

      return _mapper.Map<AdminReservationDto>(updatedReservation);
    }

    public async Task<BranchDto> CreateBranchAsync(CreateBranchDto createBranchDto)
    {
      if(createBranchDto == null)
      {
        throw new ArgumentNullException(nameof(createBranchDto));
      }

      var branch = _mapper.Map<Branch>(createBranchDto);

      var createdBranch = await _adminRepository.CreateBranchAsync(branch);

      return _mapper.Map<BranchDto>(createdBranch);
    }

    public async Task<CarDto> CreateCarAsync(CreateCarDto createCarDto)
    {
      if(createCarDto == null)
      {
        throw new ArgumentNullException(nameof(createCarDto));
      }

      var car = _mapper.Map<Car>(createCarDto);

      var createdCar = await _adminRepository.CreateCarAsync(car);

      return _mapper.Map<CarDto>(createdCar);
    }

    public async Task<VehicleTypeDto> CreateVehicleTypeAsync(CreateVehicleTypeDto createVehicleTypeDto)
    {
      if(createVehicleTypeDto == null)
      {
        throw new ArgumentNullException(nameof(createVehicleTypeDto));
      }

      var vehicleType = _mapper.Map<VehicleType>(createVehicleTypeDto);

      var createdVehicleType = await _adminRepository.CreateVehicleTypeAsync(vehicleType);

      return _mapper.Map<VehicleTypeDto>(createdVehicleType);
    }

    public async Task<IEnumerable<BranchDto>> GetAllBranchesAsync()
    {
      var branches = await _adminRepository.GetAllBranchesAsync();

      return _mapper.Map<IEnumerable<BranchDto>>(branches);
    }

    public async Task<PagedResult<AdminCarDto>> GetAllCarsAsync(int page, int pageSize)
    {
      var cars = await _adminRepository.GetAllCarsAsync(page, pageSize);

      return _mapper.Map<PagedResult<AdminCarDto>>(cars);
    }

    public async Task<PagedResult<AdminReservationDto>> GetAllReservationsAsync(int page, int pageSize)
    {
      var reservations = await _adminRepository.GetAllReservationsAsync(page, pageSize);

      return _mapper.Map<PagedResult<AdminReservationDto>>(reservations);
    }

    public async Task<IEnumerable<VehicleTypeDto>> GetAllVehicleTypesAsync()
    {
      var vehicleTypes =  await _adminRepository.GetAllVehicleTypesAsync();

      return _mapper.Map<IEnumerable<VehicleTypeDto>>(vehicleTypes);
    }

    public async Task<AdminReservationDto?> GetReservationByIdAsync(int reservationId)
    {
      var reservation = await _adminRepository.GetReservationByIdAsync(reservationId);

      if(reservation == null)
      {
        return null;
      }

      return _mapper.Map<AdminReservationDto>(reservation);
    }

    public async Task<MaintenanceTaskDto> CreateMaintenanceTaskAsync(CarReturnedDto carReturnedDto)
    {

      if(carReturnedDto == null)
      {
        throw new ArgumentNullException(nameof(carReturnedDto));
      }

      var car = await _adminRepository.GetCarByIdAsync(carReturnedDto.CarId);

      if(car == null)
      {
        throw new ArgumentException($"Car ID: {carReturnedDto.CarId} could not be found.");
      }

      if(car.CarStatus != CarStatus.OutOnHire)
      {
        throw new ArgumentException($"Car {car.CarId} is not on hire so cannot be returned.");
      }

      car.CarStatus = CarStatus.UnderMaintenance;

      var worker = await _adminRepository.GetAvailableMaintenanceWorkerAsync();

      if(worker == null)
      {
        var task = new MaintenanceTask
        {
          CarId = carReturnedDto.CarId,
          MaintenanceTaskStatus = MaintenanceTaskStatus.AwaitingAssignment
        };

        var createdTask = await _adminRepository.CreateMaintenanceTaskAsync(task);

        var report = new MaintenanceReport
        {
          MaintenanceTaskId = createdTask.MaintenanceTaskId,
          OverallResult = InspectionResult.Pending,
          MaintenanceReportStatus = MaintenanceReportStatus.Pending
        };

        var createdReport = await _adminRepository.CreateMaintenanceReportAsync(report);

        var maintenanceTaskDto = new MaintenanceTaskDto
        {
          MaintenanceTaskId = createdTask.MaintenanceTaskId,
          CarId = carReturnedDto.CarId,
          MaintenanceTaskStatus = createdTask.MaintenanceTaskStatus,
          Make = car.Make,
          Model = car.Model,
          MaintenanceReportStatus = createdReport.MaintenanceReportStatus,
          InspectionResult = createdReport.OverallResult,
          MaintenanceWorkerId = createdTask.MaintenanceWorkerId
        };

        await _adminRepository.SaveChangesAsync();

        return maintenanceTaskDto;
      }

      var assignedTask = new MaintenanceTask
      {
        CarId = carReturnedDto.CarId,
        MaintenanceWorkerId = worker.MaintenanceWorkerId,
        MaintenanceTaskStatus = MaintenanceTaskStatus.Pending
      };

      var createAssignedTask = await _adminRepository.CreateMaintenanceTaskAsync(assignedTask);

      var assignedReport = new MaintenanceReport
      {
        MaintenanceTaskId = createAssignedTask.MaintenanceTaskId,
        MaintenanceWorkerId = worker.MaintenanceWorkerId,
        OverallResult = InspectionResult.Pending,
        MaintenanceReportStatus = MaintenanceReportStatus.Pending
      };

      var createAssignedReport = await _adminRepository.CreateMaintenanceReportAsync(assignedReport);

      var assignedMaintenanceDto = new MaintenanceTaskDto
      {
        MaintenanceTaskId = createAssignedTask.MaintenanceTaskId,
        CarId = carReturnedDto.CarId,
        Make = car.Make,
        Model = car.Model,
        MaintenanceWorkerId = worker.MaintenanceWorkerId,
        MaintenanceTaskStatus = MaintenanceTaskStatus.Pending,
        MaintenanceReportStatus = createAssignedReport.MaintenanceReportStatus,
        InspectionResult = createAssignedReport.OverallResult
      };

      await _adminRepository.SaveChangesAsync();

      return assignedMaintenanceDto;
    }

    public async Task<MaintenanceWorkerDto> CreateMaintenanceWorkerAsync(AssignRoleDto assignRoleDto, string userId)
    {
      if(assignRoleDto == null)
      {
        throw new ArgumentNullException(nameof(assignRoleDto));
      }

      var createdWorker = _mapper.Map<MaintenanceWorker>(assignRoleDto);

      createdWorker.UserId = userId;

      createdWorker.MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available;

      var newWorker = await _adminRepository.CreateMaintenanceWorkerAsync(createdWorker);

      return _mapper.Map<MaintenanceWorkerDto>(newWorker);
    }

    public async Task<OnHireDto> PlaceCarOnHireAsync(int reservationId)
    {
      var reservationCar = await _adminRepository.GetReservationCarByReservationIdAsync(reservationId);

      if(reservationCar == null)
      {
        throw new ArgumentException($"No car was found for Reservation ID: {reservationId}");
      }

      if(reservationCar.Reservation.ReservationStatus != ReservationStatus.Confirmed)
      {
        throw new ArgumentException($"Reservation ID: {reservationId} requires confirmation before the car can be hired");
      }

      if(reservationCar.Reservation.PickUpDate != DateOnly.FromDateTime(DateTime.UtcNow))
      {
        throw new ArgumentException($"Car cannot be hired untill the reservation date: {reservationCar.Reservation.PickUpDate}.");
      }

      if(reservationCar.Car.CarStatus != CarStatus.Available)
      {
        throw new ArgumentException($"Car is unavailable");
      }

      reservationCar.Car.CarStatus = CarStatus.OutOnHire;

      await _adminRepository.SaveChangesAsync();

      var onHire = new OnHireDto
      {
        ReservationId = reservationCar.ReservationId,
        CarId = reservationCar.CarId,
        CarStatus = reservationCar.Car.CarStatus
      };

      return onHire;
    }

    public async Task<int> GetDashboardCarsCountAsync()
    {
      var cars = await _adminRepository.GetDashboardCarsCountAsync();

      return cars;
    }

    public async Task<int> GetDashboardReservationsCountAsync()
    {
      var reservations = await _adminRepository.GetDashboardReservationsCountAsync();

      return reservations;
    }

    public async Task<int> GetDashboardMaintenanceTasksCountAsync()
    {
      var tasks = await _adminRepository.GetDashboardMaintenanceTasksCountAsync();

      return tasks;
    }

    public async Task<PagedResult<MaintenanceWorkerDto>> GetAllMaintenanceWorkersAsync(int page, int pageSize)
    {
      var worker = await _adminRepository.GetAllMaintenanceWorkersAsync(page, pageSize);

      return _mapper.Map<PagedResult<MaintenanceWorkerDto>>(worker);
    }

    public async Task<PagedResult<MaintenanceTaskDto?>> GetTasksByMaintenanceWorkerIdAsync(int maintenanceWorkerId, int page, int pageSize)
    {
      var worker = await _adminRepository.GetMaintenanceWorkerByIdAsync(maintenanceWorkerId);

      if(worker == null)
      {
        throw new ArgumentException($"Maintenance Worker ID: {maintenanceWorkerId} was not found.");
      }

      var tasks = await _adminRepository.GetTasksByMaintenanceWorkerIdAsync(maintenanceWorkerId, page, pageSize);

      return _mapper.Map<PagedResult<MaintenanceTaskDto?>>(tasks);
    }

    public async Task<ReportDto?> GetMaintenanceReportByMaintenanceTaskIdAsync(int maintenanceTaskId)
    {
      var report = await _adminRepository.GetMaintenanceReportByMaintenanceTaskIdAsync(maintenanceTaskId);

      if(report == null)
      {
        throw new ArgumentException($"Report for Maintenance ID: {maintenanceTaskId} could not be found");
      }

      return _mapper.Map<ReportDto>(report);
    }

    public async Task<int> GetDashboardCarsUnderMaintenanceCountAsync()
    {
      var cars = await _adminRepository.GetDashboardCarsUnderMaintenanceCountAsync();

      return cars;
    }

    public async Task<PagedResult<MaintenanceTaskDto>> GetAllCarsUnderMaintenanceAsync(int page, int pageSize)
    {
      var cars = await _adminRepository.GetAllCarsUnderMaintenanceAsync(page, pageSize);

      return _mapper.Map<PagedResult<MaintenanceTaskDto>>(cars);
    }

    public async Task<CompletedRepairDto> CompleteCarRepairAsync(int carId)
    {
      var car = await _adminRepository.GetCarByIdAsync(carId);

      if(car == null)
      {
        throw new ArgumentException($"Car ID: {carId} could not be found.");
      }

      if(car.CarStatus != CarStatus.UnderMaintenance)
      {
        throw new ArgumentException($"Car ID: {carId} is not shceduled for maintenance.");
      }

      car.CarStatus = CarStatus.Available;

      await _adminRepository.SaveChangesAsync();

      var carRepairDto = new CompletedRepairDto
      {
        Make = car.Make,
        Model = car.Model,
        CompletedAt = DateTime.UtcNow
      };

      return carRepairDto;
    }

    public async Task<int> GetDashboardMaintenanceWorkersCountAsync()
    {
      var workers = await _adminRepository.GetDashboardMaintenanceWorkersCountAsync();

      return workers;
    }
  }
}