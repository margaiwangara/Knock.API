namespace Knock.API.Models
{
  public class RestaurantForUpdateDto
  {
    public string Name { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public bool OffersTakeout { get; set; }
  }
}