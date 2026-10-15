using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces
{
  public interface IAdminRepository
  {
    Task<Branch> CreateBranchAsync(Branch branch);

    Task<IEnumerable<Branch>> GetAllBranchesAsync();

    Task<Car> CreateCarAsync(Car car);

    Task<PagedResult<Car>> GetAllCarsAsync(int page, int pageSize);

    Task<int> GetDashboardCarsCountAsync();

    Task<VehicleType> CreateVehicleTypeAsync(VehicleType vehicleType);

    Task<PagedResult<Reservation>> GetAllReservationsAsync(int page, int pageSize);

    Task<int> GetDashboardReservationsCountAsync();

    Task<Reservation?> GetReservationByIdAsync(int reservationId);

    Task<Reservation> UpdateReservationAsync(Reservation reservation);

    Task<IEnumerable<VehicleType>> GetAllVehicleTypesAsync();

    Task<Car?> GetCarByIdAsync(int carId);

    Task<MaintenanceWorker?> GetAvailableMaintenanceWorkerAsync();

    Task<MaintenanceTask> CreateMaintenanceTaskAsync(MaintenanceTask maintenanceTask);

    Task<MaintenanceReport> CreateMaintenanceReportAsync(MaintenanceReport maintenanceReport);

    Task<MaintenanceWorker> CreateMaintenanceWorkerAsync(MaintenanceWorker maintenanceWorker);

    Task<MaintenanceWorker?> GetMaintenanceWorkerByIdAsync(int maintenanceWorkerId);

    Task<ReservationCar?> GetReservationCarByReservationIdAsync(int reservationId);

    Task<int> GetDashboardMaintenanceTasksCountAsync();

    Task<PagedResult<MaintenanceWorker>> GetAllMaintenanceWorkersAsync(int page, int pageSize);

    Task<PagedResult<MaintenanceTask>> GetTasksByMaintenanceWorkerIdAsync(int maintenanceWorkerId, int page, int pageSize);

    Task<MaintenanceReport?> GetMaintenanceReportByMaintenanceTaskIdAsync(int maintenanceTaskId);

    Task<int> GetDashboardCarsUnderMaintenanceCountAsync();

    Task<PagedResult<MaintenanceTask>> GetAllCarsUnderMaintenanceAsync(int page, int pageSize);

    Task<int> GetDashboardMaintenanceWorkersCountAsync();

    Task SaveChangesAsync();
  }
}