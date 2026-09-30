using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReCenterHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
