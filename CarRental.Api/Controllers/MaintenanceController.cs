using System.Security.Claims;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize(Roles = "MaintenanceWorker")]
  public class MaintenanceController : ControllerBase
  {
    private readonly IMaintenanceService _maintenanceService;

    public MaintenanceController(IMaintenanceService maintenanceService)
    {
      _maintenanceService = maintenanceService;
    }

    [HttpGet("my-tasks")]
    public async Task<ActionResult<PagedResult<MaintenanceTaskDto>>> GetMyTasks(int page = 1, int pageSize = 10)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if(userId == null)
      {
        return Unauthorized();
      }

      var myTasks = await _maintenanceService.GetTasksForWorkerAsync(userId, page, pageSize);

      return Ok(myTasks);
    }


    [HttpGet("my-tasks/dashboard")]
    public async Task<ActionResult<int>> GetWorkerTaskCount()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if(userId == null)
      {
        return Unauthorized();
      }

      var taskCount = await _maintenanceService.GetTasksCountForWorkerAsync(userId);

      return Ok(taskCount);
    }

    [HttpPost("tasks/{taskId}/start")]
    public async Task<ActionResult<StartTaskDto>> StartTask(int taskId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if(userId == null)
      {
        return Unauthorized();
      }

      var startTask = await _maintenanceService.StartTaskAsync(taskId, userId);

      return Ok(startTask);
    }

    [HttpGet("reports/{taskId}")]
    public async Task<ActionResult<ReportDto>> GetMaintenanceReport(int taskId)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if(userId == null)
      {
        return Unauthorized();
      }

      var report = await _maintenanceService.GetReportTaskByIdAsync(taskId, userId);

      if(report == null)
      {
        return Forbid();
      }

      return Ok(report);
    }

    [HttpPost("maintenance/report/{taskId}/submit")]
    public async Task<ActionResult<ReportDto>> SubmitReport(int taskId, [FromBody]InspectionResultDto inspectionResultDto)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      if(userId == null)
      {
        return Unauthorized();
      }

      var report = await _maintenanceService.SubmitReportAsync(taskId, userId, inspectionResultDto);

      return Ok(report);
    }
  }
}