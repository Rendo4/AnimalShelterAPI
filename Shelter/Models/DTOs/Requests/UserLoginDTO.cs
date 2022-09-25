using System.ComponentModel.DataAnnotations;

namespace Shelter.Models.DTOs.Requests
{
  public class UserLoginDTO
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
  
  }
}