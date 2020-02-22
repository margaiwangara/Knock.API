using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Entities;
using Knock.API.Models;
using Knock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/restaurants/{restaurantId}/reviews")]
  public class ReviewsController : ControllerBase
  {
    private readonly IKnockRepository _knockRepository;
    private readonly IMapper _mapper;

    public ReviewsController(IKnockRepository knockRepository, IMapper mapper)
    {
        _knockRepository = knockRepository ??
                    throw new ArgumentNullException(nameof(knockRepository));
        _mapper = mapper ??
                    throw new ArgumentNullException(nameof(mapper));

    }

    [HttpGet()]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForRestaurant()
    {
      var reviews = await _knockRepository.GetReviewsAsync();

      return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
    }

    [HttpGet("{reviewId}", Name="GetReviewForRestaurant")]
    [AllowAnonymous]
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

      // get user
     Guid sender = Guid.Parse(User.FindFirst(ClaimTypes.Name)?.Value);

      reviewEntity.UserId = sender;
      // if exists add review
      _knockRepository.AddReview(restaurantId, reviewEntity);
      if(await _knockRepository.SaveChangesAsync())
      {
        // update average
        await _knockRepository.GetAverageRatingAsync(restaurantId);
        
      }

      // remap to dto
      var reviewToReturn = _mapper.Map<ReviewDto>(reviewEntity);

      
      return CreatedAtRoute(nameof(GetReviewForRestaurant),
                new { restaurantId = restaurantId, reviewId = reviewToReturn.Id }, 
                reviewToReturn);
    }

    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateReviewForRestaurant(Guid restaurantId, Guid reviewId, ReviewForUpdateDto review)
    {
      // optimize function
      await DeletePutOptimize(restaurantId, reviewId);

      // get reviews
      var reviewFromRepo = await _knockRepository.GetReviewAsync(restaurantId, reviewId);

      if(reviewFromRepo == null)
      {
        return NotFound();
      }

      // map
      _mapper.Map(review, reviewFromRepo);

      // call update function
      _knockRepository.UpdateReview(reviewFromRepo);
      await _knockRepository.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteReviewForRestaurant(Guid restaurantId, Guid reviewId)
    {
      // optimize function
      await DeletePutOptimize(restaurantId, reviewId);
      // check if review exists
      var review = await _knockRepository.GetReviewAsync(restaurantId, reviewId);

      if(review == null)
      {
        return NotFound();
      }

      // delete review
      _knockRepository.DeleteReview(review);
      await _knockRepository.SaveChangesAsync();

      return NoContent();
    }

    private async Task<IActionResult> DeletePutOptimize(Guid restaurantId, Guid reviewId)
    {
      // if ids exist
      if(restaurantId == Guid.Empty)
      {
        return BadRequest();
      }

      if(reviewId == Guid.Empty)
      {
        return BadRequest();
      }

      // check if restaurant exists
      if(!await _knockRepository.RestaurantExists(restaurantId))
      {
        return NotFound();
      }

      return null;
    }
  }
}