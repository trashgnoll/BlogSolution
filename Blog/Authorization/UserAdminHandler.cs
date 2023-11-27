using Microsoft.AspNetCore.Authorization;
using Blog.Areas.Identity.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Blog.Authorization
{
    public class UserAdminHandler : AuthorizationHandler<AllowedManagementRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowedManagementRequirement requirement)
        {
            if (context.User.Claims.Where(c => c.Type == "IsAdmin").FirstOrDefault() != null )
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
