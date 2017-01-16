using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuthServerDemo.Attributes;
using AuthServerDemo.Models;
using IdentityServer4.Services;

namespace AuthServerDemo.Controllers
{
    [SecurityHeaders]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public HomeController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Error(string errorId)
        {
            var errorModel = new ErrorViewModel();

            var message = await _interaction.GetErrorContextAsync(errorId);

            if (message != null)
            {
                errorModel.Error = message;
            }

            return View("Error", errorModel);
        }
    }
}