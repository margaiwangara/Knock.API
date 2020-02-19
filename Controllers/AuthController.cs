using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Entities;
using Knock.API.Models;
using Knock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Knock.API.Controllers
{
  [AllowAnonymous]
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForRegistrationDto user)
    {
      var isAuthenticated = await _knockRepository.isAuthenticated(user.Email, user.Password);

      if(isAuthenticated == false)
      {
        return Unauthorized(new { message = "Invalid Email or Password" });
      }

      var mappedUser = _mapper.Map<UserDto>(user);
      string token = GenerateAuthToken(mappedUser);

      return Ok(new { Token = token, Id = mappedUser.Id });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserForRegistrationDto user)
    {
      // check if user is null
      if(user == null)
      {
        return BadRequest();
      }

      var mappedUser = _mapper.Map<User>(user);
      _knockRepository.AddUser(mappedUser);
      await _knockRepository.SaveChangesAsync();
      
      var remappedUser = _mapper.Map<UserDto>(mappedUser);

      // get token
      string token = GenerateAuthToken(remappedUser);

      return Ok(new { Id = remappedUser.Id, Token = token });

    }

    private string GenerateAuthToken(UserDto user)
    {

      var tokenHandler = new JwtSecurityTokenHandler();
        byte [] key = Encoding.ASCII.GetBytes("randomstuff");
        var tokenDescriptor = new SecurityTokenDescriptor 
        { 
          Subject = new ClaimsIdentity(
            new Claim[] 
            {
              new Claim(ClaimTypes.Name, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials  = new SigningCredentials(new SymmetricSecurityKey(key), 
                                      SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
    
  }
  
}