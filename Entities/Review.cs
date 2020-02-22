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
    [Column(TypeName = "decimal(2,2)")]

    public double Rating { get; set; } = 0.0;

    [ForeignKey("RestaurantId")]
    public Restaurant Restaurant { get; set; }
    public Guid RestaurantId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
    public Guid UserId { get;set; }
    public DateTimeOffset DateCreated { get; set; }
     = DateTimeOffset.UtcNow;
  }
}