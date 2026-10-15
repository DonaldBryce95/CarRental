using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;

namespace CarRental.Application.Services
{
  public class MaintenanceService : IMaintenanceService
  {
    private readonly IMaintenanceRepository _maintenanceRepository;

    private readonly IMapper _mapper;

    public MaintenanceService(IMaintenanceRepository maintenanceRepository, IMapper mapper)
    {
      _maintenanceRepository = maintenanceRepository;
      _mapper = mapper;
    }
    public async Task<PagedResult<MaintenanceTaskDto>> GetTasksForWorkerAsync(string userId, int page, int pageSize)
    {
      var tasks = await _maintenanceRepository.GetTasksForWorkerAsync(userId, page, pageSize);

      return _mapper.Map<PagedResult<MaintenanceTaskDto>>(tasks);
    }

    public async Task<StartTaskDto> StartTaskAsync(int taskId, string userId)
    {
      var worker = await _maintenanceRepository.GetMaintenanceWorkerByUserIdAsync(userId);

      if(worker == null)
      {
        throw new ArgumentException($"Worker with User ID: {userId} was not found.");
      }

      if(worker.MaintenanceWorkerStatus == Domain.Entities.MaintenanceWorkerStatus.Unavailable)
      {
        throw new ArgumentException($"Worker ID: {worker.MaintenanceWorkerId} Unavailable, cannot start new Task.");
      }

      var task = await _maintenanceRepository.GetMaintenanceTaskByIdAsync(taskId);

      if(task == null)
      {
        throw new ArgumentException($"Task ID: {taskId} was not found.");
      }

      if(task.MaintenanceWorkerId != worker.MaintenanceWorkerId)
      {
        throw new ArgumentException($"Task ID: {task.MaintenanceTaskId} does not belong to worker Worker ID: {worker.MaintenanceWorkerId}");
      }

      if(task.MaintenanceTaskStatus != Domain.Entities.MaintenanceTaskStatus.Pending)
      {
        throw new ArgumentException($"Task ID: {task.MaintenanceTaskId} cannot be started.");
      }

      task.MaintenanceTaskStatus = Domain.Entities.MaintenanceTaskStatus.InProgress;
      task.MaintenanceReport.MaintenanceReportStatus = Domain.Entities.MaintenanceReportStatus.InProgress;
      task.MaintenanceWorker.MaintenanceWorkerStatus = Domain.Entities.MaintenanceWorkerStatus.Unavailable;

      await _maintenanceRepository.SaveChangesAsync();

      var newTask = new StartTaskDto
      {
        MaintenanceTaskId = task.MaintenanceTaskId,
        Make = task.Car.Make,
        Model = task.Car.Model,
        FirstName = worker.FirstName,
        LastName = worker.LastName
      };

      return newTask;
    }

    public async Task<ReportDto?> GetReportTaskByIdAsync(int taskId, string userId)
    {
      var task = await _maintenanceRepository.GetMaintenanceTaskByIdAsync(taskId);

      if(task == null)
      {
        throw new ArgumentException($"Maintenance Task ID: {taskId} was not found.");
      }

      if(task.MaintenanceWorker.UserId != userId)
      {
        return null;
      }

      var report = new ReportDto
      {
        FirstName = task.MaintenanceWorker.FirstName,
        LastName = task.MaintenanceWorker.LastName,
        Model = task.Car.Model,
        Make = task.Car.Make,
        InteriorCondition = task.MaintenanceReport.InteriorCondition,
        Brakes = task.MaintenanceReport.Brakes,
        Lights = task.MaintenanceReport.Lights,
        Glass = task.MaintenanceReport.Glass,
        Tyres = task.MaintenanceReport.Tyres,
        OverallResult = task.MaintenanceReport.OverallResult,
        Engine = task.MaintenanceReport.Engine,
        Battery = task.MaintenanceReport.Battery,
        AirConditioning = task.MaintenanceReport.AirConditioning,
        WarningLights = task.MaintenanceReport.WarningLights,
        BodyWork = task.MaintenanceReport.BodyWork,
        FuelLevel = task.MaintenanceReport.FuelLevel
      };
      return report;
    }

    public async Task<ReportDto> SubmitReportAsync(int taskId, string userId, InspectionResultDto inspectionResultDto)
    {

      if(inspectionResultDto == null)
      {
        throw new ArgumentException("Inspection Result Cannot be Null");
      }

      var task = await _maintenanceRepository.GetMaintenanceTaskByIdAsync(taskId);

      if(task == null)
      {
        throw new ArgumentException($"Maintenance Task ID: {taskId} was not found.");
      }

      if(task.MaintenanceWorker.UserId != userId)
      {
        throw new ArgumentException($"Maintenance worker is not assigned to this task.");
      }

      var report = task.MaintenanceReport;

      report.Tyres = inspectionResultDto.Tyres;
      report.Battery = inspectionResultDto.Battery;
      report.Lights = inspectionResultDto.Lights;
      report.Brakes = inspectionResultDto.Brakes;
      report.Engine = inspectionResultDto.Engine;
      report.AirConditioning = inspectionResultDto.AirConditioning;
      report.WarningLights = inspectionResultDto.WarningLights;
      report.BodyWork = inspectionResultDto.BodyWork;
      report.Glass = inspectionResultDto.Glass;
      report.InteriorCondition = inspectionResultDto.InteriorCondition;
      report.FuelLevel = inspectionResultDto.FuelLevel;
      report.OverallResult = inspectionResultDto.OverallResult;
      report.MaintenanceReportStatus = MaintenanceReportStatus.Completed;


      task.MaintenanceTaskStatus = MaintenanceTaskStatus.Completed;

      if(inspectionResultDto.OverallResult == InspectionResult.Fail)
      {
        task.Car.CarStatus = CarStatus.UnderMaintenance;
      }
      else
      {
        task.Car.CarStatus = CarStatus.Available;
      }

      task.MaintenanceWorker.MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available;

      await _maintenanceRepository.SaveChangesAsync();

      var reportDto = new ReportDto
      {
        FirstName = task.MaintenanceWorker.FirstName,
        LastName = task.MaintenanceWorker.LastName,
        Make = task.Car.Make,
        Model = task.Car.Model,
        OverallResult = report.OverallResult,
        Tyres = report.Tyres,
        Lights = report.Lights,
        Brakes = report.Brakes,
        Engine = report.Engine,
        Battery = report.Battery,
        AirConditioning = report.AirConditioning,
        WarningLights = report.WarningLights,
        BodyWork = report.BodyWork,
        Glass = report.Glass,
        InteriorCondition = report.InteriorCondition,
        FuelLevel = report.FuelLevel
      };

      return reportDto;
    }

    public async Task<int> GetTasksCountForWorkerAsync(string userId)
    {
      var tasks = await _maintenanceRepository.GetTasksCountForWorkerAsync(userId);

      return tasks;
    }
  }
}