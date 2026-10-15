using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRental.Application.Dtos;
using CarRental.Application.Interfaces;
using CarRental.Domain.Entities;
using CarRental.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarRental.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ICustomerRepository _customerRepository;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ICustomerRepository customerRepository)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _configuration = configuration;
      _customerRepository = customerRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
      var user = new ApplicationUser
      {
        UserName = registerDto.Email,
        Email = registerDto.Email
      };

      var result = await _userManager.CreateAsync(user, registerDto.Password);

      if (!result.Succeeded)
      {
        return BadRequest(result.Errors);
      }

      var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

      if (!roleResult.Succeeded)
      {
        return BadRequest(roleResult.Errors);
      }

      var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(registerDto.Email);

      if (existingCustomer != null)
      {
        user.CustomerId = existingCustomer.CustomerId;
      }
      else
      {
        var newCustomer = new Customer
        {
          FirstName = registerDto.FirstName,
          LastName = registerDto.LastName,
          Email = registerDto.Email,
          PhoneNumber = registerDto.PhoneNumber
        };

        var customer = await _customerRepository.CreateCustomerAsync(newCustomer);

        user.CustomerId = customer.CustomerId;
      }

      var updateResult = await _userManager.UpdateAsync(user);

      if (!updateResult.Succeeded)
      {
        return BadRequest(updateResult.Errors);
      }

      return Ok(new
      {
        message = "User registered successfully"
      });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
      var user = await _userManager.FindByEmailAsync(loginDto.Email);

      if(user == null)
      {
        return Unauthorized();
      }

      var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

      if(!result.Succeeded)
      {
        return Unauthorized();
      }

      var roles = await _userManager.GetRolesAsync(user);

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email!)
      };

      if (user.CustomerId.HasValue)
      {
        claims.Add(new Claim("CustomerId", user.CustomerId.Value.ToString()));
      }

      foreach (var role in roles)
      {
        claims.Add(new Claim(ClaimTypes.Role, role));
      }

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

      var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
      issuer: _configuration["Jwt:Issuer"],
      audience: _configuration["Jwt:Audience"],
      claims: claims,
      expires: DateTime.UtcNow.AddHours(1),
      signingCredentials: credentials);

      var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

      return Ok(new
      {
        token = tokenString
      });
    }
  }
}