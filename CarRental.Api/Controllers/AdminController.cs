using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize(Roles = "Admin")]
  public class AdminController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      _adminService = adminService;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    [HttpGet("admin/customers")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomersAsync()
    {
      var customers = await _userManager.Users.Where(u => u.CustomerId != null).Select(u => new CustomerDto
      {
        UserId = u.Id,
        FirstName = u.Customer!.FirstName,
        LastName = u.Customer!.LastName
      })
      .ToListAsync();

      return Ok(customers);
    }

    [HttpGet("admin/roles")]
    public async Task<ActionResult<IEnumerable<RolesDto>>> GetAllRolesAsync()
    {
      var roles = await _roleManager.Roles.Select(r => new RolesDto
      {
        Name = r.Name!
      })
      .ToListAsync();

      return Ok(roles);
    }

    [HttpPost("users/{userId}/role")]
    public async Task<IActionResult> AssignRole(string userId, AssignRoleDto assignRoleDto)
    {
      var user = await _userManager.FindByIdAsync(userId);

      if (user == null)
      {
        return NotFound(new
        {
          message = "User not found",
          userId = userId
        });
      }

      var currentRoles = await _userManager.GetRolesAsync(user);

      if (currentRoles.Any())
      {
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
        {
          return BadRequest(removeResult.Errors);
        }
      }

      var addResult = await _userManager.AddToRoleAsync(user, assignRoleDto.Role);

      if (!addResult.Succeeded)
      {
        return BadRequest(addResult.Errors);
      }

      if (assignRoleDto.Role == "MaintenanceWorker")
      {
        await _adminService.CreateMaintenanceWorkerAsync(assignRoleDto, user.Id);

        user.CustomerId = null;
      }

      var updatedUser = await _userManager.UpdateAsync(user);

      if(!updatedUser.Succeeded)
      {
        return BadRequest(updatedUser.Errors);
      }

      return Ok(new
      {
        message = "Role assigned successfully.",
        userId = userId,
        role = assignRoleDto.Role
      });
    }

    [HttpPost("cars")]
    public async Task<ActionResult<CarDto>> CreateCar(CreateCarDto createCarDto)
    {
      var car = await _adminService.CreateCarAsync(createCarDto);

      return Ok(car);
    }

    [HttpPost("vehicle-types")]
    public async Task<ActionResult<VehicleTypeDto>> CreateVehicleType(CreateVehicleTypeDto createVehicleTypeDto)
    {
      var vehicleType = await _adminService.CreateVehicleTypeAsync(createVehicleTypeDto);

      return Ok(vehicleType);
    }

    [HttpPost("branches")]
    public async Task<ActionResult<BranchDto>> CreateBranch(CreateBranchDto createBranchDto)
    {
      var branch = await _adminService.CreateBranchAsync(createBranchDto);

      return Ok(branch);
    }

    [HttpGet("cars")]
    public async Task<ActionResult<PagedResult<AdminCarDto>>> GetAllCars(int page = 1, int pageSize = 10)
    {
      var cars = await _adminService.GetAllCarsAsync(page, pageSize);

      return Ok(cars);
    }

    [HttpGet("cars/dashboard")]
    public async Task<ActionResult<int>> GetCarsCount()
    {
      var cars = await _adminService.GetDashboardCarsCountAsync();

      return Ok(cars);
    }

    [HttpGet("branches")]
    public async Task<ActionResult<PagedResult<BranchDto>>> GetAllBranches()
    {
      var branches = await _adminService.GetAllBranchesAsync();

      return Ok(branches);
    }

    [HttpGet("reservations")]
    public async Task<ActionResult<PagedResult<AdminReservationDto>>> GetAllReservations(int page = 1, int pageSize = 10)
    {
      var reservations = await _adminService.GetAllReservationsAsync(page, pageSize);

      return Ok(reservations);
    }

    [HttpGet("reservations/dashboard")]
    public async Task<ActionResult<int>> GetReservationsCount()
    {
      var reservations = await _adminService.GetDashboardReservationsCountAsync();

      return Ok(reservations);
    }

    [HttpGet("reservations/{reservationId}")]
    public async Task<ActionResult<AdminReservationDto>> GetReservationById(int reservationId)
    {
      var reservation = await _adminService.GetReservationByIdAsync(reservationId);

      if(reservation == null)
      {
        return NotFound();
      }

      return Ok(reservation);
    }

    [HttpPut("reservations/{reservationId}/cancel")]
    public async Task<ActionResult<AdminReservationDto>> CancelReservation(int reservationId)
    {
      var cancelledReservation = await _adminService.CancelReservationAsync(reservationId);

      return Ok(cancelledReservation);
    }

    [HttpGet("vehicle-types")]
    public async Task<ActionResult<IEnumerable<VehicleTypeDto>>> GetAllVehicleTypesAsync()
    {
      var vehicleTypes = await _adminService.GetAllVehicleTypesAsync();

      return Ok(vehicleTypes);
    }

    [HttpPost("maintenance-tasks")]
    public async Task<ActionResult<MaintenanceTaskDto>> CreateMaintenanceTask(CarReturnedDto carReturnedDto)
    {
      var createdTask = await _adminService.CreateMaintenanceTaskAsync(carReturnedDto);

      return createdTask;
    }

    [HttpPut("reservations/{reservationId}/hire")]
    public async Task<ActionResult<OnHireDto>> PlaceCarOnHire(int reservationId)
    {
      var reservationCar = await _adminService.PlaceCarOnHireAsync(reservationId);

      return Ok(reservationCar);
    }

    [HttpGet("maintenance-tasks-dashboard")]
    public async Task<ActionResult<int>> GetMaintenanceWorkerTasksCount()
    {
      var tasks = await _adminService.GetDashboardMaintenanceTasksCountAsync();

      return Ok(tasks);
    }

    [HttpGet("maintenance/repairs/dashboard")]
    public async Task<ActionResult<int>> GetCarsUnderMaintenanceCount()
    {
      var cars = await _adminService.GetDashboardCarsUnderMaintenanceCountAsync();

      return Ok(cars);
    }

    [HttpGet("maintenance/repairs/tasks")]
    public async Task<ActionResult<PagedResult<MaintenanceTaskDto>>> GetAllCarsUnderMaintenanceCars(int page = 1, int pageSize = 10)
    {
      var cars = await _adminService.GetAllCarsUnderMaintenanceAsync(page, pageSize);

      return Ok(cars);
    }

    [HttpGet("maintenance-workers")]
    public async Task<ActionResult<PagedResult<MaintenanceWorkerDto>>> GetAllMaintenanceWorkers(int page = 1, int pageSize = 10)
    {
      var workers = await _adminService.GetAllMaintenanceWorkersAsync(page, pageSize);

      return Ok(workers);
    }

    [HttpGet("maintenance-worker/{maintenanceWorkerId}/tasks")]
    public async Task<ActionResult<PagedResult<MaintenanceTaskDto>>> GetWorkerTasks(int maintenanceWorkerId, int page = 1, int pageSize = 10)
    {
      var tasks = await _adminService.GetTasksByMaintenanceWorkerIdAsync(maintenanceWorkerId, page, pageSize);

      return Ok(tasks);
    }

    [HttpGet("maintenance/report/{maintenanceTaskId}")]
    public async Task<ActionResult<ReportDto>> GetMaintenanceWorkerReport(int maintenanceTaskId)
    {
      var report = await _adminService.GetMaintenanceReportByMaintenanceTaskIdAsync(maintenanceTaskId);

      return Ok(report);
    }

    [HttpPut("maintenance/complete/repair/{carId}")]
    public async Task<ActionResult<CompletedRepairDto>> CompleteCarRepair(int carId)
    {
      var repair = await _adminService.CompleteCarRepairAsync(carId);

      return Ok(repair);
    }

    [HttpGet("maintenance/workers/dash")]
    public async Task<ActionResult<int>> GetMaintenanceWorkersCount()
    {
      var workers = await _adminService.GetDashboardMaintenanceWorkersCountAsync();

      return Ok(workers);
    }
  }
}