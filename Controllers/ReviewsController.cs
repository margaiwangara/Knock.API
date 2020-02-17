using System;
using AutoMapper;
using Knock.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Route("api/{restaurantId}/reviews")]
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
  }
}