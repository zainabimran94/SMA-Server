using Microsoft.AspNetCore.Authorization;

namespace StudentAdminPortal.Authorization
{
    public class DynamicRoleHanlder : AuthorizationHandler<DynamicRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicRoleRequirement requirement)
        {
            if (context.User.IsInRole(requirement.RoleName))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

    public class DynamicRoleRequirement(string roleName) : IAuthorizationRequirement
    {
        public string RoleName { get; } = roleName;
    }
}