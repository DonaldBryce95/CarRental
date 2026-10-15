using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories
{
  public class MaintenanceRepository : IMaintenanceRepository
  {
    private readonly CarRentalDbContext _context;

    public MaintenanceRepository(CarRentalDbContext context)
    {
      _context = context;
    }

    public async Task<int> GetTasksCountForWorkerAsync(string userId)
    {
      return await _context.MaintenanceTasks.CountAsync(m => m.MaintenanceWorker.UserId == userId);
    }

    public async Task<MaintenanceTask?> GetMaintenanceTaskByIdAsync(int taskId)
    {
      return await _context.MaintenanceTasks.Include(m => m.MaintenanceReport).Include(m => m.MaintenanceWorker).Include(m => m.Car).FirstOrDefaultAsync(m => m.MaintenanceTaskId == taskId);
    }

    public async Task<MaintenanceWorker?> GetMaintenanceWorkerByUserIdAsync(string userId)
    {
      return await _context.MaintenanceWorkers.FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task<PagedResult<MaintenanceTask>> GetTasksForWorkerAsync(string userId, int page, int pageSize)
    {
      var totalCount = await _context.MaintenanceTasks.Where(m => m.MaintenanceWorker.UserId == userId).CountAsync();

      var skip = (page - 1) * pageSize;

      var tasks = await _context.MaintenanceTasks.Where(m => m.MaintenanceWorker.UserId == userId).Include(m => m.Car).Include(m => m.MaintenanceReport).AsNoTracking().OrderBy(m => m.MaintenanceTaskId).Skip(skip).Take(pageSize).ToListAsync();

      return new PagedResult<MaintenanceTask>
      {
        Items = tasks,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}