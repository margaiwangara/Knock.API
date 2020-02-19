using System;
using System.ComponentModel.DataAnnotations;
using Knock.API.ValidationAttributes;

namespace Knock.API.Models
{
  [PasswordMustNotBeEqualToEmail]
  public class UserForRegistrationDto
  {
    public Guid Id { get; set; }

    [Required(ErrorMessage="Email field is required")]
    [EmailAddress(ErrorMessage="Invalid Email Address")]
    [MaxLength(50, ErrorMessage="Maximum email length is 50 chars")]
    public string Email { get; set; }

    [Required(ErrorMessage="Password field is required")]
    [MinLength(6, ErrorMessage="Minimum password length is 6 chars")]
    public string Password { get; set; }
    public string Token { get; set; }
  }
}