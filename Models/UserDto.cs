using System;

namespace Knock.API.Models
{
  public class UserDto
  {
    public Guid Id { get; set; }
    public string Name { get;set; }
    public string Email { get; set; }
    public string Role { get; set; }
    
  }
    
}