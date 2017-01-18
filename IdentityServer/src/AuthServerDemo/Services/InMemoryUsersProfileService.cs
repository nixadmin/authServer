using IdentityServer4.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using AuthServerDemo.Data.Stores;
using IdentityServer4.Extensions;
using AuthServerDemo.Data.Entities;
using System.Security.Claims;
using IdentityModel;
using AuthServerDemo.Configuration;
using IdentityServer4;

namespace AuthServerDemo.Services
{
    public class ApplicationUserProfileService : IProfileService
    {
        private IApplicationUserStore users { get; set; }

        public ApplicationUserProfileService(IApplicationUserStore usersStore)
        {
            this.users = usersStore;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.RequestedClaimTypes.Any())
            {
                var user = GetUserBySubject(context.Subject);

                var claims = new List<Claim>();

                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                claims.Add(new Claim(JwtClaimTypes.Address, user.Address));
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));

                if (user.IsAdmin)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, Roles.Admin));
                }

                claims.Add(new Claim(JwtClaimTypes.Role, Roles.User));
                claims.Add(new Claim(JwtClaimTypes.Scope, InternalScope.Users));

                claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));

                context.IssuedClaims = claims;
            }

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = GetUserBySubject(context.Subject);
            context.IsActive = user != null;

            return Task.FromResult(0);
        }

        private ApplicationUser GetUserBySubject(ClaimsPrincipal sub)
        {
            return users.FindById(int.Parse(sub.GetSubjectId()));
        }
    }
}
