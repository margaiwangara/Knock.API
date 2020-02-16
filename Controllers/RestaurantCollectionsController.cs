using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Entities;
using Knock.API.Models;
using Knock.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Route("api/restaurantcollections")]
  public class RestaurantCollectionsController : ControllerBase
  {
    IKnockRepository _knockRepository;
    IMapper _mapper;

    public RestaurantCollectionsController(IKnockRepository knockRepository,
                        IMapper mapper)
    { 
      _knockRepository = knockRepository ??
            throw new ArgumentNullException(nameof(knockRepository));
      _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
    }

    [HttpPost]
    public async Task<ActionResult> CreateRestaurantCollection(
        IEnumerable<RestaurantForCreationDto> restaurantCollection)
    {
      if(restaurantCollection == null)
      {
        return BadRequest();
      }

      var restaurantEntities = _mapper.Map<IEnumerable<Restaurant>>(restaurantCollection);

      foreach(var restaurant in restaurantEntities)
      {
        _knockRepository.AddRestaurant(restaurant);
      }

      await _knockRepository.SaveChangesAsync();

      return Ok();
    }
  }
}