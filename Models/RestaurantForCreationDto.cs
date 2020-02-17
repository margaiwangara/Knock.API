using System.ComponentModel.DataAnnotations;

namespace Knock.API.Models
{
  public class RestaurantForCreationDto
  {
    [Required(ErrorMessage="Name field is required")]
    [MaxLength(100, ErrorMessage="Name field maximum length is 100 chars")]
    public string Name { get; set; }
    
    [Required(ErrorMessage="Address field is required")]
    [MaxLength(100, ErrorMessage="Address field maximum length is 100 chars")]
    public string Address { get; set; }
    
    [MaxLength(100, ErrorMessage="Website field maximum length is 100 chars")]
    public string Website { get; set; }
    public bool OffersTakeout { get; set; }
  }
}