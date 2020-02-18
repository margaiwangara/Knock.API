using System;
using AutoMapper;
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
  }
    
}