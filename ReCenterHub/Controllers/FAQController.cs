using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReCenterHub.Controllers
{
    [Authorize(Roles = "Client,Admin")]
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
