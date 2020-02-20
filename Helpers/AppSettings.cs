namespace Knock.API.Helpers
{
  public sealed class AppSettings 
  {
    public string Secret { get; set; }
    public int Iterations { get; set; } = 10000;
  }
}