using BlogCMS.Constants;
using BlogCMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogCMS.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IConfiguration _config;

    public LoginController(IConfiguration config)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult Login([FromBody] UserLogin userLogin)
    {
        var user = Authenticate(userLogin);

        if (user != null)
        {
            var token = GenerateToken(user);

            return Ok(token);
        }

        return Unauthorized("Nieprawidłowy login lub hasło.");
    }

    private string GenerateToken(LoginModel user)
    {
        var key = _config["Jwt:Key"];

        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException(
                "Brak Jwt:Key w appsettings.json"
            );
        }

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            );

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Username
            ),

            new Claim(
                ClaimTypes.Role,
                user.Role
            )
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    private LoginModel? Authenticate(UserLogin userLogin)
    {
        return UserConstants.Users.FirstOrDefault(
            x =>
                x.Username.Equals(
                    userLogin.Username,
                    StringComparison.OrdinalIgnoreCase
                )
                &&
                x.Password == userLogin.Password
        );
    }
}