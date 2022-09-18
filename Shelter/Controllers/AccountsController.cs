using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shelter.Models.DTOs.Responses;
using Shelter.Models.DTOs.Requests;
using Shelter.Configuration;
using Shelter.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using System;
using System.Linq;

namespace Shelter.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountsController : ControllerBase
  {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;

    public AccountsController(UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor)
    {
      _userManager = userManager;
      _jwtConfig = optionsMonitor.CurrentValue;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromBody] UserRegistrationDTO user)
    {
      if(ModelState.IsValid)
      {
        // check if user exists
        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        if(existingUser != null)
        {
          return BadRequest(new RegistrationResponse(){ Errors = new List<string>() {"Email in use"}, Success = false});
        }

        if(user.Password != user.ConfirmPassword)
        {
          return BadRequest(new RegistrationResponse(){ Errors = new List<string>() {"Passwords do no match"}, Success = false});
        }

        var newUser = new IdentityUser() { Email = user.Email, UserName = user.Email };
        var isCreated = await _userManager.CreateAsync(newUser, user.Password);
        if(isCreated.Succeeded)
        {
          var jwtToken = GenerateJwtToken( newUser);

          return Ok(new RegistrationResponse() {
            Success = true,
            Token = jwtToken
          });
        }
        else
        {
          return BadRequest(new RegistrationResponse(){ Errors = isCreated.Errors.Select(x=> x.Description).ToList(), Success = false});
        }

      }

      return BadRequest(new RegistrationResponse(){ Errors = new List<string>() {"Invalid payload"}, Success = false});
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> Login([FromBody] UserLoginDTO user)
    {
      if(ModelState.IsValid)
      {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        if(existingUser == null)
        {
          return BadRequest(new RegistrationResponse(){ Errors = new List<string>() {"Invalid Login attempt"}, Success = false});
        }
        var passCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
        if(!passCorrect)
        {
          return BadRequest(new RegistrationResponse(){ Errors = new List<string>() {"Invalid Login attempt"}, Success = false});
        }
        var jwtToken = GenerateJwtToken(existingUser);
        return Ok(new RegistrationResponse(){
          Success = true,
          Token = jwtToken
        });
      }

      return BadRequest(new RegistrationResponse(){ Errors = new List<string>() {"Invalid payload"}, Success = false});
      
    }

    private string GenerateJwtToken(IdentityUser user)
    {
      var jwtTokenHandler = new JwtSecurityTokenHandler();

      var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new []
        {
          new Claim("id", user.Id),
          new Claim(JwtRegisteredClaimNames.Email, user.Email),
          new Claim(JwtRegisteredClaimNames.Sub, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
        Expires = DateTime.UtcNow.AddHours(24),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) 
      };

      var token = jwtTokenHandler.CreateToken(tokenDescriptor);
      var jwtToken = jwtTokenHandler.WriteToken(token);

      return jwtToken;
    }


  }
}