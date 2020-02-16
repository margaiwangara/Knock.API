namespace Knock.API.Models
{
  public class RestaurantForCreationDto
  {
    public string Name { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public bool OffersTakeout { get; set; }
  }
}