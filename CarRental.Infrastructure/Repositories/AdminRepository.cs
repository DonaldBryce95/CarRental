using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repositories
{
  public class AdminRepository : IAdminRepository
  {
    private readonly CarRentalDbContext _context;

    public AdminRepository(CarRentalDbContext context)
    {
      _context = context;
    }

    public async Task<Branch> CreateBranchAsync(Branch branch)
    {
      await _context.Branches.AddAsync(branch);
      await _context.SaveChangesAsync();

      return branch;
    }

    public async Task<Car> CreateCarAsync(Car car)
    {
      await _context.Cars.AddAsync(car);
      await _context.SaveChangesAsync();

      return car;
    }

    public async Task<VehicleType> CreateVehicleTypeAsync(VehicleType vehicleType)
    {
      await _context.VehicleTypes.AddAsync(vehicleType);
      await _context.SaveChangesAsync();

      return vehicleType;
    }

    public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
    {
      var branches = await _context.Branches.AsNoTracking().OrderBy(b => b.BranchId).ToListAsync();

      return branches;
    }

    public async Task<PagedResult<Car>> GetAllCarsAsync(int page, int pageSize)
    {
      var totalCount = await _context.Cars.CountAsync();

      var skip = (page - 1) * pageSize;

      var cars = await _context.Cars.Include(c => c.VehicleType).AsNoTracking().OrderBy(c => c.CarId).Skip(skip).Take(pageSize).ToListAsync();

      return new PagedResult<Car>
      {
        Items = cars,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }

    public async Task<PagedResult<Reservation>> GetAllReservationsAsync(int page, int pageSize)
    {
      var totalCount = await _context.Reservations.CountAsync();

      var skip = (page - 1) * pageSize;

      var reservations = await _context.Reservations.Include(r => r.Customer).OrderBy(r => r.ReservationId).Skip(skip).Take(pageSize).AsNoTracking().ToListAsync();

      return new PagedResult<Reservation>
      {
        Items = reservations,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }

    public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
    {
      return await _context.Reservations.Include(r => r.Car).Include(r => r.Customer).Include(r => r.PickUpBranch).Include(r => r.ReturnBranch).FirstOrDefaultAsync(r => r.ReservationId == reservationId);
    }

    public async Task<IEnumerable<VehicleType>> GetAllVehicleTypesAsync()
    {
      return await _context.VehicleTypes.AsNoTracking().ToListAsync();
    }

    public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
    {
      _context.Reservations.Update(reservation);
      await _context.SaveChangesAsync();

      return reservation;
    }

    public async Task<Car?> GetCarByIdAsync(int carId)
    {
      return await _context.Cars.FirstOrDefaultAsync(c => c.CarId == carId);
    }

    public async Task<MaintenanceWorker?> GetAvailableMaintenanceWorkerAsync()
    {
      return await _context.MaintenanceWorkers.FirstOrDefaultAsync(m => m.MaintenanceWorkerStatus == MaintenanceWorkerStatus.Available);
    }

    public async Task<MaintenanceTask> CreateMaintenanceTaskAsync(MaintenanceTask maintenanceTask)
    {
      await _context.MaintenanceTasks.AddAsync(maintenanceTask);

      await _context.SaveChangesAsync();

      return maintenanceTask;
    }

    public async Task<MaintenanceReport> CreateMaintenanceReportAsync(MaintenanceReport maintenanceReport)
    {
      await _context.MaintenanceReports.AddAsync(maintenanceReport);

      await _context.SaveChangesAsync();

      return maintenanceReport;
    }

    public async Task<MaintenanceWorker> CreateMaintenanceWorkerAsync(MaintenanceWorker maintenanceWorker)
    {
      await _context.MaintenanceWorkers.AddAsync(maintenanceWorker);

      await _context.SaveChangesAsync();

      return maintenanceWorker;
    }

    public async Task<MaintenanceWorker?> GetMaintenanceWorkerByIdAsync(int maintenanceWorkerId)
    {
      return await _context.MaintenanceWorkers.FirstOrDefaultAsync(m => m.MaintenanceWorkerId == maintenanceWorkerId);
    }

    public async Task<ReservationCar?> GetReservationCarByReservationIdAsync(int reservationId)
    {
      return await _context.ReservationCars.Include(rc => rc.Car).Include(rc => rc.Reservation).FirstOrDefaultAsync(rc => rc.ReservationId == reservationId);
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }

    public async Task<PagedResult<MaintenanceWorker>> GetAllMaintenanceWorkersAsync(int page, int pageSize)
    {
      var totalCount = await _context.MaintenanceWorkers.CountAsync();

      var skip = (page - 1) * pageSize;

      var workers = await _context.MaintenanceWorkers.AsNoTracking().OrderBy(m => m.MaintenanceWorkerId).Skip(skip).Take(pageSize).ToListAsync();

      return new PagedResult<MaintenanceWorker>
      {
        Items = workers,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }

    public async Task<PagedResult<MaintenanceTask>> GetTasksByMaintenanceWorkerIdAsync(int maintenanceWorkerId, int page, int pageSize)
    {
      var totalCount = await _context.MaintenanceTasks.Where(m => m.MaintenanceWorkerId == maintenanceWorkerId).CountAsync();

      var skip = (page - 1) * pageSize;

      var tasks = await _context.MaintenanceTasks.Where(m => m.MaintenanceWorkerId == maintenanceWorkerId).AsNoTracking().OrderBy(m => m.MaintenanceTaskId).Include(mc => mc.Car).Include(m => m.MaintenanceReport).Skip(skip).Take(pageSize).ToListAsync();

      return new PagedResult<MaintenanceTask>
      {
        Items = tasks,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }

    public async Task<MaintenanceReport?> GetMaintenanceReportByMaintenanceTaskIdAsync(int maintenanceTaskId)
    {
      return await _context.MaintenanceReports.Include(m => m.MaintenanceWorker).Include(m => m.MaintenanceTask).ThenInclude(m => m.Car).FirstOrDefaultAsync(m => m.MaintenanceTaskId == maintenanceTaskId);
    }

    public async Task<PagedResult<MaintenanceTask>> GetAllCarsUnderMaintenanceAsync(int page, int pageSize)
    {
      var totalCount = await _context.MaintenanceTasks.Include(m => m.Car).Where(c => c.Car.CarStatus == CarStatus.UnderMaintenance).CountAsync();

      var skip = (page - 1) * pageSize;

      var cars = await _context.MaintenanceTasks.Include(m => m.Car).Include(m => m.MaintenanceReport).Where(c => c.Car.CarStatus == CarStatus.UnderMaintenance).AsNoTracking().Skip(skip).Take(pageSize).ToListAsync();

      return new PagedResult<MaintenanceTask>
      {
        Items = cars,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
      };
    }

    public async Task<int> GetDashboardCarsCountAsync()
    {
      return await _context.Cars.CountAsync();
    }

    public async Task<int> GetDashboardReservationsCountAsync()
    {
      return await _context.Reservations.CountAsync(r => r.ReservationStatus == ReservationStatus.Confirmed);
    }

    public async Task<int> GetDashboardMaintenanceTasksCountAsync()
    {
      return await _context.MaintenanceTasks.CountAsync();
    }

    public async Task<int> GetDashboardCarsUnderMaintenanceCountAsync()
    {
      return await _context.Cars.CountAsync(c => c.CarStatus == CarStatus.UnderMaintenance);
    }

    public async Task<int> GetDashboardMaintenanceWorkersCountAsync()
    {
      return await _context.MaintenanceWorkers.CountAsync();
    }
  }
}