using AuthServerDemo.Data.Stores;
using IdentityModel;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace AuthServerDemo.Services
{
    public class ApplicationUserPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IApplicationUserStore users { get; set; }

        public ApplicationUserPasswordValidator(IApplicationUserStore usersStore)
        {
            this.users = usersStore;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (await users.ValidateCredentialsAsync(context.UserName, context.Password))
            {
                var user = await users.FindByUsernameAsync(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}