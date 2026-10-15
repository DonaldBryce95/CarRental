using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Infrastructure.Identity
{
  public class ApplicationUser : IdentityUser
  {
    public int? CustomerId { get; set; }

    public Customer? Customer { get; set; }
  }
}