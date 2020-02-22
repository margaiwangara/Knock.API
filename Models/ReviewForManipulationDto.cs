using System;
using System.ComponentModel.DataAnnotations;

namespace Knock.API.Models
{
  public abstract class ReviewForManipulationDto
  {
    [Required(ErrorMessage="Content field is required")]
    [MaxLength(200, ErrorMessage="Content field maximum length is 200 chars")]
    public string Content { get; set; }

    [Range(0, 5, ErrorMessage="Rating must be between 0 and 5")]
    public virtual double Rating { get; set; }
  }
}