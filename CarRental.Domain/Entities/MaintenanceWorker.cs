namespace CarRental.Domain.Entities
{
  public class MaintenanceWorker
  {
    public int MaintenanceWorkerId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public MaintenanceWorkerStatus MaintenanceWorkerStatus { get; set; }

    public ICollection<MaintenanceTask> MaintenanceTasks { get; set; } = new List<MaintenanceTask>();

    public ICollection<MaintenanceReport> MaintenanceReports { get; set; } = new List<MaintenanceReport>();
  }
}