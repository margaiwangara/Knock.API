using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knock.API.Entities
{
  public class Review
  {
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Content { get; set; }

    [Range(0, 5)]
    public byte Rating { get; set; } = 0;

    [ForeignKey("RestaurantId")]
    public Restaurant Restaurant { get; set; }
    public Guid RestaurantId { get; set; }
    public DateTimeOffset DateCreated { get; set; }
     = DateTimeOffset.UtcNow;
  }
}