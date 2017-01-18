using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using AuthServerDemo.Configuration;
using AuthServerDemo.Data;
using AuthServerDemo.Data.Entities;
using AuthServerDemo.Initialization;
using AuthServerDemo.Services;

using IdentityServer4.Services;
using System.Reflection;
using System.IdentityModel.Tokens.Jwt;
using AuthServerDemo.Configuration.Settings;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Validation;
using AuthServerDemo.Data.Repository;
using IdentityServer4.Stores;
using AuthServerDemo.Data.Stores;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace AuthServerDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment environment;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            environment = env;

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var cert = new X509Certificate2(
                Path.Combine(environment.ContentRootPath, Configuration.GetSection("Hosting:CertName").Value), 
                Configuration.GetSection("Hosting:CertPassword").Value);

            var connectionString = Configuration.GetConnectionString(Configuration.GetDatabaseConnectionStringName());
            // var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AuthorizationServerDbContext, int>()
                .AddDefaultTokenProviders();

            services.AddDbContext<AuthorizationServerDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Roles.User, policyUser =>
                {
                    policyUser.RequireClaim(JwtClaimTypes.Role, Roles.User);
                });
                options.AddPolicy(Roles.Admin, policyAdmin =>
                {
                    policyAdmin.RequireClaim(JwtClaimTypes.Role, Roles.Admin);
                });
            });

            services.AddMvc();

            services.AddTransient<IProfileService, ApplicationUserProfileService>();
            services.AddTransient<IResourceOwnerPasswordValidator, ApplicationUserPasswordValidator>();

            // registration for redis connection

            services.AddSingleton(new RedisConnection(Configuration.GetSection("Redis:Host").Value));
            services.AddTransient<IApplicationUserRepository, ApplicationUserRedisRepository>();
            services.AddTransient<IGrantRepository, GrantRedisRepository>();

            services.AddTransient<IPersistedGrantStore, PersistedGrantRedisStore>();

            services.AddIdentityServer()
                .AddSigningCredential(cert)
                .AddInMemoryIdentityResources(FakeDataConfig.GetIdentityResources())
                .AddInMemoryApiResources(FakeDataConfig.GetApiResources())
                .AddInMemoryClients(FakeDataConfig.GetClients())

                //.AddConfigurationStore(builder =>
                //    builder.UseNpgsql(connectionString, options =>
                //        options.MigrationsAssembly(migrationAssembly)))

                //.AddOperationalStore(builder =>
                //    builder.UseNpgsql(connectionString, options =>
                //        options.MigrationsAssembly(migrationAssembly)))

                .ManageApplicationUsers();
               // .AddTestUsers(FakeDataConfig.GetUsers());
                
                //.AddAspNetIdentity<ApplicationUser>()
                //.AddProfileService<IdentityProfileService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (Configuration.IsMigrateDatabaseOnStartup())
            {
                app.ApplyMigrations(Configuration.IsMigrateDatabaseOnStatupWithTestingData());
            }

            app.UseIdentity();
            app.UseIdentityServer();

            //app.InitializeInMemoryStore();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseFacebook(Configuration);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseIdentityAuthentication(Configuration);

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}