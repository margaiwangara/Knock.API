using System;

namespace Knock.API.Entities
{
  public class Review
  {
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Content { get; set; }
    public byte Rating { get; set; }
  }
}