namespace CarRental.Domain.Entities
{
  public class MaintenanceTask
  {
    public int MaintenanceTaskId { get; set; }

    public int CarId { get; set; }

    public Car? Car { get; set; }

    public int? MaintenanceWorkerId { get; set; }

    public MaintenanceWorker? MaintenanceWorker { get; set; }

    public MaintenanceReport? MaintenanceReport { get; set; }

    public MaintenanceTaskStatus MaintenanceTaskStatus { get; set; }
  }
}