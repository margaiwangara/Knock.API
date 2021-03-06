using System.ComponentModel.DataAnnotations;

namespace Knock.API.Models
{
  public class ReviewForUpdateDto : ReviewForManipulationDto 
  {

    [Required(ErrorMessage="Rating field is required")]
    public override double Rating { get => base.Rating; set => base.Rating = value; }
  }
}