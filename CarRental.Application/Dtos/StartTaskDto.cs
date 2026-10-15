namespace CarRental.Application.Dtos
{
  public class StartTaskDto
  {
    public int MaintenanceTaskId { get; set; }

    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
  }
}