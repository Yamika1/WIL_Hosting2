using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReCenterHub.Controllers
{
    [Authorize(Roles = "Client")]
    public class AboutMeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
