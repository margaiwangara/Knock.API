using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Entities;
using Knock.API.Helpers;
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

    [HttpGet("({ids})", Name="GetRestaurantCollection")]
    public async Task<IActionResult> GetRestaurantCollection(
                  [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
      if(ids == null)
      {
        return BadRequest();
      }

      // get restaurants with the ids
      var restaurants = await _knockRepository.GetRestaurantsAsync(ids);

      if(ids.Count() != restaurants.Count())
      {
        return NotFound();
      }
      
      var restaurantsToReturn = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

      return Ok(restaurantsToReturn);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurantCollection(
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

      var restaurantCollectionToReturn = _mapper.Map<IEnumerable<RestaurantDto>>(restaurantEntities);
      string idsAsString = string.Join(",", 
                        restaurantCollectionToReturn.Select(r => r.Id));

      return CreatedAtRoute("GetRestaurantCollection",
          new { ids = idsAsString }, restaurantCollectionToReturn);
    }


  }
}