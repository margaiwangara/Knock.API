using System;

namespace Knock.API.Models
{
  public class ReviewDto
  {
    public Guid Id { get; set; }
    public string Content { get; set; }
    public byte Rating { get; set; } = 0;
    public DateTimeOffset DateCreated { get; set; }
  }
}