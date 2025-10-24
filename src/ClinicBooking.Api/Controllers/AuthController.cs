using ClinicBooking.Application.Interfaces;
using ClinicBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly ITokenService _tokens;

    public AuthController(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn, ITokenService tokens)
        => (_users, _signIn, _tokens) = (users, signIn, tokens);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
        var result = await _users.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        var roles = await _users.GetRolesAsync(user);
        var token = _tokens.Create(user, roles);
        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _users.FindByEmailAsync(dto.Email);
        if (user is null) return Unauthorized();

        var result = await _signIn.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
        if (!result.Succeeded) return Unauthorized();

        var roles = await _users.GetRolesAsync(user);
        var token = _tokens.Create(user, roles);
        return Ok(new { token });
    }
}

public record RegisterDto(string Email, string Password);
public record LoginDto(string Email, string Password);
