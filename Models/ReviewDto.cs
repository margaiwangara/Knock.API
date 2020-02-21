using System;

namespace Knock.API.Models
{
  public class ReviewDto
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public byte Rating { get; set; }
    public Guid RestaurantId { get; set; }
    public Guid UserId { get;set; }
    public DateTimeOffset DateCreated { get; set; }
  }
}