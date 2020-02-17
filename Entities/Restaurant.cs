using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Knock.API.Entities
{
  public class Restaurant
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Address { get; set; }

    [MaxLength(100)]
    #nullable enable
    public string? Website { get; set; }
    public bool OffersTakeout { get; set; }
    public DateTimeOffset DateCreated { get; set; }
        = DateTimeOffset.Now;
    public ICollection<Review> Reviews { get; set; }
        = new List<Review>();
  }
}