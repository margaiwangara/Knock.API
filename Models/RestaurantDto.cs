using System;
using System.Collections.Generic;
using Knock.API.Entities;

namespace Knock.API.Models
{
  public class RestaurantDto
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public bool OffersTakeout { get; set; }
    public double AverageRating { get; set; }
    public IEnumerable<Review> Reviews { get; set; }
  }
}