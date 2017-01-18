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

        //public static void InitializeInMemoryStore(this IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //    {
        //        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //        serviceScope
        //            .ServiceProvider
        //            .GetRequiredService<InMemoryApplicationUserStore>()
        //            .Init(userManager);
        //    }
        //}
    }
}
