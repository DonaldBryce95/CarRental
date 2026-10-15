using CarRental.Application.Dtos;
using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces
{
  public interface IMaintenanceService
  {
    Task<PagedResult<MaintenanceTaskDto>> GetTasksForWorkerAsync(string userId, int page, int pageSize);

    Task<int> GetTasksCountForWorkerAsync(string userId);

    Task<StartTaskDto> StartTaskAsync(int taskId, string userId);

    Task<ReportDto?> GetReportTaskByIdAsync(int taskId, string userId);

    Task<ReportDto> SubmitReportAsync(int taskId, string userId, InspectionResultDto inspectionResultDto);
  }
}