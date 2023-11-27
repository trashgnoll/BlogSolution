using BlogApi.Dto;
using Blog.Areas.Identity.Data;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogApi.Mapped;

public class Users
{
    public static async Task<IResult> CreateUser(UserManager<BlogUser> userMgr, UserDto user)
    {
        var identityUser = new BlogUser()
        {
            UserName = user.UserName,
            Access = user.Access,
            Email = user.UserName
        };

        if (!IsValidEmail(user.UserName)) { return Results.BadRequest(); }
        bool userExists = userMgr.FindByEmailAsync(user.UserName).Result != null;
        bool accessExists = userMgr.Users.Where(u => u.Access == user.Access).Any();
        if (userExists || accessExists) 
        { 
            return Results.Text("User or role already exist");
        }

        var result = await userMgr.CreateAsync(identityUser, user.Password);

        if (result.Succeeded)
        {
            return Results.Ok();
        }
        else
        {
            return Results.BadRequest();
        }
    }

    public static async Task<IResult> GetToken(WebApplicationBuilder builder, UserManager<BlogUser> userMgr, UserDto user)
    {
        var identityUsr = await userMgr.FindByNameAsync(user.UserName);

        if (await userMgr.CheckPasswordAsync(identityUsr, user.Password))
        {
            var issuer = builder.Configuration["Jwt:Issuer"];
            var audience = builder.Configuration["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = credentials
            };

            var adminClaim = userMgr.GetClaimsAsync(identityUsr).Result.Where(c => c.Type == "IsAdmin").FirstOrDefault();
            if (adminClaim != null)
            {
                tokenDescriptor.Subject.Claims.Append(new Claim("IsAdmin", ""));
            }

            var editorClaim = userMgr.GetClaimsAsync(identityUsr).Result.Where(c => c.Type == "IsEditor").FirstOrDefault();
            if (editorClaim != null)
            {
                tokenDescriptor.Subject.AddClaim(new Claim("IsEditor", ""));
            }

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var stringToken = jwtTokenHandler.WriteToken(token);

            return Results.Ok(stringToken);
        }
        else
        {
            return Results.Unauthorized();
        }

    }

    static bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
            return false;
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }


}
