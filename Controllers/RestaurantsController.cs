using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Entities;
using Knock.API.Models;
using Knock.API.ResourceParameters;
using Knock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Authorize(Roles = Role.Admin)]
  [Route("api/restaurants")]
  public class RestaurantsController : ControllerBase
  {
    private readonly IKnockRepository _knockRepository;
    private readonly IMapper _mapper;
    public RestaurantsController(IKnockRepository knockRepository, IMapper mapper)
    {
      _knockRepository = knockRepository ??
            throw new ArgumentNullException(nameof(knockRepository));
      _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet()]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants(
                        [FromQuery] RestaurantResourceParameters resourceParameters)
    {
      var restaurants = await _knockRepository.GetRestaurantsAsync(resourceParameters);

      return Ok(_mapper.Map<IEnumerable<RestaurantDto>>(restaurants));
    }

    [HttpGet("{restaurantId}")]
    [AllowAnonymous]
    public async Task<ActionResult<RestaurantDto>> GetRestaurant(Guid restaurantId)
    {
      var restaurant = await _knockRepository.GetRestaurantAsync(restaurantId);

      if(restaurant == null)
      {
        return NotFound();
      }

      return Ok(_mapper.Map<RestaurantDto>(restaurant));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(RestaurantForCreationDto restaurant)
    {
      if(restaurant == null)
      {
        return BadRequest();
      }

      var restaurantEntity = _mapper.Map<Restaurant>(restaurant);
      _knockRepository.AddRestaurant(restaurantEntity);
      await _knockRepository.SaveChangesAsync();

      var mappedRestaurant = _mapper.Map<RestaurantDto>(restaurantEntity);
      
      return CreatedAtAction(nameof(GetRestaurant), 
                  new { restaurantId = mappedRestaurant.Id }, restaurant);
    }

    [HttpOptions]
    public IActionResult GetRestaurantOptions()
    {
      Response.Headers.Add("Allow", "GET,OPTIONS,POST");
      return Ok();
    }

    [HttpPut("{restaurantId}")]
    public async Task<IActionResult> UpdateRestaurant(Guid restaurantId, 
                RestaurantForUpdateDto restaurant)
    {
      var restaurantFromRepo = await _knockRepository.GetRestaurantAsync(restaurantId);
      
      if(restaurantFromRepo == null)
      {
        return NotFound();
      }

      _mapper.Map(restaurant, restaurantFromRepo);

      // save changes
      await _knockRepository.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("{restaurantId}")]
    public async Task<IActionResult> DeleteRestaurant(Guid restaurantId)
    {
      var restaurant = await _knockRepository.GetRestaurantAsync(restaurantId);

      if(restaurant == null)
      {
        return NotFound();
      }

      _knockRepository.DeleteRestaurant(restaurant);
      await _knockRepository.SaveChangesAsync();

      return NoContent();
    }

  
  }
}