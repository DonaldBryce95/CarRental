namespace CarRental.Application.Dtos
{
  public class BranchDto
  {
    public int BranchId { get; set; }

    public string BranchName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string OpeningHours { get; set; } = string.Empty;

  }
}