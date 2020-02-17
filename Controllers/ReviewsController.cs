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
  [Route("api/reviews")]
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
    public async Task<ActionResult<IEnumerable<Review>>> GetReviewsForRestaurant()
    {
      var reviews = await _knockRepository.GetReviewsAsync();

      return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
    }
  }
}