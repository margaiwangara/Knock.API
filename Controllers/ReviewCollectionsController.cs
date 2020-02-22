using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Entities;
using Knock.API.Helpers;
using Knock.API.Models;
using Knock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/restaurants/{restaurantId}/reviewcollections")]
  public class ReviewCollectionsController : ControllerBase
  {
    private readonly IKnockRepository _knockRepository;
    private readonly IMapper _mapper;

    public ReviewCollectionsController(IKnockRepository knockRepository, IMapper mapper)
    {
      _knockRepository = knockRepository ??
            throw new ArgumentNullException(nameof(knockRepository));
      _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        
    }

    [HttpGet("({reviewIds})", Name="GetReviewCollectionForRestaurant")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewCollectionForRestaurant(Guid restaurantId, 
                      [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> reviewIds)
    {
      if(reviewIds == null)
      {
        return BadRequest();
      }

      if(restaurantId == null)
      {
        return BadRequest();
      }

      // get data
      var reviews = await _knockRepository.GetReviewsAsync(restaurantId, reviewIds);

      if(reviewIds.Count() != reviews.Count())
      {
        return NotFound();
      }

      // map
      var reviewsToMap = _mapper.Map<IEnumerable<ReviewDto>>(reviews);

      return Ok(reviewsToMap);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReviewCollectionForRestaurant(Guid restaurantId,
        IEnumerable<ReviewForCreationDto> reviewCollection)
    {
      if(restaurantId == null)
      {
        return BadRequest();
      }

      if(reviewCollection == null)
      {
        return BadRequest();
      }

      var reviewEntities = _mapper.Map<IEnumerable<Review>>(reviewCollection);

      foreach(var review in reviewEntities)
      {
        _knockRepository.AddReview(restaurantId, review);
      }

      if(await _knockRepository.SaveChangesAsync())
      {
        // update restaurant data
         await _knockRepository.GetAverageRatingAsync(restaurantId);
      }

      var reviewCollectionToReturn = _mapper.Map<IEnumerable<ReviewDto>>(reviewEntities);
      string idsAsString = string.Join(",", 
                        reviewCollectionToReturn.Select(r => r.Id));

      return CreatedAtRoute(nameof(GetReviewCollectionForRestaurant),
          new { restaurantId = restaurantId, reviewIds = idsAsString }, reviewCollectionToReturn);
    }

    
  }
    
}