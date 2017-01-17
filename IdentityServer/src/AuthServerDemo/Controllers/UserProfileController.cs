using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthServerDemo.Models.UserProfile;
using Microsoft.AspNetCore.Identity;
using AuthServerDemo.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using AuthServerDemo.Configuration;
using AuthServerDemo.Data;
using System;
using AuthServerDemo.Data.Stores;

namespace AuthServerDemo.Controllers
{
    [Route("api/profile")]
    [Produces("application/json")]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IInMemoryApplicationUserStore inMemoryUsers;

        public UserProfileController(UserManager<ApplicationUser> identityUserManager, IInMemoryApplicationUserStore inMemoryStore)
        {
            userManager = identityUserManager;
            inMemoryUsers = inMemoryStore;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get(string email)
        {
            try
            {
                //var users = userManager.Users.AsQueryable().Where(ApplicationUserQueries.GetUserWithRoleRestrictionsQuery(User, email)
                // var users = inMemoryUsers.GetUsersByExpression(ApplicationUserQueries.GetUserWithRoleRestrictionsQueryDictionary(User, email));
                var user = inMemoryUsers.FindByUsername(email);
                return Ok(new
                            {
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Address = user.Address,
                                IsAdmin = user.IsAdmin
                            });
            }
            catch (InvalidOperationException)
            {
                return Forbid();
            }
        }

        [Authorize(Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        Address = model.Address,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IsAdmin = model.IsAdmin
                    };

                var result = await userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    var createdUser = userManager.Users.First(q => q.UserName == user.UserName);
                    inMemoryUsers.Users.Add(createdUser);
                    return Ok(model);
                }
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(string email, [FromBody]UserProfileModel model)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(email))
            {
                var user = userManager.Users.AsQueryable().FirstOrDefault(ApplicationUserQueries.GetUserByEmailWithRoleRestrictionsQuery(User, email));

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return Ok(new {
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Address = user.Address,
                            IsAdmin = user.IsAdmin
                        });
                    }                
                }
            }

            return BadRequest(ModelState);
        }

        [Authorize(Roles.Admin)]
        [HttpDelete]
        public async Task<IActionResult> Delete(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = userManager.Users.FirstOrDefault(ApplicationUserQueries.GetUserByEmailQuery(email));

                if (user != null)
                {
                    var result = await userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                }
            }

            return BadRequest();
        }
    }
}