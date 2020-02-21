using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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
    private readonly AppSettings _options;

    private const int KeySize = 32;
    private const int SaltSize = 16;
    
    public AuthController(IKnockRepository knockRepository, IMapper mapper,
                              IOptions<AppSettings> options)
    {
      _knockRepository = knockRepository ??
                throw new ArgumentNullException(nameof(knockRepository));
      _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
      _options = options.Value ??
                throw new ArgumentNullException(nameof(options));
      
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForRegistrationDto user)
    {
      
      var userFromRepo = await _knockRepository.Authenticated(user.Email, user.Password);

      if(userFromRepo == null)
      {
        return Unauthorized(new { message = "Invalid Email or Password" });
      }

      // check if password matches
      var verified = VerifyPassword(userFromRepo.Password, user.Password);

      if(verified.Verified == false)
      {
        return Unauthorized(new { message = "Invalid Username or Password" });
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
      // get hashed password
      var HashedPassword = GeneratePasswordHash(user.Password);

      if(string.IsNullOrWhiteSpace(HashedPassword))
      {
        throw new ArgumentException(nameof(HashedPassword));
      }

      // equate password to password hash
      user.Password = HashedPassword;

      // check role
      user.Role = Role.User;
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
        var key = Encoding.ASCII.GetBytes(_options.Secret);
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

    private string GeneratePasswordHash(string password)
    {
      using(var algorithm = new Rfc2898DeriveBytes(
        password,
        SaltSize,
        _options.Iterations,
        HashAlgorithmName.SHA256
      ))
      {
        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{_options.Iterations}.{salt}.{key}";
      }

    }

    private (bool Verified, bool NeedsUpgrade) VerifyPassword(string hash, string password)
    {
      var parts = hash.Split(".", 3);

      if(parts.Length != 3)
      {
        throw new FormatException("Unexpected hash format");
      }

      var iterations = Convert.ToInt32(parts[0]);
      var salt = Convert.FromBase64String(parts[1]);
      var key = Convert.FromBase64String(parts[2]);

      var needsUpgrade = iterations != _options.Iterations;

      using(var algorithm = new Rfc2898DeriveBytes(
        password,
        salt,
        iterations,
        HashAlgorithmName.SHA256
      ))
      {
        var keyToCheck = algorithm.GetBytes(KeySize);
        var verified = keyToCheck.SequenceEqual(key);

        return (verified, needsUpgrade);
      }
    }

    
  }
  
}