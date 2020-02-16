using AutoMapper;
using Knock.API.Entities;
using Knock.API.Models;

namespace Knock.API.Profiles
{
  public class RestaurantProfile : Profile
  {
    public RestaurantProfile()
    {
      CreateMap<Restaurant, RestaurantDto>();
      CreateMap<RestaurantForCreationDto, Restaurant>();
    }
  }
}