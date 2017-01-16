using System;
using System.Linq.Expressions;
using System.Security.Claims;
using AuthServerDemo.Configuration;
using AuthServerDemo.Data.Entities;
using IdentityServer4;

namespace AuthServerDemo.Data
{
    public static class ApplicationUserQueries
    {
        public static Expression<Func<ApplicationUser, bool>> GetUserWithRoleRestrictionsQuery(ClaimsPrincipal user, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                if (user.IsInRole(Roles.Admin))
                {
                    // admin simply can get all users
                    return q => true;
                }

                return GetUserByEmailQuery(user.FindFirstValue(IdentityServerConstants.StandardScopes.Email));
            }

            return GetUserByEmailWithRoleRestrictionsQuery(user, email);
        }

        public static Expression<Func<ApplicationUser, bool>> GetUserByEmailWithRoleRestrictionsQuery(ClaimsPrincipal user, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email should be specified");
            }

            if (user.IsInRole(Roles.Admin) || user.HasClaim(IdentityServerConstants.StandardScopes.Email, email))
            {
                return GetUserByEmailQuery(email);
            }

            throw new InvalidOperationException("Invalid user permissions");
        }

        public static Expression<Func<ApplicationUser, bool>> GetUserByEmailQuery(string email)
        {
            return q => q.Email == email;
        }
    }
}
