using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Knock.API.Entities
{
  public class User
  {
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    #nullable enable
    public string? Name { get; set; }

    [MaxLength(100)]
    #nullable enable
    public string? Surname { get; set; }

    [Required]
    [MaxLength(50)]
    #nullable disable
    public string Email { get; set; }

    [Required]
    #nullable disable
    public string Password { get; set; }

    [Required]
    [DefaultValue("user")]
    public string Role { get; set; }
    public DateTimeOffset DateCreated { get; set; }
      = DateTimeOffset.UtcNow;

    public ICollection<Review> Reviews { get; set; }
        = new List<Review>();
  }
}