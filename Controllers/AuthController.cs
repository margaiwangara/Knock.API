using System;
using AutoMapper;
using Knock.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Route("api/auth")]
  public class AuthController : ControllerBase
  {
    private readonly IKnockRepository _knockRepository;
    private readonly IMapper _mapper;
    
    public AuthController(IKnockRepository knockRepository, IMapper mapper)
    {
      _knockRepository = knockRepository ??
                throw new ArgumentNullException(nameof(knockRepository));
      _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
    }

    // [HttpPost("register")]
    // public async Task<ActionResult<UserForRegistrationDto>> RegisterUser(UserForRegistrationDto user)
    // {

    // }
  }
}