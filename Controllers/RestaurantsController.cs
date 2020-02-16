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
  [Route("api/restaurants")]
  public class RestaurantsController : ControllerBase
  {
    IKnockRepository _knockRepository;
    IMapper _mapper;
    public RestaurantsController(IKnockRepository knockRepository, IMapper mapper)
    {
      _knockRepository = knockRepository ??
            throw new ArgumentNullException(nameof(knockRepository));
      _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet()]
    public async Task<ActionResult> GetRestaurants()
    {
      var restaurants = await _knockRepository.GetRestaurantsAsync();

      return Ok(_mapper.Map<IEnumerable<RestaurantDto>>(restaurants));
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

    [HttpPost]
    public async Task<ActionResult> CreateRestaurant(Restaurant restaurant)
    {
      _knockRepository.AddRestaurant(restaurant);
      await _knockRepository.SaveChangesAsync();

      return CreatedAtAction(nameof(GetRestaurant), 
                  new { restaurantId = restaurant.Id }, restaurant);
    }

    [HttpPut("{restaurantId}")]
    public async Task<ActionResult> UpdateRestaurant(Guid restaurantId, Restaurant restaurant)
    {
      if(restaurant.Id != restaurantId)
      {
        return BadRequest();
      }

      await _knockRepository.SaveChangesAsync();

      return NoContent();
    }

    

  }
}