using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces
{
  public interface IAdminService
  {
    Task<BranchDto> CreateBranchAsync(CreateBranchDto createBranchDto);

    Task<IEnumerable<BranchDto>> GetAllBranchesAsync();

    Task<CarDto> CreateCarAsync(CreateCarDto createCarDto);

    Task<PagedResult<AdminCarDto>> GetAllCarsAsync(int page, int pageSize);

    Task<int> GetDashboardCarsCountAsync();

    Task<VehicleTypeDto> CreateVehicleTypeAsync(CreateVehicleTypeDto createVehicleTypeDto);

    Task<PagedResult<AdminReservationDto>> GetAllReservationsAsync(int page, int pageSize);

    Task<int> GetDashboardReservationsCountAsync();

    Task<AdminReservationDto?> GetReservationByIdAsync(int reservationId);

    Task<AdminReservationDto> CancelReservationAsync(int reservationId);

    Task<IEnumerable<VehicleTypeDto>> GetAllVehicleTypesAsync();

    Task<MaintenanceTaskDto> CreateMaintenanceTaskAsync(CarReturnedDto carReturnedDto);

    Task<MaintenanceWorkerDto> CreateMaintenanceWorkerAsync(AssignRoleDto assignRoleDto, string userId);

    Task<int> GetDashboardMaintenanceTasksCountAsync();

    Task<PagedResult<MaintenanceWorkerDto>> GetAllMaintenanceWorkersAsync(int page, int pageSize);

    Task<PagedResult<MaintenanceTaskDto?>> GetTasksByMaintenanceWorkerIdAsync(int maintenanceWorkerId, int page, int pageSize);

    Task<ReportDto?> GetMaintenanceReportByMaintenanceTaskIdAsync(int maintenanceTaskId);

    Task<OnHireDto> PlaceCarOnHireAsync(int reservationId);

    Task<int> GetDashboardCarsUnderMaintenanceCountAsync();

    Task<PagedResult<MaintenanceTaskDto>> GetAllCarsUnderMaintenanceAsync(int page, int pageSize);

    Task<int> GetDashboardMaintenanceWorkersCountAsync();

    Task<CompletedRepairDto> CompleteCarRepairAsync(int carId);
  }
}