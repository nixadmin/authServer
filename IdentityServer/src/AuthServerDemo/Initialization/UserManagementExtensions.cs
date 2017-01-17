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
            builder.Services.AddSingleton<IInMemoryApplicationUserStore, InMemoryApplicationUserStore>();// .AddSingleton(new InMemoryApplicationUserStore());
            builder.AddProfileService<InMemoryUsersProfileService>();
            builder.AddResourceOwnerValidator<InMemoryUsersPasswordValidator>();

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
