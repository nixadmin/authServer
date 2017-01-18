using AuthServerDemo.Data.Entities;
using AuthServerDemo.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthServerDemo.Initialization
{
    public static class ApplicationUserInitializationExtensions
    {
        public static void CopyUsersFromDatabaseToRedis(this IApplicationBuilder app)
        {
            var userRepository = app.ApplicationServices.GetService<IApplicationUserRepository>();
            var identityUserSotore = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();

            userRepository.AddRangeAsync(identityUserSotore.Users).Wait();
        }
    }
}