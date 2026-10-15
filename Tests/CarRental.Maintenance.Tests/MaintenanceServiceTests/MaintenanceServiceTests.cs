using AutoMapper;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using Moq;
using Shouldly;

namespace MaintenanceServiceTests;

public class MaintenanceServiceTests
{
    private readonly Mock<IMaintenanceRepository> _maintenanceRepositoryMock;

    private readonly Mock<IMapper> _mapperMock;

    private readonly MaintenanceService _maintenanceService;

    public MaintenanceServiceTests()
    {
        _maintenanceRepositoryMock = new Mock<IMaintenanceRepository>();

        _mapperMock = new Mock<IMapper>();

        _maintenanceService = new MaintenanceService(_maintenanceRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetTasksForWorkerAsync_ShouldReturnMaintenanceTaskDto_WhenDataIsValid()
    {
        int page = 1;
        int pageSize = 10;

        var task1 = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorkerId = 1
        };

        var task2 = new MaintenanceTask
        {
            MaintenanceTaskId = 2,
            MaintenanceWorkerId = 1
        };

        var tasks = new PagedResult<MaintenanceTask>
        {
            Items = new List<MaintenanceTask>
            {
                task1,
                task2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        var taskDto1 = new MaintenanceTaskDto
        {
            MaintenanceTaskId = 1,
            MaintenanceWorkerId = 1,
        };

        var taskDto2 = new MaintenanceTaskDto
        {
            MaintenanceTaskId = 2,
            MaintenanceWorkerId = 1
        };

        var tasksDto = new PagedResult<MaintenanceTaskDto>
        {
            Items = new List<MaintenanceTaskDto>
            {
                taskDto1,
                taskDto2
            },
            Page = 1,
            PageSize = 10,
            TotalCount = 2
        };

        _maintenanceRepositoryMock.Setup(x => x.GetTasksForWorkerAsync("1234", page, pageSize)).ReturnsAsync(tasks);

        _mapperMock.Setup(x => x.Map<PagedResult<MaintenanceTaskDto>>(tasks)).Returns(tasksDto);

        var result = await _maintenanceService.GetTasksForWorkerAsync("1234", page, pageSize);

        result.ShouldBeEquivalentTo(tasksDto);

        _maintenanceRepositoryMock.Verify(x => x.GetTasksForWorkerAsync("1234", page, pageSize), Times.Once);

        _mapperMock.Verify(x => x.Map<PagedResult<MaintenanceTaskDto>>(tasks), Times.Once);
    }

    [Fact]
    public async Task StartTaskAsync_ShouldThrowArgumentException_WhenWorkerDoesNotExist()
    {
        string userId = "1234";

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceWorkerByUserIdAsync(userId)).ReturnsAsync((MaintenanceWorker?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.StartTaskAsync(1, userId));

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceWorkerByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task StartTaskAsync_ShouldThrowArgumentException_WhenWorkerIsNotAvailable()
    {

        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = "1234",
            FirstName = "John",
            LastName = "Doe",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Unavailable
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceWorkerByUserIdAsync("1234")).ReturnsAsync(worker);

        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.StartTaskAsync(1, "1234"));

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceWorkerByUserIdAsync("1234"), Times.Once);
    }

    [Fact]
    public async Task StartTaskAsync_ShouldThrowArgumentException_WhenTaskDoesNotExist()
    {

        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = "1234",
            FirstName = "John",
            LastName = "Doe",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceWorkerByUserIdAsync("1234")).ReturnsAsync(worker);

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync((MaintenanceTask?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.StartTaskAsync(1, "1234"));

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceWorkerByUserIdAsync("1234"), Times.Once);

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task StartTaskAsync_ShouldThrowArgumentException_WhenTaskBelong_ToADifferentWorker()
    {

        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = "1234",
            FirstName = "John",
            LastName = "Doe",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available
        };

        var task = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorkerId = 2
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceWorkerByUserIdAsync("1234")).ReturnsAsync(worker);

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync(task);

        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.StartTaskAsync(1, "1234"));

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceWorkerByUserIdAsync("1234"), Times.Once);

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task StartTaskAsync_ShouldThrowArgumentException_WhenTaskIsNotPending()
    {

        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = "1234",
            FirstName = "John",
            LastName = "Doe",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available
        };

        var task = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorkerId = 1,
            MaintenanceTaskStatus = MaintenanceTaskStatus.Completed
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceWorkerByUserIdAsync("1234")).ReturnsAsync(worker);

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync(task);

        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.StartTaskAsync(1, "1234"));

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceWorkerByUserIdAsync("1234"), Times.Once);

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task StartTaskAsync_ShouldReturnStartDto_WhenWorkerAndTaskAreValid()
    {

        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = "1234",
            FirstName = "John",
            LastName = "Doe",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Available
        };

        var car = new Car
        {
            Make = "Honda",
            Model = "Civic"
        };

        var report = new MaintenanceReport
        {
            MaintenanceReportStatus = MaintenanceReportStatus.Pending
        };

        var task = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorkerId = 1,
            MaintenanceTaskStatus = MaintenanceTaskStatus.Pending,
            Car = car,
            MaintenanceReport = report,
            MaintenanceWorker = worker
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceWorkerByUserIdAsync("1234")).ReturnsAsync(worker);

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync(task);

        var result = await _maintenanceService.StartTaskAsync(1, "1234");

        result.MaintenanceTaskId.ShouldBe(1);
        result.Make.ShouldBe("Honda");
        result.Model.ShouldBe("Civic");
        result.FirstName.ShouldBe("John");
        result.LastName.ShouldBe("Doe");

        task.MaintenanceTaskStatus.ShouldBe(MaintenanceTaskStatus.InProgress);
        report.MaintenanceReportStatus.ShouldBe(MaintenanceReportStatus.InProgress);
        worker.MaintenanceWorkerStatus.ShouldBe(MaintenanceWorkerStatus.Unavailable);

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceWorkerByUserIdAsync("1234"), Times.Once);

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);

        _maintenanceRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetReportTaskByIdAsync_ShouldThrowArgumentException_WhenTaskDoesNotExist()
    {
        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync((MaintenanceTask?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.GetReportTaskByIdAsync(1, "1234"));

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetReportTaskByIdAsync_ShouldReturnNull_WhenTaskBelongsToDifferentWorker()
    {
        var worker = new MaintenanceWorker
        {
            UserId = "4321"
        };

        var task = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorker = worker,
            MaintenanceWorkerId = 1
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync(task);

        var result = await _maintenanceService.GetReportTaskByIdAsync(1, "1234");

        result.ShouldBe(null);

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetReportTaskByIdAsync_ShouldReturnReportDto_WhenDataIsValid()
    {
        var worker = new MaintenanceWorker
        {
            UserId = "1234",
            FirstName = "John",
            LastName = "Doe"
        };

        var car = new Car
        {
            CarId = 1,
            Make = "Honda",
            Model = "Civic"
        };

        var report = new MaintenanceReport
        {
            MaintenanceTaskId = 1,
            MaintenanceReportId = 1,
            InteriorCondition = InspectionResult.Pending,
            Brakes = InspectionResult.Pending,
            Lights = InspectionResult.Pending,
            Glass  = InspectionResult.Pending,
            Tyres = InspectionResult.Pending,
            OverallResult = InspectionResult.Pending,
            Engine = InspectionResult.Pending,
            Battery = InspectionResult.Pending,
            AirConditioning = InspectionResult.Pending,
            WarningLights = InspectionResult.Pending,
            BodyWork = InspectionResult.Pending,
            FuelLevel = InspectionResult.Pending,
        };

        var task = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorker = worker,
            MaintenanceWorkerId = 1,
            Car = car,
            MaintenanceReport = report,
            CarId = 1
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync(task);

        var result = await _maintenanceService.GetReportTaskByIdAsync(1, "1234");

         result.Make.ShouldBe("Honda");
         result.Model.ShouldBe("Civic");

         result.AirConditioning.ShouldBe(InspectionResult.Pending);
         result.Battery.ShouldBe(InspectionResult.Pending);
         result.Lights.ShouldBe(InspectionResult.Pending);
         result.Tyres.ShouldBe(InspectionResult.Pending);
         result.Brakes.ShouldBe(InspectionResult.Pending);
         result.Glass.ShouldBe(InspectionResult.Pending);
         result.FuelLevel.ShouldBe(InspectionResult.Pending);
         result.BodyWork.ShouldBe(InspectionResult.Pending);
         result.OverallResult.ShouldBe(InspectionResult.Pending);
         result.InteriorCondition.ShouldBe(InspectionResult.Pending);

         result.FirstName.ShouldBe("John");
         result.LastName.ShouldBe("Doe");

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task SubmitReportAsync_ShouldThrowArgumentException_WhenDtoIsNull()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.SubmitReportAsync(2, "1224", null));
    }

    [Fact]
    public async Task SubmitReportAsync_ShouldThrowArgumentException_WhenTaskDoesNotExist()
    {
        var dto = new InspectionResultDto
        {
            FuelLevel = InspectionResult.Pending,
            Battery = InspectionResult.Pending,
            WarningLights = InspectionResult.Pending,
            BodyWork = InspectionResult.Pending,
            Glass = InspectionResult.Pending,
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync((MaintenanceTask?) null);

        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.SubmitReportAsync(1, "1234", dto));

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task SubmitReportAsync_ShouldThrowArgumentException_WhenTaskBelongsToADifferentWorker()
    {
        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = "1234"
        };

        var task = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorker = worker,
            MaintenanceWorkerId = 1
        };

        var dto = new InspectionResultDto
        {
            FuelLevel = InspectionResult.Pending,
            Battery = InspectionResult.Pending,
            WarningLights = InspectionResult.Pending,
            BodyWork = InspectionResult.Pending,
            Glass = InspectionResult.Pending,
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1)).ReturnsAsync(task);

        await Assert.ThrowsAsync<ArgumentException>(() => _maintenanceService.SubmitReportAsync(1, "4321", dto));

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task SubmitReportAsync_ShouldReturnReportDto_WhenDataIsValid()
    {
        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = "1234",
            FirstName = "John",
            LastName = "Doe",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Unavailable
        };

        var car = new Car
        {
            CarId = 1,
            Make = "Honda",
            Model = "Civic",
            CarStatus = CarStatus.UnderMaintenance
        };

        var report = new MaintenanceReport
        {
            MaintenanceTaskId = 1,
            MaintenanceReportId = 1,
            MaintenanceReportStatus = MaintenanceReportStatus.InProgress
        };

        var task = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorker = worker,
            MaintenanceWorkerId = 1,
            Car = car,
            CarId = 1,
            MaintenanceReport = report,
            MaintenanceTaskStatus = MaintenanceTaskStatus.InProgress
        };

        var dto = new InspectionResultDto
        {
            FuelLevel = InspectionResult.Pass,
            Battery = InspectionResult.Pass,
            WarningLights = InspectionResult.Pass,
            BodyWork = InspectionResult.Pass,
            Glass = InspectionResult.Pass,
            Tyres = InspectionResult.Pass,
            Lights = InspectionResult.Pass,
            Brakes = InspectionResult.Pass,
            Engine = InspectionResult.Pass,
            AirConditioning = InspectionResult.Pass,
            InteriorCondition = InspectionResult.Pass,
            OverallResult = InspectionResult.Pass
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1))
        .ReturnsAsync(task);

        var result = await _maintenanceService.SubmitReportAsync(1, "1234", dto);

        result.FirstName.ShouldBe("John");
        result.LastName.ShouldBe("Doe");
        result.Make.ShouldBe("Honda");
        result.Model.ShouldBe("Civic");
        result.OverallResult.ShouldBe(InspectionResult.Pass);
        result.FuelLevel.ShouldBe(InspectionResult.Pass);


        task.MaintenanceTaskStatus.ShouldBe(MaintenanceTaskStatus.Completed);

        report.MaintenanceReportStatus.ShouldBe(MaintenanceReportStatus.Completed);
        car.CarStatus.ShouldBe(CarStatus.Available);

        worker.MaintenanceWorkerStatus.ShouldBe(MaintenanceWorkerStatus.Available);

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1),
        Times.Once);

        _maintenanceRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitReportAsync_ShouldReturnCarUnderMaintenance_WhenOverallResultIsFail()
    {
        var worker = new MaintenanceWorker
        {
            MaintenanceWorkerId = 1,
            UserId = "1234",
            FirstName = "John",
            LastName = "Doe",
            MaintenanceWorkerStatus = MaintenanceWorkerStatus.Unavailable
        };

        var car = new Car
        {
            CarId = 1,
            Make = "Honda",
            Model = "Civic",
            CarStatus = CarStatus.UnderMaintenance
        };

        var report = new MaintenanceReport
        {
            MaintenanceTaskId = 1,
            MaintenanceReportId = 1,
            MaintenanceReportStatus = MaintenanceReportStatus.InProgress
        };

        var task = new MaintenanceTask
        {
            MaintenanceTaskId = 1,
            MaintenanceWorker = worker,
            MaintenanceWorkerId = 1,
            Car = car,
            CarId = 1,
            MaintenanceReport = report,
            MaintenanceTaskStatus = MaintenanceTaskStatus.InProgress
        };

        var dto = new InspectionResultDto
        {
            FuelLevel = InspectionResult.Pass,
            Battery = InspectionResult.Pass,
            WarningLights = InspectionResult.Pass,
            BodyWork = InspectionResult.Pass,
            Glass = InspectionResult.Pass,
            Tyres = InspectionResult.Pass,
            Lights = InspectionResult.Pass,
            Brakes = InspectionResult.Pass,
            Engine = InspectionResult.Fail,
            AirConditioning = InspectionResult.Pass,
            InteriorCondition = InspectionResult.Pass,
            OverallResult = InspectionResult.Fail
        };

        _maintenanceRepositoryMock.Setup(x => x.GetMaintenanceTaskByIdAsync(1))
        .ReturnsAsync(task);

        var result = await _maintenanceService.SubmitReportAsync(1, "1234", dto);

        result.FirstName.ShouldBe("John");
        result.LastName.ShouldBe("Doe");
        result.Make.ShouldBe("Honda");
        result.Model.ShouldBe("Civic");
        result.OverallResult.ShouldBe(InspectionResult.Fail);
        result.FuelLevel.ShouldBe(InspectionResult.Pass);


        task.MaintenanceTaskStatus.ShouldBe(MaintenanceTaskStatus.Completed);

        report.MaintenanceReportStatus.ShouldBe(MaintenanceReportStatus.Completed);
        car.CarStatus.ShouldBe(CarStatus.UnderMaintenance);

        worker.MaintenanceWorkerStatus.ShouldBe(MaintenanceWorkerStatus.Available);

        _maintenanceRepositoryMock.Verify(x => x.GetMaintenanceTaskByIdAsync(1),
        Times.Once);

        _maintenanceRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTasksCountForWorkerAsync_ShouldReturnCount_WhenDataIsValid()
    {
        int count = 5;

        _maintenanceRepositoryMock.Setup(x => x.GetTasksCountForWorkerAsync("1234")).ReturnsAsync(count);

        var result = await _maintenanceService.GetTasksCountForWorkerAsync("1234");

        result.ShouldBe(count);

        _maintenanceRepositoryMock.Verify(x => x.GetTasksCountForWorkerAsync("1234"), Times.Once);
    }
}
