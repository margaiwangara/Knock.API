using AutoMapper;
using Knock.API.Entities;
using Knock.API.Models;

namespace Knock.API.Profiles
{
  public class ReviewProfile : Profile
  {

    public ReviewProfile()
    {
      
      CreateMap<Review, ReviewDto>();
      CreateMap<ReviewForCreationDto, Review>();
        
    }
  }
    
}