using AutoMapper;
using Knock.API.Entities;
using Knock.API.Models;

namespace Knock.API.Profiles
{
  public class UserProfile : Profile
  {
    public UserProfile()
    {
      CreateMap<User, UserDto>()
          .ForMember(dest => dest.Name, 
              options => options.MapFrom(u => $"{u.Name} {u.Surname}")); // map name and surname to name
      CreateMap<UserForRegistrationDto, User>();
    }
  }
    
}