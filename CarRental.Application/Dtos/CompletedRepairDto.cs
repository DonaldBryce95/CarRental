namespace CarRental.Application.Dtos
{
  public class CompletedRepairDto
  {
    public string Make { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public DateTime CompletedAt { get; set; }
  }
}