using AuthServerDemo.Data.Entities;
using AuthServerDemo.Data.Stores;
using AuthServerDemo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServerDemo.Initialization
{
    public static class UserManagementExtensions
    {
        public static IIdentityServerBuilder ManageApplicationUsers(this IIdentityServerBuilder builder)
        {
            builder.Services.AddSingleton<IApplicationUserStore, ApplicationUserStore>();// .AddSingleton(new InMemoryApplicationUserStore());
            builder.AddProfileService<ApplicationUserProfileService>();
            builder.AddResourceOwnerValidator<ApplicationUserPasswordValidator>();

            return builder;
        }
    }
}
