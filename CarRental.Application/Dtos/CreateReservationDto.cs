using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos
{
  public class CreateReservationDto
  {
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CarId { get; set; }

    public DateOnly PickUpDate { get; set; }

    public DateOnly ReturnDate { get; set; }

    public TimeSpan PickUpTime { get; set; }

    public TimeSpan ReturnTime { get; set; }

    [Range(1, int.MaxValue)]
    public int PickUpBranchId { get; set; }

    [Range(1, int.MaxValue)]
    public int ReturnBranchId { get; set; }

  }
}