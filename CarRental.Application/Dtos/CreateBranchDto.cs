using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos
{
  public class CreateBranchDto
  {
    [Required]
    [StringLength(75, MinimumLength = 2)]
    public string BranchName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(25, MinimumLength = 5)]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(75, MinimumLength = 2)]
    public string OpeningHours { get; set; } = string.Empty;
  }
}