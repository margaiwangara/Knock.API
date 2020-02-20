using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Models;
using Knock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knock.API.Controllers
{
  [ApiController]
  [Route("api/users")]
  [Authorize]
  public class UsersController : ControllerBase
  {
    private readonly IKnockRepository _knockRepository;
    private readonly IMapper _mapper;

    public UsersController(IKnockRepository knockRepository, IMapper mapper)
    {
      _knockRepository = knockRepository ??
                throw new ArgumentNullException(nameof(knockRepository));
      _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
      var usersFromRepo = await _knockRepository.GetUsersAsync();

      return Ok(_mapper.Map<IEnumerable<UserDto>>(usersFromRepo));
    }
  }
}