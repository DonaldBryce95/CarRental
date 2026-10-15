using CarRental.Domain.Entities;

namespace CarRental.Application.Dtos
{
  public class MaintenanceWorkerDto
  {
    public int MaintenanceWorkerId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public MaintenanceWorkerStatus MaintenanceWorkerStatus { get; set; }
  }
}