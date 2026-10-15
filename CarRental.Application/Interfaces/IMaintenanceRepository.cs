using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces
{
  public interface IMaintenanceRepository
  {
    Task<PagedResult<MaintenanceTask>> GetTasksForWorkerAsync(string userId, int page, int pageSize);

    Task<int> GetTasksCountForWorkerAsync(string userId);

    Task<MaintenanceWorker?> GetMaintenanceWorkerByUserIdAsync(string userId);

    Task<MaintenanceTask?> GetMaintenanceTaskByIdAsync(int taskId);

    Task SaveChangesAsync();
  }
}