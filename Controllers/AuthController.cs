using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Knock.API.Entities;
using Knock.API.Helpers;
using Knock.API.Models;
using Knock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

    private readonly AppSettings _settings;
    
    public AuthController(IKnockRepository knockRepository, IMapper mapper,
                              IOptions<AppSettings> settings)
    {
      _knockRepository = knockRepository ??
                throw new ArgumentNullException(nameof(knockRepository));
      _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
      _settings = settings.Value ??
                throw new ArgumentNullException(nameof(settings));
      
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForRegistrationDto user)
    {
      
      var userFromRepo = await _knockRepository.Authenticated(user.Email, user.Password);

      if(userFromRepo == null)
      {
        return Unauthorized(new { message = "Invalid Email or Password" });
      }

      // map to dto
      var mappedUser = _mapper.Map<UserDto>(userFromRepo);
      string token = GenerateAuthToken(mappedUser);

      return Ok(new { Id = mappedUser.Id, Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserForRegistrationDto user)
    {
      // check if user is null
      if(user == null)
      {
        return BadRequest();
      }

      // check is email exists
      if(await _knockRepository.EmailExists(user.Email))
      {
        return BadRequest(new { message = "Email already exists" });
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
        var key = Encoding.ASCII.GetBytes("randomstuffthatiamsupposedtowritehere");
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