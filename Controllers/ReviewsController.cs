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
  [Route("api/restaurants/{restaurantId}/reviews")]
  public class ReviewsController : ControllerBase
  {
    IKnockRepository _knockRepository;
    IMapper _mapper;
    public ReviewsController(IKnockRepository knockRepository, IMapper mapper)
    {
        _knockRepository = knockRepository ??
                    throw new ArgumentNullException(nameof(knockRepository));
        _mapper = mapper ??
                    throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForRestaurant()
    {
      var reviews = await _knockRepository.GetReviewsAsync();

      return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
    }

    [HttpGet("{reviewId}", Name="GetReviewForRestaurant")]
    public async Task<ActionResult<ReviewDto>> GetReviewForRestaurant(Guid restaurantId, Guid reviewId)
    {
      // check if restaurant exists
      if(!await _knockRepository.RestaurantExists(restaurantId))
      {
        return NotFound();
      }

      var review = await _knockRepository.GetReviewAsync(restaurantId, reviewId);

      if(review == null)
      {
        return NotFound();
      }

      return Ok(_mapper.Map<ReviewDto>(review));

    }

    [HttpPost]
    public async Task<IActionResult> CreateReviewForRestaurant(Guid restaurantId, ReviewForCreationDto review)
    {
      // check if restaurant exists
      if(!await _knockRepository.RestaurantExists(restaurantId))
      {
        return NotFound();
      }

      // map review to review
      var reviewEntity = _mapper.Map<Review>(review);

      // if exists add review
      _knockRepository.AddReview(restaurantId, reviewEntity);
      await _knockRepository.SaveChangesAsync();

      // remap to dto
      var reviewToReturn = _mapper.Map<ReviewDto>(reviewEntity);

      return CreatedAtRoute(nameof(GetReviewForRestaurant),
                new { restaurantId = restaurantId, reviewId = reviewToReturn.Id }, 
                reviewToReturn);
    }
  }
}