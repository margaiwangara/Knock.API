using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knock.API.Entities;
using Knock.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Route("api/restaurants")]
  public class RestaurantsController : ControllerBase
  {
    IKnockRepository _knockRepository;

    public RestaurantsController(IKnockRepository knockRepository)
    {
      _knockRepository = knockRepository ??
            throw new ArgumentNullException(nameof(knockRepository));
    }

    [HttpGet()]
    public async Task<ActionResult> GetRestaurants()
    {
      var restaurants = await _knockRepository.GetRestaurantsAsync();

      return Ok(restaurants);
    }

    [HttpGet("{restaurantId}")]
    public async Task<ActionResult> GetRestaurant(Guid restaurantId)
    {
      var restaurant = await _knockRepository.GetRestaurantAsync(restaurantId);

      if(restaurant == null)
      {
        return NotFound();
      }

      return Ok(restaurant);
    }

    public async Task<ActionResult> CreateRestaurant(Restaurant restaurant)
    {
      _knockRepository.AddRestaurant(restaurant);
      await _knockRepository.SaveChangesAsync();

      return CreatedAtAction(nameof(GetRestaurant), 
                  new { restaurantId = restaurant.Id }, restaurant);
    }


  }
}