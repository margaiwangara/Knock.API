using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Helpers;
using Knock.API.Models;
using Knock.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Route("api/{restaurantId}/reviewcollections")]
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

    [HttpGet("({reviewIds})")]
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
      var reviewsToMap = _mapper.Map<ReviewDto>(reviews);

      return Ok(reviewsToMap);
    }

    
  }
    
}