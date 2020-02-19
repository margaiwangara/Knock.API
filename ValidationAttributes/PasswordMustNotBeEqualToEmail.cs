using System.ComponentModel.DataAnnotations;
using Knock.API.Models;

namespace Knock.API.ValidationAttributes
{
  public class PasswordMustNotBeEqualToEmail : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, 
                            ValidationContext validationContext)
    {
      var user = (UserForRegistrationDto) validationContext.ObjectInstance;

      if(user.Email == user.Password)
      {
        return new ValidationResult(ErrorMessage, 
                        new [] { nameof(UserForRegistrationDto) });
      }

      return ValidationResult.Success;
    }

  }
}